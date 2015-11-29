﻿using System.Collections.Generic;

namespace ReportUnit.Model
{
	public class TestSuite
	{
		public TestSuite()
		{
			TestList = new List<Test>();
			Status = Status.Unknown;
		}

		public string TestListAsHtml { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }

		public Status Status { get; set; }

		/// <summary>
		/// Error or other status messages
		/// </summary>
		public string StatusMessage { get; set; }

		public string StartTime { get; set; }

		public string EndTime { get; set; }

		/// <summary>
		/// How long the test fixture took to run (in milliseconds)
		/// </summary>
		public double Duration { get; set; }

		public List<Test> TestList { get; private set; }
	}
}
