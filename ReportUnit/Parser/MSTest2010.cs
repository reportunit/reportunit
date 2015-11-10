using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Threading.Tasks;

using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

using ReportUnit.Model;
using ReportUnit.Utils;
using ReportUnit.Logging;

namespace ReportUnit.Parser
{
    internal class MSTest2010 : IParser
    {
        private string resultsFile;
        private XNamespace xns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        private Logger logger = Logger.GetLogger();

        public Report Parse(string resultsFile)
        {
            XDocument doc = XDocument.Load(resultsFile);

            Report report = new Report();

            report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
            report.TestRunner = TestRunner.MSTest2010;

            // run-info & environment values -> RunInfo
            var runInfo = CreateRunInfo(doc, report).Info;
            report.AddRunInfo(runInfo);

            // report counts
            var resultNodes = doc.Descendants(xns + "UnitTestResult");
            report.Total = resultNodes.Count();
            report.Passed = resultNodes.Where(x => x.Attribute("outcome").Value.Equals("Passed")).Count();
            report.Failed = resultNodes.Where(x => x.Attribute("outcome").Value.Equals("Failed")).Count();
            report.Inconclusive = resultNodes
                .Where(x => 
                    x.Attribute("outcome").Value.Equals("Inconclusive") 
                    || x.Attribute("outcome").Value.Equals("passedButRunAborted") 
                    || x.Attribute("outcome").Value.Equals("disconnected") 
                    || x.Attribute("outcome").Value.Equals("notRunnable") 
                    || x.Attribute("outcome").Value.Equals("warning") 
                    || x.Attribute("outcome").Value.Equals("pending"))
                .Count();
            report.Skipped = resultNodes.Where(x => x.Attribute("outcome").Value.Equals("NotExecuted")).Count();
            report.Errors = resultNodes
                .Where(x => 
                    x.Attribute("outcome").Value.Equals("Passed") 
                    || x.Attribute("outcome").Value.Equals("Aborted") 
                    || x.Attribute("outcome").Value.Equals("timeout"))
                .Count();

            // report duration
            XElement times = doc.Descendants(xns + "Times").First();
            report.StartTime = times.Attribute("start").Value;
            report.EndTime = times.Attribute("finish").Value;

            // ToDo: add fixtures + tests

            return report;
        }

        private RunInfo CreateRunInfo(XDocument doc, Report report)
        {
            // run-info & environment values -> RunInfo
            RunInfo runInfo = new RunInfo();

            runInfo.TestRunner = report.TestRunner;
            runInfo.Info.Add("TestRunner Version", "");
            runInfo.Info.Add("File", report.FileName);

            runInfo.Info.Add("Machine Name", doc.Descendants(xns + "UnitTestResult").First().Attribute("computerName").Value);
                        
            return runInfo;
        }
    }
}
