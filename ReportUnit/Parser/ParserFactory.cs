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

                string fileExtension = Path.GetExtension(filePath).ToLower();

                if (fileExtension.EndsWith("trx"))
                {
                    // MSTest2010
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

                    // check if its a mstest 2010 xml file 
                    // will need to check the "//TestRun/@xmlns" attribute - value = http://microsoft.com/schemas/VisualStudio/TeamTest/2010
                    XmlNode testRunNode = doc.SelectSingleNode("ns:TestRun", nsmgr);
                    if (testRunNode != null && testRunNode.Attributes != null && testRunNode.Attributes["xmlns"] != null && testRunNode.Attributes["xmlns"].InnerText.Contains("2010"))
                    {
                        return TestRunner.MSTest2010;
                    }
                }

                if (fileExtension.EndsWith("xml"))
                {
                    // Gallio
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ns", "http://www.gallio.org/");

                    XmlNode model = doc.SelectSingleNode("//ns:testModel", nsmgr);
                    if (model != null) return TestRunner.Gallio;

                    // NUnit
                    // NOTE: not all nunit test files (ie when have nunit output format from other test runners) will contain the environment node
                    //            but if it does exist - then it should have the nunit-version attribute
                    XmlNode envNode = doc.SelectSingleNode("//environment");
                    if (envNode != null && envNode.Attributes != null && envNode.Attributes["nunit-version"] != null) return TestRunner.NUnit;

                    // check for test-suite nodes - if it has those - its probably nunit tests
                    var testSuiteNodes = doc.SelectNodes("//test-suite");
                    if (testSuiteNodes != null && testSuiteNodes.Count > 0) return TestRunner.NUnit;
                }
            }
            catch { }

            return TestRunner.Unknown;
        }
    }
}
