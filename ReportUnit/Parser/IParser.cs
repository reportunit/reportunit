namespace ReportUnit.Parser
{
	using ReportUnit.Layer;

	internal interface IParser
	{
		/// <summary>
		/// Load the testResultFile and ensure its of a valid format (eg valid xml)
		/// </summary>
		/// <param name="testResultFile"></param>
		IParser LoadFile(string testResultFile);

		/// <summary>
		/// Process the test result file data and load into ProcessFile
		/// </summary>
		/// <returns></returns>
		Report ProcessFile();
	}
}
