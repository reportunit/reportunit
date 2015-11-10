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
    internal class Gallio : IParser
    {
        private string resultsFile;
        private XNamespace xns = "http://www.gallio.org/";
        private Logger logger = Logger.GetLogger();

        public Report Parse(string resultsFile)
        {
            this.resultsFile = resultsFile;

            XDocument doc = XDocument.Load(resultsFile);

            Report report = new Report();

            report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
            report.AssemblyName = doc.Descendants(xns + "files").First().Descendants(xns + "file").First().Value;
            report.TestRunner = TestRunner.Gallio;

            // run-info & environment values -> RunInfo
            var runInfo = CreateRunInfo(doc, report).Info;
            report.AddRunInfo(runInfo);

            // test cases
            var tests = doc.Descendants(xns + "testStep")
                .Where(x => x.Attribute("isTestCase").Value.Equals("true", StringComparison.CurrentCultureIgnoreCase));

            // report counts
            var statistics = doc.Descendants(xns + "statistics").First();
            report.Total = tests.Count();
            report.Passed = Int32.Parse(statistics.Attribute("passedCount").Value);
            report.Failed = Int32.Parse(statistics.Attribute("failedCount").Value);
            report.Inconclusive = Int32.Parse(statistics.Attribute("inconclusiveCount").Value);
            report.Skipped = Int32.Parse(statistics.Attribute("skippedCount").Value);
            report.Errors = 0;

            // report duration
            XElement testPackageRun = doc.Descendants(xns + "testPackageRun").First();
            report.StartTime = testPackageRun.Attribute("startTime").Value;
            report.EndTime = testPackageRun.Attribute("endTime").Value;

            var suitesList = new List<string>();
            TestSuite testSuite = null;

            tests.AsParallel().ToList().ForEach(tc =>
            {
                var testSuiteName = tc.Attribute("fullName").Value;
                testSuiteName = testSuiteName.Contains('/') 
                    ? testSuiteName.Split('/')[testSuiteName.Split('/').Length - 2] 
                    : testSuiteName;

                if (!suitesList.Contains(testSuiteName))
                {
                    testSuite = new TestSuite();
                    testSuite.Name = testSuiteName;
                    testSuite.StartTime = tc.Parent.Attribute("startTime").Value;
                    testSuite.EndTime = tc.Parent.Attribute("endTime").Value;

                    report.TestSuiteList.Add(testSuite);

                    suitesList.Add(testSuiteName);
                }

                var test = new Model.Test();

                test.Name = tc.Attribute("name").Value;
                test.Status = StatusExtensions.ToStatus(tc.Parent.Descendants(xns + "outcome").First().Attribute("status").Value);

                // main a master list of all status
                // used to build the status filter in the view
                report.StatusList.Add(test.Status);

                var entry = tc.Descendants(xns + "entry");

                // description
                var description = entry != null 
                    ? entry.Where(x => x.Attribute("key").Value.Equals("Description")) 
                    : null;
                test.Description =  description != null && description.Count() > 0 
                    ? description.First().Value 
                    : "";

                // error and other status messages
                var ignoreReason = entry != null 
                    ? entry.Where(x => x.Attribute("key").Value.Equals("IgnoreReason")) 
                    : null;
                test.StatusMessage = ignoreReason != null && ignoreReason.Count() > 0 
                    ? ignoreReason.First().Value 
                    : "";

                var testLog = tc.Parent.Descendants(xns + "testLog");
                test.StatusMessage += testLog != null && testLog.Count() > 0 
                    ? testLog.First().Value 
                    : "";

                // assign categories
                var category = entry != null 
                    ? entry.Where(x => x.Attribute("key").Value.Equals("Category")) 
                    : null;
                if (category != null && category.Count() > 0)
                {
                    category.ToList().ForEach(s => 
                    {
                        string cat = s.Value;

                        test.CategoryList.Add(cat);
                        report.CategoryList.Add(cat);
                    });
                }

                testSuite.TestList.Add(test);
                testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);
            });

            return report;
        }

        private RunInfo CreateRunInfo(XDocument doc, Report report)
        {
            // run-info & environment values -> RunInfo
            RunInfo runInfo = new RunInfo();

            runInfo.TestRunner = report.TestRunner;
            runInfo.Info.Add("File", report.AssemblyName);

            var children = doc.Descendants(xns + "children").First();

            var testKind = children.Descendants(xns + "entry")
                .Where(x => x.Attribute("key").Value.Equals("TestKind", StringComparison.CurrentCultureIgnoreCase));
            var testKindValue = testKind != null && testKind.Count() > 0 
                ? testKind.First().Descendants(xns + "value").First().Value 
                : "";
            runInfo.Info.Add("TestKind", testKindValue);

            var codebase = children.Descendants(xns + "entry")
                .Where(x => x.Attribute("key").Value.Equals("CodeBase", StringComparison.CurrentCultureIgnoreCase));
            var codebaseValue = codebase != null && codebase.Count() > 0 
                ? codebase.First().Descendants(xns + "value").First().Value 
                : "";
            runInfo.Info.Add("CodeBase", codebaseValue);

            var framework = children.Descendants(xns + "entry")
                .Where(x => x.Attribute("key").Value.Equals("Framework", StringComparison.CurrentCultureIgnoreCase));
            var frameworkValue = framework != null && framework.Count() > 0 
                ? testKind.First().Descendants(xns + "value").First().Value 
                : "";
            runInfo.Info.Add("Framework", frameworkValue);

            var version = children.Descendants(xns + "entry")
                .Where(x => x.Attribute("key").Value.Equals("Version", StringComparison.CurrentCultureIgnoreCase));
            var versionValue = version != null && version.Count() > 0 
                ? testKind.First().Descendants(xns + "value").First().Value 
                : "";
            runInfo.Info.Add("Version", versionValue);

            return runInfo;
        }
    }
}
