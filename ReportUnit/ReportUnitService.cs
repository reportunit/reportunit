using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

using ReportUnit.Logging;
using ReportUnit.Model;
using ReportUnit.Parser;

namespace ReportUnit
{
    class ReportUnitService
    {
        private const string Ns = "ReportUnit.Parser";
        private readonly Logger _logger = Logger.GetLogger();

        public ReportUnitService() { }

        public void CreateReport(string input, string outputDirectory)
        {
            var attributes = File.GetAttributes(input);
            IEnumerable<FileInfo> filePathList;

            if ((FileAttributes.Directory & attributes) == FileAttributes.Directory)
            {
                filePathList = new DirectoryInfo(input).GetFiles("*.xml", SearchOption.AllDirectories)
                    .OrderByDescending(f => f.CreationTime);
            }
            else
            {
                if (File.Exists(input))
                {
                    filePathList = new[] {new FileInfo(input)};
                }
                else
                {
                    filePathList = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles(input);
                }
            }

            InitializeRazor();

            var compositeTemplate = new CompositeTemplate();

            foreach (var filePath in filePathList)
            {
                var testRunner = GetTestRunner(filePath.FullName);

                if (!(testRunner.Equals(TestRunner.Unknown)))
                {
                    var parser = (IParser)Assembly.GetExecutingAssembly().CreateInstance(Ns + "." + Enum.GetName(typeof(TestRunner), testRunner));
                    var report = parser.Parse(filePath.FullName);

                    compositeTemplate.AddReport(report);
                }
            }
            if (compositeTemplate.ReportList == null)
            {
                Logger.GetLogger().Fatal("No reports added - invalid files?");
                return;
            }
            if (compositeTemplate.ReportList.Count > 1)
            {
                compositeTemplate.SideNavLinks = compositeTemplate.SideNavLinks.Insert(0, Templates.SideNav.IndexLink);

                var summary = Engine.Razor.RunCompile(Templates.TemplateManager.GetSummaryTemplate(), "summary", typeof(Model.CompositeTemplate), compositeTemplate, null);
                File.WriteAllText(Path.Combine(outputDirectory, "Index.html"), summary);
            }

            foreach (var report in compositeTemplate.ReportList)
            {
                report.SideNavLinks = compositeTemplate.SideNavLinks;

                var html = Engine.Razor.RunCompile(Templates.TemplateManager.GetFileTemplate(), "report", typeof(Model.Report), report, null);
                File.WriteAllText(Path.Combine(outputDirectory, report.FileName + ".html"), html);
            }
        }

        private TestRunner GetTestRunner(string inputFile)
        {
            var testRunner = new ParserFactory(inputFile).GetTestRunnerType();

            _logger.Info("The file " + inputFile + " contains " + Enum.GetName(typeof(TestRunner), testRunner) + " test results");

            return testRunner;
        }

        private void InitializeRazor()
        {
            var templateConfig = new TemplateServiceConfiguration();
            templateConfig.DisableTempFileLocking = true;
            templateConfig.EncodedStringFactory = new RawStringFactory();
            templateConfig.CachingProvider = new DefaultCachingProvider(x => { });
            var service = RazorEngineService.Create(templateConfig);
            Engine.Razor = service;
        }
    }
}
