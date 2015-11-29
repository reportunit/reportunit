using System.IO;
using System.Xml;


namespace ReportUnit.Parser
{
	internal class ParserFactory
    {

        private readonly string _filePath;

        public ParserFactory(string filePath)
        {
            _filePath = filePath;
        }

        public TestRunner GetTestRunnerType()
        {
            var doc = new XmlDocument();

	        try
            {
                doc.Load(_filePath);

                if (doc.DocumentElement == null)
                    return TestRunner.Unknown;

	            var extension = Path.GetExtension(_filePath);
	            if (extension != null)
	            {
		            var fileExtension = extension.ToLower();

		            XmlNamespaceManager nsmgr;
		            if (fileExtension.EndsWith("trx"))
		            {
			            // MSTest2010
			            nsmgr = new XmlNamespaceManager(doc.NameTable);
			            nsmgr.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

			            // check if its a mstest 2010 xml file 
			            // will need to check the "//TestRun/@xmlns" attribute - value = http://microsoft.com/schemas/VisualStudio/TeamTest/2010
			            var testRunNode = doc.SelectSingleNode("ns:TestRun", nsmgr);
			            if (testRunNode != null && testRunNode.Attributes != null && testRunNode.Attributes["xmlns"] != null && testRunNode.Attributes["xmlns"].InnerText.Contains("2010"))
			            {
				            return TestRunner.MsTest2010;
			            }
		            }

		            if (fileExtension.EndsWith("xml"))
		            {
			            // Gallio
			            nsmgr = new XmlNamespaceManager(doc.NameTable);
			            nsmgr.AddNamespace("ns", "http://www.gallio.org/");

			            var model = doc.SelectSingleNode("//ns:testModel", nsmgr);
			            if (model != null) return TestRunner.Gallio;


			            // xUnit - will have <assembly ... test-framework="xUnit.net 2....."/>
			            var assemblyNode = doc.SelectSingleNode("//assembly");
			            if (assemblyNode != null && assemblyNode.Attributes != null &&
			                assemblyNode.Attributes["test-framework"] != null)
			            {
				            var testFramework = assemblyNode.Attributes["test-framework"].InnerText.ToLower();

				            if (testFramework.Contains("xunit"))
				            {
					            if (testFramework.Contains(" 2."))
					            {
						            return TestRunner.XUnitV2;
					            }
					            else if (testFramework.Contains(" 1."))
					            {
						            return TestRunner.XUnitV1;
					            }
				            }
			            }

			            // NUnit
			            // NOTE: not all nunit test files (ie when have nunit output format from other test runners) will contain the environment node
			            //            but if it does exist - then it should have the nunit-version attribute
			            var envNode = doc.SelectSingleNode("//environment");
			            if (envNode != null && envNode.Attributes != null && envNode.Attributes["nunit-version"] != null) return TestRunner.NUnit;

			            // check for test-suite nodes - if it has those - its probably nunit tests
			            var testSuiteNodes = doc.SelectNodes("//test-suite");
			            if (testSuiteNodes != null && testSuiteNodes.Count > 0) return TestRunner.NUnit;
		            }
	            }
            }
            catch
            {
	            // ignored
            }

	        return TestRunner.Unknown;
        }
    }
}
