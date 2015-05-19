namespace ReportUnit.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;
	using ReportUnit.Layer;
	using ReportUnit.Support;

	internal class MsTest2010 : IParser
	{
		/// <summary>
		/// XmlDocument instance
		/// </summary>
		private XmlDocument _doc;

		/// <summary>
		/// The input file from MS Test TestResult.xml
		/// </summary>
		private string _testResultFile = "";

		/// <summary>
		/// Contains report level data to be passed to the Folder level report to build summary
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
            _nsmgr.AddNamespace("t", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

			return this;
		}

		public Report ProcessFile()
		{
			// create a data instance to be passed to the folder level report
			_report = new Report();
			_report.FileName = this._testResultFile;
			_report.RunInfo.TestRunner = TestRunner.MSTest2010;

			Console.WriteLine("[INFO] Processing file '" + _testResultFile + "'..");

			// get total count of tests from the input file
			_report.Total = _doc.SelectNodes("descendant::t:UnitTestResult", _nsmgr).Count;

            Console.WriteLine("[INFO] Number of tests: " + _report.Total);

			// only proceed if the test count is more than 0
			if (_report.Total >= 1)
			{
				Console.WriteLine("[INFO] Processing root and test-suite elements...");

				// pull values from XML source
				_report.AssemblyName = _doc.GetElementsByTagName("UnitTest")[0]["TestMethod"].Attributes["codeBase"].InnerText;

				_report.Passed = _doc.SelectNodes("descendant::t:UnitTestResult[@outcome='Passed']", _nsmgr).Count;
				_report.Failed = _doc.SelectNodes("descendant::t:UnitTestResult[@outcome='Failed']", _nsmgr).Count;
				_report.Inconclusive = _doc.SelectNodes("descendant::t:UnitTestResult[@outcome='Inconclusive' or @outcome='notRunnable' or @outcome='passedButRunAborted' or @outcome='disconnected' or @outcome='warning' or @outcome='pending']", _nsmgr).Count;
				_report.Skipped = _doc.SelectNodes("descendant::t:UnitTestResult[@outcome='NotExecuted']", _nsmgr).Count;
				_report.Errors = _doc.SelectNodes("descendant::t:UnitTestResult[@outcome='Error' or @outcome='Aborted' or @outcome='timeout']", _nsmgr).Count;

				try
				{
					XmlNode times = _doc.SelectSingleNode("descendant::t:Times", _nsmgr);
					if (times != null) _report.Duration = DateTimeHelper.DifferenceInMilliseconds(times.Attributes["start"].InnerText, times.Attributes["finish"].InnerText);
				}
				catch { }

				try
				{
					// try to parse the TestRun node
					XmlNode testRun = _doc.SelectSingleNode("descendant::t:TestRun", _nsmgr);

					if (testRun != null)
					{
                        _report.RunInfo.Info = new Dictionary<string, string> {
                            {"TestResult File", _testResultFile},
                            {"Machine Name", _doc.SelectNodes("descendant::t:UnitTestResult", _nsmgr)[0].Attributes["computerName"].InnerText},
                            {"TestRunner", _report.RunInfo.TestRunner.ToString()},
                            {"TestRunner Version", testRun.Attributes["xmlns"].InnerText}
                        };

                        var userInfo = testRun.Attributes["runUser"].InnerText;

                        if (!string.IsNullOrWhiteSpace(userInfo))
                        {
                            _report.RunInfo.Info.Add("User", userInfo.Split('\\').Last());
                            _report.RunInfo.Info.Add("User Domain", userInfo.Split('\\').First());
                        }
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("[ERROR] There was an error processing the _ENVIRONMENT_ node: " + ex.Message);
				}

				ProcessFixtureBlocks();

				_report.Status = ReportHelper.GetFixtureStatus(_report.TestFixtures.SelectMany(tf => tf.Tests).ToList());
			}
			else
			{
				Console.Write("There are no tests available in this file.");

				try
				{
					_report.Status = Status.Passed;

					return _report;
				}
				catch (Exception ex)
				{
					Console.WriteLine("Something weird happened: " + ex.Message);
					return null;
				}
			}

			return _report;
		}


		/// <summary>
		/// Processes the tests level blocks
		/// Adds all tests to the output
		/// </summary>
		private void ProcessFixtureBlocks()
		{
			Console.WriteLine("[INFO] Building fixture blocks...");

			int testCount = 0;
			var unitTestResults = _doc.SelectNodes("descendant::t:UnitTestResult", _nsmgr);

			// run for each test-suite
			foreach (XmlNode testResult in unitTestResults)
			{

				Test tc = new Test();
				tc.Name = testResult.Attributes["testName"].InnerText;
				tc.Status = testResult.Attributes["outcome"].InnerText.AsStatus();

				try
				{
					TimeSpan d;
					var durationTimeSpan = testResult.Attributes["duration"].InnerText;
					if (TimeSpan.TryParse(durationTimeSpan, out d))
					{
						tc.Duration = d.TotalMilliseconds;
					}
				}
				catch (Exception)
				{
					tc.Duration = DateTimeHelper.DifferenceInMilliseconds(testResult.Attributes["startTime"].InnerText, testResult.Attributes["endTime"].InnerText);
				}

				// check for any errors or messages
				if (testResult.HasChildNodes)
				{
					string errorMsg = "", descMsg = "";
					foreach (XmlNode node in testResult.ChildNodes)
					{
						if (node.Name.Equals("Output", StringComparison.CurrentCultureIgnoreCase) && node.HasChildNodes)
						{
							foreach (XmlNode msgNode in node.ChildNodes)
							{
								if (msgNode.Name.Equals("ErrorInfo", StringComparison.CurrentCultureIgnoreCase) && msgNode.HasChildNodes)
								{
									errorMsg = msgNode["Message"] != null ? "<pre>" + msgNode["Message"].InnerText : "";
									errorMsg += msgNode["StackTrace"] != null ? msgNode["StackTrace"].InnerText.Replace("\r", "").Replace("\n", "") : "";
									errorMsg += "</pre>";
									errorMsg = errorMsg == "<pre></pre>" ? "" : errorMsg;
								}
								else if (msgNode.Name.Equals("StdOut", StringComparison.CurrentCultureIgnoreCase))
								{
									descMsg += "<p class='description'>Description: " + msgNode.InnerText;
									descMsg += "</p>";
									descMsg = descMsg == "<p class='description'>Description: </p>" ? "" : descMsg;
								}
							}
						}
					}
					tc.StatusMessage = descMsg + errorMsg;
				}

				// get test details and fixture
				string testId = testResult.Attributes["testId"].InnerText;
				var testDefinition = _doc.SelectSingleNode("descendant::t:UnitTest[@id='" + testId + "']/t:TestMethod", _nsmgr);
				var className = testDefinition.Attributes["className"].InnerText;

				// get the test fixture details
				var testFixture = _report.TestFixtures.SingleOrDefault(f => f.Name.Equals(className, StringComparison.CurrentCultureIgnoreCase));
				if (testFixture == null)
				{
					testFixture = new TestSuite();
					testFixture.Name = className;

					_report.TestFixtures.Add(testFixture);
				}

				// update test fixture with details from the test
				testFixture.Duration += tc.Duration;
				testFixture.Status = ReportHelper.GetFixtureStatus(new List<Status> { testFixture.Status, tc.Status });
				testFixture.Tests.Add(tc);

				Console.Write("\r{0} tests processed...", ++testCount);
			}
		}
	}
}
