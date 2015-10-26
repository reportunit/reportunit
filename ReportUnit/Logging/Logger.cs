namespace ReportUnit.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class Logger
    {
        private Queue<Log> queue = new Queue<Log>();

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
            string logs = "";

            foreach (Log log in queue)
            {
                logs += log.ToString() + "<br />";
            }

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

        private static Logger instance;

        private Logger() { }

        public static Logger GetLogger()
        {
            if (instance == null)
            {
                instance = new Logger();
            }

            return instance;
        }
    }
}
