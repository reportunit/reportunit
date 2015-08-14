namespace ReportUnit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Design;
    using Layer;
    using Logging;
    using Parser;
    using Support;    

    internal class ReportBuilder
    {
        private Theme theme;
        private Source source;

        /// <summary>
        /// FileParser for the type of test results to be parsed
        /// </summary>
        private IParser testFileParser;

        /// <summary>
        /// Logger
        /// </summary>
        private static Logger logger = Logger.GetLogger();

        /// <summary>
        /// Flag for file-level (single) report to add a class for BODY to disable sidenav
        /// This is switched off by default for a folder-level report
        /// For a test-suite report created standalone, this needs to be switched on
        /// Examples:
        ///     reportunit "path-to-folder"
        ///     reportunit "path-to-folder" "output-folder"
        ///         BEHAVIOR: This flag will be FALSE
        ///     reportunit "input.xml" "output.html"
        ///         BEHAVIOR: This flag will be TRUE
        /// </summary>
        private bool isSingle = false;

        public void FolderReport(string resultsDirectory, string outputDirectory)
        {
            List<string> allFiles = Directory.GetFiles(resultsDirectory, "*.*", SearchOption.TopDirectoryOnly).ToList();
            var links = new List<string>();
            string outputFile;

            // if no files, end process
            if (allFiles.Count == 0)
            {
                logger.Info("No XML files were found in the given location. Exiting..");
                return;
            }

            string html = HTML.Folder.Base;
            source = new Source();

            // force link to always point to the existing folder
            links.Add("./Index.html");

            foreach (string file in allFiles) 
            {
                ///Console.WriteLine("\n" + new string('-', 9));
                Console.WriteLine("");

                outputFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(file) + ".html");
                Report reportData = BuildReport(file, outputFile);

                if (reportData != null)
                {
                    // force link to always point to the existing folder
                    links.Add("./" + Path.GetFileNameWithoutExtension(file) + ".html");

                    int passed = reportData.Passed;
                    int failed = reportData.Failed + reportData.Errors;
                    int other = reportData.Inconclusive + reportData.Skipped;

                    string runResult;

                    if (reportData.Status == Status.Inconclusive || reportData.Status == Status.Skipped || reportData.Status == Status.Unknown)
                    {
                        runResult = "other";
                    }
					else if (reportData.Status == Status.Error || reportData.Status == Status.Failed)
					{
						runResult = Status.Failed.ToString();
					}
					else
                    {
                        runResult = reportData.Status.ToString();
                    }

                    html = html.Replace(ReportHelper.MarkupFlag("insertResult"), HTML.Folder.Row)
                                .Replace(ReportHelper.MarkupFlag("fullFilename"), Path.GetFileNameWithoutExtension(file) + ".html")
                                .Replace(ReportHelper.MarkupFlag("filename"), Path.GetFileNameWithoutExtension(file))
                                .Replace(ReportHelper.MarkupFlag("assembly"), Path.GetFileName(reportData.AssemblyName))
                                .Replace(ReportHelper.MarkupFlag("runresult"), runResult.ToLower())
                                .Replace(ReportHelper.MarkupFlag("totalTests"), reportData.Total.ToString())
                                .Replace(ReportHelper.MarkupFlag("totalPassed"), passed.ToString())
                                .Replace(ReportHelper.MarkupFlag("totalFailed"), failed.ToString())
                                .Replace(ReportHelper.MarkupFlag("allOtherTests"), other.ToString());

                    if (reportData.Total > 0)
                    {
                        html = html.Replace(ReportHelper.MarkupFlag("passedPercentage"), (Convert.ToInt32(passed) * 100 / Convert.ToInt32(reportData.Total)).ToString())
                                .Replace(ReportHelper.MarkupFlag("failedPercentage"), (Convert.ToInt32(failed) * 100 / Convert.ToInt32(reportData.Total)).ToString())
                                .Replace(ReportHelper.MarkupFlag("othersPercentage"), (Convert.ToInt32(other) * 100 / Convert.ToInt32(reportData.Total)).ToString());
                    }
                    else
                    {
                        html = html.Replace(ReportHelper.MarkupFlag("passedPercentage"), "0")
                                .Replace(ReportHelper.MarkupFlag("failedPercentage"), "0")
                                .Replace(ReportHelper.MarkupFlag("othersPercentage"), "0");
                    }
                }
            }

            string navLinks = "";
            foreach (string link in links)
            {
                navLinks += HTML.Folder.NavLink
                        .Replace(ReportHelper.MarkupFlag("linkSrc"), link)
                        .Replace(ReportHelper.MarkupFlag("linkName"), Path.GetFileNameWithoutExtension(link));
            }

            foreach (KeyValuePair<string, string> pair in source.SourceFiles)
            {
                File.WriteAllText(pair.Key, pair.Value
                        .Replace(ReportHelper.MarkupFlag("nav"), navLinks)
                        .Replace(ReportHelper.MarkupFlag("filename"), Path.GetFileNameWithoutExtension(pair.Key))
                        .Replace(ReportHelper.MarkupFlag("consoleLogs"), logger.GetLogsAsString()));
            }

            File.WriteAllText(Path.Combine(outputDirectory, "Index.html"), html
                .Replace(ReportHelper.MarkupFlag("nav"), navLinks)
                .Replace(ReportHelper.MarkupFlag("consoleLogs"), logger.GetLogsAsString()));
        }

        public void FolderReport(string resultsDirectory)
        {
            FolderReport(resultsDirectory, resultsDirectory);
        }

        public void FileReport(string resultsFile, string outputFile)
        {
            isSingle = true;

            BuildReport(resultsFile, outputFile);
        }

        private Report BuildReport(string resultsFile, string outputFile)
        {
            testFileParser = ParserFactory.LoadParser(resultsFile);

            if (testFileParser == null) 
                return null;

            Report report = testFileParser.ProcessFile();

            if (report == null) 
                return null;

            string html = HTML.File.Base;

            if (report.Total > 0)
            {
                try
                {
                    // do the replacing here
                    html = html.Replace(ReportHelper.MarkupFlag("totalTests"), report.Total.ToString())
                                    .Replace(ReportHelper.MarkupFlag("passed"), report.Passed.ToString())
                                    .Replace(ReportHelper.MarkupFlag("failed"), report.Failed.ToString())
                                    .Replace(ReportHelper.MarkupFlag("inconclusive"), report.Inconclusive.ToString())
                                    .Replace(ReportHelper.MarkupFlag("skipped"), report.Skipped.ToString())
                                    .Replace(ReportHelper.MarkupFlag("errors"), report.Errors.ToString())
                                    .Replace(ReportHelper.MarkupFlag("inXml"), Path.GetFullPath(report.FileName))
                                    .Replace(ReportHelper.MarkupFlag("duration"), report.Duration.ToString())
                                    .Replace(ReportHelper.MarkupFlag("result"), report.Status.ToString())
                                    .Replace(ReportHelper.MarkupFlag("name"), report.AssemblyName);

                    foreach (KeyValuePair<string, string> pair in report.RunInfo.Info)
                    {
                        html = html.Replace(ReportHelper.MarkupFlag("runinfo"), HTML.File.RunInfoRow + ReportHelper.MarkupFlag("runinfo"))
                                .Replace(ReportHelper.MarkupFlag("runInfoParam"), pair.Key)
                                .Replace(ReportHelper.MarkupFlag("runInfoValue"), pair.Value);
                    }

                    html = BuildSuiteBlocks(report, html, outputFile);
                }
                catch (Exception ex)
                {
                    logger.Fatal("Something weird happened: " + ex.Message);
                    return null;
                }
            }
            else
            {
                // replace OPTIONALCSS here with css to hide elements
                html = html.Replace(ReportHelper.MarkupFlag("noTestsMessage"), HTML.File.NoTestsMessage)
                            .Replace(ReportHelper.MarkupFlag("optionalCss"), HTML.File.NoTestsCSS + ReportHelper.MarkupFlag("optionalCss"))
                            .Replace(ReportHelper.MarkupFlag("inXml"), Path.GetFileName(resultsFile));
            }

            // finally, save the source as the output file
            if (source == null)
                File.WriteAllText(outputFile, html.Apply(theme));
            else
                source.SourceFiles.Add(outputFile, html.Apply(theme));

            return report;
        }

        private string BuildSuiteBlocks(Report report, string html, string outputFile)
        {
            var categories = new List<string>();

            // run for each test fixture
            foreach (TestSuite suite in report.TestFixtures)
            {
                html = html.Replace(ReportHelper.MarkupFlag("inserttest"), "")
                        .Replace(ReportHelper.MarkupFlag("insertfixture"), HTML.File.Fixture)
                        .Replace(ReportHelper.MarkupFlag("fixturename"), suite.Name)
                        .Replace(ReportHelper.MarkupFlag("fixtureresult"), suite.Status.ToString().ToLower());

                if (!string.IsNullOrWhiteSpace(suite.StatusMessage))
                {
                    html = html.Replace(ReportHelper.MarkupFlag("fixturestatusmsg"), suite.StatusMessage);
                }

                if (!string.IsNullOrWhiteSpace(suite.StartTime) && !string.IsNullOrWhiteSpace(suite.EndTime))
                {
                    html = html.Replace(ReportHelper.MarkupFlag("fixtureStartedAt"), suite.StartTime)
                        .Replace(ReportHelper.MarkupFlag("fixtureEndedAt"), suite.EndTime)
                        .Replace(ReportHelper.MarkupFlag("footerDisplay"), "")
                        .Replace(ReportHelper.MarkupFlag("fixtureStartedAtDisplay"), "")
                        .Replace(ReportHelper.MarkupFlag("fixtureEndedAtDisplay"), "");
                }
                else if (!string.IsNullOrWhiteSpace(suite.StartTime))
                {
                    html = html.Replace(ReportHelper.MarkupFlag("fixtureStartedAt"), suite.StartTime)
                        .Replace(ReportHelper.MarkupFlag("footerDisplay"), "")
                        .Replace(ReportHelper.MarkupFlag("fixtureStartedAtDisplay"), "")
                        .Replace(ReportHelper.MarkupFlag("fixtureEndedAtDisplay"), "style='display:none;'");
                }
                else if (suite.Duration > 0)
                {
                    html = html.Replace(ReportHelper.MarkupFlag("fixtureStartedAt"), suite.Duration.ToString())
                        .Replace(ReportHelper.MarkupFlag("footerDisplay"), "")
                        .Replace(ReportHelper.MarkupFlag("fixtureStartedAtDisplay"), "")
                        .Replace(ReportHelper.MarkupFlag("fixtureEndedAtDisplay"), "style='display:none;'");
                }
                else
                {
                    html = html.Replace(ReportHelper.MarkupFlag("footerDisplay"), "style='display:none;'")
                        .Replace(ReportHelper.MarkupFlag("fixtureStartedAtDisplay"), "style='display:none;'")
                        .Replace(ReportHelper.MarkupFlag("fixtureEndedAtDisplay"), "style='display:none;'");
                }

                // add each test case of the test fixture
                foreach (Test test in suite.Tests)
                {
                    // test-level replacements
                    html = html.Replace(ReportHelper.MarkupFlag("inserttest"), HTML.File.Test)
                        .Replace(ReportHelper.MarkupFlag("testname"), test.Name)
                        .Replace(ReportHelper.MarkupFlag("teststatus"), test.Status.ToString().ToLower())
                        .Replace(ReportHelper.MarkupFlag("teststatusmsg"), test.StatusMessage);

                    foreach (string category in test.Categories)
                    {
                        html = html.Replace(ReportHelper.MarkupFlag("testFeature"), category + " " + ReportHelper.MarkupFlag("testFeature"));

                        // add all categories to create filters
                        if (!categories.Contains(category))
                        {
                            categories.Add(category);
                        }
                    }

                    html = html.Replace(ReportHelper.MarkupFlag("testFeature"), "");
                }
            }

            if (categories.Count == 0)
            {
                html = html.Replace(ReportHelper.MarkupFlag("optionalCss"), "<style>.feature-toggle { display: none; }</style>");
            }
            else
            {
                foreach (string category in categories)
                {
                    html = html.Replace(ReportHelper.MarkupFlag("categoryList"), HTML.File.CategoryLi + ReportHelper.MarkupFlag("categoryList"))
                                .Replace(ReportHelper.MarkupFlag("categoryName"), category);
                }
            }

            // add topbar for folder-level report to allow backward navigation to Index.html
            if (isSingle)
            {
                html = html.Replace("<body>", "<body class='single'>");
            }

            return html;
        }

        public ReportBuilder(Theme theme)
        {
            this.theme = theme;
        }
    }
}
