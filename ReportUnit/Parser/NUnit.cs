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
    internal class NUnit : IParser
    {
        private string resultsFile;

        private Logger logger = Logger.GetLogger();

        public Report Parse(string resultsFile)
        {
            this.resultsFile = resultsFile;

            XDocument doc = XDocument.Load(resultsFile);

            Report report = new Report();

            report.FileName = Path.GetFileNameWithoutExtension(resultsFile);
            report.AssemblyName = doc.Root.Attribute("name").Value;
            report.TestRunner = TestRunner.NUnit;

            // run-info & environment values -> RunInfo
            var runInfo = CreateRunInfo(doc, report);
            report.AddRunInfo(runInfo.Info);

            // report counts
            report.Total = doc.Descendants("test-case").Count();

            report.Passed = 
                doc.Root.Attribute("passed") != null 
                    ? Int32.Parse(doc.Root.Attribute("passed").Value) 
                    : doc.Descendants("test-case").Where(x => x.Attribute("result").Value.Equals("success", StringComparison.CurrentCultureIgnoreCase)).Count();

            report.Failed = 
                doc.Root.Attribute("failed") != null 
                    ? Int32.Parse(doc.Root.Attribute("failed").Value) 
                    : Int32.Parse(doc.Root.Attribute("failures").Value);
            
            report.Errors = 
                doc.Root.Attribute("errors") != null 
                    ? Int32.Parse(doc.Root.Attribute("errors").Value) 
                    : 0;
            
            report.Inconclusive = 
                doc.Root.Attribute("inconclusive") != null 
                    ? Int32.Parse(doc.Root.Attribute("inconclusive").Value) 
                    : Int32.Parse(doc.Root.Attribute("inconclusive").Value);
            
            report.Skipped = 
                doc.Root.Attribute("skipped") != null 
                    ? Int32.Parse(doc.Root.Attribute("skipped").Value) 
                    : Int32.Parse(doc.Root.Attribute("skipped").Value);
            
            report.Skipped += 
                doc.Root.Attribute("ignored") != null 
                    ? Int32.Parse(doc.Root.Attribute("ignored").Value) 
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

            IEnumerable<XElement> suites = doc
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
                    String.IsNullOrEmpty(testSuite.StartTime) && ts.Attribute("time") != null 
                        ? ts.Attribute("time").Value 
                        : testSuite.StartTime; 

                testSuite.EndTime = 
                    ts.Attribute("end-time") != null 
                        ? ts.Attribute("end-time").Value 
                        : "";

                // any error messages or stack-trace
                testSuite.StatusMessage = 
                    ts.Element("failure") != null 
                        ? ts.Element("failure").Element("message").Value 
                        : "";

                // Test Cases
                ts.Descendants("test-case").AsParallel().ToList().ForEach(tc =>
                {
                    var test = new Model.Test();

                    test.Name = tc.Attribute("name").Value;
                    test.Status = StatusExtensions.ToStatus(tc.Attribute("result").Value);
                    
                    // main a master list of all status
                    // used to build the status filter in the view
                    report.StatusList.Add(test.Status);

                    // TestCase Time Info
                    test.StartTime = 
                        tc.Attribute("start-time") != null 
                            ? tc.Attribute("start-time").Value 
                            : "";
                    test.StartTime = 
                        String.IsNullOrEmpty(test.StartTime) && (tc.Attribute("time") != null) 
                            ? tc.Attribute("time").Value 
                            : test.StartTime;
                    test.EndTime = 
                        tc.Attribute("end-time") != null 
                            ? tc.Attribute("end-time").Value 
                            : "";

                    // description
                    var description = 
                        tc.Descendants("property")
                        .Where(c => c.Attribute("name").Value.Equals("Description", StringComparison.CurrentCultureIgnoreCase));
                    test.Description = 
                        description.Count() > 0 
                            ? description.ToArray()[0].Attribute("value").Value 
                            : "";

                    bool hasCategories = 
                        tc.Descendants("property")
                        .Where(c => c.Attribute("name").Value.Equals("Category", StringComparison.CurrentCultureIgnoreCase)).Count() > 0;

                    if (hasCategories) 
                    {
                        List<XElement> cats = tc
                            .Descendants("property")
                            .Where(c => c.Attribute("name").Value.Equals("Category", StringComparison.CurrentCultureIgnoreCase))
                            .ToList();
                                    
                        cats.ForEach(x => 
                        {
                            string cat = x.Attribute("value").Value;

                            test.CategoryList.Add(cat);
                            report.CategoryList.Add(cat);
                        });
                    }
                    
                    // error and other status messages
                    test.StatusMessage = 
                        tc.Element("failure") != null 
                            ? tc.Element("failure").Element("message").Value 
                            : "";
                    test.StatusMessage += 
                        tc.Element("failure") != null 
                            ? tc.Element("failure").Element("stack-trace") != null 
                                ? tc.Element("failure").Element("stack-trace").Value 
                                : "" 
                            : "";

                    testSuite.TestList.Add(test);
                });

                testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);

                report.TestSuiteList.Add(testSuite);
            });

            return report;
        }

        private RunInfo CreateRunInfo(XDocument doc, Report report)
        {
            RunInfo runInfo = new RunInfo();
            runInfo.TestRunner = report.TestRunner;

            XElement env = doc.Descendants("environment").First();
            runInfo.Info.Add("Test Results File", resultsFile);
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
        
        public NUnit() { }
    }
}
