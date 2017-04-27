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

        public ExitCode CreateReport(string input, string outputDirectory)
        {
            // by default, return true for runstatus
            var runStatus = ExitCode.Success;
    		var attributes = File.GetAttributes(input);
		    List<string> filePathList;

        	 if ((FileAttributes.Directory & attributes) == FileAttributes.Directory)
        	{
                filePathList = Directory.GetFiles(input, "*.*", SearchOption.TopDirectoryOnly).ToList();
	        }
	        else
	        {
	            filePathList = new List<string>();
	            filePathList.Add(input);
            }

            InitializeRazor();

        	var compositeTemplate = new CompositeTemplate();
	
        	foreach (var filePath in filePathList)
        	{
            	var testRunner = GetTestRunner(filePath);

            	if (!(testRunner.Equals(TestRunner.Unknown)))
            	{
                    var parser = (IParser)Assembly.GetExecutingAssembly().CreateInstance(Ns + "." + Enum.GetName(typeof(TestRunner), testRunner));
                    var report = parser.Parse(filePath);

                    if (runStatus == ExitCode.Success)
                    {
                        if (report.Failed > 0) runStatus = ExitCode.Failure;
                        else if (report.Errors > 0) runStatus = ExitCode.Error;
                        else if (report.Inconclusive > 0) runStatus = ExitCode.Inconclusive;
                    }

                    compositeTemplate.AddReport(report);
                }
            }
            if (compositeTemplate.ReportList == null)
            {
                Logger.GetLogger().Fatal("No reports added - invalid files?");
                return ExitCode.BadInput;
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

            Console.WriteLine("Program ExitCode: " + runStatus);

            return runStatus;
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
