﻿using System.Collections.Generic;
using RazorEngine;
using RazorEngine.Templating;

namespace ReportUnit.Model
{
    public class CompositeTemplate
    {
        private List<Report> _reportList;

        public void AddReport(Report report)
        {
            if (_reportList == null)
            {
                _reportList = new List<Report>();
            }

            _reportList.Add(report);
            SideNavLinks += Engine.Razor.RunCompile(Templates.SideNav.Link, "sidenav", typeof(Report), report, null);
        }

        public List<Report> ReportList
        {
            get
            {
                return _reportList;
            }
        }

        public string SideNavLinks { get; internal set; }
    }
}
