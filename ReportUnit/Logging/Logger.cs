using System;
using System.Collections.Generic;

namespace ReportUnit.Logging
{
    internal class Logger
    {
        private static Logger instance;
        private readonly Queue<Log> queue = new Queue<Log>();

        private Logger()
        {
        }

        public void Log(Level level, string message)
        {
            var log = new Log();

            log.Timestamp = DateTime.Now;
            log.Level = level;
            log.Message = message;

            Console.WriteLine(log.ToString());

            queue.Enqueue(log);
        }

        public Queue<Log> GetLogs()
        {
            return queue;
        }

        public string GetLogsAsString()
        {
            var logs = "";

            foreach (var log in queue)
                logs += log + "<br />";

            return logs;
        }

        public void Debug(string message)
        {
            Log(Level.Debug, message);
        }

        public void Info(string message)
        {
            Log(Level.Info, message);
        }

        public void Warning(string message)
        {
            Log(Level.Warning, message);
        }

        public void Error(string message)
        {
            Log(Level.Error, message);
        }

        public void Fatal(string message)
        {
            Log(Level.Fatal, message);
        }

        public static Logger GetLogger()
        {
            if (instance == null)
                instance = new Logger();

            return instance;
        }
    }
}