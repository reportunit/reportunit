namespace ReportUnit.Layer
{
    /// <summary>
    /// Detailed information on the environment and machine that the tests were run under
    /// </summary>
    internal class RunInfo
    {
        /// <summary>
        /// Windows user that the tests were run under
        /// </summary>
        public string User { get; set; }

        public string UserDomain { get; set; }

        public string MachineName { get; set; }

        public string Platform { get; set; }

        /// <summary>
        /// Operating system
        /// </summary>
        public string OsVersion { get; set; }

        public string ClrVersion { get; set; }

        /// <summary>
        /// The type of test runner that generated the data (eg NUnit, mstest)
        /// </summary>
        public string TestRunner { get; set; }

        /// <summary>
        /// Version of the TestRunner
        /// </summary>
        public string TestRunnerVersion { get; set; }
    }
}
