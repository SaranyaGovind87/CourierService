using System;
using System.IO;

namespace CourierService.Logging
{
    public enum LogLevel { Info, Warning, Error }

    public class FileLogger
    {
        private static readonly string LogFilePath = "delivery_log.txt";
        private static readonly object _lock = new();

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level.ToString().ToUpper()}] {message}";

            lock (_lock)
            {
                // 1. Log to File
                File.AppendAllLines(LogFilePath, new[] { logEntry });

                // 2. Log to Console with Colors
                Console.ForegroundColor = level switch
                {
                    LogLevel.Error => ConsoleColor.Red,
                    LogLevel.Warning => ConsoleColor.Yellow,
                    _ => ConsoleColor.Cyan
                };
                Console.WriteLine(logEntry);
                Console.ResetColor();
            }
        }
    }
}
