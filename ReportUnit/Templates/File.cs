using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportUnit.Templates
{
    internal class File
    {
        public static string GetSource()
        {
            return @"
            <!DOCTYPE html>
            <html lang='en'>
            <!--
                ReportUnit 1.5 | http://reportunit.relevantcodes.com/
                Created by Anshoo Arora (Relevant Codes) | Released under the MIT license

                Template from:

                    ExtentReports Library | http://relevantcodes.com/extentreports-for-selenium/ | https://github.com/anshooarora/
                    Copyright (c) 2015, Anshoo Arora (Relevant Codes) | Copyrights licensed under the New BSD License | http://opensource.org/licenses/BSD-3-Clause
                    Documentation: http://extentreports.relevantcodes.com 
            -->
            " +
                @"
                <head>
                    <meta charset='utf-8'>
                    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                    <meta name='viewport' content='width=device-width, initial-scale=1'>
                    <meta name='description' content=''>
                    <meta name='author' content=''>
                    <title>ReportUnit TestRunner Report</title>
                    <link href='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.2/css/materialize.min.css' rel='stylesheet' type='text/css'>
                    <link href='https://cdn.rawgit.com/reportunit/reportunit/005dcf934c5a53e60b9ec88a2a118930b433c453/cdn/reportunit.css' type='text/css' rel='stylesheet' />
                    
                </head>
                <body>
                    <div class='header'>
                        <nav>
                            <ul id='slide-out' class='side-nav fixed'>
                                @Model.SideNavLinks
                            </ul>

                            <span class='file-name'>@Model.FileName</span>
                            
                            <ul class='left'>
                                <li class='logo'>
                                    <a href='http://reportunit.relevantcodes.com/'><span>ReportUnit</span></a>
                                </li>
                            </ul>

                            <ul class='right nav-right'>
                                <li class='nav-item'>
                                    <a class='modal-trigger waves-effect waves-light run-info-icon tooltipped' data-position='bottom' data-tooltip='Run Info' href='#modal1'><i class='mdi-action-info-outline'></i></a>
                                </li>
                                <!-- <div class='nav-item'>
                                    <a class='modal-trigger waves-effect waves-light console-logs-icon tooltipped' data-position='bottom' data-tooltip='Console Logs' href='#modal2'><i class='mdi-action-assignment'></i></a>
                                </div> -->
                                <li class='nav-item'>
                                    v1.50.0
                                </li>
                            </ul>
                        </nav>
                    </div>
                    <div class='main'>
                        <div class='main-wrap'>
                            @if (Model.Total == 0)
                            {
                                <div class='row no-tests'>
                                    <div clas='col s12 m6 l4'>
                                        <div class='no-tests-message card-panel no-margin-v'>
                                            <p>
                                                No tests were found in @Model.FileName.
                                            </p>
                                            @if (!String.IsNullOrEmpty(@Model.StatusMessage))
                                            {
                                                <pre>
                                                    @Model.StatusMessage
                                                </pre>
                                            }
                                        </div>
                                    </div>
                                </div>
                            }
                            else
                            {
                            <div class='row dashboard'>
                                <div class='col s12 m6 l4'>
                                    <div class='card-panel'>
                                        <div alt='Count of all passed tests' title='Count of all passed tests'>Suite Summary</div>    
                                        <div class='chart-box'>
                                            <canvas class='text-centered' id='suite-analysis'></canvas>
                                        </div>
                                        <div>
                                            <span class='weight-light'><span class='suite-pass-count weight-normal'></span> suites(s) passed</span>
                                        </div> 
                                        <div>
                                            <span class='weight-light'><span class='suite-fail-count weight-normal'></span> suites(s) failed, <span class='suite-others-count weight-normal'></span> others</span>
                                        </div> 
                                    </div>
                                </div>
                                <div class='col s12 m6 l4'>
                                    <div class='card-panel'>
                                        <div alt='Count of all failed tests' title='Count of all failed tests'>Tests Summary</div>
                                        <div class='chart-box'>
                                            <canvas class='text-centered' id='test-analysis'></canvas>
                                        </div>
                                        <div>
                                            <span class='weight-light'><span class='test-pass-count weight-normal'>@Model.Passed</span> test(s) passed</span>
                                        </div> 
                                        <div>
                                            <span class='weight-light'><span class='test-fail-count weight-normal'>@Model.Failed</span> test(s) failed, <span class='test-others-count weight-normal'>@(Model.Total - (Model.Passed + Model.Failed))</span> others</span>
                                        </div> 
                                    </div>
                                </div>
                                <div class='col s12 m12 l4'>
                                    <div class='card-panel'>
                                        <div alt='Count of all inconclusive tests' title='Count of all inconclusive tests'>Pass Percentage</div>
                                        <div class='panel-lead pass-percentage'></div>
                                        <div class='progress'>
                                            <div class='determinate'></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class='row'>
                                <div id='suites' class='suites'>
                                    <div class='col'>
                                        <div class='card-panel no-padding no-margin-v suite-list v-spacer'>
                                            <div class='filters card-panel'>
                                                <div>
                                                    <a class='dropdown-button button' href='#' data-hover='true' data-beloworigin='true' data-constrainwidth='true' data-activates='suite-toggle' alt='Filter suites' title='Filter suites'><i class='mdi-file-folder-open icon'></i></a><ul class='dropdown-content' id='suite-toggle'> 
                                                    <ul>
                                                        @foreach (var status in Model.StatusList.Distinct().ToList())
                                                        {
                                                            <li class='@status.ToString()'><a href='#!'>@status.ToString()</a></li>
                                                        }
                                                        <li class='divider'></li> 
                                                        <li class='clear'><a href='#!'>Clear Filters</a></li> 
                                                    </ul>
                                                </div> 
                                                <div>
                                                    <a class='dropdown-button button' href='#' data-hover='true' data-beloworigin='true' data-constrainwidth='true' data-activates='tests-toggle' alt='Filter tests' title='Filter tests'><i class='mdi-action-subject icon'></i></a><ul class='dropdown-content' id='tests-toggle'> 
                                                    <ul>
                                                        @foreach (var status in Model.StatusList.Distinct().ToList())
                                                        {
                                                            <li class='@status.ToString()'><a href='#!'>@status.ToString()</a></li>
                                                        }
                                                        <li class='divider'></li> 
                                                        <li class='clear'><a href='#!'>Clear Filters</a></li> 
                                                    </ul>
                                                </div> 
                                                @if (Model.CategoryList.Count > 0) 
                                                {
                                                    <div> 
                                                        <a class='category-toggle dropdown-button button' href='#' data-hover='true' data-beloworigin='true' data-constrainwidth='false' data-activates='category-toggle' alt='Filter categories' title='Filter categories'><i class='mdi-image-style icon'></i></a><ul class='dropdown-content' id='category-toggle'>
                                                        <ul>
                                                            @foreach (var cat in Model.CategoryList.Distinct().ToList())
                                                            {
                                                                <li class='@cat'><a href='#!'>@cat</a></li>
                                                            }
                                                            <li class='divider'></li> 
                                                            <li class='clear'><a href='#!'>Clear Filters</a></li> 
                                                        </ul> 
                                                    </div> 
                                                }
                                                <div>
                                                    <a title='Clear Filters' alt='Clear Filters' id='clear-filters' class='clear'><i class='mdi-navigation-close icon'></i></a> 
                                                </div> &nbsp;
                                                <div> 
                                                    <a title='Enable Dashboard' alt='Enable Dashboard' id='enableDashboard' class='enabled'><i class='mdi-action-track-changes icon active'></i></a> 
                                                </div>
                                            </div>
                                            <ul id='suite-collection' class='no-margin-v'>
                                                @for (var ix = 0; ix < Model.TestSuiteList.Count; ix++)
                                                {
                                                    <li class='suite @Model.TestSuiteList[ix].Status.ToString().ToLower()'>
                                                        <div class='suite-head'>
                                                            <div class='suite-name'>@Model.TestSuiteList[ix].Name</div>
                                                            <div class='suite-result @Model.TestSuiteList[ix].Status.ToString().ToLower() right label'>@Model.TestSuiteList[ix].Status.ToString()</div>
                                                        </div>
                                                        <div class='suite-content hide'>
                                                            <span alt='Suite started at time' title='Suite started at time' class='startedAt label green lighten-2 text-white'>@Model.TestSuiteList[ix].StartTime</span>
                                                            @if (!String.IsNullOrEmpty(@Model.TestSuiteList[ix].EndTime))
                                                            {
                                                                <span alt='Suite ended at time' title='Suite ended at time' class='endedAt label label red lighten-2 text-white'>@Model.TestSuiteList[ix].EndTime</span>
                                                            }
                                                            <div class='fixture-status-message'>
                                                                @if (!String.IsNullOrEmpty(@Model.TestSuiteList[ix].Description)) 
                                                                {
                                                                    <div class='suite-desc'>@Model.TestSuiteList[ix].Description</div>
                                                                }
                                                                @if (!String.IsNullOrEmpty(@Model.TestSuiteList[ix].StatusMessage)) 
                                                                {
                                                                    <div class='suite-desc'>@Model.TestSuiteList[ix].StatusMessage</div>
                                                                }
                                                            </div>
                                                            <table class='bordered'>
                                                                <thead>
                                                                    <tr>
                                                                        <th>TestName</th>
                                                                        <th>Status</th>
                                                                        @if (Model.TestSuiteList.Count > 0 && Model.TestSuiteList[ix].TestList.Any(x => x.CategoryList.Count > 0))
                                                                        {
                                                                            <th>Category</th>
                                                                        }
                                                                        @if (Model.TestSuiteList.Count > 0 && Model.TestSuiteList[ix].TestList.Where(x => !String.IsNullOrEmpty(x.Description) || !String.IsNullOrEmpty(x.StatusMessage)).Count() > 0) 
                                                                        {
                                                                            <th>StatusMessage</th>
                                                                        }
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    @foreach (var test in Model.TestSuiteList[ix].TestList)
                                                                    {
                                                                        <tr class='@test.Status.ToString().ToLower() test-status'>
                                                                            <td class='test-name'>
                                                                                @{var testName = test.Name.Replace(""<"", ""&lt;"").Replace("">"", ""&gt;"");}
                                                                                @if (!String.IsNullOrEmpty(@test.Description))
                                                                                {
                                                                                    <a class='showDescription name' href='#'>@testName</a>
                                                                                    <p class='hide description'>@test.Description</p>
                                                                                }
                                                                                else
                                                                                {
                                                                                    <span class='name'>@testName</span>
                                                                                }
                                                                            </td>
                                                                            <td class='test-status @test.Status.ToString().ToLower()'>
                                                                                <span class='label @test.Status.ToString().ToLower()'>@test.Status.ToString()</span>
                                                                            </td>
                                                                            @if (Model.TestSuiteList.Count > 0 && Model.TestSuiteList[ix].TestList.Any(x => x.CategoryList.Count > 0))
                                                                            {
                                                                                <td>
                                                                                    @if (test.CategoryList.Count > 0)
                                                                                    {
                                                                                        <div class='category-list'>
                                                                                            @foreach (var cat in test.CategoryList)
                                                                                            {
                                                                                                <span class='label category'>@cat</span>
                                                                                            }
                                                                                        </div>
                                                                                    }
                                                                                </td>
                                                                            }
                                                                            @if (Model.TestSuiteList.Count > 0 && Model.TestSuiteList[ix].TestList.Where(x => !String.IsNullOrEmpty(x.StatusMessage)).Count() > 0) 
                                                                            {
                                                                                if (!String.IsNullOrEmpty(@test.StatusMessage)) 
                                                                                {
                                                                                    <td>
                                                                                    
                                                                                        <div class='badge showStatusMessage error'><i class='mdi-alert-warning'></i></div>
                                                                                        <pre class='hide'>@test.StatusMessage</pre>
                                                                                    </td>
                                                                                }
                                                                                else 
                                                                                {
                                                                                    <td class='grey lighten-4'></td>
                                                                                }
                                                                            }
                                                                            <td class='test-features hide @test.GetCategories()'></td>
                                                                        </tr>
                                                                    }
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </li>
                                                }
                                            </ul>
                                        </div>
                                    </div>
                                    <div class='col'>
                                        <div class='card-panel suite-details no-margin-v v-spacer'>
                                            <h5 class='suite-name-displayed'></h5>
                                            <div class='details-container'></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            }
                        </div>
                    </div>
                    <div id='modal1' class='modal'>
                        <div class='modal-content'>
                            <h4><!--%FILENAME%--> RunInfo</h4>
                            <table class='bordered'>
                                <thead>
                                    <tr>
                                        <th>Param</th><th>Value</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>    
                                        <td>TestRunner</td>
                                        <td>@Model.TestRunner.ToString()</td>
                                    </tr>
                                    @if (Model.RunInfo != null)
                                    {
                                        foreach (var key in Model.RunInfo.Keys)
                                        {
                                            <tr>
                                                <td>@key</td>
                                                <td>@Model.RunInfo[key]</td>
                                            </tr>
                                        }
                                    }
                                </tbody>
                            </table>
                        </div>
                        <div class='modal-footer'>
                            <a href='#!' class='modal-action modal-close waves-effect waves-green btn-flat'>Close</a>
                        </div>
                        <div class='hidden total-tests'><!--%TOTALTESTS%--></div>
                        <div class='hidden total-passed'><!--%PASSED%--></div>
                        <div class='hidden total-failed'><!--%FAILED%--></div>
                        <div class='hidden total-inconclusive'><!--%INCONCLUSIVE%--></div>
                        <div class='hidden total-errors'><!--%ERRORS%--></div>
                        <div class='hidden total-skipped'><!--%SKIPPED%--></div>
                    </div>
                    <div id='dynamicModal' class='modal modal-trigger' in_duration='0' induration='0'>
                        <div class='modal-content'>
                            <h4></h4>
                            <pre></pre>
                        </div>
                    </div>
                </body>
                <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script> 
                <script src='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.2/js/materialize.min.js'></script> 
                <script src='https://cdnjs.cloudflare.com/ajax/libs/Chart.js/1.0.2/Chart.min.js'></script>
                <script src='https://cdn.rawgit.com/reportunit/reportunit/005dcf934c5a53e60b9ec88a2a118930b433c453/cdn/reportunit.js' type='text/javascript'></script>

            </html>
            ".Replace("\r\n", "").Replace("\t", "").Replace("    ", ""); 
        }
    }
}
