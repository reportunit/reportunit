using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using ReportUnit.Parser;

namespace ReportUnit.Model
{
    public class Report
    {
        public List<Status> StatusList;

        public List<string> CategoryList;

        public List<TestSuite> TestSuiteList { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        /// <summary>
        /// Error or other status messages
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// File name generated that this data is for
        /// </summary>
        public string FileName { get; set; }

        public TestRunner TestRunner { get; set; }

        public Dictionary<string, string> RunInfo { get; private set; }

        public void AddRunInfo(Dictionary<string, string> runInfo)
        {
            RunInfo = runInfo;
        }

        /// <summary>
        /// Name of the assembly that the tests are for
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Overall result of the test run (eg Passed, Failed)
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// How long the test suite took to run (in milliseconds)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Total number of tests run
        /// </summary>
        public double Total { get; set; }

        public double Passed { get; set; }

        public double Failed { get; set; }

        public double Inconclusive { get; set; }

        public double Skipped { get; set; }

        public double Errors { get; set; }

        public string SideNavLinks { get; set; }

        public Report()
        {
            TestSuiteList = new List<TestSuite>();
            CategoryList = new List<string>();
            StatusList = new List<Status>();
        }
    }
}
