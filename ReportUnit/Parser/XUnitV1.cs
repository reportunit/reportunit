using System;
using System.IO;
using System.Linq;
using System.Xml;
using ReportUnit.Support;
using ReportUnit.Layer;

namespace ReportUnit.Parser
{
    using Logging;

	internal class XUnitV1 : IParser
	{
		/// <summary>
		/// XmlDocument instance
		/// </summary>
		private XmlDocument _doc;

		/// <summary>
		/// The input file from Xunit TestResult.xml
		/// </summary>
		private string _testResultFile = "";

		/// <summary>
		/// Usually evaluates to the assembly name. Used to clean up test names so its easier to read in the outputted html.
		/// </summary>
		private string _fileNameWithoutExtension;

		/// <summary>
		/// The path the assembly file was originally built to
		/// </summary>
		private string _assemblyFilePath;

		/// <summary>
		/// The folder the file was originally built to
		/// </summary>
		private string _assemblyFolder;

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
			_assemblyFilePath = (_doc.SelectNodes("//assembly") != null && _doc.SelectNodes("//assembly").Count > 0) ? _doc.SelectNodes("//assembly")[0].Attributes["name"].InnerText : _testResultFile;
			_assemblyFolder = _assemblyFilePath.Replace(Path.GetFileName(_assemblyFilePath), "");


			// create a data instance to be passed to the folder level report
			_report = new Report();
			_report.FileName = this._testResultFile;
			_report.RunInfo.TestRunner = TestRunner.XUnitV1;

			// get total count of tests from the input file
			_report.Total = _doc.GetElementsByTagName("test").Count;
			_report.AssemblyName = Path.GetFileName(_assemblyFilePath);

			Console.WriteLine("[INFO] Number of tests: " + _report.Total);

			// only proceed if the test count is more than 0
			if (_report.Total >= 1)
			{
				Console.WriteLine("[INFO] Processing root and collection elements...");

				// pull values from XML source
				_report.Passed = _doc.SelectNodes(".//test[@result='Success' or @result='Passed' or @result='Pass']").Count;
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
			if (_report.Duration > 0) _report.RunInfo.Info.Add("Duration", string.Format("{0} ms", _report.Duration));


			try
			{
				// try to parse the assembly node
				XmlNode assembly = _doc.GetElementsByTagName("assembly")[0];
				ProcessLastRun(assembly);
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
				ProcessLastRun(null);
				_report.RunInfo.Info.Add("TestRunner", _report.RunInfo.TestRunner.ToString());
				Console.WriteLine("[ERROR] There was an error processing the _ASSEMBLY_ node: " + ex.Message);
			}

		}
		private void ProcessLastRun(XmlNode assembly)
		{
			string lastRun = null;
			// try to parse the assembly node
			if (assembly != null && assembly.Attributes != null)
			{
				if (assembly.Attributes["run-date"] != null && assembly.Attributes["run-time"] != null)
				{
					lastRun = string.Format("{0} {1}", assembly.Attributes["run-time"].InnerText, assembly.Attributes["run-date"].InnerText);
				}
			}
			if (string.IsNullOrWhiteSpace(lastRun))
			{
				lastRun = System.IO.File.GetLastWriteTime(_testResultFile).ToString("d MMM yyyy HH:mm");
			}


			_report.RunInfo.Info.Add("Last Run", lastRun);
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
			foreach (XmlNode colNode in collectionNodes)
			{
				// add each test of the test-fixture
				foreach (XmlNode testCaseNode in colNode.SelectNodes(".//test"))
				{
					var testCollection = GetCollectionForTest(colNode, testCaseNode);

					errorMsg = descMsg = "";

					var tc = new Test();
					tc.Name = testCaseNode.Attributes["method"] != null ? testCaseNode.Attributes["method"].InnerText : testCaseNode.Attributes["name"].InnerText.Replace(testCollection.Name + ".", "");
					tc.Status = testCaseNode.Attributes["result"].InnerText.AsStatus();

					if (testCaseNode.Attributes["time"] != null)
					{
						try
						{
							Double d;
							var durationTimeSpan = testCaseNode.Attributes["time"].InnerText;
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

					var message = testCaseNode.SelectSingleNode(".//message");

					if (message != null)
					{
						errorMsg = "<pre>";// + message.InnerText.Trim();
						errorMsg += testCaseNode.SelectSingleNode(".//stack-trace") != null ? " -> " + testCaseNode.SelectSingleNode(".//stack-trace").InnerText.Replace("\r", "") : "";
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
					testCollection.Tests.Add(tc);

					Console.Write("\r{0} tests processed...", ++testCount);
				}
			}

			// update status of all the collections
			foreach (var testCollection in _report.TestFixtures)
			{
				testCollection.Status = ReportHelper.GetFixtureStatus(testCollection.Tests);
			}
		}

		private TestSuite GetCollectionForTest(XmlNode colNode, XmlNode testCaseNode)
		{
			// work otu the collection name
			String collectionName = null;

			if (testCaseNode.Attributes["type"] != null)
			{
				collectionName = testCaseNode.Attributes["type"].InnerText.Replace(_fileNameWithoutExtension + ".", "");
			}
			if (string.IsNullOrWhiteSpace(collectionName) && colNode.Attributes["name"] != null)
			{
				collectionName = colNode.Attributes["name"].InnerText
					.Replace("Test collection for", "")
					.Replace("xUnit.net v1 Tests for", "")
					.Replace(_assemblyFolder, "")
					.Trim();
			}
			if (string.IsNullOrWhiteSpace(collectionName))
			{
				collectionName = _fileNameWithoutExtension;
			}

			// check if the collection exists
			TestSuite testCollection = _report.TestFixtures.FirstOrDefault(c => String.Equals(c.Name, collectionName));
			if (testCollection == null)
			{
				testCollection = new TestSuite();
				testCollection.Name = collectionName;

				_report.TestFixtures.Add(testCollection);
			}


			if (colNode.Attributes != null && colNode.Attributes.Count > 0)
			{
				// in xunit v1 xml reports - often have only one collection 
				testCollection = _report.TestFixtures.FirstOrDefault(tf => string.IsNullOrWhiteSpace(tf.Name)) ?? new TestSuite();

				

				if (colNode.Attributes["time"] != null)
				{
					double duration;
					if (double.TryParse(colNode.Attributes["time"].InnerText, out duration))
					{
						testCollection.Duration = duration;
						testCollection.StartTime = duration.ToString();
					}
				}
			}

			return testCollection;
		}
	}
}
