using System;

namespace LogHub
{
    public class SimpleParser : ILogParser
    {
        public LogEntry Parse(string rawString)
        {
            if (string.IsNullOrWhiteSpace(rawString)) return null;
            
            var parts = rawString.Split(':', 2);
            if (parts.Length < 2) return null;

            if (Enum.TryParse<LogSeverity>(parts[0].Trim(), true, out var severity))
            {
                return new LogEntry { Severity = severity, Message = parts[1].Trim() };
            }
            return null;
        }
    }

    public class SeverityFilter : ILogFilter
    {
        private readonly LogSeverity _minSeverity;
        public SeverityFilter(LogSeverity minSeverity) => _minSeverity = minSeverity;
        
        public bool IsValid(LogEntry entry) => entry.Severity >= _minSeverity;
    }
}