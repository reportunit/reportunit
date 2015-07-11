using System;
using System.IO;
using System.Linq;
using System.Xml;
using ReportUnit.Support;
using ReportUnit.Layer;

namespace ReportUnit.Parser
{
    using Logging;

	internal class XUnitV2 : IParser
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

        /// <summary>
        /// Logger
        /// </summary>
        Logger logger = Logger.GetLogger();

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
			_report.RunInfo.TestRunner = TestRunner.XUnitV2;

			// get total count of tests from the input file
			_report.Total = _doc.GetElementsByTagName("test").Count;
			_report.AssemblyName = _doc.SelectNodes("//assembly")[0].Attributes["name"].InnerText;

			Console.WriteLine("[INFO] Number of tests: " + _report.Total);

			// only proceed if the test count is more than 0
			if (_report.Total >= 1)
			{
				Console.WriteLine("[INFO] Processing root and collection elements...");

				// pull values from XML source
				_report.Passed = _doc.SelectNodes(".//test[@result='Success' or @result='Passed']").Count;
				_report.Failed = _doc.SelectNodes(".//test[@result='Fail' or @result='Failed' or @result='Failure']").Count;
				_report.Inconclusive = _doc.SelectNodes(".//test[@result='Inconclusive' or @result='NotRunnable']").Count;
				_report.Skipped = _doc.SelectNodes(".//test[@result='Skipped' or @result='Ignored']").Count;
				_report.Errors = _doc.SelectNodes(".//test[@result='Error' or @result='Errored']").Count;

				try
				{
					double duration;
					if (double.TryParse(_doc.SelectNodes("//assembly")[0].Attributes["time"].InnerText, out duration))
					{
						_report.Duration = duration;
					}
				}
				catch
				{
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
				// try to parse the assembly node
				XmlNode assembly = _doc.GetElementsByTagName("assembly")[0];
				if (assembly != null && assembly.Attributes != null)
				{
					if (assembly.Attributes["environment"] != null) _report.RunInfo.Info.Add("Environment", assembly.Attributes["environment"].InnerText);
					_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
					if (assembly.Attributes["test-framework"] != null) _report.RunInfo.Info.Add("TestRunner Version", assembly.Attributes["test-framework"].InnerText);
				}
				else
				{
					_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
				}
			}
			catch (Exception ex)
			{
				_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
				Console.WriteLine("[ERROR] There was an error processing the _ASSEMBLY_ node: " + ex.Message);
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
			XmlNodeList collectionNodes = _doc.SelectNodes(".//collection");
			int testCount = 0;

			// run for each test collection
			foreach (XmlNode suite in collectionNodes)
			{
				var testSuite = new TestSuite();
				testSuite.Name = suite.Attributes["name"].InnerText.Replace("Test collection for", "").Replace(_fileNameWithoutExtension + ".", "").Trim();

				if (suite.Attributes["time"] != null)
				{
					double duration;
					if (double.TryParse(suite.Attributes["time"].InnerText, out duration))
					{
						testSuite.Duration = duration;
						testSuite.StartTime = duration.ToString();
					}
				}

				// check if the testSuite has any errors (ie error in the TestFixtureSetUp)
				/*var testSuiteFailureNode = suite.SelectSingleNode("failure");
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
				}*/


				// add each test of the test-fixture
				foreach (XmlNode testcase in suite.SelectNodes(".//test"))
				{
					errorMsg = descMsg = "";

					var tc = new Test();
					tc.Name = testcase.Attributes["name"].InnerText.Replace(_fileNameWithoutExtension + ".", "").Replace(testSuite.Name + ".", "");
					tc.Status = testcase.Attributes["result"].InnerText.AsStatus();

					if (testcase.Attributes["time"] != null)
					{
						try
						{
							Double d;
							var durationTimeSpan = testcase.Attributes["time"].InnerText;
							if (Double.TryParse(durationTimeSpan, out d))
							{
								tc.Duration = d;
							}
						}
						catch (Exception) { }
					}

					/*var categories = testcase.SelectNodes(".//property[@name='Category']");
					foreach (XmlNode category in categories)
					{
						tc.Categories.Add(category.Attributes["value"].InnerText);
					}*/

					var message = testcase.SelectSingleNode(".//message");

					if (message != null)
					{
						errorMsg = "<pre>";// + message.InnerText.Trim();
						errorMsg += testcase.SelectSingleNode(".//stack-trace") != null ? " -> " + testcase.SelectSingleNode(".//stack-trace").InnerText.Replace("\r", "") : "";
						errorMsg += "</pre>";
						errorMsg = errorMsg == "<pre></pre>" ? "" : errorMsg;
					}

					/*XmlNode desc = testcase.SelectSingleNode(".//property[@name='Description']");

					if (desc != null)
					{
						descMsg += "<p class='description'>Description: " + desc.Attributes["value"].InnerText.Trim();
						descMsg += "</p>";
						descMsg = descMsg == "<p class='description'>Description: </p>" ? "" : descMsg;
					}*/

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
