namespace ReportUnit.Support
{
	using System.Collections.Generic;
	using System.Linq;
	using ReportUnit.Layer;

	internal class ReportHelper
	{
		// fixture level status codes
		public static Status GetFixtureStatus(IEnumerable<Test> tests)
		{
			return GetFixtureStatus(tests.Select(t => t.Status).ToList());
		}

		// fixture level status codes
		public static Status GetFixtureStatus(List<Status> statuses)
		{
			if (statuses.Any(x => x == Status.Failed)) return Status.Failed;
			if (statuses.Any(x => x == Status.Error)) return Status.Error;
			if (statuses.Any(x => x == Status.Inconclusive)) return Status.Inconclusive;
			if (statuses.Any(x => x == Status.Passed)) return Status.Passed;
			if (statuses.Any(x => x == Status.Skipped)) return Status.Skipped;

			return Status.Unknown;
		}

		public static string MarkupFlag(string name)
		{
			return "<!--%" + name.ToUpper() + "%-->";
		}
	}
}
