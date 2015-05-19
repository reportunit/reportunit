namespace ReportUnit.Parser
{
	using System;
	using System.IO;
    using System.Xml;

	internal static class ParserFactory
	{
		/// <summary>
		/// Find the appropriate Parser for the test file
		/// </summary>
		/// <param name="resultsFile"></param>
		/// <returns></returns>
		public static IParser LoadParser(string resultsFile)
		{
			if (!File.Exists(resultsFile))
			{
				Console.WriteLine("[ERROR] Input file does not exist: " + resultsFile);
				return null;
			}
			
			string fileExtension = Path.GetExtension(resultsFile);
			
            if (string.IsNullOrWhiteSpace(fileExtension))
			{
				Console.WriteLine("[ERROR] Input file does not have a file extension: " + resultsFile);
				return null;
			}

            IParser fileParser = null; 
            
            fileExtension = fileExtension.ToLower();

            switch (TestRunnerType(resultsFile))
            {
                case TestRunner.NUnit:
                    Console.WriteLine("[INFO] The file " + resultsFile + " contains NUnit test results");
                    fileParser = new NUnit().LoadFile(resultsFile);
                    break;
                case TestRunner.Gallio:
                    Console.WriteLine("[INFO] The file " + resultsFile + " contains Gallio test results");
                    fileParser = new Gallio().LoadFile(resultsFile);
                    break;
                case TestRunner.MSTest2010:
                    Console.WriteLine("[INFO] The file " + resultsFile + " contains MSTest 2010 test results");
                    fileParser = new MsTest2010().LoadFile(resultsFile);
                    break;
                default:
                    Console.WriteLine("[ERROR] Skipping " + resultsFile + ". It is not of a known test runner type.");
                    break;
            }

			return fileParser;
		}

        private static TestRunner TestRunnerType(string filePath)
        {
            XmlDocument doc = new XmlDocument();

            XmlNamespaceManager nsmgr;

            try
            {
                doc.Load(filePath);

                if (doc.DocumentElement == null)
                    return TestRunner.Unknown;

                // NUnit
                XmlNode envNode = doc.SelectSingleNode("//environment");

                if (envNode != null && envNode.Attributes["nunit-version"] != null)
                    return TestRunner.NUnit;

                // Gallio
                nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ns", "http://www.gallio.org/");

                XmlNode model = doc.SelectSingleNode("//ns:testModel", nsmgr);

                if (model != null)
                    return TestRunner.Gallio;

                // MSTest2010
                if (Path.GetExtension(filePath).ToLower().Contains("trx"))
                {
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

                    // check is mstest 2010 xml file 
                    // will need to check the "//TestRun/@xmlns" attribute - value = http://microsoft.com/schemas/VisualStudio/TeamTest/2010
                    XmlNode testRunNode = doc.SelectSingleNode("ns:TestRun", nsmgr);

                    if (testRunNode != null && testRunNode.Attributes["xmlns"] != null)
                        return TestRunner.MSTest2010;
                }
            }
            catch { }

            return TestRunner.Unknown;
        }
	}
}
