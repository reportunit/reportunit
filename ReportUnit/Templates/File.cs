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
            return
                    @"<!DOCTYPE html>
                        <html lang='en'>
                        <!--
                            ReportUnit 2.0 | http://reportunit.relevantcodes.com/
                            Created by Anshoo Arora (Relevant Codes) | Released under the MIT license
                        -->" +
                        @"<head>
                            <meta charset='utf-8'>
                            <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <meta name='viewport' content='width=device-width, initial-scale=1'>
                            <meta name='description' content=''>
                            <meta name='author' content=''>
                            <title>ReportUnit TestRunner Report</title>
                            <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.1/css/materialize.min.css'>
                            <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,600' rel='stylesheet' type='text/css'>
                            <style>
                                html { 
                                    background-color: #f1f4f8;  
                                }
                                body {
                                    font-size: 13px;
                                }
                                pre {
                                    background-color: #f8f9fa;
                                    border: 1px solid #ddd;
                                    border-radius: 4px;
                                    color: #444;
                                    font-family: Consolas, monospace;
                                    font-size: 13px !important;
                                    padding: 10px;
                                    white-space: pre-wrap;
                                    white-space: -moz-pre-wrap;
                                    white-space: -pre-wrap;    
                                    white-space: -o-pre-wrap;  
                                    word-wrap: break-word;     
                                }
                                td:first-child {
                                    max-width: 45%;
                                    min-width: 150px;
                                    padding-right: 25px;
                                    vertical-align: top;
                                }
                                td {
                                    padding: 8px;
                                    word-break: break-all;
                                }
                                .test-features {
                                    display: none !important;
                                }

                                /* -- [ material overrides ] -- */
                                .z-depth-1, nav, .card-panel, .card, .toast, .btn, .btn-large, .btn-floating, .dropdown-content, .collapsible {
                                    box-shadow: 0 1px 1px 0 rgba(0, 0, 0, 0.06), 0 1px 4px 0 rgba(0, 0, 0, 0.04);
                                }
                                [type='checkbox']:checked + label::before {
                                    border-color: transparent #eee #eee transparent;
                                }
                                [type='checkbox'] + label::before {
                                    border: 2px solid #ddd;
                                    margin-top: 4px;
                                }
                                .select-wrapper input.select-dropdown, .input-field label {
                                    font-size: 0.8rem;
                                }
                                .material-tooltip, label {
                                    font-size: 13px !important;
                                }
                                .input-field label {
                                    font-size: 11px !important;
                                }
                                .dropdown-content li > a, .dropdown-content li > span {
                                    font-size: 0.85rem;
                                    line-height: 1.2rem;
                                    padding: 0.5rem 1rem;
                                }

                                /* ---- [ global ] ---- */
                                .card-head {
                                    padding-bottom: 20px;
                                }
                                .card-panel {
                                    padding: 15px;
                                }
                                .vh100 {
                                    height: 100vh;
                                }
                                .btn {
                                    font-size: 12px;
                                }
                                .btn i {
                                    font-size: 13px;
                                }
                                .input-field label {
                                    color: #db7093;
                                    left: 0;
                                }
                                .hidden {
                                    display: none;
                                }
                                .text-white {
                                    color: #fff !important;
                                }
                                .weight-light {
                                    font-weight: 400;
                                }
                                .weight-normal {
                                    font-weight: 500;
                                }
                                .weight-bold {
                                    font-weight: 700;
                                }
                                .no-padding {
                                    padding: 0 !important;
                                }
                                .no-padding-h {
                                    padding-left: 0 !important;
                                    padding-right: 0 !important;
                                }
                                .no-padding-v {
                                    padding-bottom: 0 !important;
                                    padding-top: 0 !important;
                                }

                                /* ---- [ sidenav / topnav ] ---- */
                                nav {
                                    background: #29ccf7;
                                    padding-left: 270px;
                                    padding-right: 20px;
                                }
                                .side-nav {
                                    box-shadow: 0 6px 5px 0 rgba(0, 0, 0, 0.16);
                                    width: 260px;
                                }
                                .menu-toggle {
                                    background: #14b7e1;
                                    cursor: pointer;
                                    margin-right: -15px;
                                    padding: 0 18px !important;
                                }
                                nav, nav .nav-wrapper i, nav a.button-collapse, nav a.button-collapse i, nav li, .side-nav a, nav input, nav label {
                                    color: #fff !important;
                                    height: 50px;
                                    line-height: 50px;
                                }
                                .side-nav li {
                                    display: block;
                                }
                                .side-nav > li.report-item > a {
                                    color: #444 !important;    
                                    font-size: 13px;
                                }
                                .side-nav > li.report-item i {
                                    margin-right: 15px;
                                }
                                nav a.button-collapse {
                                    display: inline-block;
                                }
                                .nav-item {
                                    border-left: 1px solid #40e3fc;
                                    display: inline-block;
                                    height: 50px;
                                    padding: 0 10px;
                                }
                                .logo, .logo:hover, .logo a:hover {
                                    background: #21c4ee !important;
                                    height: 50px;
                                }
                                .file-name {
                                    padding-left: 15px;
                                }
                                .console-logs-icon, .run-info-icon {
                                    font-size: 20px;
                                }
            
                                /* ---- [ main ] ---- */
                                .main {
                                    background-color: #f1f4f8;
                                }
                                .main-wrap {
                                    padding: 10px 10px 0 270px;
                                }
                                .main-wrap > .row {
                                    margin-bottom: 0;
                                }
            
                                /* ---- [ dashboard ] ---- */
                                .dashboard.hide + .row {
                                    margin-top: -10px;
                                }
                                .dashboard .card-panel {
	                                height: 260px;
                                }
                                #test-analysis, #suite-analysis {
                                    height: 115px;
                                    margin: 30px auto 0;
                                    text-align: center;
                                    width: 145px;
                                }
                                .fh .card-panel {
                                    height: 275px;
                                    max-height: 275px;
                                    min-height: 275px;
                                }
                                .chart > div {
                                    text-align: center;
                                }
                                .chart-box {
                                    display: block;
                                    margin: 0 auto 20px;
                                    text-align: center;
                                }
                                .doughnut-legend li span {
                                    border-radius: 2px;
                                    display: block;
                                    float: left;
                                    height: 10px;
                                    margin-right: 8px;
                                    margin-top: 4px;
                                    width: 10px;
                                }
                                .doughnut-legend {
                                    display: inline-block;
                                    list-style: none;
                                    margin: 10px 0 0 -35px;
                                    font-size: 12px;
                                }
                                .doughnut-legend li {
                                    text-align: left;
                                }
                                .doughnut-legend li:first-letter {
                                    text-transform: capitalize;
                                }
                                .panel-lead {
                                    font-size: 24px !important;
                                    padding: 50px 0 100px;
                                    text-align: center;
                                }
                
                                /* ---- [ filters ] ---- */
                                .filters {
                                    padding-left: 10px;
                                    padding-top: 20px;
                                }
                                .btn, .btn-large {
                                    background-color: #ee6e73;
                                }
                                .dropdown-content li > a, .dropdown-content li > span {
                                    padding: 7px 20px;
                                }
                
                                /* ---- [ fixture / suites ] ---- */
                                .suite {
                                    border-bottom: 1px solid #eee;
                                    padding: 0 15px;
                                }
                                .suite.active {
                                    background: #f5f7fa;
                                }
                                .suite:last-child {
                                    border-bottom: none;
                                }
                                .suite-head {
                                    display: block;
                                    padding: 15px 0;
                                }
                                .suite-desc {
                                    margin-top: 10px;
                                }
                                .suite-status-message {
                                    background-color: #f7f8fa;
                                }
                                .suite-name {
                                    display: inline-block;
                                    max-width: 75%;
                                    word-break: break-all;
                                }
                                .suite-footer { 
                                    padding: 10px 0; 
                                }
                                p.description {
                                    border: 1px solid #ddd;
                                    color: #444;
                                    padding: 10px;
                                }
                                .endedAt {
                                    margin-left: 4px;
                                }

                                /* ---- [ details container ] ---- */
                                .details-view {
                                    position: absolute;
                                }
                                .details-container {
                                    padding-bottom: 250px;
                                }
                                .details-container > div {
                                    margin-top: 10px;
                                }
                                .details-container > table {
                                    margin-top: 20px;
                                }
                                .pin {
                                    height: 96%;
                                    right: 1%;
                                    overflow-y: auto;
                                    width: 49%;
                                }

                                /* ---- [ labels ] ---- */
                                .label {
                                    border: 1px solid transparent;
                                    border-radius: 2px;
                                    font-size: 11px !important; 
                                    font-weight: 500;
                                    padding: 2px 4px;
                                    text-transform: capitalize;
                                }
                                .label.fail, .label.failed, .label.failure, .label.invalid {
                                    border-color: #c64444;
                                    color: #c64444;
                                }
                                .label.pass, .label.success, .label.passed {
                                    border-color: #60b963;
                                    color: #60b963;
                                }
                                .label.error { 
                                    border-color: #ec407a;
                                    color: #ec407a;
                                }
                                .label.warning, .label.bad, .label.inconclusive, .label.pending, .label.notrunnable, .label.disconnected, .label.passedbutrunaborted { 
                                    border-color: #d88519;
                                    color: #d88519;
                                }                    
                                .label.skipped, .label.not-run, .label.notrun { 
                                    border-color: #2196f3;
                                    color: #2196f3;
                                }
                                td > .label {
                                    border: medium none;
                                    font-size: 12px !important;
                                    font-weight: 400;
                                    padding: 0;
                                }

                                /* ---- [ single ] ---- */
                                .single .page-title .logo {
                                    display: inline-block;
                                }
                                .single .page-title span {
									border-color: #fff;
								}
                                .single #slide-out, .single .button-collapse, .single .mdi-file-folder-open {
                                    display: none !important;
                                }
                                .single .main {
                                    padding-left: 0 !important;
                                }
                                .single .main-wrap {
                                    margin: 0 auto;
                                    padding-left: 40px;
                                    padding-right: 40px;
                                }
                                .single .nav-main {
                                    margin: 0 auto;
                                    padding-left: 52px;
                                }
                                .single .logo-single {
                                    display: inline-block !important;
                                }
                                .single .reportunit-logo-main {
                                    float: left;
                                    margin-right: 0;
                                    padding: 0;
                                }

                                /* ---- [ media queries ] ---- */
                                @@media (max-width: 1600px) {
                                    .main-wrap .col.s4 {
                                        padding-right: 0;
                                    }
                                }
                                @@media (max-width: 992px) {
                                    .main {
                                        padding-left: 0;
                                    }
                                    .nav-main {
	                                    padding-left: 100px;
                                    }
                                    .single .nav-main {
	                                    padding-left: 100px;
                                    }
                                    .main-wrap {
	                                    padding-left: 40px;
                                    }
                                    .button-collapse {
                                        padding-left: 10px;
                                    }
                                    .single .main-wrap {
                                        width: 95%;
                                    }
                                    .filters .toggle-type {
                                        display: none;
                                    }
                                    .filters .col.s3 {
                                        width: 40%;
                                    }
                                }
                                @@media (min-width: 992px) and (max-width: 1300px) {
                                    /* fix for pie charts going out of bounds */
                                    .main-wrap .row:first-child .col.s12 {
                                        width: 33.33% !important;
                                    }
                                    .filters .toggle-type {
                                        display: none;
                                    }
                                }
                            </style>
                            <!--%OPTIONALCSS%-->
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
                                    <span class='file-name'>@Model.FileName</span>
                                    <div class='right hide-on-med-and-down nav-right'>
                                        <!-- <span><i class='mdi-file-folder-open'></i>&nbsp;&nbsp;</span> -->
                                        <div class='nav-item'>
                                            <a class='modal-trigger waves-effect waves-light run-info-icon tooltipped' data-position='bottom' data-tooltip='Run Info' href='#modal1'><i class='mdi-action-info-outline'></i></a>
                                        </div>
                                        <!-- <div class='nav-item'>
                                            <a class='modal-trigger waves-effect waves-light console-logs-icon tooltipped' data-position='bottom' data-tooltip='Console Logs' href='#modal2'><i class='mdi-action-assignment'></i></a>
                                        </div> -->
                                        <div class='nav-item'>
                                            <input type='checkbox' id='enableDashboard' />
                                            <label for='enableDashboard'>Enable Dashboard</label>
                                        </div>
                                    </div>
                                </nav>
                            </div>
                            <div class='main'>
                                <div class='main-wrap'>
                                    <!--%NOTESTSMESSAGE%-->
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
                                        <div class='filters'>
                                            <div class='col l2 m4 s12'>
                                                <div class='input-field suite-toggle'>
                                                    <select>
                                                        <option value='0' selected>Choose your option</option>
                                                        @foreach (var status in Model.StatusList.Distinct().ToList())
													    {
														    <option value=''>@status.ToString()</option>
													    }
                                                        <option value='6'>Clear Filters</option>
                                                    </select>
                                                    <label>Filter Suites</label>
                                                </div>
                                            </div>
                                            <div class='col l2 m4 s12'>
                                                <div class='input-field tests-toggle'>
                                                    <select>
                                                        <option value='0' selected>Choose your option</option>
                                                        @foreach (var status in Model.StatusList.Distinct().ToList())
													    {
														    <option value=''>@status.ToString()</option>
													    }
                                                        <option value='6'>Clear Filters</option>
                                                    </select>
                                                    <label>Filter Tests</label>
                                                </div>
                                            </div>
                                            @if (Model.CategoryList.Count > 0) 
                                            {
                                                <div class='col l3 m4 s12'>
                                                    <div class='input-field feature-toggle'>
                                                        <select>
                                                            <option value='0' selected>Choose your option</option>
                                                            @foreach (var cat in Model.CategoryList.Distinct().ToList())
													        {
														        <option value=''>@cat</option>
													        }
                                                            <option value='x'>Clear Filters</option>
                                                        </select>
                                                        <label>Filter Features</label>
                                                    </div>
                                                </div>
                                            }
                                        </div>
                                    </div>
                                    <div class='row'>
                                        <div id='suites' class='suites'>
                                            <div class='col s5'>
                                                <div class='card-panel no-padding'>
                                                    @for (var ix = 0; ix < Model.TestSuiteList.Count; ix++)
                                                    {
                                                        <div class='suite @Model.TestSuiteList[ix].Status.ToString().ToLower()'>
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
				                                                        </tr>
			                                                        </thead>
			                                                        <tbody>
                                                                        @foreach (var test in Model.TestSuiteList[ix].TestList)
                                                                        {
                                                                            <tr class='@test.Status.ToString().ToLower() test-status'>
                                                                                <td class='test-name'>@test.Name</td>
                                                                                <td class='test-status @test.Status.ToString().ToLower()'>
                                                                                    <span class='label @test.Status.ToString().ToLower()'>@test.Status.ToString()</span>
                                                                                    @if (!String.IsNullOrEmpty(@test.Description))
                                                                                    {
                                                                                        <p class='description'>@test.Description</p>
                                                                                    }
                                                                                    @if (!String.IsNullOrEmpty(@test.StatusMessage)) {
                                                                                        <pre>@test.StatusMessage</pre>
                                                                                    }
                                                                                </td>
                                                                                <td class='test-features @test.GetCategories()'></td>
                                                                            </tr>
                                                                        }
			                                                        </tbody>
		                                                        </table>
	                                                        </div>
                                                        </div>
                                                    }
                                                </div>
                                            </div>
                                            <div class='col s7 suite-details'>
                                                <div class='card-panel details-view pin'>
                                                    <h5 class='suite-name-displayed'></h5>
                                                    <div class='details-container'></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
											@foreach (var key in Model.RunInfo.Keys)
											{
												<tr>
													<td>@key</td>
													<td>@Model.RunInfo[key]</td>
												</tr>
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
                        <script src='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.97.1/js/materialize.min.js'></script> 
                        <script src='https://cdnjs.cloudflare.com/ajax/libs/Chart.js/1.0.2/Chart.min.js'></script> 
                        <script type='text/javascript'>
                            var menuWidth = 260;
                            var pinWidth = '49%';

                            /* test case counts */
                            var total = $('.test-name').length;
                            var passed = $('td.passed').length;
                            var failed = $('td.failed').length;
                            var inconclusive = $('td.inconclusive').length;
                            var errors = $('td.error').length;
                            var skipped = $('td.skipped').length;

                            $(document).ready(function() {
                                /* init */
                                $('select').material_select();
                                $('.modal-trigger').leanModal();
                                $('.tooltipped').tooltip({delay: 10});
                                $('.button-collapse').sideNav({ menuWidth: 260 });
            
                                /* Enable Dashboard checkbox is started as unchecked */
                                $('#enableDashboard').prop('checked', true);

                                /* control content container position for vertical scroll */
                                $(window).scroll(function() {
                                    var scrollTop = $('.dashboard').is(':visible') ? 475 : 135;
                                    if ($(window).scrollTop() > scrollTop) {
                                        var margin = $('.dashboard').is(':visible') ? '-' + scrollTop + 'px' : '-135px';
                                        $('.details-view').css('position', 'fixed').css('margin-top', margin);
                                    } 
                                    else {
                                        $('.details-view').removeAttr('style').css('position', 'absolute');
                                    }
                                    $('.pin').css('width', pinWidth);
                                });

                                /* expand/collapse side-nav on menu click */
                                $('.menu-toggle').click(function() {
                                    menuWidth = menuWidth > 100 ? 70 : 260;
                                    if (pinWidth == '55%') { pinWidth = '49%'; }
                                    else { pinWidth = '55%'; }
									$('.logo .left, .side-nav input, .side-nav label').toggleClass('hide');
									$('.side-nav').animate({
										width: menuWidth + 'px'
									}, 200);
									$('.main-wrap, nav').animate({
										'padding-left': (menuWidth + 10) + 'px'
									}, 200);
                                    $('.pin').animate({
                                        'width': pinWidth
                                    }, 200);
								});

                                /* for a single report item, hide sidenav */
                                if ($('.report-item').length == 1) {
                                    $('#slide-out').addClass('hide');
                                    pinWidth = '56%';
                                    $('.pin').css('width', pinWidth);
                                    $('.main-wrap, nav').css('padding-left', '20px');
                                }

                                /* toggle dashboard on 'Enable Dashboard' click */
                                $('#enableDashboard').click(function() {
                                    $(this).toggleClass('enabled');
                                    $('.dashboard').toggleClass('hide');
                                });

                                /* show suite data on click */
                                $('.suite').click(function() {
                                    $('.suite').removeClass('active');
                                    $('.suite-name-displayed, .details-container').html('');
                                    var t = $(this);
                                    t.toggleClass('active');
	                                var html = t.find('.suite-content').html();
	                                $('.suite-name-displayed').text(t.find('.suite-name').text());
	                                $('.details-container').append(html);
                                });

                                $('#slide-out .report-item > a').filter(function(){
                                    return this.href.match(/[^\/]+$/)[0] == document.location.pathname.match(/[^\/]+$/)[0];
                                }).parent().addClass('active');

                                /* filters -> by suite status */
                                $('.suite-toggle li').click(function() {
                                    var opt = $(this).text().toLowerCase();

                                    if (opt != 'choose your option') {
                                        if (opt == 'clear filters') {
                                            resetFilters();
                                        } 
                                        else {
                                            var status = $(this).text().toLowerCase();
											$('#suites .suite').addClass('hide');
											$('#suites .suite.' + status).removeClass('hide');
                                        }
                                    }
                                });

                                /* filters -> by test status */
                                $('.tests-toggle li').click(function() {
                                    var opt = $(this).text().toLowerCase();

                                    if (opt != 'choose your option') {
                                        if (opt == 'clear filters') {
                                            resetFilters();
                                        } 
                                        else {
                                            $('.suite table tr.test-status:not(.' + opt + '), .details-container tr.test-status:not(.' + opt).addClass('hide');
                                            $('.suite table tr.test-status.' + opt + ', .details-container tr.test-status.' + opt).removeClass('hide');
                                            
                                            hideEmptySuites();
                                        }
                                    }
                                });

                                /* filters -> by category */
                                $('.feature-toggle li').click(function() {
                                    var opt = $(this).text();

                                    if (opt.toLowerCase() != 'choose your option') {
                                        if (opt.toLowerCase() == 'clear filters') {
                                            resetFilters();
                                        }  
                                        else {
                                            filterByCategory(opt);
                                        }
                                    }
                                });

                                /* auto-filter based on URL */
                                if (document.location.href.indexOf('?') > -1) {
                                    var evt = window.location.search.split('?')[1];
                                    switch (evt.toLowerCase()) {
                                        case 'passedtests': clickListItem('tests-toggle', 1); break;
                                        case 'failedtests': clickListItem('tests-toggle', 2); break;
                                        case 'errortests': clickListItem('tests-toggle', 3); break;
                                        case 'inconclusivetests': clickListItem('tests-toggle', 4); break;
                                        case 'skippedtests': clickListItem('tests-toggle', 5); break;
                                        case 'passedsuites': clickListItem('suite-toggle', 1); break;
                                        case 'failedsuites': clickListItem('suite-toggle', 2); break;
                                        case 'errorsuites': clickListItem('suite-toggle', 3); break;
                                        case 'inconclusivesuites': clickListItem('suite-toggle', 4); break;
                                        case 'skippedsuites': clickListItem('suite-toggle', 5); break;
                                        default: break;
                                    }
                                }

                                function filterByCategory(cat) {
                                    resetFilters();

                                    $('td.test-features').each(function() {
                                        if (!($(this).hasClass(cat))) {
                                            $(this).closest('tr').addClass('hide');
                                        }
                                    });

                                    hideEmptySuites();
                                }

                                function hideEmptySuites() {
                                    $('.suite').each(function() {
                                        var t = $(this);
                                        if (t.find('tr.test-status').length == t.find('tr.test-status.hide').length) {
                                            t.addClass('hide');
                                        }
                                    });
                                }

                                function resetFilters() {
                                    $('.suite, tr.test-status').removeClass('hide');
                                    $('.suite-toggle li:first-child, .tests-toggle li:first-child, .feature-toggle li:first-child').click();
                                }

                                function clickListItem(listClass, index) {
                                    $('.' + listClass).find('li').get(index).click();
                                }

                                var passedPercentage = Math.round(((passed / total) * 100)) + '%';
								$('.pass-percentage').text(passedPercentage);
								$('.dashboard .determinate').attr('style', 'width:' + passedPercentage);

                                suitesChart(); testsChart();
                                $('ul.doughnut-legend').addClass('right');
                                
                                resetFilters();
                                $('.suite:first-child').click();
                            });

                            var options = {
	                            segmentShowStroke : true, 
	                            segmentStrokeColor : '#fff', 
	                            segmentStrokeWidth : 1, 
	                            percentageInnerCutout : 55, 
	                            animationSteps : 30, 
	                            animationEasing : 'easeOutBounce', 
	                            animateRotate : true, 
	                            animateScale : false,
	                            legendTemplate : '<ul class=\'<%=name.toLowerCase()%>-legend\'><% for (var i=0; i<segments.length; i++){%><li><span style=\'background-color:<%=segments[i].fillColor%>\'></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'
                            };

                            /* suites chart */
                            function suitesChart() {
                                var passed = $('.suite-result.passed').length;
                                var failed = $('.suite-result.failed').length;
                                var others = $('.suite-result.error, .suite-result.inconclusive, .suite-result.skipped').length;
                                $('.suite-pass-count').text(passed);
                                $('.suite-fail-count').text(failed);
                                $('.suite-others-count').text(others);
	                            var data = [
		                            { value: passed, color: '#00af00', highlight: '#32bf32', label: 'Pass' },
		                            { value: failed, color:'#F7464A', highlight: '#FF5A5E', label: 'Fail' },
		                            { value: $('.suite-result.error').length, color:'#ff6347', highlight: '#ff826b', label: 'Error' },
		                            { value: $('.suite-result.inconclusive').length, color: '#FDB45C', highlight: '#FFC870', label: 'Warning' },
		                            { value: $('.suite-result.skipped').length, color: '#1e90ff', highlight: '#4aa6ff', label: 'Skip' }
	                            ];
	                            var ctx = $('#suite-analysis').get(0).getContext('2d');
	                            var suiteChart = new Chart(ctx).Doughnut(data, options);
	                            drawLegend(suiteChart, 'suite-analysis');
                            }

                            /* tests chart */
                            function testsChart() {
                                $('.test-pass-count').text(passed);
                                $('.test-fail-count').text(failed);
                                $('.test-others-count').text(errors + inconclusive + skipped);
                                var data = [
                                    { value: passed, color: '#00af00', highlight: '#32bf32', label: 'Pass' },
                                    { value: failed, color:'#F7464A', highlight: '#FF5A5E', label: 'Fail' },
                                    { value: errors, color:'#ff6347', highlight: '#ff826b', label: 'Error' },
                                    { value: inconclusive, color: '#FDB45C', highlight: '#FFC870', label: 'Warning' },
                                    { value: skipped, color: '#1e90ff', highlight: '#4aa6ff', label: 'Skip' }
                                ];
                                var ctx = $('#test-analysis').get(0).getContext('2d');
                                testChart = new Chart(ctx).Doughnut(data, options);
                                drawLegend(testChart, 'test-analysis');
                            }

                            /* draw legend for test and step charts [DASHBOARD] */
                            function drawLegend(chart, id) {
	                            var helpers = Chart.helpers;
	                            var legendHolder = document.getElementById(id);
	                            legendHolder.innerHTML = chart.generateLegend();
	                            helpers.each(legendHolder.firstChild.childNodes, function(legendNode, index) {
		                            helpers.addEvent(legendNode, 'mouseover', function() {
			                            var activeSegment = chart.segments[index];
			                            activeSegment.save();
			                            activeSegment.fillColor = activeSegment.highlightColor;
			                            chart.showTooltip([activeSegment]);
			                            activeSegment.restore();
		                            });
	                            });
	                            Chart.helpers.addEvent(legendHolder.firstChild, 'mouseout', function() {
		                            chart.draw();
	                            });
	                            $('#' + id).after(legendHolder.firstChild);
                            }
                        </script>
                    </html>".Replace("\r\n", "").Replace("\t", "").Replace("    ", ""); 
        }
    }
}
