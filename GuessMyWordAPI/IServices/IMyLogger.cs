using GuessMyWordAPI.enums;

namespace GuessMyWordAPI.IServices
{
    public interface IMyLogger
    {
        string Log(string message, LogSeverity severity = LogSeverity.Info);
        string Info(string message);
        string Warning(string message);
        string Error(string message);
    }
}
