namespace ReportUnit.Layer
{
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
		/// How long the test took to run (in milliseconds)
		/// </summary>
		public double Duration { get; set; }
	}
}
