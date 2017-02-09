using System;
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
            var codebase = Assembly.GetExecutingAssembly().CodeBase;
            TestContext.Progress.WriteLine("CodeBase: " + codebase);

            codebase = Path.GetDirectoryName(codebase);
            if (codebase == null)
            {
                throw new Exception("Failed to get assembly path");
            }
            var codebaseDir = codebase.Replace("file:/","");
            TestContext.Progress.WriteLine("CodeBaseDir: " + codebaseDir);
            
            var assemblyDir = new Uri(codebaseDir).LocalPath;
            if (assemblyDir == null || !Directory.Exists(assemblyDir))
            {
                throw new Exception("Failed to get assembly path");
            }

            TestContext.Progress.WriteLine("Assembly: " + assemblyDir);

            ResourcesDir = Path.Combine(assemblyDir, "..", "..", "Resources");
            if (!Directory.Exists(ResourcesDir))
            {
                throw new Exception("Can't find Resources folder");
            }
            TestContext.Progress.WriteLine("Resources: " + ResourcesDir);

            ExecutableDir = Path.Combine(assemblyDir, "..", "..", "..", "ReportUnit", "bin");
            if (!Directory.Exists(ExecutableDir))
            {
                throw new Exception("Can't find ReportUnit folder");
            }
            TestContext.Progress.WriteLine("Executable: " + ExecutableDir);
        }

        [Test]
        public void Test()
        {
            var filename = Path.Combine(ExecutableDir, "ReportUnit.exe");
            var proc = System.Diagnostics.Process.Start(filename, "test_junit_01.xml");
            if (proc == null)
            {
                throw new Exception("Failed to start");
            }
            if (!proc.WaitForExit(5000))
            {
                throw new Exception("Timeout");
            }

            if (proc.ExitCode != 0)
            {
                throw new Exception("Exit code " + proc.ExitCode);
            }
        }
    }
}
