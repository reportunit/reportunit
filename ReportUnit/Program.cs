/*
* Copyright (c) 2015 Anshoo Arora (Relevant Codes)
* 
* MIT license
* 
* See the accompanying LICENSE file for terms.
*/

using System.Linq;

namespace ReportUnit
{
	using System;
	using System.IO;
	using Logging;

	class Program
    {
		/// <summary>
		/// ReportUnit usage
		/// </summary>
		private const string ReportUnitUsage = "[INFO] Usage 1:  ReportUnit \"path-to-folder\"" + "\n[INFO] Usage 2:  ReportUnit \"input-folder\" \"output-folder\"" + "\n[INFO] Usage 3:  ReportUnit \"input.xml\" \"output.html\"";

		/// <summary>
        /// Logger
        /// </summary>
        private static readonly Logger Logger = Logger.GetLogger();

        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Accepts 3 types of input arguments
        ///     Type 1: reportunit "path-to-folder"
        ///         args.length = 1 && args[0] is a directory
        ///     Type 2: reportunit "path-to-folder" "output-folder"
        ///         args.length = 2 && both args are directories
        ///     Type 3: reportunit "input.xml" "output.html"
        ///         args.length = 2 && args[0] is xml-input && args[1] is html-output
        /// </param>
        static void Main(string[] args)
        {
            CopyrightMessage();

            if (args.Length == 0 || args.Length > 2)
            {
                Logger.Error("Invalid number of arguments specified.\n" + ReportUnitUsage);
                return;
            }

            if (args.Any(arg => arg.Trim() == "" || arg == "\\\\"))
            {
	            Logger.Error("Invalid argument(s) specified.\n" + ReportUnitUsage);
	            return;
            }

            for (var ix = 0; ix < args.Length; ix++)
            {
                args[ix] = args[ix].Replace('"', '\\');
            }

            if (args.Length == 2)
            {
	            var s = Path.GetExtension(args[0]);
	            var extension1 = Path.GetExtension(args[1]);
	            if (extension1 != null && (s != null && ((s.ToLower().Contains("xml") || s.ToLower().Contains("trx")) && (extension1.ToLower().Contains("htm")))))
                {
                    if (!Directory.GetParent(args[1]).Exists)
                        Directory.CreateDirectory(Directory.GetParent(args[1]).FullName);

                    new ReportUnitService().CreateFileReport(args[0], args[1]);
                    return;
                }

                if (!Directory.Exists(args[0]))
                {
                    Logger.Error("Input directory " + args[0] + " not found.");
                    return;
                }

                if (!Directory.Exists(args[1]))
                    Directory.CreateDirectory(args[1]);

                if (Directory.Exists(args[0]) && Directory.Exists(args[1]))
                {
                    new ReportUnitService().CreateFolderReport(args[0], args[1]);
                }
                else
                {
                    Logger.Error("Invalid files specified.\n" + ReportUnitUsage);
                }

                return;
            }

	        var extension = Path.GetExtension(args[0]);
	        if (extension != null && extension.ToLower().Contains("xml"))
            {
                new ReportUnitService().CreateFileReport(args[0], Path.ChangeExtension(args[0], "html"));
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Logger.Error("The path of directory you have specified does not exist.\n" + ReportUnitUsage);
                return;
            }

            new ReportUnitService().CreateFolderReport(args[0], args[0]);
        }

        private static void CopyrightMessage()
        {
            Console.WriteLine("\n--\nReportUnit v1.5. Report generator for the test-runner family.");
            Console.WriteLine("http://reportunit.relevantcodes.com/");
            Console.WriteLine("Copyright (c) 2015 Anshoo Arora (Relevant Codes)");
            Console.WriteLine("Developers:  Anshoo Arora, Sandra Greenhalgh\n--\n");
        }

    }
}
