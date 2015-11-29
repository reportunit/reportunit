namespace ReportUnit.Logging
{
	using System;

	internal class Log
	{
		public DateTime Timestamp;
		public Level Level;
		public string Message;

		public override string ToString()
		{
			return string.Format("[{0}][{1}] {2}", Timestamp.ToString("yyyy.MM.dd HH:mm:ss"), Level, Message);
		}
	}
}
