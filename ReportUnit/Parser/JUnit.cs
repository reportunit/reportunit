namespace ReportUnit.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using ReportUnit.Extensions;
    using ReportUnit.Model;
    using ReportUnit.Utils;

    internal class JUnit : IParser
    {
        public Report Parse(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist.", filePath));
            }
            bool singleSuite = false;

            XDocument doc = XDocument.Load(filePath);

            var docroot = doc.Root;

            if (docroot == null)
            {
                throw new NullReferenceException(@"Could not read document root.");
            }

            var report = new Report();
            report.FileName = Path.GetFileNameWithoutExtension(filePath);
            report.TestRunner = TestRunner.JUnit;

            var runInfo = this.CreateRunInfo(doc, report);
            if (runInfo != null)
            {
                report.AddRunInfo(runInfo);
            }

            IEnumerable<XElement> suites = docroot.Descendants("testsuite");

            var suitesList = suites as XElement[] ?? suites.ToArray();
            if (suitesList.Any())
            {
                suitesList.AsParallel().ToList().ForEach(
                    ts =>
                        {
                            var testSuite = new TestSuite();
                            testSuite.Name = ts.GetNullableAttribute("name");
                            testSuite.StartTime = ts.GetNullableAttribute("timestamp");

                            // we will add host and test suite ID to description.
                            var hostName = ts.GetNullableAttribute("hostname");
                            var id = ts.GetNullableAttribute("id");
                            if (hostName != string.Empty)
                            {
                                testSuite.Description = "Hostname: " + hostName;
                            }

                            if (id != string.Empty)
                            {
                                testSuite.Description += " Id: " + id;
                            }

                            var duration = ts.GetNullableAttribute("time");
                            if (duration != string.Empty)
                            {
                                var parseResult = Regex.Replace(duration, @"[^0-9.]", "");
                                testSuite.Duration = double.Parse(parseResult);
                            }
                            
                            ts.Descendants("testcase").AsParallel().ToList().ForEach(
                                tc =>
                                    {
                                        var test = new Test();
                                        test.Name = tc.GetNullableAttribute("name");

                                        var classCategory = tc.GetNullableAttribute("classname");
                                        if (classCategory != string.Empty)
                                        {
                                            test.CategoryList.Add(classCategory);
                                        }

                                        var failureElement = tc.Descendants("failure").FirstOrDefault();
                                        if (failureElement == null)
                                        {
                                            test.Status = Status.Passed;
                                        }
                                        else
                                        {
                                            test.Status = Status.Failed;
                                            test.StatusMessage = failureElement.Value;
                                            var typeAttribute = failureElement.GetNullableAttribute("type");
                                            if (typeAttribute != string.Empty)
                                                // we add the type attribute to the failure if it's present.
                                            {
                                                test.StatusMessage += " Type: " + typeAttribute;
                                            }
                                        }

                                        var errorElement = tc.Descendants("error").FirstOrDefault();

                                        if (errorElement != null)
                                        {
                                            // we overwrite the status because it means that there was an error.
                                            test.Status = Status.Error;
                                            test.StatusMessage += errorElement.Value;
                                            // we add the type attribute if it's there.
                                            var typeAttribute = errorElement.GetNullableAttribute("type");
                                            if (typeAttribute != string.Empty)
                                            {
                                                test.StatusMessage += " Type: " + typeAttribute;
                                            }
                                        }

                                        var systemOutElement = tc.Descendants("system-out").FirstOrDefault();
                                        if (systemOutElement != null)
                                        {
                                            test.StatusMessage += " System-Out: " + systemOutElement.Value;
                                        }

                                        var systemErrElement = tc.Descendants("system-err").FirstOrDefault();
                                        if (systemErrElement != null)
                                        {
                                            test.StatusMessage += " System-Err: " + systemErrElement.Value;
                                        }

                                        testSuite.TestList.Add(test);
                                    });

                            testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);

                            report.TestSuiteList.Add(testSuite);
                        });
            }
            else
            {
                // some junit reports only contain one suite. so we try to get the results from that
                if (doc.Descendants("testsuite") != null)
                {
                    singleSuite = true;
                    var testSuites = doc.Descendants("testsuite");

                    testSuites.AsParallel().ToList().ForEach(
                        ts =>
                        {
                            var testSuite = new TestSuite();
                            testSuite.Name = ts.GetNullableAttribute("name");
                            testSuite.StartTime = ts.GetNullableAttribute("timestamp");

                            // we will add host and test suite ID to description.
                            var hostName = ts.GetNullableAttribute("hostname");
                            var id = ts.GetNullableAttribute("id");
                            if (hostName != string.Empty)
                            {
                                testSuite.Description = "Hostname: " + hostName;
                            }

                            if (id != string.Empty)
                            {
                                testSuite.Description += " Id: " + id;
                            }

                            var duration = ts.GetNullableAttribute("time");
                            testSuite.Duration = duration == string.Empty ? 0 : double.Parse(duration);

                            ts.Descendants("testcase").AsParallel().ToList().ForEach(
                                tc =>
                                {
                                    var test = new Test();
                                    test.Name = tc.GetNullableAttribute("name");

                                    var classCategory = tc.GetNullableAttribute("classname");
                                    if (classCategory != string.Empty)
                                    {
                                        test.CategoryList.Add(classCategory);
                                    }

                                    var failureElement = tc.Descendants("failure").FirstOrDefault();
                                    if (failureElement == null)
                                    {
                                        test.Status = Status.Passed;
                                    }
                                    else
                                    {
                                        test.Status = Status.Failed;
                                        test.StatusMessage = failureElement.Value;
                                        var typeAttribute = failureElement.GetNullableAttribute("type");
                                        if (typeAttribute != string.Empty)
                                        // we add the type attribute to the failure if it's present to get more info.
                                        {
                                            test.StatusMessage += " Type: " + typeAttribute;
                                        }
                                    }

                                    var errorElement = tc.Descendants("error").FirstOrDefault();

                                    if (errorElement != null)
                                    {
                                        // we overwrite the status because it means that there was an error.
                                        test.Status = Status.Error;
                                        test.StatusMessage += errorElement.Value;
                                        // we add the type attribute if it's there.
                                        var typeAttribute = errorElement.GetNullableAttribute("type");
                                        if (typeAttribute != string.Empty)
                                        {
                                            test.StatusMessage += " Type: " + typeAttribute;
                                        }
                                    }

                                    var systemOutElement = tc.Descendants("system-out").FirstOrDefault();
                                    if (systemOutElement != null)
                                    {
                                        test.StatusMessage += " System-Out: " + systemOutElement.Value;
                                    }

                                    var systemErrElement = tc.Descendants("system-err").FirstOrDefault();
                                    if (systemErrElement != null)
                                    {
                                        test.StatusMessage += " System-Err: " + systemErrElement.Value;
                                    }

                                    testSuite.TestList.Add(test);
                                });

                            testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);

                            report.TestSuiteList.Add(testSuite);
                        });
                }
            }

            report.Total = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count);

            report.Failed =
                report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count(x => x.Status == Status.Failed));

            report.Errors = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count(x => x.Status == Status.Error));

            report.Skipped = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count(x => x.Status == Status.Skipped));

            report.Duration = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Sum(x => x.Duration));

            if (singleSuite)
            {
                report.StartTime = docroot.Attribute("timestamp") != null
                                       ? docroot.Attribute("timestamp").Value
                                       : string.Empty;
            }
            else
            {
                report.StartTime = report.TestSuiteList.First().StartTime;
            }
            
            var testSuitePackageInfo =
                doc.Descendants("testsuites")
                    .Where(x => x.Attribute("failures") != null && x.Attribute("package") != null);

            // avoid multiple enumerations of IEnumerable by casting to array.
            var suitePackageInfo = testSuitePackageInfo as XElement[] ?? testSuitePackageInfo.ToArray();

            report.StatusMessage = suitePackageInfo.Any() ? suitePackageInfo.First().Value : string.Empty;
            report.CategoryList.Sort();

            return report;
        }

        /// <summary>
        /// The create run info.
        /// </summary>
        /// <param name="doc">
        /// The doc.
        /// </param>
        /// <param name="report">
        /// The report.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        private Dictionary<string, string> CreateRunInfo(XDocument doc, Report report)
        {
            return null;
            //            throw new System.NotImplementedException();
        }
    }
}
