namespace ReportUnit.HTML
{
    internal class Folder
    {
        /// <summary>
        /// Main source written to the executive-summary level report
        /// </summary>
        internal static string Base
        {
            get
            {
                return
                    @"<!DOCTYPE html>
                    <html lang='en'>
                        <!--
                            ReportUnit [Folder Summary] v1.0 | http://reportunit.github.io/
                            Created by Anshoo Arora (Relevant Codes) | Released under the MIT license
                        --> 
                        <head>
                            <meta charset='utf-8'>
                            <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <meta name='viewport' content='width=device-width, initial-scale=1'>
                            <meta name='description' content=''>
                            <meta name='author' content=''>
                            <title>ReportUnit Executive Report</title>
                            <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/css/materialize.min.css'>
                            <link href='http://fonts.googleapis.com/css?family=Nunito:300|Open+Sans:400,600' rel='stylesheet' type='text/css'>
                            <style>
                                html {
                                    background-color: #f3f6fa;
                                }
                                body {
                                    font-family: 'Open Sans';
                                    font-size: 14px;
                                }

                                /* ---- [ global ] ---- */
                                .card-head {
                                    padding-bottom: 20px;
                                }
                                .card-panel {
                                    box-shadow: 0 1px 1px 0 rgba(0, 0, 0, 0.16), 0 1px 4px 0 rgba(0, 0, 0, 0.12);
                                }
                                .input-field label {
                                    color: #cf6487;
                                }

                                /* ---- [ sidenav ] ---- */
                                .side-nav.fixed {
									width: 300px;
								}
                                .side-nav a {
                                    line-height: 22px;
                                    height: auto;
                                }
                                .side-nav.fixed a {
                                    font-size: 14px;
                                    height: auto;
                                    padding: 20px 15px;
                                }
                                nav a.button-collapse {
                                    display: inline-block;
                                }
                                .reportunit-logo-main {
                                    float: right;
                                    font-size: 13px;
                                    margin-right: 45px;
                                    padding: 0 10px;
                                }
                                .reportunit-logo-main > span {
                                    border: 1px solid #fff;
                                    padding: 2px 6px;
                                }
                                .side-nav a {
                                    height: auto;
                                    padding: 20px 15px;
                                }
                                .logo {
                                    border-bottom: 1px solid #ddd;
                                    margin-left: -4px;
                                }
                                .logo span {
                                    border: 1px solid #222;
                                    font-size: 14px;
                                    padding: 2px 7px;
                                }
                                .logo:hover, .logo a:hover {
                                    background-color: transparent !important;
                                }
                                .nav-main {
                                    padding-left: 335px;
                                }
            
                                /* ---- [ main ] ---- */
                                .main {
                                    background-color: #f3f6fa;
                                    /* padding-left: 300px; */
                                }
                                .main-wrap {
                                    padding: 40px 40px 100px 325px;
                                }
                                .main-wrap > .row {
                                    margin-bottom: 0;
                                }
            
                                /* ---- [ dashboard ] ---- */
                                .card-panel {
                                    padding: 15px;
                                }
                                .card-panel > div {
                                    font-family: Nunito;
                                    font-size: 13px;
                                }
                                .chart {
                                    height: 100px;
                                    margin: 10px auto 25px;
                                    position: relative;
                                    text-align: center;
                                    width: 100px;
                                }
                                .chart canvas {
                                    position: absolute;
                                    top: 0;
                                    left: 0;
                                }
                                .percent {
                                    display: inline-block;
                                    line-height: 100px;
                                    z-index: 2;
                                }
                
                                /* ---- [ result table ] ---- */
                                .result-table td:last-child {
                                    min-width: 20%;
                                }
                                table.responsive-table.bordered tbody tr:last-child {
                                    border-right: none;
                                }
                                .totals-row {
                                    border-bottom: medium none !important;
                                }
                                .label {
                                    border-radius: 2px; color: #fff; font-size: 10px;font-weight:400 !important;padding: 2px 7px;text-transform:uppercase;
                                }
                                .failed > .label, .failure > .label {
                                    background-color: #eb4549;
                                }
                                .passed > .label, .success > .label {
                                    background-color: #32cd32;
                                }
                                .other > .label {
                                    background-color: #FFA81C;
                                }
                
                                /* ---- [ bootstrap ] ---- */
                                /*!
                                * Bootstrap v3.3.4 (http://getbootstrap.com)
                                * Copyright 2011-2015 Twitter, Inc.
                                * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)
                                */
                                @-webkit-keyframes progress-bar-stripes {
                                    from {
                                        background-position: 40px 0;
                                    }
                                    to {
                                        background-position: 0 0;
                                    }
                                }
                                @-o-keyframes progress-bar-stripes {
                                    from {
                                        background-position: 40px 0;
                                    }
                                    to {
                                        background-position: 0 0;
                                    }
                                }
                                @keyframes progress-bar-stripes {
                                    from {
                                        background-position: 40px 0;
                                    }
                                    to {
                                        background-position: 0 0;
                                    }
                                }
                                .progress {
                                    overflow: hidden;
                                    height: 20px;
                                    margin-bottom: 20px;
                                    background-color: #f5f5f5;
                                    border-radius: 4px;
                                    -webkit-box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.1);
                                    box-shadow: inset 0 1px 2px rgba(0, 0, 0, 0.1);
                                }
                                .progress-bar {
                                    float: left;
                                    width: 0%;
                                    height: 100%;
                                    font-size: 12px;
                                    line-height: 20px;
                                    color: #ffffff;
                                    text-align: center;
                                    background-color: #337ab7;
                                    -webkit-box-shadow: inset 0 -1px 0 rgba(0, 0, 0, 0.15);
                                    box-shadow: inset 0 -1px 0 rgba(0, 0, 0, 0.15);
                                    -webkit-transition: width 0.6s ease;
                                    -o-transition: width 0.6s ease;
                                    transition: width 0.6s ease;
                                }
                                .progress-striped .progress-bar, .progress-bar-striped {
                                    background-image: -webkit-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: -o-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    -webkit-background-size: 40px 40px;
                                    background-size: 40px 40px;
                                }
                                .progress.active .progress-bar, .progress-bar.active {
                                    -webkit-animation: progress-bar-stripes 2s linear infinite;
                                    -o-animation: progress-bar-stripes 2s linear infinite;
                                    animation: progress-bar-stripes 2s linear infinite;
                                }
                                .progress-bar-success {
                                    background-color: #5cb85c;
                                }
                                .progress-striped .progress-bar-success {
                                    background-image: -webkit-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: -o-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                }
                                .progress-bar-info {
                                    background-color: #5bc0de;
                                }
                                .progress-striped .progress-bar-info {
                                    background-image: -webkit-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: -o-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                }
                                .progress-bar-warning {
                                    background-color: #f0ad4e;
                                }
                                .progress-striped .progress-bar-warning {
                                    background-image: -webkit-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: -o-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                }
                                .progress-bar-danger {
                                    background-color: #d9534f;
                                }
                                .progress-striped .progress-bar-danger {
                                    background-image: -webkit-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: -o-linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                    background-image: linear-gradient(45deg, rgba(255, 255, 255, 0.15) 25%, transparent 25%, transparent 50%, rgba(255, 255, 255, 0.15) 50%, rgba(255, 255, 255, 0.15) 75%, transparent 75%, transparent);
                                }
                                .clearfix:before, .clearfix:after {
                                    content: ' ';
                                    display: table;
                                }
                                .clearfix:after {
                                    clear: both;
                                }
                                .center-block {
                                    display: block;
                                    margin-left: auto;
                                    margin-right: auto;
                                }
                
                                /* ---- [ bootstrap overrides ] ---- */
                                .progress { 
                                    height: 17px; margin: 5px 0;
                                }
                                .progress-bar {
                                    font-size: 10px;
                                    font-weight: 400;
                                    line-height: 16px;
                                }
                                .progress-bar-warning {
                                    background-color: #ffa81c;
                                }

                                /* ---- [ media queries ] ---- */
                                @media (max-width: 992px) {
                                    .main {
                                        padding-left: 0;
                                    }
                                    .button-collapse {
                                        padding-left: 10px;
                                    }
                                }
                            </style>
                        </head>
                        <body>    
                            <div class='header'>
                                <nav>
                                    <ul id='slide-out' class='side-nav fixed'>
                                        <li class='logo'><a href='http://reportunit.github.io'><span>ReportUnit</span></a></li>
                                        <!--%NAV%-->
                                    </ul>
                                    <a href='#' data-activates='slide-out' class='button-collapse'><i class='mdi-navigation-menu'></i></a>
                                    <div class='nav-main'>
                                        <div class='page-title'>
                                            <span><i class='mdi-file-folder-open'></i>&nbsp;&nbsp;Executive Summary</span>
                                        </div>
                                    </div>
                                </nav>
                            </div>
                            <div class='main'>
                                <div class='main-wrap'>
                                    <div class='row'>
                                        <div class='col s12 m6 l3'>
                                            <div class='card-panel'>
                                                <div>Total Tests</div>
                                                <div class='chart total-tests' data-percent=''>
                                                    <span class='percent'></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col s12 m6 l3'>
                                            <div class='card-panel'>
                                                <div>Passed</div>    
                                                <div class='chart total-passed' data-percent=''>
                                                    <span class='percent'></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col s12 m6 l3'>
                                            <div class='card-panel'>
                                                <div>Failed</div>
                                                <div class='chart total-failed' data-percent=''>
                                                    <span class='percent'></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col s12 m6 l3'>
                                            <div class='card-panel'>
                                                <div>Other</div>
                                                <div class='chart total-other' data-percent=''>
                                                    <span class='percent'></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class='row'>
                                        <div class='col s12'>
                                            <div class='card-panel'>
                                                <div class='card-head'>
                                                    <i class='mdi-editor-insert-drive-file'></i>
                                                    &nbsp;&nbsp;File Summary
                                                </div>
                                                <table class='bordered responsive-table result-table'>
                                                    <tr>
                                                        <th>File</th>
                                                        <th>Assembly</th>
                                                        <th>Result</th>
                                                        <th>Total</th>
                                                        <th>Passed</th>
                                                        <th>Failed</th>
                                                        <th>Other</th>
                                                        <th>Quick Status</th>
                                                    </tr>
                                                    <!--%INSERTRESULT%-->
                                                    <tr class='totals-row'>
                                                        <td>TOTAL</td>
                                                        <td></td>
                                                        <td></td>
                                                        <td class='totals-all'></td>
                                                        <td class='totals-passed' ></td>
                                                        <td class='totals-failed'></td>
                                                        <td class='totals-others'></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/easy-pie-chart/2.1.4/jquery.easypiechart.min.js'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/js/materialize.min.js'></script>
                            <script>
                                $(function() {
                                    $('.button-collapse').sideNav();
                                    var total = testCountByStatus('td.total-count');
                                    var passed = testCountByStatus('td.pass-count');
                                    var failed = testCountByStatus('td.fail-count');
                                    var other = testCountByStatus('td.others-count');
                                    $('.total-tests').easyPieChart({ lineWidth: 7,  trackColor: '#f1f2f3', barColor: '#9c27b0', lineCap: 'butt', scaleColor: '#fff', size: 100, });
                                    $('.total-tests').data('easyPieChart').update(total);
                                    $('.total-passed').easyPieChart({ lineWidth: 7,  trackColor: '#f1f2f3', barColor: '#53b657', lineCap: 'butt', scaleColor: '#fff', size: 100, });
                                    $('.total-passed').data('easyPieChart').update(passed / total * 100);
                                    $('.total-failed').easyPieChart({ lineWidth: 7,  trackColor: '#f1f2f3', barColor: '#f8576c', lineCap: 'butt', scaleColor: '#fff', size: 100, });
                                    $('.total-failed').data('easyPieChart').update(failed / total * 100);
                                    $('.total-other').easyPieChart({ lineWidth: 7,  trackColor: '#f1f2f3', barColor: '#ffc107', lineCap: 'butt', scaleColor: '#fff', size: 100, });
                                    $('.total-other').data('easyPieChart').update(other / total * 100);
                                    $('.nav.nav-sidebar a').filter(function(){
                                        return this.href.match(/[^\/]+$/)[0] == document.location.pathname.match(/[^\/]+$/)[0];
                                    }).parent().addClass('active');
                                    function testCountByStatus(status) {
                                        var sum = 0;
                                        $(status).each(function() {
                                            sum += Number($(this).text());
                                        });
                                        return sum;
                                    }
                                    $('.totals-all, .total-tests > span').text(total);
                                    $('.totals-passed, .total-passed > span').text(passed);
                                    $('.totals-failed, .total-failed > span').text(failed);
                                    $('.totals-others, .total-other > span').text(other);
                                });
                            </script>
                        </body>
                    </html>";
            }
        }

