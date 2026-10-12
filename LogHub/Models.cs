namespace LogHub
{
    public enum LogSeverity 
    { 
        Info, 
        Warning, 
        Error, 
        Fatal 
    }

    public class LogEntry
    {
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }
    }
}