using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportUnit.Templates
{
    internal class Summary
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

		                ExtentReports Library 2.40.0 | http://relevantcodes.com/extentreports-for-selenium/ | https://github.com/anshooarora/
		                Copyright (c) 2015, Anshoo Arora (Relevant Codes) | Copyrights licensed under the New BSD License | http://opensource.org/licenses/BSD-3-Clause
		                Documentation: http://extentreports.relevantcodes.com 
                -->
                " +
                @"<head>
	                <meta charset='utf-8'>
	                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
	                <meta name='viewport' content='width=device-width, initial-scale=1'>
	                <meta name='description' content=''>
	                <meta name='author' content=''>
	                <title>ReportUnit TestRunner Report</title>
	                <link href='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.2/css/materialize.min.css' rel='stylesheet' type='text/css'>
	                <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' type='text/css'>
	                <link href='http://cdn.rawgit.com/reportunit/reportunit/master/resources/reportunit.css' rel='stylesheet' type='text/css'>
                </head>
                <body>    
	                <div class='header'>
		                <nav>
			                <ul id='slide-out' class='side-nav fixed'>
				                <li class='logo'>
					                <a class='left' href='http://reportunit.relevantcodes.com/'><span>ReportUnit</span></a>
					                <a class='menu-toggle right'><i class='mdi-navigation-menu small'></i></a>
				                </li>
				                <!--%SIDENAV%-->
			                </ul>
			                <span class='file-name'>Executive Summary</span>
			                <div class='right hide-on-med-and-down nav-right'>
				                <div class='nav-item'>
					                <a class='modal-trigger waves-effect waves-light run-info-icon tooltipped' data-position='bottom' data-tooltip='Run Info' href='#modal1'><i class='mdi-action-info-outline'></i></a>
				                </div>
				                <!-- <div class='nav-item'>
					                <a class='modal-trigger waves-effect waves-light console-logs-icon tooltipped' data-position='bottom' data-tooltip='Console Logs' href='#modal2'><i class='mdi-action-assignment'></i></a>
				                </div> -->
				                <div class='nav-item'>
					                v1.50.0
				                </div>
			                </div>
		                </nav>
	                </div>
	                <div class='main'>
		                <div class='main-wrap'>
			                <!-- <div class='row dashboard'>
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
							                <span class='weight-light'><span class='test-pass-count weight-normal'></span> test(s) passed</span>
						                </div> 
						                <div>
							                <span class='weight-light'><span class='test-fail-count weight-normal'></span> test(s) failed, <span class='test-others-count weight-normal'></span> others</span>
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
			                </div> -->
			                <div class='row'>
                                <div class='col s12'>
                                    <div class='card-panel'>
                                        <table class='table'>
                                            <thead>
                                                <tr>
                                                    <th>File</th>
                                                    <th>TestRunner</th>
                                                    <th>Result</th>
                                                    <th>Total Tests</th>
                                                    <th>Passed</th>
                                                    <th>Failed</th>
                                                    <th>Others</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                @for (int ix = 0; ix < Model.Count; ix++)
                                                {
                                                    <tr>
                                                        <td><a href='./@Model[ix].GetHtmlFileName()'>@Model[ix].FileName</a></td>
                                                        <td>@Model[ix].TestRunner</td>
                                                        <td>@Model[ix].Status</td>
                                                        <td>@Model[ix].Total</td>
                                                        <td>@Model[ix].Passed</td>
                                                        <td>@Model[ix].Failed</td>
                                                        <td>@(Model[ix].Inconclusive + Model[ix].Skipped + Model[ix].Errors)</td>
                                                    </tr>
                                                }
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
			                </div>
		                </div>
	                </div>
	                <div id='modal2' class='modal'>
		                <div class='modal-content'>
			                <h4>Console Log</h4>
			                <!--%CONSOLELOGS%-->
		                </div>
		                <div class='modal-footer'>
			                <a href='#!' class='modal-action modal-close waves-effect waves-green btn-flat'>Close</a>
		                </div>
	                </div>
                </body>
                <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script> 
                <script src='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.2/js/materialize.min.js'></script> 
                <script src='https://cdnjs.cloudflare.com/ajax/libs/Chart.js/1.0.2/Chart.min.js'></script>
                <script src='file:///C:/Users/Anshoo/Documents/Visual Studio 2013/Projects/reportunit/resources/reportunit.js'></script>
                <script src='http://cdn.rawgit.com/reportunit/reportunit/master/resources/reportunit.js' type='text/javascript'></script> --> 
            </html>
            ".Replace("\r\n", "").Replace("\t", "").Replace("    ", ""); ;
        }
    }
}
