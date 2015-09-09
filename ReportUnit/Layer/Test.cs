namespace ReportUnit.Layer
{
    using System.Collections.Generic;

    /// <summary>
    /// Information for an individual test
    /// </summary>
    internal class Test
    {
        public string Name { get; set; }

        /// <summary>
        /// Status of the test run (eg Passed, Failed)
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Error messages, description, etc
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Console Logs
        /// </summary>
        public string ConsoleLogs { get; set; }

        /// <summary>
        /// How long the test took to run (in milliseconds)
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Categories & features associated with the test
        /// </summary>
        public List<string> Categories;

        public Test()
        {
            Categories = new List<string>();
	        Status = Layer.Status.Unknown;
        }
    }
}
