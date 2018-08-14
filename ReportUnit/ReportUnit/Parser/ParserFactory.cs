using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using ReportUnit.Logging;

namespace ReportUnit.Parser
{
    internal class ParserFactory
    {
        private readonly Logger _logger = Logger.GetLogger();

        private readonly string _filePath;

        private bool _validJunitSchema;

        public ParserFactory(string filePath)
        {
            _filePath = filePath;
            _validJunitSchema = true;
        }

        public TestRunner GetTestRunnerType()
        {
            var doc = new XmlDocument();

            try
            {
                doc.Load(_filePath);

                if (doc.DocumentElement == null)
                    return TestRunner.Unknown;

                var fileExtension = Path.GetExtension(_filePath).ToLower();

                XmlNamespaceManager nsmgr;
                if (fileExtension.EndsWith("trx"))
                {
                    // MSTest2010
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ns", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

                    // check if its a mstest 2010 xml file 
                    // will need to check the "//TestRun/@xmlns" attribute - value = http://microsoft.com/schemas/VisualStudio/TeamTest/2010
                    var testRunNode = doc.SelectSingleNode("ns:TestRun", nsmgr);
                    if (testRunNode != null && testRunNode.Attributes != null && testRunNode.Attributes["xmlns"] != null &&
                        testRunNode.Attributes["xmlns"].InnerText.Contains("2010"))
                    {
                        return TestRunner.MSTest2010;
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

                    if (ValidateJUnitXsd(doc))
                    {
                        return TestRunner.JUnit;
                    }

                    // NUnit
                    // NOTE: not all nunit test files (ie when have nunit output format from other test runners) will contain the environment node
                    //            but if it does exist - then it should have the nunit-version attribute
                    var envNode = doc.SelectSingleNode("//environment");
                    if (envNode != null && envNode.Attributes != null && envNode.Attributes["nunit-version"] != null)
                        return TestRunner.NUnit;

                    // check for test-suite nodes - if it has those - its probably nunit tests
                    var testSuiteNodes = doc.SelectNodes("//test-suite");
                    if (testSuiteNodes != null && testSuiteNodes.Count > 0) return TestRunner.NUnit;
                }
            }
            catch (Exception ex)
            {
                _logger.Warning(string.Format("Error when trying to determine testrunner for file {0}: {1}", _filePath, ex.Message));
            }

            return TestRunner.Unknown;
        }

        private bool ValidateJUnitXsd(XmlDocument doc)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("ReportUnit.Resources.Schemas.JUnit.xsd"))
                using (var reader = new StreamReader(stream))
                {
                    var schema = new XmlSchemaSet();
                
                    schema.Add("", XmlReader.Create(reader));

                    doc.Schemas.Add(schema);
                    doc.Schemas.Compile();
                    doc.Validate((s, o) =>
                    {
                        _validJunitSchema = false;
                    });

                    return _validJunitSchema;
                }                
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
