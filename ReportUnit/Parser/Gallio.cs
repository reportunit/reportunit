using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;


namespace ReportUnit.Parser
{
	internal class Gallio : IParser
	{
		public string ResultsFile { get; private set; }
		private readonly XNamespace _xns = "http://www.gallio.org/";


		public Report Parse(string resultsFile)
		{
			ResultsFile = resultsFile;

			var doc = XDocument.Load(resultsFile);

			var report = new Report();

			report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
			report.AssemblyName = doc.Descendants(_xns + "files").First().Descendants(_xns + "file").First().Value;
			report.TestRunner = TestRunner.Gallio;

			// run-info & environment values -> RunInfo
			var runInfo = CreateRunInfo(doc, report).Info;
			report.AddRunInfo(runInfo);

			// test cases
			var tests = doc.Descendants(_xns + "testStep")
				.Where(x => x.Attribute("isTestCase").Value.Equals("true", StringComparison.CurrentCultureIgnoreCase));

			// report counts
			var statistics = doc.Descendants(_xns + "statistics").First();
			var xElements = tests as XElement[] ?? tests.ToArray();
			report.Total = xElements.Count();
			report.Passed = int.Parse(statistics.Attribute("passedCount").Value);
			report.Failed = int.Parse(statistics.Attribute("failedCount").Value);
			report.Inconclusive = int.Parse(statistics.Attribute("inconclusiveCount").Value);
			report.Skipped = int.Parse(statistics.Attribute("skippedCount").Value);
			report.Errors = 0;

			// report duration
			var testPackageRun = doc.Descendants(_xns + "testPackageRun").First();
			report.StartTime = testPackageRun.Attribute("startTime").Value;
			report.EndTime = testPackageRun.Attribute("endTime").Value;

			var suitesList = new List<string>();
			TestSuite testSuite = null;

			xElements.AsParallel().ToList().ForEach(tc =>
			{
				var testSuiteName = tc.Attribute("fullName").Value;
				testSuiteName = testSuiteName.Contains('/')
					? testSuiteName.Split('/')[testSuiteName.Split('/').Length - 2]
					: testSuiteName;

				if (!suitesList.Contains(testSuiteName))
				{
					if (tc.Parent != null)
						testSuite = new TestSuite
						{
							Name = testSuiteName,
							StartTime = tc.Parent.Attribute("startTime").Value,
							EndTime = tc.Parent.Attribute("endTime").Value
						};

					report.TestSuiteList.Add(testSuite);

					suitesList.Add(testSuiteName);
				}

				var test = new Test
				{
					Name = tc.Attribute("name").Value
				};

				if (tc.Parent != null)
				{
					test.Status = tc.Parent.Descendants(_xns + "outcome").First().Attribute("status").Value.ToStatus();

					// main a master list of all status
					// used to build the status filter in the view
					report.StatusList.Add(test.Status);

					var entry = tc.Descendants(_xns + "entry");

					// description
					var enumerable = entry as XElement[] ?? entry.ToArray();
					var description = enumerable.Where(x => x.Attribute("key").Value.Equals("Description"));
					var elements = description as XElement[] ?? description.ToArray();
					test.Description = elements.Any()
						? elements.First().Value
						: "";

					// error and other status messages
					var ignoreReason = enumerable.Where(x => x.Attribute("key").Value.Equals("IgnoreReason"));
					var reason = ignoreReason as XElement[] ?? ignoreReason.ToArray();
					test.StatusMessage = reason.Any()
						? reason.First().Value
						: "";

					if (tc.Parent != null)
					{
						var testLog = tc.Parent.Descendants(_xns + "testLog");
						var log = testLog as XElement[] ?? testLog.ToArray();
						test.StatusMessage += log.Any()
							? log.First().Value
							: "";
					}

					// assign categories
					var category = enumerable.Where(x => x.Attribute("key").Value.Equals("Category"));
					var xElements1 = category as XElement[] ?? category.ToArray();
					if (xElements1.Any())
					{
						xElements1.ToList().ForEach(s =>
						{
							var cat = s.Value;

							test.CategoryList.Add(cat);
							report.CategoryList.Add(cat);
						});
					}
				}

				if (testSuite == null) return;
				testSuite.TestList.Add(test);
				testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);
			});

			return report;
		}

		private RunInfo CreateRunInfo(XContainer doc, Report report)
		{
			// run-info & environment values -> RunInfo
			var runInfo = new RunInfo();

			runInfo.TestRunner = report.TestRunner;
			runInfo.Info.Add("File", report.AssemblyName);

			var children = doc.Descendants(_xns + "children").First();

			var testKind = children.Descendants(_xns + "entry")
				.Where(x => x.Attribute("key").Value.Equals("TestKind", StringComparison.CurrentCultureIgnoreCase));
			var xElements = testKind as XElement[] ?? testKind.ToArray();
			var testKindValue = xElements.Any()
				? xElements.First().Descendants(_xns + "value").First().Value
				: "";
			runInfo.Info.Add("TestKind", testKindValue);

			var codebase = children.Descendants(_xns + "entry")
				.Where(x => x.Attribute("key").Value.Equals("CodeBase", StringComparison.CurrentCultureIgnoreCase));
			var enumerable = codebase as XElement[] ?? codebase.ToArray();
			var codebaseValue = enumerable.Any()
				? enumerable.First().Descendants(_xns + "value").First().Value
				: "";
			runInfo.Info.Add("CodeBase", codebaseValue);

			var framework = children.Descendants(_xns + "entry")
				.Where(x => x.Attribute("key").Value.Equals("Framework", StringComparison.CurrentCultureIgnoreCase));
			var frameworkValue = framework.Any()
				? xElements.First().Descendants(_xns + "value").First().Value
				: "";
			runInfo.Info.Add("Framework", frameworkValue);

			var version = children.Descendants(_xns + "entry")
				.Where(x => x.Attribute("key").Value.Equals("Version", StringComparison.CurrentCultureIgnoreCase));
			var versionValue = version.Any()
				? xElements.First().Descendants(_xns + "value").First().Value
				: "";
			runInfo.Info.Add("Version", versionValue);

			return runInfo;
		}
	}
}
