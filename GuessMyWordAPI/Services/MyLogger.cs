using GuessMyWordAPI.enums;
using GuessMyWordAPI.IServices;

namespace GuessMyWordAPI.Services
{

    public class MyLogger : IMyLogger
    {
        private readonly string path;
        private bool fileExists = false;

        public MyLogger()
        {
            var logsDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }
            path = Path.Combine(logsDir, $"{DateTime.Now:ddMMyyyy.HHmmss.fff}.txt");
            Console.WriteLine($"Log file path is: {path}.");
            if (!File.Exists(path))
            {
                Console.WriteLine("Log path does not exist, trying to create.");
                try
                {
                    var file = File.Create(path);
                    file.Flush();
                    file.Close();
                    fileExists = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to create file:\n{ex.ToString()}");
                }
            }
            else
            {
                fileExists = true;
            }
        }

        public string Error(string message)
        {
            return Log(message, LogSeverity.Error);
        }

        public string Info(string message)
        {
            return Log(message, LogSeverity.Info);
        }

        public string Log(string message, LogSeverity severity = LogSeverity.Info)
        {
            var date = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff");
            var s = severity == LogSeverity.Info ? "[I]" : (severity == LogSeverity.Warning ? "[W]" : "[I]");
            var msg = $"{date} - {s} - {message}";
            Console.WriteLine(msg);
            if (fileExists)
            {
                File.AppendAllText(path, $"{msg}\n");
            }
            return msg;
        }

        public string Warning(string message)
        {
            return Log(message, LogSeverity.Warning);
        }
    }
}
