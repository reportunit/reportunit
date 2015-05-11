namespace ReportUnit.Parser
{
	using System;
	using System.IO;

	internal static class ParserFactory
	{
		/// <summary>
		/// Find the appropriate Parser for the test file
		/// </summary>
		/// <param name="testResultFile"></param>
		/// <returns></returns>
		public static IParser LoadParser(string testResultFile)
		{
			if (!File.Exists(testResultFile))
			{
				Console.WriteLine("[ERROR] Input file does not exist: " + testResultFile);
				return null;
			}
			
			string pathExtension = Path.GetExtension(testResultFile);
			if (string.IsNullOrWhiteSpace(pathExtension))
			{
				Console.WriteLine("[ERROR] Input file does not have a file extension: " + testResultFile);
				return null;
			}
			pathExtension = pathExtension.ToLower();

			
			IParser fileParser = null;
			if (fileParser == null && pathExtension.Contains("xml"))
			{
				fileParser = new NUnit().LoadFile(testResultFile);
				if (fileParser != null) Console.WriteLine("[INFO] The file " + testResultFile + " contains NUnit test results");
			}
			if (fileParser == null && pathExtension.Contains("trx"))
			{
				fileParser = new MsTest2010().LoadFile(testResultFile);
				if (fileParser != null) Console.WriteLine("[INFO] The file " + testResultFile + " contains MSTest 2010 test results");
			}


			if (fileParser == null)
			{
				Console.WriteLine("[ERROR] Skipping " + testResultFile + ". It is not of a known test runner type.");
				return null;
			}

			return fileParser;
		}
	}
}
