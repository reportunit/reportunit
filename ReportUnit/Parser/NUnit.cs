using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parser
{
    internal class NUnit : IParser
    {
        private Logger logger = Logger.GetLogger();
        private string resultsFile;

        public Report Parse(string resultsFile)
        {
            this.resultsFile = resultsFile;

            var doc = XDocument.Load(resultsFile);

            var report = new Report();

            report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
            report.AssemblyName = doc.Root.Attribute("name") != null ? doc.Root.Attribute("name").Value : null;
            report.TestRunner = TestRunner.NUnit;

            // run-info & environment values -> RunInfo
            var runInfo = CreateRunInfo(doc, report);
            if (runInfo != null)
                report.AddRunInfo(runInfo.Info);

            // report counts
            report.Total = doc.Descendants("test-case").Count();

            report.Passed =
                doc.Root.Attribute("passed") != null
                    ? int.Parse(doc.Root.Attribute("passed").Value)
                    : doc.Descendants("test-case")
                        .Where(
                            x =>
                                x.Attribute("result").Value.Equals("success", StringComparison.CurrentCultureIgnoreCase))
                        .Count();

            report.Failed =
                doc.Root.Attribute("failed") != null
                    ? int.Parse(doc.Root.Attribute("failed").Value)
                    : int.Parse(doc.Root.Attribute("failures").Value);

            report.Errors =
                doc.Root.Attribute("errors") != null
                    ? int.Parse(doc.Root.Attribute("errors").Value)
                    : 0;

            report.Inconclusive =
                doc.Root.Attribute("inconclusive") != null
                    ? int.Parse(doc.Root.Attribute("inconclusive").Value)
                    : int.Parse(doc.Root.Attribute("inconclusive").Value);

            report.Skipped =
                doc.Root.Attribute("skipped") != null
                    ? int.Parse(doc.Root.Attribute("skipped").Value)
                    : int.Parse(doc.Root.Attribute("skipped").Value);

            report.Skipped +=
                doc.Root.Attribute("ignored") != null
                    ? int.Parse(doc.Root.Attribute("ignored").Value)
                    : 0;

            // report duration
            report.StartTime =
                doc.Root.Attribute("start-time") != null
                    ? doc.Root.Attribute("start-time").Value
                    : doc.Root.Attribute("date").Value + " " + doc.Root.Attribute("time").Value;

            report.EndTime =
                doc.Root.Attribute("end-time") != null
                    ? doc.Root.Attribute("end-time").Value
                    : "";

            // report status messages
            var testSuiteTypeAssembly = doc.Descendants("test-suite")
                .Where(x => x.Attribute("result").Value.Equals("Failed") && x.Attribute("type").Value.Equals("Assembly"));
            report.StatusMessage = testSuiteTypeAssembly != null && testSuiteTypeAssembly.Count() > 0
                ? testSuiteTypeAssembly.First().Value
                : "";

            var suites = doc
                .Descendants("test-suite")
                .Where(x => x.Attribute("type").Value.Equals("TestFixture", StringComparison.CurrentCultureIgnoreCase));

            suites.AsParallel().ToList().ForEach(ts =>
            {
                var testSuite = new TestSuite();
                testSuite.Name = ts.Attribute("name").Value;

                // Suite Time Info
                testSuite.StartTime =
                    ts.Attribute("start-time") != null
                        ? ts.Attribute("start-time").Value
                        : string.Empty;

                testSuite.StartTime =
                    string.IsNullOrEmpty(testSuite.StartTime) && ts.Attribute("time") != null
                        ? ts.Attribute("time").Value
                        : testSuite.StartTime;

                testSuite.EndTime =
                    ts.Attribute("end-time") != null
                        ? ts.Attribute("end-time").Value
                        : "";

                // any error messages and/or stack-trace
                var failure = ts.Element("failure");
                if (failure != null)
                {
                    var message = failure.Element("message");
                    if (message != null)
                        testSuite.StatusMessage = message.Value;

                    var stackTrace = failure.Element("stack-trace");
                    if (stackTrace != null && !string.IsNullOrWhiteSpace(stackTrace.Value))
                        testSuite.StatusMessage = string.Format(
                            "{0}\n\nStack trace:\n{1}", testSuite.StatusMessage, stackTrace.Value);
                }

                var output = ts.Element("output")?.Value;
                if (!string.IsNullOrWhiteSpace(output))
                    testSuite.StatusMessage += $"\n\nOutput:\n" + output;

                // get test suite level categories
                var suiteCategories = GetCategories(ts, false);

                // Test Cases
                ts.Descendants("test-case").AsParallel().ToList().ForEach(tc =>
                {
                    var test = new Test();

                    test.Name = tc.Attribute("name").Value;
                    test.Status = tc.Attribute("result").Value.ToStatus();

                    // main a master list of all status
                    // used to build the status filter in the view
                    report.StatusList.Add(test.Status);

                    // TestCase Time Info
                    test.StartTime =
                        tc.Attribute("start-time") != null
                            ? tc.Attribute("start-time").Value
                            : "";
                    test.StartTime =
                        string.IsNullOrEmpty(test.StartTime) && tc.Attribute("time") != null
                            ? tc.Attribute("time").Value
                            : test.StartTime;
                    test.EndTime =
                        tc.Attribute("end-time") != null
                            ? tc.Attribute("end-time").Value
                            : "";

                    // description
                    var description =
                        tc.Descendants("property")
                            .Where(
                                c =>
                                    c.Attribute("name")
                                        .Value.Equals("Description", StringComparison.CurrentCultureIgnoreCase));
                    test.Description =
                        description.Count() > 0
                            ? description.ToArray()[0].Attribute("value").Value
                            : "";

                    // get test case level categories
                    var categories = GetCategories(tc, true);

                    // if this is a parameterized test, get the categories from the parent test-suite
                    var parameterizedTestElement = tc
                        .Ancestors("test-suite").ToList()
                        .Where(
                            x =>
                                x.Attribute("type")
                                    .Value.Equals("ParameterizedTest", StringComparison.CurrentCultureIgnoreCase))
                        .FirstOrDefault();

                    if (null != parameterizedTestElement)
                    {
                        var paramCategories = GetCategories(parameterizedTestElement, false);
                        categories.UnionWith(paramCategories);
                    }

                    //Merge test level categories with suite level categories and add to test and report
                    categories.UnionWith(suiteCategories);
                    test.CategoryList.AddRange(categories);
                    report.CategoryList.AddRange(categories);


                    // error and other status messages
                    test.StatusMessage =
                        tc.Element("failure") != null
                            ? tc.Element("failure").Element("message").Value.Trim()
                            : "";
                    test.StatusMessage +=
                        tc.Element("failure") != null
                            ? tc.Element("failure").Element("stack-trace") != null
                                ? tc.Element("failure").Element("stack-trace").Value.Trim()
                                : ""
                            : "";

                    test.StatusMessage += tc.Element("reason") != null &&
                                          tc.Element("reason").Element("message") != null
                        ? tc.Element("reason").Element("message").Value.Trim()
                        : "";

                    // add NUnit console output to the status message
                    test.StatusMessage += tc.Element("output") != null
                        ? tc.Element("output").Value.Trim()
                        : "";

                    testSuite.TestList.Add(test);
                });

                testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);

                report.TestSuiteList.Add(testSuite);
            });

            //Sort category list so it's in alphabetical order
            report.CategoryList.Sort();

            return report;
        }

        /// <summary>
        ///     Returns categories for the direct children or all descendents of an XElement
        /// </summary>
        /// <param name="elem">XElement to parse</param>
        /// <param name="allDescendents">If true, return all descendent categories.  If false, only direct children</param>
        /// <returns></returns>
        private HashSet<string> GetCategories(XElement elem, bool allDescendents)
        {
            //Define which function to use
            var parser = allDescendents
                ? ((e, s) => e.Descendants(s))
                : new Func<XElement, string, IEnumerable<XElement>>((e, s) => e.Elements(s));

            //Grab unique categories
            var categories = new HashSet<string>();
            var hasCategories = parser(elem, "categories").Any();
            if (hasCategories)
            {
                var cats = parser(elem, "categories").Elements("category").ToList();

                cats.ForEach(x =>
                {
                    var cat = x.Attribute("name").Value;
                    categories.Add(cat);
                });
            }

            return categories;
        }

        private RunInfo CreateRunInfo(XDocument doc, Report report)
        {
            if (doc.Element("environment") == null)
                return null;

            var runInfo = new RunInfo();
            runInfo.TestRunner = report.TestRunner;

            var env = doc.Descendants("environment").First();
            runInfo.Info.Add("Test Results File", resultsFile);
            if (env.Attribute("nunit-version") != null)
                runInfo.Info.Add("NUnit Version", env.Attribute("nunit-version").Value);
            runInfo.Info.Add("Assembly Name", report.AssemblyName);
            runInfo.Info.Add("OS Version", env.Attribute("os-version").Value);
            runInfo.Info.Add("Platform", env.Attribute("platform").Value);
            runInfo.Info.Add("CLR Version", env.Attribute("clr-version").Value);
            runInfo.Info.Add("Machine Name", env.Attribute("machine-name").Value);
            runInfo.Info.Add("User", env.Attribute("user").Value);
            runInfo.Info.Add("User Domain", env.Attribute("user-domain").Value);

            return runInfo;
        }
    }
}