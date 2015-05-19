namespace ReportUnit
{
    using System;
    using System.IO;
    using System.Linq;

    using ReportUnit.Layer;
    using ReportUnit.Parser;
    using ReportUnit.Support;
    using System.Collections.Generic;

    internal class FileBuilder
    {
        /// <summary>
        /// FileParser for the type of test results to be parsed
        /// </summary>
        private IParser testFileParser;

        /// <summary>
        /// Flag for folder-level report to add a DIV to navigate back to the Folder/Executive Summary report
        /// This is switched off by default for a test-suite report created standalone
        /// reportunit "path-to-folder"
        ///     BEHAVIOR: This flag will be TRUE
        /// reportunit "input" "output"
        ///     BEHAVIOR: This flag will be FALSE
        /// </summary>
        private bool addTopbar = false;

        public void CreateFolderReport(string resultsDirectory, string outputDirectory)
        {
            List<string> allFiles = Directory.GetFiles(resultsDirectory, "*.*", SearchOption.TopDirectoryOnly).ToList();

            // if no files, end process
            if (allFiles.Count == 0)
            {
                Console.WriteLine("[INFO] No XML files were found in the given location. Exiting..");
                return;
            }

            addTopbar = true;
            string html = HTML.Folder.Base;

            foreach (string file in allFiles) 
            {
                Console.WriteLine("\n" + new string('-', 9));

                Report reportData = BuildReport(file, Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(file) + ".html"));

                if (reportData != null)
                {
                    int passed = reportData.Passed;
                    int failed = reportData.Failed + reportData.Errors;
                    int other = reportData.Inconclusive + reportData.Skipped;

                    string runResult;

                    if (reportData.Status == Status.Inconclusive || reportData.Status == Status.Skipped || reportData.Status == Status.Unknown)
                    {
                        runResult = "other";
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

            File.WriteAllText(Path.Combine(outputDirectory, "Index.html"), html);
        }

        public void CreateFolderReport(string resultsDirectory)
        {
            CreateFolderReport(resultsDirectory, resultsDirectory);
        }

        public void CreateFileReport(string resultsFile, string outputFile)
        {
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

                    BuildSuiteBlocks(report, html, outputFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something weird happened: " + ex.Message);
                    return null;
                }
            }
            else
            {
                // replace OPTIONALCSS here with css to hide elements
                html = html.Replace(ReportHelper.MarkupFlag("insertNoTestsMessage"), HTML.File.NoTestsMessage)
                            .Replace("/*%OPTIONALCSS%*/", HTML.File.NoTestsCSS)
                            .Replace(ReportHelper.MarkupFlag("inXml"), Path.GetFileName(resultsFile));

                // add topbar for folder-level report to allow backward navigation to Index.html
                if (addTopbar)
                {
                    html = html.Replace(ReportHelper.MarkupFlag("topbar"), HTML.File.Topbar);
                }

                // finally, save the source as the output file
                File.WriteAllText(outputFile, html);
            }

            return report;
        }

        private void BuildSuiteBlocks(Report report, string html, string outputFile)
        {
            // run for each test fixture
            foreach (TestSuite suite in report.TestFixtures)
            {
                html = html.Replace(ReportHelper.MarkupFlag("inserttest"), "")
                        .Replace(ReportHelper.MarkupFlag("insertfixture"), HTML.File.Fixture)
                        .Replace(ReportHelper.MarkupFlag("fixturename"), suite.Name)
                        .Replace(ReportHelper.MarkupFlag("fixtureresult"), suite.Status.ToString().ToLower());

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
                }
            }

            // add topbar for folder-level report to allow backward navigation to Index.html
            if (addTopbar)
            {
                html = html.Replace(ReportHelper.MarkupFlag("topbar"), HTML.File.Topbar);
            }

            // finally, save the source as the output file
            File.WriteAllText(outputFile, html);
        }
    }
}
