namespace ReportUnit.Layer
{
    using System.Collections.Generic;

    /// <summary>
    /// Detailed information on the environment and machine that the tests were run under
    /// </summary>
    internal class RunInfo
    {
        public Dictionary<string, string> Info { get; set; }

        /// <summary>
        /// The type of test runner that generated the data (eg NUnit, mstest)
        /// </summary>
        public Parser.TestRunner TestRunner { get; set; }
    }
}
