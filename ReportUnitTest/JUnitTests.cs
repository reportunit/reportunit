using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace ReportUnitTest
{
    [TestFixture]
    public class JUnitTests
    {
        public static string ExecutableDir;
        public static string ResourcesDir;

        [OneTimeSetUp]
        public static void Setup()
        {
            var assemblyDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            TestContext.Progress.WriteLine("AssemblyDir: " + assemblyDir);
            if (assemblyDir == null || !Directory.Exists(assemblyDir))
            {
                throw new Exception("Failed to get assembly path");
            }
            
            ResourcesDir = Path.Combine(assemblyDir, "..", "..", "Resources");
            TestContext.Progress.WriteLine("ResourcesDir: " + ResourcesDir);
            if (!Directory.Exists(ResourcesDir))
            {
                throw new Exception("Can't find Resources folder");
            }

            ExecutableDir = Path.Combine(assemblyDir, "..", "..", "..", "ReportUnit", "bin");
            TestContext.Progress.WriteLine("ExecutableDir: " + ExecutableDir);
            if (!Directory.Exists(ExecutableDir))
            {
                throw new Exception("Can't find ReportUnit folder");
            }

            if (!File.Exists(Path.Combine(ExecutableDir, "ReportUnit.exe")))
            {
                throw new Exception("Can't find ReportUnit.exe");
            }
        }

        [Test]
        public void test_junit_one_testsuite_multiple_testcases()
        {
            TestContext.Progress.WriteLine("*** Test ***");
            GenerateHtmlReport("test_junit_one_testsuite_multiple_testcases.xml");
            ValidateHtmlReport("test_junit_one_testsuite_multiple_testcases.html");
            TestContext.Progress.WriteLine("*** Test - PASS ***");
        }

        [Test]
        public void test_junit_multiple_testsuite_multiple_testcases()
        {
            TestContext.Progress.WriteLine("*** Test ***");
            GenerateHtmlReport("test_junit_multiple_testsuite_multiple_testcases.xml");
            ValidateHtmlReport("test_junit_multiple_testsuite_multiple_testcases.html");
            TestContext.Progress.WriteLine("*** Test - PASS ***");
        }

        #region Private

        private static void ValidateHtmlReport(string htmlReportFileName)
        {
            TestContext.Progress.WriteLine("*** Validating HTML Report ***");
            var htmlFile = Path.Combine(ResourcesDir, "JUnit", htmlReportFileName);
            if (!File.Exists(htmlFile))
            {
                throw new Exception("No HTML report");
            }

            var vNuJarDirectory = Path.Combine(ResourcesDir, "vnu.jar_17.2.1");
            var processInfo = new ProcessStartInfo()
            {
                FileName = "java",
                Arguments = "-Xss8m -jar vnu.jar --asciiquotes " + htmlFile,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = false,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = vNuJarDirectory,
            };

            RunProcess(processInfo, 60000, true);

            TestContext.Progress.WriteLine("*** Validating HTML Report - PASS ***");
        }

        private static void GenerateHtmlReport(string junitXmlFileName)
        {
            TestContext.Progress.WriteLine("*** Generating HTML Report ***");
            var filename = Path.Combine(ExecutableDir, "ReportUnit.exe");
            var processInfo = new ProcessStartInfo()
            {
                FileName = filename,
                Arguments = Path.Combine(ResourcesDir, "JUnit", junitXmlFileName),
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = false,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = ExecutableDir
            };

            if (IsRunningOnMono())
            {
                processInfo.FileName = "mono";
                processInfo.Arguments = filename + " " + processInfo.Arguments;
            }
            
            RunProcess(processInfo, 5000, true);

            TestContext.Progress.WriteLine("*** Generating HTML Report - PASS ***");
        }

        private static void RunProcess(ProcessStartInfo processInfo, int milliseconds, bool redirect)
        {
            TestContext.Progress.WriteLine("Start Process...");
            TestContext.Progress.WriteLine("Filename: " + processInfo.FileName);
            TestContext.Progress.WriteLine("Arguments: " + processInfo.Arguments);

            var proc = Process.Start(processInfo);
            if (proc == null)
            {
                throw new Exception("Failed to start");
            }

            if (redirect)
            {
                while (!proc.StandardOutput.EndOfStream)
                {
                    TestContext.Progress.WriteLine(proc.StandardOutput.ReadLine());
                }

                while (!proc.StandardError.EndOfStream)
                {
                    TestContext.Progress.WriteLine(proc.StandardError.ReadLine());
                }
            }

            if (!proc.WaitForExit(milliseconds))
            {
                throw new Exception("Timeout");
            }

            if (proc.ExitCode != 0)
            {
                throw new Exception("Exit code " + proc.ExitCode);
            }
        }

        private static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        #endregion
    }
}
