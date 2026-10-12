using Xunit;
using LogHub;
using System.Collections.Generic;

namespace LogHub.Tests
{
    // Заглушка источника логов (имитирует чтение из файла для тестов)
    public class StubReader : ILogReader
    {
        private readonly IEnumerable<string> _lines;
        public StubReader(IEnumerable<string> lines) => _lines = lines;
        public IEnumerable<string> Read() => _lines;
    }

    // Заглушка приёмника (сохраняет логи в память, а не в файл)
    public class MockSink : ILogSink
    {
        public List<LogEntry> WrittenEntries { get; } = new List<LogEntry>();
        public void Write(LogEntry entry) => WrittenEntries.Add(entry);
    }

    public class LogProcessorTests
    {
        [Fact]
        public void Parser_ValidString_ReturnsParsedEntry()
        {
            var parser = new SimpleParser();
            var result = parser.Parse("Error: Disk full");
            
            Assert.NotNull(result);
            Assert.Equal(LogSeverity.Error, result.Severity);
            Assert.Equal("Disk full", result.Message);
        }

        [Fact]
        public void Parser_InvalidString_ReturnsNull()
        {
            var parser = new SimpleParser();
            var result = parser.Parse("InvalidFormatWithoutColon");
            Assert.Null(result);
        }

        [Fact]
        public void Filter_BelowMinSeverity_ReturnsFalse()
        {
            var filter = new SeverityFilter(LogSeverity.Warning);
            var entry = new LogEntry { Severity = LogSeverity.Info, Message = "Test" };
            Assert.False(filter.IsValid(entry));
        }

        [Fact]
        public void Filter_AboveOrEqualMinSeverity_ReturnsTrue()
        {
            var filter = new SeverityFilter(LogSeverity.Warning);
            var entry = new LogEntry { Severity = LogSeverity.Error, Message = "Test" };
            Assert.True(filter.IsValid(entry));
        }

        [Fact]
        public void Processor_RoutesValidLogsToAllSinks()
        {
            var reader = new StubReader(new[] { "Error: Bad thing happened" });
            var parser = new SimpleParser();
            var filter = new SeverityFilter(LogSeverity.Info);
            var sink1 = new MockSink();
            var sink2 = new MockSink();

            var processor = new LogProcessor(reader, parser, filter, new[] { sink1, sink2 });
            processor.ProcessLogs();

            Assert.Single(sink1.WrittenEntries);
            Assert.Single(sink2.WrittenEntries);
            Assert.Equal("Bad thing happened", sink1.WrittenEntries[0].Message);
        }

        [Fact]
        public void Processor_DoesNotRouteInvalidOrFilteredLogs()
        {
            var reader = new StubReader(new[] { "Info: Skip me", "BadStringFormat" });
            var parser = new SimpleParser();
            var filter = new SeverityFilter(LogSeverity.Warning);
            var sink = new MockSink();

            var processor = new LogProcessor(reader, parser, filter, new[] { sink });
            processor.ProcessLogs();

            Assert.Empty(sink.WrittenEntries);
        }
    }
}