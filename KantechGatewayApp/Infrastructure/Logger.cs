using System;
using System.IO;

namespace KantechGatewayApp.KantechGatewayApp.Infrastructure
{
    public sealed class Logger
    {
        private static readonly string logRoot = Path.GetFullPath(System.Configuration.ConfigurationManager.AppSettings["Root.Logs"]);

        private static string LogPath(string subFolder = null)
        {
            string fname = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["Log.RollDaily"]) ? $"app_{DateTime.Today:ddMMyyyy}.log" : "app.log";
            string logDir = string.IsNullOrEmpty(subFolder) ? logRoot : Path.Combine(logRoot, subFolder);
            Directory.CreateDirectory(logDir);
            return Path.Combine(logDir, fname);
        }

        public static void Info(string msg, string subFolder = null)
        {
            Write("INFO", msg, subFolder);
        }
        public static void Warn(string msg, string subFolder = null)
        {
            Write("WARN", msg, subFolder);
        }
        public static void Error(string msg, Exception ex = null, string subFolder = null)
        {
            Write("ERROR", ex is null ? msg : $"{msg} :: {ex}", subFolder);
        }

        private static void Write(string level, string msg, string subFolder)
        {
            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {msg}";
            lock (typeof(Logger))
                File.AppendAllText(LogPath(subFolder), line + Environment.NewLine);
        }
    }
}