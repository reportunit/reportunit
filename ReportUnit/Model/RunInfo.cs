using System;
using System.Collections.Generic;

using ReportUnit.Parser;

namespace ReportUnit.Model
{
    /// <summary>
    /// Detailed information on the environment and machine that the tests were run under
    /// </summary>
    internal class RunInfo
    {
        public RunInfo()
        {
            Info = new Dictionary<string, string>();
        }

        /// <summary>
        /// Execution info such as username, machine-name, domain etc.
        /// </summary>
        public Dictionary<string, string> Info { get; private set; }

        /// <summary>
        /// The type of test runner that generated the data (eg NUnit, mstest)
        /// </summary>
        public TestRunner TestRunner { get; set; }
    }
}
