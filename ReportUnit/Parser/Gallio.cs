namespace ReportUnit.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using ReportUnit.Layer;
    using ReportUnit.Support;

	internal class Gallio : IParser
    {
        /// <summary>
        /// XmlDocument instance
        /// </summary>
        private XmlDocument _doc;

        /// <summary>
        /// The input file from NUnit TestResult.xml
        /// </summary>
        private string _testResultFile = "";

        /// <summary>
        /// Contains test-suite level data to be passed to the Folder level report to build summary
        /// </summary>
        private Report _report;

        /// <summary>
        /// Xml namespace for processing file
        /// </summary>
        private XmlNamespaceManager _nsmgr;

	    public IParser LoadFile(string testResultFile)
	    {
            if (_doc == null) _doc = new XmlDocument();

            _testResultFile = testResultFile;

            _doc.Load(testResultFile);

            _nsmgr = new XmlNamespaceManager(_doc.NameTable);
            _nsmgr.AddNamespace("ns", "http://www.gallio.org/");

            return this;
	    }

	    public Report ProcessFile()
	    {
            // create a data instance to be passed to the folder level report
            _report = new Report();
            _report.FileName = this._testResultFile;
            _report.RunInfo.TestRunner = TestRunner.Gallio;

            Console.WriteLine("[INFO] Processing file '" + _testResultFile + "'..");

            // get total count of tests from the input file
            _report.Total = _doc.SelectNodes("descendant::ns:testStep[@isTestCase='true']", _nsmgr).Count;
            _report.AssemblyName = _doc.SelectSingleNode("//ns:files/ns:file", _nsmgr).InnerText;

            Console.WriteLine("[INFO] Number of tests: " + _report.Total);

            if (_report.Total >= 1)
            {
                Console.WriteLine("[INFO] Processing root and test-suite elements...");

                XmlNodeList l = _doc.SelectNodes("//ns:testStepRun//ns:outcome[@status='passed']", _nsmgr);

                // pull values from XML source
                _report.Passed = Int32.Parse(_doc.SelectSingleNode("//ns:statistics/@passedCount", _nsmgr).InnerText);
                _report.Failed = Int32.Parse(_doc.SelectSingleNode("//ns:statistics/@failedCount", _nsmgr).InnerText);
                _report.Inconclusive = Int32.Parse(_doc.SelectSingleNode("//ns:statistics/@inconclusiveCount", _nsmgr).InnerText);
                _report.Skipped = Int32.Parse(_doc.SelectSingleNode("//ns:statistics/@skippedCount", _nsmgr).InnerText);
                _report.Errors = 0;

                XmlNode testPackageRun = _doc.SelectSingleNode("descendant::ns:testPackageRun", _nsmgr);

                if (testPackageRun != null)
                {
                    _report.Duration = DateTimeHelper.DifferenceInMilliseconds(testPackageRun.Attributes["startTime"].InnerText, testPackageRun.Attributes["endTime"].InnerText);
                }

                try
                {
                    _report.RunInfo.Info = new Dictionary<string, string> {
                        {"TestResult File", _testResultFile},
                        {"Assembly", _doc.SelectSingleNode("//ns:codeReference/@assembly", _nsmgr).InnerText},
                        {"Code Location", _doc.SelectSingleNode("//ns:codeLocation/@path", _nsmgr).InnerText},
                        {"TestKind", _doc.SelectSingleNode("(//ns:entry[@key='TestKind'])[1]/ns:value", _nsmgr).InnerText},
                        {"File", _doc.SelectSingleNode("(//ns:entry[@key='File'])[1]/ns:value", _nsmgr).InnerText},
                        {"CodeBase", _doc.SelectSingleNode("(//ns:entry[@key='CodeBase'])[1]/ns:value", _nsmgr).InnerText},
                        {"Framework", _doc.SelectSingleNode("(//ns:entry[@key='Framework'])[1]/ns:value", _nsmgr).InnerText},
                        {"Version", _doc.SelectSingleNode("(//ns:entry[@key='Version'])[1]/ns:value", _nsmgr).InnerText}
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ERROR] There was an error processing RunInfo: " + ex.Message);
                }

                ProcessFixtureBlocks();

                _report.Status = _doc.SelectNodes("descendant::ns:testPackageRun/ns:testStepRun/ns:result/ns:outcome/@status", _nsmgr)[0].InnerText.AsStatus();
            }
            else
            {
                Console.Write("There are no tests available in this file.");

                _report.Status = Status.Passed;
            }

            return _report;
	    }

        private void ProcessFixtureBlocks()
        {
            Console.WriteLine("[INFO] Building fixture blocks...");

			string errorMsg = null;
			string descMsg = null;
            XmlNodeList suites = _doc.SelectNodes("//ns:testStep[@isTestCase='true']/ancestor::ns:testStepRun[2]", _nsmgr);
			int testCount = 0;

			// run for each test-suite
			foreach (XmlNode suite in suites)
            {
                var testSuite = new TestSuite();
                
                testSuite.Name = suite.SelectSingleNode("./ns:testStep", _nsmgr).Attributes["name"].InnerText;
                testSuite.StartTime = suite.Attributes["startTime"].InnerText;
                testSuite.EndTime = suite.Attributes["endTime"].InnerText;

                XmlNodeList tests = suite.SelectNodes("./ns:children/ns:testStepRun/ns:testStep[@isTestCase='true']", _nsmgr);

                foreach (XmlNode test in tests)
                {
                    errorMsg = descMsg = "";

                    var tc = new Test();
                    tc.Name = test.Attributes["name"].InnerText;
                    tc.Status = test.SelectSingleNode("./following-sibling::ns:result/ns:outcome/@status", _nsmgr).InnerText.AsStatus();



                    testSuite.Tests.Add(tc);
                }

                testSuite.Status = ReportHelper.GetFixtureStatus(testSuite.Tests);

                _report.TestFixtures.Add(testSuite);
            }
        }
    }
}
