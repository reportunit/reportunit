/*
* Copyright (c) 2015 Anshoo Arora (Relevant Codes)
* 
* MIT license
* 
* See the accompanying LICENSE file for terms.
*/

namespace ReportUnit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using ReportUnit.Parser;
    using ReportUnit.Logging;

    class Program
    {
        /// <summary>
        /// ReportUnit usage
        /// </summary>
        private static string USAGE = "[INFO] Usage 1:  ReportUnit \"path-to-folder\"" +
                                                "\n[INFO] Usage 2:  ReportUnit \"input-folder\" \"output-folder\"" +
                                                "\n[INFO] Usage 3:  ReportUnit \"input.xml\" \"output.html\"";

        /// <summary>
        /// Logger
        /// </summary>
        private static Logger _logger = Logger.GetLogger();

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
        static int Main(string[] args)
        {
            CopyrightMessage();

            if (args.Length == 0 || args.Length > 2)
            {
                _logger.Error("Invalid number of arguments specified.\n" + USAGE);
                return ((int) ExitCode.BadInput);
            }

            foreach (string arg in args)
            {
                if (arg.Trim() == "" || arg == "\\\\")
                {
                    _logger.Error("Invalid argument(s) specified.\n" + USAGE);
                    return ((int)ExitCode.BadInput);
                }
            }

            for (int ix = 0; ix < args.Length; ix++)
            {
                args[ix] = args[ix].Replace('"', '\\');
            }

            var exitCode = ExitCode.Success;

            if (args.Length == 2)
            {
                if ((Path.GetExtension(args[0]).ToLower().Contains("xml") || Path.GetExtension(args[0]).ToLower().Contains("trx")) && (Path.GetExtension(args[1]).ToLower().Contains("htm")))
                {
                    if (!Directory.GetParent(args[1]).Exists)
                        Directory.CreateDirectory(Directory.GetParent(args[1]).FullName);

                    exitCode = new ReportUnitService().CreateReport(args[0], Directory.GetParent(args[1]).FullName);
                    return ((int) exitCode);
                }

                if (!Directory.Exists(args[0]))
                {
                    _logger.Error("Input directory " + args[0] + " not found.");
                    return ((int)ExitCode.BadInput);
                }

                if (!Directory.Exists(args[1]))
                    Directory.CreateDirectory(args[1]);

                if (Directory.Exists(args[0]) && Directory.Exists(args[1]))
                {
                    exitCode = new ReportUnitService().CreateReport(args[0], args[1]);
                    return ((int)exitCode);
                }
                else
                {
                    _logger.Error("Invalid files specified.\n" + USAGE);
                    return ((int)ExitCode.BadInput);
                }
            }

            if (File.Exists(args[0]) && (Path.GetExtension(args[0]).ToLower().Contains("xml") || Path.GetExtension(args[0]).ToLower().Contains("trx")))
            {
                exitCode = new ReportUnitService().CreateReport(args[0], Directory.GetParent(args[0]).FullName);
                return ((int)exitCode);
            }

            if (!Directory.Exists(args[0]))
            {
                _logger.Error("The path of file or directory you have specified does not exist.\n" + USAGE);
                return ((int)ExitCode.BadInput);
            }

            exitCode = new ReportUnitService().CreateReport(args[0], args[0]);
            return ((int)exitCode);
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
