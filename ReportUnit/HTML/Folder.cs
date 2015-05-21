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
                return @"<!DOCTYPE html>
                    <html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'> 
                    <!--
	                    ReportUnit [Folder Summary] v1.0 | http://reportunit.github.io/
                    --> 
	                    <head>
		                    <title>ReportUnit Executive Report</title>
		                    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300,400,600' rel='stylesheet' type='text/css' />
		                    <link href='http://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css' rel='stylesheet' />
		                    <link href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css' rel='stylesheet'>
		                    <style type='text/css'>
			                    html {overflow-y: scroll;}
                                body {font-family: 'Open Sans', Arial;font-size: 14px; line-height: 1.3; margin: 0;}
                                table {border-collapse: collapse;width: 100%;}
                                a {color: #1366d7;}
                                /*---:[ containers ]:---*/
                                #reportunit-container {margin: 0;padding: 0;width: 100%;}
                                #content {padding: 40px 0 100px;}
                                .header, .dashboard, .content {margin: 0 auto;width: 1053px;}
                                /*---:[ header ]:---*/
                                .header { font-size: 14px; font-weight: 300; padding-bottom: 15px;}
                                #title { padding-top: 30px; }
                                .title-reportunit { color: #2c91ef; font-weight: 600; }
                                .header .name {color: #999;float: right; font-size: 12px; margin-right: 7px; margin-top: -17px;}
                                .header .name { display: none; }
                                /*---:[ dashboard ]:---*/
                                #dashboard { border: 1px solid #ddd; }
                                .dashboard {padding: 20px 0 0; text-align: center;}
                                .dashboard > div {display: inline-block;}
                                .wrap {background-color: #fff; padding: 20px; width: 420px; }
								.dashboard .wrap:first-child { margin-left: -20px; }
                                .dashboard .wrap:nth-child(2) { margin-right: 2px;}
                                .content .wrap { margin: 0; padding: 0; width: 100%; }
                                .wrap > .head { font-size: 13px; font-weight: 600;}
                                .flot-container {height: 200px; margin: 0 auto; text-align: center; width: 300px; }
                                .placeholder {width: 100%; height: 100%; font-size: 14px; line-height: 1.2em; background-color: transparent;}
                                .legend table { border-spacing: 5px; }
                                /*---:[ content ]:---*/
                                .reportunit-table tr {border-bottom: 1px solid #edf1f2;}
                                .reportunit-table tr:nth-child(2n) { background-color: #fafbfc;}
                                .totals-row {font-weight: 600;}
                                .reportunit-table th {border-bottom: 2px solid #dcdcdc; font-size: 14px;padding: 15px 14px 10px;text-align: left;}
                                .reportunit-table td {padding: 14px 12px;word-break: break-all;word-wrap: break-word;}
                                .reportunit-table td:last-child {min-width: 250px;}
                                .totals-row td {font-size: 13px;padding: 12px;}
                                .progress {margin-bottom: 0;}
                                .label {font-size: 12px;font-weight:600;padding: 2px 7px;text-transform:capitalize;}
                                .failed > .label, .failure > .label {background-color: #eb4549;}
                                .passed > .label, .success > .label {background-color: #32cd32;}
                                .other > .label {background-color: #FFA81C;}
                                .progress { height: 17px; }
                                .progress-bar { font-size: 12px; font-weight: 600; line-height: 16px; }
                                .progress-bar-warning {background-color: #ffa81c;}
                                /*OPTIONALCSS*/
		                    </style>
	                    </head>
	                    <body>
		                    <div id='reportunit-container'>
			                    <div class='reportunit-container'>
				                    <div id='header'>
					                    <div class='header'>
						                    <div id='title'>
							                    <span><span class='title-reportunit'>ReportUnit.</span></span>
						                    </div>
						                    <div class='name'>
							                    Folder Summary
						                    </div>
					                    </div>
				                    </div>
				                    <div id='dashboard'>
					                    <div class='dashboard'>
						                    <div class='wrap'>
                                                <div class='head'>
                                                    Overall Status
                                                </div>
                                                <div class='flot-container'>
                                                    <div id='file-summary' class='placeholder'></div>
                                                </div>
                                                <div class='footer'>
                                                
                                                </div>
                                            </div>
                                            <div class='wrap'>
                                                <div class='head'>
                                                    Test Status
                                                </div>
                                                <div class='flot-container'>
                                                    <div id='test-summary' class='placeholder'></div>
                                                </div>
                                                <div class='footer'>
                                                
                                                </div>
                                            </div>
					                    </div>
				                    </div>
				                    <div id='content'>
					                    <div class='content'>
						                    <div class='summary'>
                                                <div class='wrap'>
							                        <table class='reportunit-table'>
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
                                                </wrap>
						                    </div>
					                    </div>
				                    </div>
			                    </div>
		                    </div>
	                    </body>
	                    <script type='text/javascript' src='http://code.jquery.com/jquery-1.10.1.min.js'></script>
	                    <script src='https://cdnjs.cloudflare.com/ajax/libs/flot/0.8.3/jquery.flot.js'></script>
                        <script src='https://cdnjs.cloudflare.com/ajax/libs/flot/0.8.3/jquery.flot.pie.min.js'></script>
                        <script src='http://cdn.jsdelivr.net/jquery.flot.tooltip/0.7.1/jquery.flot.tooltip.min.js'></script>
	                    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js'></script>
	                    <script type='text/javascript'>
		                   $(document).ready(function() {
                                function testCountByStatus(status) {
                                    var sum = 0;
                                    $(status).each(function() {
                                        sum += Number($(this).text());
                                    });
                                    return sum;
                                }
                                var data = [
                                    { label: 'Pass', data: $('td.file-status.pass').length + $('td.file-status.passed').length + $('td.file-status.success').length }, 
                                    { label: 'Fail', data: $('td.file-status.fail').length + $('td.file-status.failure').length + $('td.file-status.failed').length }
                                ];
                                $('.totals-all').text(testCountByStatus('td.total-count'));
                                $('.totals-passed').text(testCountByStatus('td.pass-count'));
                                $('.totals-failed').text(testCountByStatus('td.fail-count'));
                                $('.totals-others').text(testCountByStatus('td.others-count'));
                                var plotObj = $.plot($('#file-summary'), data, {
                                    series: {
                                        pie: { show: true,
                                            innerRadius: 0.45,
                                            label: {
                                                show: true,
                                                radius: .9
                                            },
                                            stroke: { 
                                                width: 10
                                            }
                                        }
                                    },
                                    legend: {
                                        show: false
                                    },
                                    colors: ['#4caf50','#f8576c'],
                                    grid: {
                                        hoverable: true
                                    },
                                    tooltip: true,
                                    tooltipOpts: {
                                        content: '%p.0%, %s', // show percentages, rounding to 2 decimal places
                                        shifts: {
                                            x: 20,
                                            y: 0
                                        },
                                        defaultTheme: true
                                    }
                                });
                                data = [
                                    { label: 'Pass', data: testCountByStatus('td.pass-count') }, 
                                    { label: 'Fail', data: testCountByStatus('td.fail-count') },
                                    { label: 'Other', data: testCountByStatus('td.others-count') },
                                ];
                                var plotObj = $.plot($('#test-summary'), data, {
                                    series: {
                                        pie: {
                                            show: true,
                                            innerRadius: 0.45,
                                            label: {
                                                show: true,
                                                radius: .9
                                            },
                                            stroke: { 
                                                width: 10
                                            }
                                        }
                                    },
                                    legend: {
                                        show: false
                                    },
                                    colors: ['#4caf50','#f8576c','#ffc107','#7e57c2'],
                                    grid: {
                                        hoverable: true
                                    },
                                    tooltip: true,
                                    tooltipOpts: {
                                        content: '%p.0%, %s', // show percentages, rounding to 2 decimal places
                                        shifts: {
                                            x: 20,
                                            y: 0
                                        },
                                        defaultTheme: true
                                    }
                                });
                            })
	                    </script>
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
		                        <div class='progress-bar progress-bar-success progress-bar-striped' style='width: <!--%PASSEDPERCENTAGE%-->%'><span class='sr-only'><!--%PASSEDPERCENTAGE%-->%</span><!--%TOTALPASSED%--></div>
                                <div class='progress-bar progress-bar-danger progress-bar-striped' style='width: <!--%FAILEDPERCENTAGE%-->%'><span class='sr-only'><!--%FAILEDPERCENTAGE%-->%</span><!--%TOTALFAILED%--></div>
                                <div class='progress-bar progress-bar-warning progress-bar-striped' style='width: <!--%OTHERSPERCENTAGE%-->%'><span class='sr-only'><!--%OTHERSPERCENTAGE%-->%</span><!--%ALLOTHERTESTS%--></div>
		                    </div>
	                    </td>
                    </tr>
                    <!--%INSERTRESULT%-->";
            }
        }
    }
}
