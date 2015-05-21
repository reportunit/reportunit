namespace ReportUnit
{
    using System;
    using System.IO;
    using System.Linq;

    using ReportUnit.Design;

    class Program
    {
        /// <summary>
        /// ReportUnit usage
        /// </summary>
        private static string reportUnitUsage = "[INFO] Usage 1:  ReportUnit \"path-to-folder\"\n[INFO] Usage 2:  ReportUnit \"input-folder\" \"output-folder\"\n[INFO] Usage 3:  ReportUnit \"input.xml\" \"output.html\"";

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Accepts 2 types of input arguments
        ///     Type 1:  ReportUnit "path-to-folder"
        ///         args.length = 1 && args[0] is a directory
        ///     Type 2: ReportUnit "input.xml" "output.html"
        ///         args.length = 2 && args[0] is xml-input && args[1] is html-output
        /// </param>
        static void Main(string[] args)
        {
            
        }
    }
}
