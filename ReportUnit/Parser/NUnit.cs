using System;
using System.Xml;
using ReportUnit.Support;
using ReportUnit.Layer;

namespace ReportUnit.Parser
{

    internal class NUnit : IParser
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

        public IParser LoadFile(string testResultFile)
        {
            if (_doc == null) _doc = new XmlDocument();

            _testResultFile = testResultFile;

            _doc.Load(testResultFile);

            return this;
        }

        public Report ProcessFile()
        {
            // create a data instance to be passed to the folder level report
            _report = new Report();
            _report.FileName = this._testResultFile;
            _report.RunInfo.TestRunner = TestRunner.NUnit;

            // get total count of tests from the input file
            _report.Total = _doc.GetElementsByTagName("test-case").Count;
            _report.AssemblyName = _doc.SelectNodes("//test-suite")[0].Attributes["name"].InnerText;

            Console.WriteLine("[INFO] Number of tests: " + _report.Total);

            // only proceed if the test count is more than 0
            if (_report.Total >= 1)
            {
                Console.WriteLine("[INFO] Processing root and test-suite elements...");

                // pull values from XML source
                _report.Passed = _doc.SelectNodes(".//test-case[@result='Success' or @result='Passed']").Count;
                _report.Failed = _doc.SelectNodes(".//test-case[@result='Failed' or @result='Failure']").Count;
                _report.Inconclusive = _doc.SelectNodes(".//test-case[@result='Inconclusive' or @result='NotRunnable']").Count;
                _report.Skipped = _doc.SelectNodes(".//test-case[@result='Skipped' or @result='Ignored']").Count;
                _report.Errors = _doc.SelectNodes(".//test-case[@result='Error']").Count;

                _report.Status = _doc.SelectNodes("//test-suite")[0].Attributes["result"].InnerText.AsStatus();

                try
                {
                    double duration;
                    if (double.TryParse(_doc.SelectNodes("//test-suite")[0].Attributes["duration"].InnerText, out duration))
                    {
                        _report.Duration = duration;
                    }
                }
                catch
                {
                    try
                    {
                        double duration;
                        if (double.TryParse(_doc.SelectNodes("//test-suite")[0].Attributes["time"].InnerText, out duration))
                        {
                            _report.Duration = duration;
                        }
                    }
                    catch { }
				}

				ProcessRunInfo();
	            ProcessFixtureBlocks();
            }
            else
            {
                Console.Write("There are no tests available in this file.");

                _report.Status = Status.Passed;
            }

            return _report;
        }

		/// <summary>
		/// Find meta information about the whole test run
		/// </summary>
	    private void ProcessRunInfo()
	    {
			_report.RunInfo.Info.Add("TestResult File", _testResultFile);

			try
			{
				DateTime lastModified = System.IO.File.GetLastWriteTime(_testResultFile);
				_report.RunInfo.Info.Add("Last Run", string.Format("{0} {1}", lastModified.ToString("d MMM yyyy HH:mm")));
			}
			catch (Exception) {}

			if (_report.Duration > 0) _report.RunInfo.Info.Add("Duration", string.Format("{0} ms", _report.Duration));


			try
			{
				// try to parse the environment node
				// some attributes in the environment node are different for 2.x and 3.x
				XmlNode env = _doc.GetElementsByTagName("environment")[0];
				if (env != null)
				{
					_report.RunInfo.Info.Add("User", env.Attributes["user"].InnerText);
					_report.RunInfo.Info.Add("User Domain", env.Attributes["user-domain"].InnerText);
					_report.RunInfo.Info.Add("Machine Name", env.Attributes["machine-name"].InnerText);
					_report.RunInfo.Info.Add("Platform", env.Attributes["platform"].InnerText);
					_report.RunInfo.Info.Add("Os Version", env.Attributes["os-version"].InnerText);
					_report.RunInfo.Info.Add("Clr Version", env.Attributes["clr-version"].InnerText);
					_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
					_report.RunInfo.Info.Add("TestRunner Version", env.Attributes["nunit-version"].InnerText);
				}
				else
				{
					_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
				}
			}
			catch (Exception ex)
			{
				_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
				Console.WriteLine("[ERROR] There was an error processing the _ENVIRONMENT_ node: " + ex.Message);
			}

	    }
		
