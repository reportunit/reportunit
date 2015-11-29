using System.Linq;

namespace ReportUnit.Logging
{
	using System;
	using System.Collections.Generic;

	internal class Logger
    {
        private readonly Queue<Log> _queue = new Queue<Log>();

        public void Log(Level level, string message)
        {
	        var log = new Log {Timestamp = DateTime.Now, Level = level, Message = message};


	        Console.WriteLine(log.ToString());

            _queue.Enqueue(log);
        }

        public Queue<Log> GetLogs()
        {
            return _queue;
        }

        public string GetLogsAsString()
        {
	        return _queue.Aggregate("", (current, log) => current + (log + "<br />"));
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

        private static Logger _instance;

        private Logger() { }

        public static Logger GetLogger()
        {
	        return _instance ?? (_instance = new Logger());
        }
    }
}
