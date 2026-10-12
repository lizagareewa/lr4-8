using System.Collections.Generic;

namespace LogHub
{
    public class LogProcessor
    {
        private readonly ILogReader _reader;
        private readonly ILogParser _parser;
        private readonly ILogFilter _filter;
        private readonly IEnumerable<ILogSink> _sinks;

        public LogProcessor(ILogReader reader, ILogParser parser, ILogFilter filter, IEnumerable<ILogSink> sinks)
        {
            _reader = reader;
            _parser = parser;
            _filter = filter;
            _sinks = sinks;
        }

        public void ProcessLogs()
        {
            foreach (var rawString in _reader.Read())
            {
                var entry = _parser.Parse(rawString);
                
                if (entry != null && _filter.IsValid(entry))
                {
                    foreach (var sink in _sinks)
                    {
                        sink.Write(entry);
                    }
                }
            }
        }
    }
}