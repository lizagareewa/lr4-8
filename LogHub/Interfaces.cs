using System.Collections.Generic;

namespace LogHub
{
    public interface ILogReader { IEnumerable<string> Read(); }
    public interface ILogParser { LogEntry Parse(string rawString); }
    public interface ILogFilter { bool IsValid(LogEntry entry); }
    public interface ILogSink { void Write(LogEntry entry); }
}