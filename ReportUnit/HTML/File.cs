namespace ReportUnit.HTML
{
    internal class File
    {
        /// <summary>
        /// Main source written to the test-suite level report
        /// Fixture and Test blocks are written for each instance from the XML file
        /// Topbar is added to the source only when the Folder level report is created
        /// </summary>
        public static string Base
        {
            get
            {
                return @"<!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'> 
                <!--
	                ReportUnit [TestSuite Summary] v1.0 | http://reportunit.github.io/
                --> 
	                <head>
		                <title>ReportUnit</title>
		                <link href='http://fonts.googleapis.com/css?family=Open+Sans:300,400,600' rel='stylesheet' type='text/css' />
		                <link href='http://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css' rel='stylesheet' />
		                <link href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css' rel='stylesheet'>
		                <style type='text/css'>
                            html { overflow-y: scroll; }
			                body { color: #222; font-family: 'Open Sans', Verdana; font-size: 14px; margin: 0; line-height: 1.3; }
			                a { color: inherit; text-decoration: none; }
                            select { font-family: 'Open Sans'; margin-top: -4px; padding: 7px 10px; }
			                pre { background: none repeat scroll 0 0 #f5f6f8; border: 1px solid #ccc; color: #111; font-family: monospace,Consolas; margin-top: 15px; padding: 5px 10px; text-transform: none !important; white-space: pre-wrap; }
                            p.description {border: 1px solid #ddd;border-radius: 4px;color: #222;font-size: 13px;line-height: 20px;margin-top: 15px;padding: 10px;}
			                #reportunit-container { margin: 0; width: 100%; }
			                #header, #dashboard, #filters, #content { width: 100%; }
			                .header, .topbar, .dashboard, .content, .filters { margin: 0 auto; width: 1053px; }
			                /*header*/
			                #header { margin-bottom: 35px; }
                            #topbar {border-bottom: 1px solid #ddd;padding: 7px 0 10px;}						
                            .back {background-color: #5bc0de;color: #fff;border-radius: 2px;font-family: 'Open Sans';font-size: 13px;font-weight: 600;padding: 3px 10px 4px;}
                            .back:hover {color: #fff;text-decoration: none;}
			                #title { font-size: 14px; font-weight: 300; margin-top: 30px; }
                            .title-reportunit { color: #2c91ef; font-weight: 600;}
			                .menu { float: right; margin-right: 5px; margin-top: -28px; }
			                .menu li { cursor: pointer; display: inline-block; font-size: 13px; list-style: outside none none; margin-right: 5px; padding: 10px 14px 10px 0; text-transform: uppercase;}
                            .selected { color: #2c91ef; }
			                /*dashboard*/
			                #dashboard { background-color: #f8f8f8; border-bottom: 1px solid #ddd; padding: 40px 0; }
			                #tabs-2 { display: none; }
                            #summary { float: left; margin: 3px 20px 0 20px; }
			                .summary-item {background: none repeat scroll 0 0 #fff;border-left: 1px solid #c2c2c2;display: inline-block;margin: 10px 17px;padding: 25px 15px 15px;width: 184px;}
			                .summary-item span { display: block; text-align: center; }
			                .summary-value {color: #222;float: left;font-size: 22px;margin-top: -16px;}
                            .summary-param {color: #3a4b60;float: right;font-size: 13px;font-weight: 300;margin-top: 31px;}
                            #tabs .simple-grey { border: 1px solid #b4bfc3; width: 80%; }
                            #tabs .simple-grey tr { border-bottom: 1px solid #b4bfc3; }
                            #tabs .simple-grey th { background-color: #393939; color: #fff !important; }
                            #tabs .simple-grey td { padding-bottom: 6px; padding-top: 6px; }
                            #tabs .simple-grey td:first-child { font-weight: 600; }
                            .last-box { margin-right: 0 !important; }
			                /*filters*/
			                .filters { margin-top: 50px; }
			                .dropdown-menu a { color: inherit !important; font-size: 13px; text-transform: none !important; }
                            .btn i {font-size: 11px; padding-right: 6px; }
                            /*controls*/
                            .controls { float: right; margin-right: 8px; }
			                .dropdown-menu i { font-size: 11px; padding-right: 10px; }
			                .dropdown-menu div { display: inline-block; width: 24px !important; }
			                /*content*/
			                #content { margin: 10px 0 100px; }
			                .fixtures { max-width: 1053px; }
			                .fixture-container { border: 1px solid #929da1; cursor: pointer; margin-bottom: 10px; width: 518px; }
							.fixture-head { padding: 20px 15px 60px; word-break: break-all; }
			                .fixture-name {float: left; font-size: 19px; font-weight: 600; max-width: 410px; word-break: break-all; }
							.fixture-footer { background-color: #f8fafa; padding: 10px 0 10px 15px; }
							.fixture-footer div { display: block; }
			                .fixture-result {border-radius: 0.25em;color: #fff !important;float: right;font-size: 12px !important;font-weight: 600;margin-top: 3px;padding: 2px 7px;}
							.startedAt, .endedAt { font-family: Verdana; font-size: 11px; }
							.startedAt i, .endedAt i { font-size: 12px; padding-right: 5px; }
							.startedAt i { color: #5cb85c; }
							.endedAt i { color: #d9534f; padding-left: 10px;}
                            .fixture-result.fail, .fixture-result.failed, .fixture-result.failure {background-color: #eb4549;}
                            .fixture-result.pass, .fixture-result.success, .fixture-result.passed {background-color: #32cd32;}
			                .fixture-result.error { background-color: tomato; }
	                        .fixture-result.warning, .fixture-result.bad, .fixture-result.inconclusive { background-color: orange; } 
	                        .fixture-result.skipped, .fixture-result.not-run, .fixture-result.notrun { background-color: #1e90ff; }
			                .fixture-content { cursor: auto !important; display: none; padding-bottom: 20px; }
			                .is-expanded { border: 1px solid #444 !important; border-top: 2px solid #444 !important; color: #000; height: auto; }
	                        .has-pre { width: 517px; }
			                .simple-grey { border-collapse: collapse; margin: 0 auto; text-align: left; width: 95%; }
	                        .simple-grey tr { border-bottom: 1px solid #e5e5e5; }
                            .simple-grey tr:last-child { border-bottom: none; }
			                .simple-grey th { font-size: 14px; }
	                        .simple-grey th, .simple-grey td { padding: 8px; text-align: left; }
	                        .simple-grey td { word-break: break-all; }
			                .simple-grey td:first-child { min-width: 150px; max-width: 400px; padding-right: 25px; }
	                        .simple-grey td:nth-child(2) { min-width: 60px; padding-right: 25px; }
			                .failed, .failure, .passed, .success, .warning, .bad, .inconclusive, .skipped, .ignored, .invalid, .error, .not-run, .notrun { text-transform: capitalize; }
	                        .failed, .failure { color: red; }
			                .error { color: tomato; }
	                        .passed, .success { color: #5cb85c; } 
	                        .warning, .bad, .inconclusive, .error { color: #f0ad4e; } 
	                        .skipped, .not-run, .notrun, .ignored { color: #1e90ff; }
			                .btn-group > .btn:first-child:not(:last-child):not(.dropdown-toggle) { background-color: #f1f5f8; border: medium none; border-bottom-right-radius: 0; border-top-right-radius: 0; font-size: 13px;padding: 11px 18px; }
			                .transparent { color: #ccc !important; }
                            /*%OPTIONALCSS%*/
                        </style>
	                </head>
	                <body>
		                <div id='reportunit-container'>
			                <div id='header'>
                                <!--%TOPBAR%-->
				                <div class='header'>
					                <div id='title'>
						                <span><span class='title-reportunit'>ReportUnit.</span></span>
					                </div>
					                <div class='menu'>
						                <ul>
							                <li><span class='tabs-1 selected'>Summary</span></li>
							                <li><span class='tabs-2'>Run Info</span></li>
						                </ul>
					                </div>
				                </div>
			                </div>
			                <div id='dashboard'>
                                <div class='dashboard'>
                                    <!--%INSERTNOTESTSMESSAGE%-->
				                    <div id='tabs'>
					                    <div id='tabs-1'>
						                    <div class='graphs'>
							                    <div id='summary'></div>
							                    <div class='summary-items'>
					                                <div class='summary-item total-tests'>
						                                <span class='summary-value'><!--%TOTALTESTS%--></span>
						                                <span class='summary-param'>Total Tests</span>
					                                </div>
					                                <div class='summary-item step-filter passed-tests'>
						                                <span class='summary-value'><!--%PASSED%--></span>
						                                <span id='passed-success' class='summary-param'>Passed</span>
					                                </div>
					                                <div class='summary-item step-filter last-box failed-tests'>
						                                <span class='summary-value'><!--%FAILED%--></span>
						                                <span id='failed-failure' class='summary-param'>Failed</span>
					                                </div>
					                                <div class='summary-item step-filter inconclusive-tests'>
						                                <span class='summary-value'><!--%INCONCLUSIVE%--></span>
						                                <span id='inconclusive-notrunnable' class='summary-param'>Inconclusive</span>
					                                </div>
					                                <div class='summary-item step-filter error-tests'>
						                                <span class='summary-value'><!--%ERRORS%--></span>
						                                <span id='error-errors' class='summary-param'>Errors</span>
					                                </div>
					                                <div class='summary-item step-filter last-box skipped-tests'>
						                                <span class='summary-value'><!--%SKIPPED%--></span>
						                                <span id='skipped-ignored-notrun' class='summary-param'>Skipped</span>
					                                </div>
                                                </div>
						                    </div>
					                    </div>
					                    <div id='tabs-2'>
						                    <table class='simple-grey'>
							                    <thead>
								                    <tr><th>Param</th><th>Value</th></tr>
                                                    <!--%RUNINFO%-->
							                    </thead>
						                    </table>
					                    </div>
				                    </div> 
                                </div>
			                </div>
			                <div id='filters'>
				                <div class='filters'>
                                    <div class='filter-message'>
                                        <span></span>
                                    </div>
					                <div class='btn-group'>
						                <button type='button' class='btn btn-default fixtures-toggle' data-toggle='dropdown' aria-expanded='false'><i class='fa fa-bars'></i>Filter Fixtures <span class='caret'></span></button>
						                <ul class='dropdown-menu fixture-filter' role='menu'>
							                <li><a id='passed-success' href='#'><div><i class='passed transparent fa fa-check-circle-o'></i></div>Passed</a></li>
							                <li><a id='failed-failure' href='#'><div><i class='failed transparent fa fa-times-circle'></i></div>Failed</a></li>
							                <li><a id='error-errors' href='#'><div><i class='error transparent fa fa-exclamation-circle'></i></div>Error</a></li>
							                <li><a id='inconclusive-notrunnable' href='#'><div><i class='inconclusive transparent fa fa-question'></i></div>Inconclusive</a></li>
							                <li><a id='skipped-ignored-notrun' href='#'><div><i class='skipped transparent fa fa-angle-double-right'></i></div>Skipped</a></li>
							                <li class='divider'></li>
							                <li><a class='clear-all' href='#'>Clear filters</a></li>
						                </ul>
					                </div>
					                <div class='btn-group'>
						                <button type='button' class='btn btn-default tests-toggle' data-toggle='dropdown' aria-expanded='false' title='Use this dropdown to filter all tests in the selected status. Note: this is only a test-level filter, not fixture-level. Filtering all PASSED tests for example may still show you failed fixtures as all the FAILED tests are hidden from view. This filter will not change the Fixture status upon filtering.' alt='Use this dropdown to filter all tests in the selected status. Note: this is only a test-level filter, not fixture-level. Filtering all PASSED tests for example may still show you failed fixtures as all the FAILED tests are hidden from view. This filter will not change the Fixture status upon filtering.'><i class='fa fa-cog'></i>Filter Tests <span class='caret'></span></button>
						                <ul class='dropdown-menu test-filter' role='menu'>
							                <li><a id='passed-success' href='#'><div><i class='passed transparent fa fa-check-circle-o'></i></div>Passed</a></li>
							                <li><a id='failed-failure' href='#'><div><i class='failed transparent fa fa-times-circle'></i></div>Failed</a></li>
							                <li><a id='error-errors' href='#'><div><i class='error transparent fa fa-exclamation-circle'></i></div>Error</a></li>
							                <li><a id='inconclusive-notrunnable' href='#'><div><i class='inconclusive transparent fa fa-question'></i></div>Inconclusive</a></li>
							                <li><a id='skipped-ignored-notrun' href='#'><div><i class='skipped transparent fa fa-angle-double-right'></i></div>Skipped</a></li>
							                <li class='divider'></li>
							                <li><a class='clear-all' href='#'>Clear filters</a></li>
						                </ul>
					                </div>
                                    <div class='controls'>
						                <div class='btn-group'>
							                <button type='button' class='btn btn-default tests-toggle-type' data-toggle='dropdown' aria-expanded='false' title='Selecting Accordion will allow only one fixture to be expanded at once. Use this open if you would want to view only one fixture at once.' alt='Selecting Accordion will allow only one fixture to be expanded at once. Use this open if you would want to view only one fixture at once.'><i class='fa fa-toggle-on'></i><span class='toggle-type-text'>Accordion</span> <span class='caret'></span></button>
							                <ul class='dropdown-menu select-toggle-type accordion' role='menu'>
								                <li><a class='optAccordion' href='#'><div><i class='fa fa-toggle-on'></i></div>Accordion</a></li>
                                                <li class='optToggle'><a href='#'><div><i class='fa fa-toggle-off'></i></div>Toggle</a></li>
							                </ul>
						                </div>
                                    </div>
				                </div>
			                </div>
			                <div id='content'>
				                <div class='content'>
					                <div class='fixtures'>
				                        <!--%INSERTFIXTURE%-->
			                        </div>
                                </div>
                            </div>
		                </div>	
	                </body>
	                <script type='text/javascript' src='http://code.jquery.com/jquery-1.10.1.min.js'></script>
	                <script type='text/javascript' src='https://www.google.com/jsapi'></script>
	                <script src='http://cdnjs.cloudflare.com/ajax/libs/masonry/3.2.2/masonry.pkgd.min.js' type='text/javascript' charset='utf-8'></script>
	                <script src='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js'></script>
	                <script type='text/javascript'>
                        $(document).ready(function() {
                            var $container = $('.fixtures').masonry({
				                columnWidth: 522,
				                gutter: 5
			                });
			                $container.on( 'click', '.fixture-container', function(evt) {
				                var cls = evt.target.className;
                                cls = evt.target.nodeName.toLowerCase();
				                if (cls.indexOf('div') >= 0 || cls.indexOf('span') >= 0) {
					                var elm = $(this);
					                var content = elm.find('.fixture-content');
					                cls = '';
					                if (content.is(':visible')) {
						                elm.removeClass('is-expanded has-pre');
						                content.hide();
					                }
					                else {
                                        if ($('.select-toggle-type').hasClass('accordion'))
                                            $('.fixture-container').removeClass('is-expanded has-pre').find('.fixture-content').hide();
						                if (elm.find('pre').length > 0) cls = 'has-pre';
						                elm.addClass('is-expanded ' + cls);
						                content.fadeIn(200);
					                }
					                $container.masonry();
				                }
			                });
                            $('.menu li').click(function(evt) {
				                var elm = $(this).children('span');
				                if (elm.hasClass('selected'))
					                return;
				                $('#' + $('.menu span.selected').removeClass('selected').attr('class')).hide();
				                $('#' + elm.attr('class')).fadeIn(200); elm.addClass('selected'); 
			                });
                            $('.select-toggle-type li').click(function() {
				                var ul = $('.select-toggle-type');
				                var elm = $(this);
				                if (elm.attr('class') == 'optToggle') {
					                ul.addClass('toggle').removeClass('accordion');
					                $('.optAccordion').find('i').removeClass('fa-toggle-on').addClass('fa-toggle-off');
					                elm.find('i').removeClass('fa-toggle-off').addClass('fa-toggle-on');
                                    $('.toggle-type-text').text('Toggle').parent().children('i').removeClass('fa-toggle-on').addClass('fa-toggle-off');
				                }
				                else {
					                ul.addClass('accordion').removeClass('toggle');
					                $('.optToggle').find('i').removeClass('fa-toggle-on').addClass('fa-toggle-off');
					                elm.find('i').removeClass('fa-toggle-off').addClass('fa-toggle-on');
                                    $('.toggle-type-text').text('Accordion').parent().children('i').removeClass('fa-toggle-off').addClass('fa-toggle-on');
				                }
			                });
			                $('.fixture-filter li').click(function() {
				                var elm = $(this);
				                if (elm.find('a').hasClass('clear-all')) { clearFilters(); return; }
				                var i = elm.find('i');
				                if (i.hasClass('filter-selected')) { resetCurrent(i); return; }
				                toggle_i();
				                i.removeClass('transparent').addClass('filter-selected');
				                $('.fixture-container').hide();
				                $('.fixture-result.' + elm.find('a').text().toLowerCase().trim()).closest('.fixture-container').show();
				                $('.fixtures').masonry();
			                });
			                $('.test-filter li').click(function() {
				                var elm = $(this);
				                if (elm.find('a').hasClass('clear-all')) { clearFilters(); return; }
				                var i = elm.find('i');
				                if (i.hasClass('filter-selected')) { resetCurrent(i); return; }
				                toggle_i();
				                i.removeClass('transparent').addClass('filter-selected');
				                $('tr, .fixture-container').removeClass('filtered').show();
				                var cls = '.' + elm.find('a').attr('id').replace(/-/g, ',.');
				                $('td:nth-child(2)').not(cls).closest('.fixture-container tr').addClass('filtered').fadeOut('fast');
				                $('.fixture-content').filter(function() {
					                return ($(this).find('tr.filtered').length == $(this).find('tr').length - 1);
				                }).closest('.fixture-container').hide();
				                $('.fixtures').masonry();
			                });
			                function clearFilters() {
				                $('tr, .fixture-container').removeClass('filtered').show();
				                $('.dropdown-menu i').filter(function() {
					                return $(this).hasClass('filter-selected');
				                }).removeClass('filter-selected').addClass('transparent');
				                $('.fixtures').masonry();
			                }
			                function resetCurrent(i) {
				                i.addClass('transparent').removeClass('filter-selected');
				                $('tr, .fixture-container').removeClass('filtered').show();
				                $('.fixtures').masonry();
			                }
			                function toggle_i() {
				                $('.dropdown-menu i').filter(function() {
					                return $(this).hasClass('filter-selected');
				                }).removeClass('filter-selected').addClass('transparent');
			                }
			                $('.select-toggle-type').val('optToggle');
			                $('.fixture-dropdown').dropdown();
			                $('.tests-dropdown').dropdown();
		                });
                        function writeLog(log) {
                            $('.filter-message span').text(log);
                        }
		                google.load('visualization', '1', {packages:['corechart']});
		                google.setOnLoadCallback(summary);
		                function summary() {
			                var data = google.visualization.arrayToDataTable([ 
				                ['Fixture Status', 'Count'], 
				                ['Pass', $('td.passed').length + $('td.success').length], 
				                ['Fail', $('td.failed').length + $('td.failure').length], 
				                ['Inconclusive/NotRunnable', $('td.inconclusive').length + $('td.notrunnable').length], 
				                ['Skipped', $('td.skipped').length + $('td.ignored').length + $('td.not-run').length], 
				                ['Invalid/Error', $('td.invalid').length + $('td.error').length] 
			                ]);
			                var chart = new google.visualization.PieChart(document.getElementById('summary'));
			                chart.draw(data, { chartArea: {left: 10, 'width': '85%', 'height': '85%'}, 
						                backgroundColor: { fill:'transparent' }, 
						                colors: ['#00af00', 'red', 'orange', '#1e90ff', 'tomato'], 
						                is3D: true,
						                pieSliceText: 'value', 
						                height: 225, 
						                width: 350 
					                });}
                    </script>
                </html>";
            }
        }

        /// <summary>
        /// Topbar is written to the source for Folder level summary
        /// It adds a go-back link to navigate back to the Index page
        /// </summary>
        public static string Topbar
        {
            get
            {
                return @"<div id='topbar'>
							<div class='topbar'>
								<a class='back' href='Index.html'><i class='fa fa-chevron-circle-left'></i> &nbsp; Executive Summary</a>
							</div>
						</div>";
            }
        }

        /// <summary>
        /// Fixture block
        /// Box in which all tests will be created
        /// It is masonry style and can be used as a toggle or accordion (default)
        /// </summary>
        public static string Fixture
        {
            get
            {
                return @"<div class='fixture-container'>
					        <div class='fixture-head'>
						        <span class='fixture-name'><!--%FIXTURENAME%--></span>
						        <span class='fixture-result <!--%FIXTURERESULT%-->'><!--%FIXTURERESULT%--></span>
					        </div>
					        <div class='fixture-content'>
						        <table class='simple-grey'>
							        <tr>
								        <th>TestName</th>
								        <th>Status</th>
							        </tr>
							        <!--%INSERTTEST%-->
						        </table>
					        </div>
                            <div <!--%FOOTERDISPLAY%--> class='fixture-footer'>
						        <span <!--%FIXTURESTARTEDATDISPLAY%--> class='startedAt'><i class='fa fa-clock-o'></i><!--%FIXTURESTARTEDAT%--></span>
						        <span <!--%FIXTUREENDEDATDISPLAY%--> class='endedAt'><i class='fa fa-clock-o'></i><!--%FIXTUREENDEDAT%--></span>
					        </div>
				        </div>
                        <!--%INSERTFIXTURE%-->";
            }
        }

        /// <summary>
        /// Test block
        /// Each test is added as a row inside the Fixture container
        /// </summary>
        public static string Test
        {
            get
            {
                return @"<tr>
							<td class='test-name'><!--%TESTNAME%--></td>
							<td class='<!--%TESTSTATUS%-->'><!--%TESTSTATUS%--><!--%TESTSTATUSMSG%--></td>
						</tr>
                        <!--%INSERTTEST%-->";
            }
        }

        public static string RunInfoRow
        {
            get
            {
                return @"<tr>
                            <td><!--%RUNINFOPARAM%--></td><td><!--%RUNINFOVALUE%--></td>
                        </tr>";
            }
        }

        /// <summary>
        /// Message to display when there are no tests in the TestResult.xml file
        /// </summary>
        public static string NoTestsMessage
        {
            get
            {
                return @"<div class='no-tests-available'>
                            <div role='alert' class='alert alert-danger'>
	                            <strong>No tests!</strong>  There were no tests found for <!--%INXML%-->.
                            </div>
                        </div>";
            }
        }

        public static string NoTestsCSS
        {
            get
            {
                return @".menu, #filters, #content, #tabs { display: none; }
                         #dashboard { padding: 40px 0 25px; }
                         .no-tests-available { margin: 0 auto; width: 99%; }";
            }
        }
    }
}
