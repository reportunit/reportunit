using System;
using System.Collections.Generic;
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

		public void CreateFolderReport(string inputDirectory, string outputDirectory)
        {
            InitializeRazor();

            var filePathList = Directory.GetFiles(inputDirectory, "*.*", SearchOption.TopDirectoryOnly).ToList();

            var reportCollection = new Dictionary<Report, string>();
            var sidenavLinks = Templates.SideNav.GetIndexLink();
            var reportList = new List<Report>();

            foreach (var report in from filePath in filePathList let testRunner = GetTestRunner(filePath) where !testRunner.Equals(TestRunner.Unknown) let parser = (IParser)Assembly.GetExecutingAssembly().CreateInstance(Ns + "." + Enum.GetName(typeof(TestRunner), testRunner)) where parser != null select parser.Parse(filePath))
            {
	            reportList.Add(report);

	            var reportHtml = Engine.Razor.RunCompile(Templates.File.GetSource(), "reportKey", typeof(Report), report);

	            sidenavLinks += Engine.Razor.RunCompile(Templates.SideNav.GetSource(), "sidenavKey", typeof(Report), report);

	            reportCollection.Add(report, reportHtml);
            }

            var summaryHtml = Engine.Razor.RunCompile(Templates.Summary.GetSource(), "summaryKey", typeof(List<Report>), reportList); 
            File.WriteAllText(Path.Combine(outputDirectory, "Index.html"), summaryHtml.Replace("<!--%SIDENAV%-->", sidenavLinks));
            
            // sidenav links can only be known after all files are processed
            // some files may be invalid, so the entire input file collection may not be used
            // only valid processed files go here ->
            foreach (var entry in reportCollection)
            {
                File.WriteAllText(Path.Combine(outputDirectory, entry.Key.GetHtmlFileName()), entry.Value.Replace("<!--%SIDENAV%-->", sidenavLinks));
            }
        }

        public void CreateFileReport(string inputFile, string outputFile)
        {
            InitializeRazor();

            var testRunner = GetTestRunner(inputFile);

            var html = "";

            if (!(testRunner.Equals(TestRunner.Unknown)))
            {
                var parser = (IParser) Assembly.GetExecutingAssembly().CreateInstance(Ns + "." + Enum.GetName(typeof(TestRunner), testRunner));
	            if (parser != null)
	            {
		            var report = parser.Parse(inputFile);

		            html = Engine.Razor.RunCompile(Templates.File.GetSource(), "reportKey", typeof(Report), report);
	            }
            }

            File.WriteAllText(outputFile, html);
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
