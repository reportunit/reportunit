namespace ReportUnit
{
    using System;

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
            new FileBuilder().CreateFolderReport(@"C:\Users\Anshoo\Documents\Visual Studio 2013\Projects\reportunit\ReportUnit\bin\Debug");
            Console.Read();

            //if (args.Length == 0 || args.Length > 2)
            //{
            //    Console.WriteLine("[ERROR] Invalid number of arguments specified.\n" + reportUnitUsage);
            //    return;
            //}

            //foreach (string arg in args)
            //{
            //    if (arg.Trim() == "" || arg == "\\\\")
            //    {
            //        Console.WriteLine("[ERROR] Invalid argument(s) specified.\n" + reportUnitUsage);
            //        return;
            //    }
            //}

            //for (int ix = 0; ix < args.Length; ix++)
            //{
            //    args[ix] = args[ix].Replace('"', '\\');
            //}

            //if (args.Length == 2)
            //{
            //    if ((Path.GetExtension(args[0]).ToLower().Contains("xml")) && (Path.GetExtension(args[1]).ToLower().Contains("htm")))
            //    {
            //        new FileBuilder().CreateFileReport(args[0], args[1]);
            //    }
            //    else if (Directory.Exists(args[0]) && Directory.Exists(args[1]))
            //    {
            //        new FileBuilder().CreateFolderReport(args[0], args[1]);
            //    }
            //    else
            //    {
            //        Console.WriteLine("[ERROR] Invalid files specified.\n" + reportUnitUsage);
            //    }

            //    return;
            //}

            //if (!Directory.Exists(args[0]))
            //{
            //    Console.WriteLine("{ERROR] The path of directory you have specified does not exist.\n" + reportUnitUsage);
            //    return;
            //}

            //new FileBuilder().CreateFolderReport(args[0]);
        }
    }
}
