﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Model;
using ReportUnit.Utils;
using ReportUnit.Logging;

namespace ReportUnit.Parser
{
    internal class MSTest2010 : IParser
    {
        //private string resultsFile;
        private XNamespace xns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
        private Logger logger = Logger.GetLogger();

        public Report Parse(string resultsFile)
        {
            var doc = XDocument.Load(resultsFile);

            var report = new Report();

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
            var times = doc.Descendants(xns + "Times").First();
            report.StartTime = times.Attribute("start").Value;
            report.EndTime = times.Attribute("finish").Value;

            // ToDo: add fixtures + tests
            doc.Descendants(xns + "UnitTestResult").AsParallel().ToList().ForEach(tc =>
            {
                var test = new Model.Test();

                test.Name = tc.Attribute("testName").Value;
                test.Status = StatusExtensions.ToStatus(tc.Attribute("outcome").Value);

                // main a master list of all status
                // used to build the status filter in the view
                report.StatusList.Add(test.Status);

                // TestCase Time Info
                test.StartTime =
                    tc.Attribute("startTime") != null
                        ? tc.Attribute("startTime").Value
                        : "";
                test.EndTime =
                    tc.Attribute("endTime") != null
                        ? tc.Attribute("endTime").Value
                        : "";

                var timespan = Convert.ToDateTime(test.StartTime) - Convert.ToDateTime(test.EndTime);
                test.Duration = timespan.Milliseconds;

                // error and other status messages
                test.StatusMessage = tc.Element(xns + "Output") != null ? tc.Element(xns + "Output").Value.Trim() : "";

                var unitTestElement = doc.Descendants(xns + "UnitTest").FirstOrDefault(x => x.Attribute("name").Value.Equals(test.Name));

                if (unitTestElement != null)
                {
                    
                    var descriptionElement = unitTestElement.Element(xns + "Description");
                    if (descriptionElement != null)
                    {
                        test.Description = descriptionElement.Value;
                    }

                    var categories = (from testCategory in unitTestElement.Descendants(xns + "TestCategoryItem")
                                          select testCategory.Attributes("TestCategory").Select(x => x.Value).FirstOrDefault()).ToList();
                    
                    test.CategoryList = categories;
                    
                    
                    if (categories.Any())
                    {
                        foreach (var suiteName in categories)
                        {
                            AddTestToSuite(report, test, suiteName);
                        }
                    }
                    else
                    {
                        var suiteName = unitTestElement.Element(xns + "TestMethod").Attribute("className").Value;
                        AddTestToSuite(report, test, suiteName);
                    }
                }
            });

            //report.TestSuiteList = report.TestSuiteList.OrderBy(x => x.Name).ToList();
            return report;
        }

        private static void AddTestToSuite(Report report, Test test, string suiteName)
        {
            var testSuite = report.TestSuiteList.SingleOrDefault(t => t.Name.Equals(suiteName));

            if (testSuite == null)
            {
                testSuite = new TestSuite { Name = suiteName };
                report.TestSuiteList.Add(testSuite);
            }

            report.TestSuiteList = report.TestSuiteList.OrderBy(x => x.Status).ToList();
            testSuite.TestList.Add(test);
            testSuite.Duration += test.Duration;
            testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);
        }

        private RunInfo CreateRunInfo(XDocument doc, Report report)
        {
            // run-info & environment values -> RunInfo
            var runInfo = new RunInfo();

            runInfo.TestRunner = report.TestRunner;
            runInfo.Info.Add("TestRunner Version", "");
            runInfo.Info.Add("File", report.FileName);

            runInfo.Info.Add("Machine Name", doc.Descendants(xns + "UnitTestResult").First().Attribute("computerName").Value);

            return runInfo;
        }
    }
}