		/// <summary>
        /// Processes the fixture level blocks
        /// Adds all tests to the output
        /// </summary>
        private void ProcessFixtureBlocks()
        {
            Console.WriteLine("[INFO] Building fixture blocks...");

            string errorMsg = null;
            string descMsg = null;
            XmlNodeList testSuiteNodes = _doc.SelectNodes("//test-suite[@type='TestFixture']");
            int testCount = 0;

            // run for each test-suite
            foreach (XmlNode suite in testSuiteNodes)
            {
                var testSuite = new TestSuite();
                testSuite.Name = suite.Attributes["name"].InnerText;

                if (suite.Attributes["start-time"] != null && suite.Attributes["end-time"] != null)
                {
                    var startTime = suite.Attributes["start-time"].InnerText.Replace("Z", "");
                    var endTime = suite.Attributes["end-time"].InnerText.Replace("Z", "");

                    testSuite.StartTime = startTime;
                    testSuite.EndTime = endTime;
                    testSuite.Duration = DateTimeHelper.DifferenceInMilliseconds(startTime, endTime);
                }
                else if (suite.Attributes["time"] != null)
                {
                    double duration;
                    if (double.TryParse(suite.Attributes["time"].InnerText.Replace("Z", ""), out duration))
                    {
                        testSuite.Duration = duration;
                    }

                    testSuite.StartTime = duration.ToString();
                }
                
				// check if the testSuite has any errors (ie error in the TestFixtureSetUp)
	            var testSuiteFailureNode = suite.SelectSingleNode("failure");
				if (testSuiteFailureNode != null && testSuiteFailureNode.HasChildNodes)
				{
					errorMsg = descMsg = "";
					var message = testSuiteFailureNode.SelectSingleNode(".//message");

					if (message != null)
					{
						errorMsg = "<pre>" + message.InnerText.Trim();
						errorMsg += testSuiteFailureNode.SelectSingleNode(".//stack-trace") != null ? " -> " + testSuiteFailureNode.SelectSingleNode(".//stack-trace").InnerText.Replace("\r", "").Replace("\n", "") : "";
						errorMsg += "</pre>";
						errorMsg = errorMsg == "<pre></pre>" ? "" : errorMsg;
					}
					testSuite.StatusMessage = errorMsg;
				}


                // add each test of the test-fixture
                foreach (XmlNode testcase in suite.SelectNodes(".//test-case"))
                {
                    errorMsg = descMsg = "";

                    var tc = new Test();
                    tc.Name = testcase.Attributes["name"].InnerText.Replace("<", "[").Replace(">", "]");
                    tc.Status = testcase.Attributes["result"].InnerText.AsStatus();

                    if (testcase.Attributes["time"] != null)
                    {
                        try
                        {
                            TimeSpan d;
                            var durationTimeSpan = testcase.Attributes["duration"].InnerText;
                            if (TimeSpan.TryParse(durationTimeSpan, out d))
                            {
                                tc.Duration = d.TotalMilliseconds;
                            }
                        }
                        catch (Exception) { }
                    }

                    var message = testcase.SelectSingleNode(".//message");

                    if (message != null)
                    {
                        errorMsg = "<pre>" + message.InnerText.Trim();
                        errorMsg += testcase.SelectSingleNode(".//stack-trace") != null ? " -> " + testcase.SelectSingleNode(".//stack-trace").InnerText.Replace("\r", "").Replace("\n", "") : "";
                        errorMsg += "</pre>";
                        errorMsg = errorMsg == "<pre></pre>" ? "" : errorMsg;
                    }

                    XmlNode desc = testcase.SelectSingleNode(".//property[@name='Description']");

                    if (desc != null)
                    {
                        descMsg += "<p class='description'>Description: " + desc.Attributes["value"].InnerText.Trim();
                        descMsg += "</p>";
                        descMsg = descMsg == "<p class='description'>Description: </p>" ? "" : descMsg;
                    }

                    tc.StatusMessage = descMsg + errorMsg;
                    testSuite.Tests.Add(tc);

                    Console.Write("\r{0} tests processed...", ++testCount);
                }

                testSuite.Status = ReportHelper.GetFixtureStatus(testSuite.Tests);

                _report.TestFixtures.Add(testSuite);
            }
        }

        public NUnit() { }
    }
}
