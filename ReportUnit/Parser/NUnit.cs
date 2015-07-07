using System;
using System.IO;
using System.Linq;
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
		/// Usually evaluates to the assembly name. Used to clean up test names so its easier to read in the outputted html.
		/// </summary>
		private string _fileNameWithoutExtension;

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
	        _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this._testResultFile);

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

				_report.Status = ReportHelper.GetFixtureStatus(_report.TestFixtures.SelectMany(x => x.Tests));
            }
            else
            {
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
                _report.RunInfo.Info.Add("Last Run", lastModified.ToString("d MMM yyyy HH:mm"));
            }
            catch (Exception) { }

            if (_report.Duration > 0) _report.RunInfo.Info.Add("Duration", string.Format("{0} ms", _report.Duration));


            try
            {
                // try to parse the environment node
                // some attributes in the environment node are different for 2.x and 3.x
                XmlNode env = _doc.GetElementsByTagName("environment")[0];
                if (env != null && env.Attributes != null)
                {
					if (env.Attributes["user"] != null) _report.RunInfo.Info.Add("User", env.Attributes["user"].InnerText);
					if (env.Attributes["user-domain"] != null) _report.RunInfo.Info.Add("User Domain", env.Attributes["user-domain"].InnerText);
					if (env.Attributes["machine-name"] != null) _report.RunInfo.Info.Add("Machine Name", env.Attributes["machine-name"].InnerText);
					if (env.Attributes["platform"] != null) _report.RunInfo.Info.Add("Platform", env.Attributes["platform"].InnerText);
					if (env.Attributes["os-version"] != null) _report.RunInfo.Info.Add("Os Version", env.Attributes["os-version"].InnerText);
					if (env.Attributes["clr-version"] != null) _report.RunInfo.Info.Add("Clr Version", env.Attributes["clr-version"].InnerText);
                    _report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
					if (env.Attributes["nunit-version"] != null) _report.RunInfo.Info.Add("TestRunner Version", env.Attributes["nunit-version"].InnerText);
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
			// if other test runner type is outputting its results in nunit format - then may not have full markup - so just get all "test-suite" nodes
	        if (testSuiteNodes.Count == 0)
			{
				testSuiteNodes = _doc.SelectNodes(string.Format("//test-suite[contains(@name, '{0}.')]", _fileNameWithoutExtension));
		        
	        }
            int testCount = 0;

            // run for each test-suite
            foreach (XmlNode suite in testSuiteNodes)
            {
                var testSuite = new TestSuite();
				testSuite.Name = suite.Attributes["name"].InnerText.Replace(_fileNameWithoutExtension + ".", "").Trim();

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
                        errorMsg = message.InnerText.Trim();
                        errorMsg += testSuiteFailureNode.SelectSingleNode(".//stack-trace") != null ? " -> " + testSuiteFailureNode.SelectSingleNode(".//stack-trace").InnerText.Replace("\r", "").Replace("\n", "") : "";
                    }
                    testSuite.StatusMessage = errorMsg;
                }


                // add each test of the test-fixture
                foreach (XmlNode testcase in suite.SelectNodes(".//test-case"))
                {
                    errorMsg = descMsg = "";

                    var tc = new Test();
					tc.Name = testcase.Attributes["name"].InnerText.Replace("<", "[").Replace(">", "]").Replace(testSuite.Name + ".", "").Replace(_fileNameWithoutExtension + ".", "");

					// figure out the status reslt of the test
	                if (testcase.Attributes["result"] != null)
	                {
		                tc.Status = testcase.Attributes["result"].InnerText.AsStatus();
	                }
					else if (testcase.Attributes["executed"] != null && testcase.Attributes["success"] != null)
					{
						bool success, executed;
						bool.TryParse(testcase.Attributes["executed"].InnerText, out executed);
						bool.TryParse(testcase.Attributes["success"].InnerText, out success);

						tc.Status = success ? Status.Passed : 
											executed ? Status.Failed : Status.Skipped;
					}
					else if (testcase.Attributes["success"] != null)
					{
						bool success;
						bool.TryParse(testcase.Attributes["success"].InnerText, out success);
						tc.Status = success ? Status.Passed : Status.Failed;
					}


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

                    var categories = testcase.SelectNodes(".//property[@name='Category']");

                    foreach (XmlNode category in categories)
                    {
                        tc.Categories.Add(category.Attributes["value"].InnerText);
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
