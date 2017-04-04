using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ReportUnit.Extensions;
using ReportUnit.Model;
using ReportUnit.Utils;

namespace ReportUnit.Parser
{
    internal class JUnit : IParser
    {
        public Report Parse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist.", filePath));
            }
            var singleSuite = false;

            var doc = XDocument.Load(filePath);

            var docRoot = doc.Root;

            if (docRoot == null)
            {
                throw new NullReferenceException("Could not read document root.");
            }

            var report = new Report
            {
                FileName = Path.GetFileNameWithoutExtension(filePath),
                TestRunner = TestRunner.JUnit
            };

            var suites = docRoot.Descendants("testsuite");
            
            // Lets avoid multiple enumerations of the collection.
            var suitesList = suites as XElement[] ?? suites.ToArray();
            if (suitesList.Any())
            {
                suitesList.AsParallel().ToList().ForEach(
                    ts =>
                    {
                        var testSuite = CreateTestSuite(ts);
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
                            var testSuite = CreateTestSuite(ts);
                            report.TestSuiteList.Add(testSuite);
                        });
                }
            }

            // instead of filling this info with stuff from XML, let's just use the data we already have.
            report.Total = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count);

            report.Failed =
                report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count(x => x.Status == Status.Failed));

            report.Errors = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count(x => x.Status == Status.Error));

            report.Skipped = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Count(x => x.Status == Status.Skipped));

            report.Duration = report.TestSuiteList.Sum(testSuite => testSuite.TestList.Sum(x => x.Duration));

            // if there's no <testsuites> root node this should be true.
            if (singleSuite)
            {
                report.StartTime = docRoot.GetNullableAttribute("timestamp");
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
        /// Creates the test suite element. 
        /// </summary>
        /// <param name="testSuiteElement"></param>
        /// <returns>The TestSuite object</returns>
        private TestSuite CreateTestSuite(XElement testSuiteElement)
        {
            var testSuite = new TestSuite();
            testSuite.Name = testSuiteElement.GetNullableAttribute("name");
            testSuite.StartTime = testSuiteElement.GetNullableAttribute("timestamp");
            
            // we will add host and test suite ID to description.
            var hostName = testSuiteElement.GetNullableAttribute("hostname");
            var id = testSuiteElement.GetNullableAttribute("id");
            if (hostName != string.Empty)
            {
                testSuite.Description = "Hostname: " + hostName;
            }

            if (id != string.Empty)
            {
                testSuite.Description += " Id: " + id;
            }

            testSuite.Description += GetTestSuiteProperties(testSuiteElement);

            var duration = testSuiteElement.GetNullableAttribute("time");
            if (duration != string.Empty)
            {
                var parseResult = Regex.Replace(duration, @"[^0-9.]", "");
                testSuite.Duration = double.Parse(parseResult);
            }

            testSuiteElement.Descendants("testcase").AsParallel().ToList().ForEach(
                tc =>
                {
                    var test = CreateTestCase(tc);

                    testSuite.TestList.Add(test);
                });

            testSuite.Status = ReportUtil.GetFixtureStatus(testSuite.TestList);
            return testSuite;
        }

        /// <summary>
        /// The create test case method.
        /// </summary>
        /// <param name="testCaseElement">The test case.</param>
        /// <returns>The Tests</returns>
        private Test CreateTestCase(XElement testCaseElement)
        {
            var test = new Test();
            test.Name = testCaseElement.GetNullableAttribute("name");

            var classCategory = testCaseElement.GetNullableAttribute("classname");
            if (classCategory != string.Empty)
            {
                test.CategoryList.Add(classCategory);
            }

            var failureElement = testCaseElement.Descendants("failure").FirstOrDefault();
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
                {
                    // we add the type attribute to the failure if it's present.
                    test.StatusMessage += " Type: " + typeAttribute;
                }
            }

            var errorElement = testCaseElement.Descendants("error").FirstOrDefault();

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

            // additional info that could come in XML file, we'll store it in the StatusMessage.
            var systemOutElement = testCaseElement.Descendants("system-out").FirstOrDefault();
            if (systemOutElement != null)
            {
                test.StatusMessage += " System-Out: " + systemOutElement.Value;
            }

            var systemErrElement = testCaseElement.Descendants("system-err").FirstOrDefault();
            if (systemErrElement != null)
            {
                test.StatusMessage += " System-Err: " + systemErrElement.Value;
            }

            return test;
        }

        /// <summary>
        /// The create run info.
        /// </summary>
        /// <param name="testSuite">
        /// The test Suite.
        /// </param>
        /// <returns>
        /// A string with all the properties in the test suite.
        /// </returns>
        private string GetTestSuiteProperties(XElement testSuite)
        {
            var stringBuilder = new StringBuilder();
            var propertyList = testSuite.Descendants("properties").Descendants("property");
            

            // cast to array to avoid multiple enumerations of collection.
            var xElements = propertyList as XElement[] ?? propertyList.ToArray();
                
            if (xElements.Any())
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Test Suite Properties:");
                    
                foreach (var prop in xElements)
                {
                    var propertyName = prop.GetNullableAttribute("name");
                    var propertyValue = prop.GetNullableAttribute("value");

                    if (propertyName != string.Empty && propertyValue != string.Empty)
                    {
                        stringBuilder.AppendLine(string.Format("{0}: {1}", propertyName, propertyValue));
                    }
                }
            }
            
            return stringBuilder.ToString();
        }
    }
}
