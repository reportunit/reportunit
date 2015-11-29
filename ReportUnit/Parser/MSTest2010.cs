using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;

namespace ReportUnit.Parser
{
	internal class MsTest2010 : IParser
	{
		private readonly XNamespace _xns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";


		public Report Parse(string resultsFile)
		{
			var doc = XDocument.Load(resultsFile);

			var report = new Report
			{
				FileName = Path.GetFileNameWithoutExtension(resultsFile),
				TestRunner = TestRunner.MsTest2010
			};


			// run-info & environment values -> RunInfo
			var runInfo = CreateRunInfo(doc, report).Info;
			report.AddRunInfo(runInfo);

			// report counts
			var resultNodes = doc.Descendants(_xns + "UnitTestResult");
			var xElements = resultNodes as XElement[] ?? resultNodes.ToArray();
			report.Total = xElements.Count();
			report.Passed = xElements.Count(x => x.Attribute("outcome").Value.Equals("Passed"));
			report.Failed = xElements.Count(x => x.Attribute("outcome").Value.Equals("Failed"));
			report.Inconclusive = xElements.Count(x => x.Attribute("outcome").Value.Equals("Inconclusive")
													   || x.Attribute("outcome").Value.Equals("passedButRunAborted")
													   || x.Attribute("outcome").Value.Equals("disconnected")
													   || x.Attribute("outcome").Value.Equals("notRunnable")
													   || x.Attribute("outcome").Value.Equals("warning")
													   || x.Attribute("outcome").Value.Equals("pending"));
			report.Skipped = xElements.Count(x => x.Attribute("outcome").Value.Equals("NotExecuted"));
			report.Errors = xElements.Count(x => x.Attribute("outcome").Value.Equals("Passed")
												 || x.Attribute("outcome").Value.Equals("Aborted")
												 || x.Attribute("outcome").Value.Equals("timeout"));

			// report duration
			var times = doc.Descendants(_xns + "Times").First();
			report.StartTime = times.Attribute("start").Value;
			report.EndTime = times.Attribute("finish").Value;

			// ToDo: add fixtures + tests

			return report;
		}

		private RunInfo CreateRunInfo(XContainer doc, Report report)
		{
			// run-info & environment values -> RunInfo
			var runInfo = new RunInfo { TestRunner = report.TestRunner };

			runInfo.Info.Add("TestRunner Version", "");
			runInfo.Info.Add("File", report.FileName);

			runInfo.Info.Add("Machine Name", doc.Descendants(_xns + "UnitTestResult").First().Attribute("computerName").Value);

			return runInfo;
		}
	}
}
