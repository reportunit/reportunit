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
            var processInfo = PrepareProcess("ReportUnit.exe", "test_junit_one_testsuite_multiple_testcases.xml");

            RunProcess(processInfo, 5000, true);

            ValidateHtmlReport("test_junit_one_testsuite_multiple_testcases.html");
        }

        private static void ValidateHtmlReport(string htmlReportFileName)
        {
            var htmlFile = Path.Combine(ResourcesDir, "JUnit", htmlReportFileName);
            if (!File.Exists(htmlFile))
            {
                throw new Exception("No HTML report");
            }

            var vNuJarDirectory = Path.Combine(ResourcesDir, "vnu.jar_17.2.1");
            var processInfo = new ProcessStartInfo()
            {
                FileName = "java",
                Arguments = "-Xss512k -jar vnu.jar " + htmlFile,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = vNuJarDirectory,
            };

            RunProcess(processInfo, 60000, false);
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
            if (!proc.WaitForExit(milliseconds))
            {
                throw new Exception("Timeout");
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
            if (proc.ExitCode != 0)
            {
                throw new Exception("Exit code " + proc.ExitCode);
            }
        }

        private ProcessStartInfo PrepareProcess(string reportUnitFileName, string junitXmlFileName)
        {
            var filename = Path.Combine(ExecutableDir, reportUnitFileName);
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

            return processInfo;
        }

        private static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
        
    }
}
