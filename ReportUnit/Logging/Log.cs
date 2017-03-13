using System;

namespace ReportUnit.Logging
{
    internal class Log
    {
        public Level Level;
        public string Message;
        public DateTime Timestamp;

        public override string ToString()
        {
            return string.Format("[{0}][{1}] {2}", Timestamp.ToString("yyyy.MM.dd HH:mm:ss"), Level, Message);
        }
    }
}