        /// <summary>
        /// Each row represents an input file from NUnit
        /// </summary>
        public static string Row
        {
            get
            {
                return @"<tr>
                        <td><a href='<!--%FULLFILENAME%-->'><!--%FILENAME%--></a></td>
                        <td><!--%ASSEMBLY%--></td>
                        <td class='file-status <!--%RUNRESULT%-->'><span class='label'><!--%RUNRESULT%--></span></td>
                        <td class='total-count'><!--%TOTALTESTS%--></td>
                        <td class='pass-count'><!--%TOTALPASSED%--></td>
                        <td class='fail-count'><!--%TOTALFAILED%--></td>
                        <td class='others-count'><!--%ALLOTHERTESTS%--></td>
                        <td>
                            <div class='progress'>
                                <div class='progress-bar progress-bar-success progress-bar-striped' style='width: <!--%PASSEDPERCENTAGE%-->%'><span class='sr-only'><!--%PASSEDPERCENTAGE%-->%</span></div>
                                <div class='progress-bar progress-bar-danger progress-bar-striped' style='width: <!--%FAILEDPERCENTAGE%-->%'><span class='sr-only'><!--%FAILEDPERCENTAGE%-->%</span></div>
                                <div class='progress-bar progress-bar-warning progress-bar-striped' style='width: <!--%OTHERSPERCENTAGE%-->%'><span class='sr-only'><!--%OTHERSPERCENTAGE%-->%</span></div>
                            </div>
                        </td>
                    </tr>
                    <!--%INSERTRESULT%-->";
            }
        }

        public static string NavLink
        {
            get
            {
                return @"<li><a class='waves-effect' href='<!--%LINKSRC%-->'><!--%LINKNAME%--></a></li>";
            }
        }
    }
}
