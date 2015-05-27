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

            // get total count of tests from the input file
            _report.Total = _doc.SelectNodes("descendant::ns:testStep[@isTestCase='true']", _nsmgr).Count;
            _report.AssemblyName = _doc.SelectSingleNode("//ns:files/ns:file", _nsmgr).InnerText;

            Console.WriteLine("[INFO] Number of tests: " + _report.Total);

            if (_report.Total >= 1)
            {
                Console.WriteLine("[INFO] Processing root and test-suite elements...");

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
                        {"File", _doc.SelectSingleNode("(//ns:file)[1]", _nsmgr).InnerText}
                    };

                    var assembly = _doc.SelectSingleNode("//ns:codeReference/@assembly", _nsmgr);
                    if (assembly != null)
                        _report.RunInfo.Info.Add("Assembly", assembly.InnerText);
    
                    var codeLocation = _doc.SelectSingleNode("//ns:codeLocation/@path", _nsmgr);
                    if (codeLocation != null)
                        _report.RunInfo.Info.Add("CodeLocation", codeLocation.InnerText);

                    var testKind = _doc.SelectSingleNode("(//ns:entry[@key='TestKind'])[1]/ns:value", _nsmgr);
                    if (testKind != null)
                        _report.RunInfo.Info.Add("TestKind", testKind.InnerText);
    
                    var codeBase = _doc.SelectSingleNode("(//ns:entry[@key='CodeBase'])[1]/ns:value", _nsmgr);
                    if (codeBase != null)
                        _report.RunInfo.Info.Add("CodeBase", codeBase.InnerText);
    
                    var framework = _doc.SelectSingleNode("(//ns:entry[@key='Framework'])[1]/ns:value", _nsmgr);
                    if (framework != null)
                        _report.RunInfo.Info.Add("Framework", framework.InnerText);

                    var version = _doc.SelectSingleNode("(//ns:entry[@key='Version'])[1]/ns:value", _nsmgr);
                    if (version != null)
                        _report.RunInfo.Info.Add("Version", version.InnerText);

                    var ignoreReason = _doc.SelectSingleNode("(//ns:entry[@key='IgnoreReason'])[1]/ns:value", _nsmgr);
                    if (ignoreReason != null)
                        _report.RunInfo.Info.Add("IgnoreReason", ignoreReason.InnerText);
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
                testSuite.StartTime = suite.Attributes["startTime"].InnerText.Split('.')[0];
                testSuite.EndTime = suite.Attributes["endTime"].InnerText.Split('.')[0];

                XmlNodeList tests = suite.SelectNodes("./ns:children/ns:testStepRun/ns:testStep[@isTestCase='true']", _nsmgr);

                foreach (XmlNode test in tests)
                {
                    errorMsg = descMsg = "";

                    var tc = new Test();
                    tc.Name = test.Attributes["name"].InnerText;
                    tc.Status = test.SelectSingleNode("./following-sibling::ns:result/ns:outcome/@status", _nsmgr).InnerText.AsStatus();

                    var desc = test.SelectSingleNode("./ns:metadata/ns:entry[@key='Description']/ns:value", _nsmgr);

                    if (desc != null)
                    {
                        descMsg += "<p class='description'>Description: " + desc.InnerText.Trim();
                        descMsg += "</p>";
                        descMsg = descMsg == "<p class='description'>Description: </p>" ? "" : descMsg;
                    }

                    var testStepRun = test.ParentNode.SelectSingleNode(".//ns:testLog", _nsmgr);

                    if (testStepRun != null && testStepRun.InnerText.Trim() != "")
                        errorMsg = "<pre>" + testStepRun.InnerText + "</pre>";

                    tc.StatusMessage = descMsg + errorMsg;

                    testSuite.Tests.Add(tc);

                    Console.Write("\r{0} tests processed...", ++testCount);
                }

                testSuite.Status = ReportHelper.GetFixtureStatus(testSuite.Tests);

                _report.TestFixtures.Add(testSuite);
            }
        }
    }
}
