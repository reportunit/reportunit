using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUnit.Templates
{
    internal class Suite
    {
        public static string GetSource()
        {
                return @"<div class='suite'>
                            <div class='suite-head'>
                                <div class='suite-name'>@Model.Name</div>
                                <div class='suite-result @Model.Status.ToString().ToLower() right label'>@Model.Status.ToString()</div>
                            </div>
                            <div class='suite-content hide'>
                                <span alt='Suite started at time' title='Suite started at time' class='startedAt label green lighten-2 text-white'>@Model.StartTime</span>
                                @if (!String.IsNullOrEmpty(@Model.EndTime))
                                {
                                    <span alt='Suite ended at time' title='Suite ended at time' class='endedAt label label red lighten-2 text-white'>@Model.EndTime</span>
                                }
                                <div class='fixture-status-message'>
                                    @if (!String.IsNullOrEmpty(Model.Description)) 
                                    {
                                        <div class='suite-desc'>@Model.Description</div>
                                    }
                                    @if (!String.IsNullOrEmpty(Model.StatusMessage)) 
                                    {
                                        <div class='suite-desc'>@Model.StatusMessage</div>
                                    }
                                </div>
                                <table class='bordered'>
                                    <thead>
                                        <tr>
                                            <th>TestName</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        @Model.TestListAsHtml
                                    </tbody>
                                </table>
                            </div>
                        </div>".Replace("\r\n", "").Replace("\t", "").Replace("    ", ""); ;
        }
    }
}
