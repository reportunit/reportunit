using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
        private const string _ns = "ReportUnit.Parser";
        private Logger _logger = Logger.GetLogger();

        public ReportUnitService() { }

        public void CreateReport(string input, string outputDirectory)
        {
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
                    IParser parser = (IParser)Assembly.GetExecutingAssembly().CreateInstance(_ns + "." + Enum.GetName(typeof(TestRunner), testRunner));
                    var report = parser.Parse(filePath);

                    compositeTemplate.AddReport(report);
                }
            }

            if (compositeTemplate.ReportList.Count > 1)
            {
                compositeTemplate.SideNavLinks = compositeTemplate.SideNavLinks.Insert(0, Templates.SideNav.IndexLink);

                string summary = Engine.Razor.RunCompile(Templates.Summary.GetSource(), "summary", typeof(Model.CompositeTemplate), compositeTemplate, null);
                File.WriteAllText(Path.Combine(outputDirectory, "Index.html"), summary);
            }

			foreach (var report in compositeTemplate.ReportList)
            {
                report.SideNavLinks = compositeTemplate.SideNavLinks;
                
                var html = Engine.Razor.RunCompile(Templates.File.GetSource(), "report", typeof(Model.Report), report, null);
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
            TemplateServiceConfiguration templateConfig = new TemplateServiceConfiguration();
            templateConfig.DisableTempFileLocking = true;
            templateConfig.EncodedStringFactory = new RawStringFactory();
            templateConfig.CachingProvider = new DefaultCachingProvider(x => { });
            var service = RazorEngineService.Create(templateConfig);
            Engine.Razor = service;
        }
    }
}
