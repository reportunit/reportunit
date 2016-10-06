using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parser
{
    internal class ParserFactory
    {
        private Logger logger = Logger.GetLogger();

        private string filePath;

        private bool validJunitSchema;

        public ParserFactory(string filePath)
        {
            this.filePath = filePath;
            this.validJunitSchema = true;
        }

        public TestRunner GetTestRunnerType()
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

                    XmlNode model = doc.SelectSingleNode("//ns:testModel", nsmgr);
                    if (model != null) return TestRunner.Gallio;


                    // xUnit - will have <assembly ... test-framework="xUnit.net 2....."/>
                    XmlNode assemblyNode = doc.SelectSingleNode("//assembly");
                    if (assemblyNode != null && assemblyNode.Attributes != null &&
                        assemblyNode.Attributes["test-framework"] != null)
                    {
                        string testFramework = assemblyNode.Attributes["test-framework"].InnerText.ToLower();

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
                    XmlNode envNode = doc.SelectSingleNode("//environment");
                    if (envNode != null && envNode.Attributes != null && envNode.Attributes["nunit-version"] != null)
                        return TestRunner.NUnit;

                    // check for test-suite nodes - if it has those - its probably nunit tests
                    var testSuiteNodes = doc.SelectNodes("//test-suite");
                    if (testSuiteNodes != null && testSuiteNodes.Count > 0) return TestRunner.NUnit;
                }
            }
            catch (Exception ex)
            {
                logger.Warning("Error when trying to determine testrunner for file {filePath}: {ex.Message}");
            }

            return TestRunner.Unknown;
        }

        private bool ValidateJUnitXsd(XmlDocument doc)
        {
            try
            {
                XmlSchemaSet schema = new XmlSchemaSet();
                using (var file = new FileStream(@"Schemas\junit.xsd", FileMode.Open))
                {
                    schema.Add("", XmlReader.Create(file));

                    doc.Schemas.Add(schema);
                    doc.Schemas.Compile();
                    doc.Validate((s, o) =>
                    {
                        this.validJunitSchema = false;
                    });

                    return this.validJunitSchema;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
