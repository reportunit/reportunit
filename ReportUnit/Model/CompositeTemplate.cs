using System.Collections.Generic;
using RazorEngine;
using RazorEngine.Templating;
using ReportUnit.Templates;

namespace ReportUnit.Model
{
    public class CompositeTemplate
    {
        public List<Report> ReportList { get; private set; }

        public string SideNavLinks { get; internal set; }

        public void AddReport(Report report)
        {
            if (ReportList == null)
                ReportList = new List<Report>();

            ReportList.Add(report);

            SideNavLinks += Engine.Razor.RunCompile(SideNav.Link, "sidenav", typeof(Report), report, null);
        }
    }
}