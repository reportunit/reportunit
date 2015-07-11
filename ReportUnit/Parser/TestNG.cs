using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace ReportUnit.Parser
{
    using Layer;
    using Logging;
    using Support;

    internal class TestNG : IParser
    {
        /// <summary>
        /// XmlDocument instance
        /// </summary>
        private XmlDocument _doc;

        /// <summary>
        /// The input file from NUnit TestResult.xml
        /// </summary>
        private string _testResultFile = "";

        /// <summary>
        /// Usually evaluates to the assembly name. Used to clean up test names so its easier to read in the outputted html.
        /// </summary>
        private string _fileNameWithoutExtension;

        /// <summary>
        /// Contains test-suite level data to be passed to the Folder level report to build summary
        /// </summary>
        private Report _report;

        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger();

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

            // create a data instance to be passed to the folder level report
            _report = new Report();
            _report.FileName = this._testResultFile;
            _report.RunInfo.TestRunner = TestRunner.TestNG;

            // get total count of tests from the input file
            _report.Total = _doc.GetElementsByTagName("test-method").Count;
            _report.AssemblyName = "";

            logger.Info("Number of tests: " + _report.Total);

            // only proceed if the test count is more than 0
            if (_report.Total >= 1)
            {
                logger.Info("[Processing test method elements...");

                // pull values from XML source
                _report.Passed = _doc.SelectNodes(".//test-method[@status='PASS']").Count;
                _report.Failed = _doc.SelectNodes(".//test-method[@status='FAIL']").Count;
                _report.Inconclusive = 0;
                _report.Skipped = _doc.SelectNodes(".//test-method[@status='SKIP']").Count;
                _report.Errors = _doc.SelectNodes(".//test-method[@status='ERROR']").Count;

                try
                {
                    double duration;
                    if (double.TryParse(_doc.SelectNodes("//suite")[0].Attributes["duration-ms"].InnerText, out duration))
                    {
                        _report.Duration = duration;
                    }
                }
                catch { }

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

            try
            {
                DateTime lastModified = System.IO.File.GetLastWriteTime(_testResultFile);
                _report.RunInfo.Info.Add("Last Run", lastModified.ToString("d MMM yyyy HH:mm"));
            }
            catch (Exception) { }

            if (_report.Duration > 0) _report.RunInfo.Info.Add("Duration", string.Format("{0} ms", _report.Duration));
        }

        /// <summary>
        /// Processes the fixture level blocks
        /// Adds all tests to the output
        /// </summary>
        private void ProcessFixtureBlocks()
        {
            logger.Info("Building fixture blocks...");

            string errorMsg = null;
            XmlNodeList testSuiteNodes = _doc.SelectNodes("//test");
            int testCount = 0;

            // run for each test-suite
            foreach (XmlNode suite in testSuiteNodes)
            {
                var testSuite = new TestSuite();
                testSuite.Name = suite.Attributes["name"].InnerText.Replace(_fileNameWithoutExtension + ".", "").Trim();

                if (suite.Attributes["started-at"] != null && suite.Attributes["finished-at"] != null)
                {
                    var startTime = suite.Attributes["started-at"].InnerText.Replace("Z", "");
                    var endTime = suite.Attributes["finished-at"].InnerText.Replace("Z", "");

                    testSuite.StartTime = startTime;
                    testSuite.EndTime = endTime;
                    testSuite.Duration = DateTimeHelper.DifferenceInMilliseconds(startTime, endTime);
                }

                // add each test of the test-fixture
                foreach (XmlNode testcase in suite.SelectNodes(".//test-method"))
                {
                    errorMsg = "";

                    var tc = new Test();
                    tc.Name = testcase.Attributes["name"].InnerText.Replace("<", "[").Replace(">", "]").Replace(testSuite.Name + ".", "").Replace(_fileNameWithoutExtension + ".", "");

                    // figure out the status reslt of the test
                    if (testcase.Attributes["status"] != null)
                    {
                        tc.Status = testcase.Attributes["status"].InnerText.AsStatus();
                    }

                    if (testcase.Attributes["duration-ms"] != null)
                    {
                        try
                        {
                            TimeSpan d;
                            var durationTimeSpan = testcase.Attributes["duration-ms"].InnerText;
                            if (TimeSpan.TryParse(durationTimeSpan, out d))
                            {
                                tc.Duration = d.TotalMilliseconds;
                            }
                        }
                        catch (Exception) { }
                    }

                    var message = testcase.SelectSingleNode(".//message");

                    if (message != null)
                    {
                        errorMsg = "<pre>" + message.InnerText.Trim();
                        errorMsg += testcase.SelectSingleNode(".//message") != null ? " -> " + testcase.SelectSingleNode(".//message").InnerText.Replace("\r", "").Replace("\n", "") : "";
                        errorMsg += "</pre>";
                        errorMsg = errorMsg == "<pre></pre>" ? "" : errorMsg;
                    }

                    var stackTrace = testcase.SelectSingleNode(".//full-stacktrace");

                    if (stackTrace != null)
                    {
                        errorMsg += "<pre>" + message.InnerText.Trim();
                        errorMsg += testcase.SelectSingleNode(".//full-stacktrace") != null ? " -> " + testcase.SelectSingleNode(".//full-stacktrace").InnerText.Replace("\r", "").Replace("\n", "") : "";
                        errorMsg += "</pre>";
                        errorMsg = errorMsg == "<pre></pre>" ? "" : errorMsg;
                    }

                    tc.StatusMessage = errorMsg;
                    testSuite.Tests.Add(tc);

                    Console.Write("\r{0} tests processed...", ++testCount);
                }

                testSuite.Status = ReportHelper.GetFixtureStatus(testSuite.Tests);

                _report.TestFixtures.Add(testSuite);
            }
        }
    }
}
