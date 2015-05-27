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
                        <html lang='en'>
                            <!--
                                ReportUnit [TestSuite Summary] v1.0 | http://reportunit.github.io/
                                Created by Anshoo Arora | Released under the <> license
                            --> 
                            <head>
                                <meta charset='utf-8'>
                                <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                                <meta name='viewport' content='width=device-width, initial-scale=1'>
                                <meta name='description' content=''>
                                <meta name='author' content=''>
                                <title>ReportUnit TestRunner Report</title>
                                <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/css/materialize.min.css'>
                                <style>
                                    html { 
                                        background-color: #f6f7fa;  
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
                                    th {
                                        font-weight: 400;
                                    }
                                    td:first-child {
                                        font-weight: 300;
                                        max-width: 400px;
                                        min-width: 150px;
                                        padding-right: 25px;
                                    }
                                    td {
                                        padding: 8px;
                                        word-break: break-all;
                                    }

                                    /* ---- [ global ] ---- */
                                    .card-head {
                                        padding-bottom: 20px;
                                    }
                                    .card-panel {
                                        box-shadow: 0 1px 1px 0 rgba(0, 0, 0, 0.16), 0 2px 10px 0 rgba(0, 0, 0, 0.12);
                                    }
                                    .btn {
                                        font-size: 12px;
                                    }
                                    .btn i {
                                        font-size: 13px;
                                    }
            
                                    /* ---- [ sidenav ] ---- */
                                    .side-nav a {
                                        line-height: 22px;
                                        height: auto;
                                    }
                                    .side-nav.fixed a {
                                        height: auto;
                                        padding: 20px 15px;
                                    }
                                    .logo {
                
                                        border-bottom: 1px solid #ddd;
                                    }
                                    .logo, .logo a {
                                        cursor: default !important;
                
                                    }
                                    .logo:hover, .logo a:hover {
                                        background-color: transparent !important;
                                    }
                                    .logo-single {
                                        display: none;
                                    }
                                    .nav-main {
                                        padding-left: 325px;
                                    }
            
                                    /* ---- [ main ] ---- */
                                    .main {
                                        background-color: #f6f7fa;
                                        padding-left: 300px;
                                    }
                                    .main-wrap {
                                        padding: 40px 20px 100px;
                                    }
                                    .main-wrap > .row {
                                        margin-bottom: 0;
                                    }
            
                                        /* ---- [ dashboard ] ---- */
                                        .card-panel {
                                            padding: 15px;
                                        }
                                        .card-panel > div {
                                            font-size: 14px;
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
                
                                        /* ---- [ filters ] ---- */
                                        .filters {
                                            padding-left: 10px;
                                            padding-top: 50px;
                                        }
                                        .toggle-type {
                                            float: right;
                                            margin-right: 20px;
                                            margin-top: 35px;
                                        }
                                        .btn, .btn-large {
                                            background-color: #ee6e73;
                                        }
                                        .dropdown-content li > a, .dropdown-content li > span {
                                            font-size: 14px;
                                            padding: 7px 20px;
                                        }
                
                                        /* ---- [ fixture / suites ] ---- */
                                        .fixtures .card-panel {
                                            border: 1px solid #9d9d9d;
                                            box-shadow: none;
                                            cursor: pointer;
                                        }
                                        .fixture-head { 
                                            padding-bottom: 60px;
                                            word-break: break-all;                 
                                        }
                                        .fixture-name {
                                            float: left; 
                                            font-size: 20px;
                                            font-weight: 500;
                                            max-width: 410px; 
                                            word-break: break-all; 
                                        }
                                        .fixture-footer { 
                                            padding: 10px 0; 
                                        }
                                        .fixture-result {
                                            border-radius: 0.25em;
                                            color: #fff !important;
                                            float: right;
                                            font-size: 11px !important; 
                                            margin-top: 3px;
                                            padding: 2px 7px 1px; 
                                            text-transform: uppercase !important; 
                                        }
                                        p.description {
                                            border: 1px solid #ddd;
                                            color: #222;
                                            font-size: 14px;
                                            font-weight: 300;
                                            padding: 10px;
                                        }
                                        .startedAt, .endedAt { 
                                            font-size: 13px; 
                                        }
                                        .startedAt i, .endedAt i { 
                                            font-size: 13px; 
                                            padding-right: 5px; 
                                        }
                                        .startedAt i { 
                                            color: #5cb85c; 
                                        }
                                        .endedAt i { 
                                            color: #d9534f; 
                                            padding-left: 10px;
                                        }
                                        .fixture-result.fail, .fixture-result.failed, .fixture-result.failure {
                                            background-color: #eb4549;
                                        }
                                        .fixture-result.pass, .fixture-result.success, .fixture-result.passed {
                                            background-color: #32cd32;
                                        }
                                        .fixture-result.error { 
                                            background-color: tomato; 
                                        }
                                        .fixture-result.warning, .fixture-result.bad, .fixture-result.inconclusive { 
                                            background-color: orange; 
                                        }                    
                                        .fixture-result.skipped, .fixture-result.not-run, .fixture-result.notrun { 
                                            background-color: #1e90ff; 
                                        }
                                        .fixture-content { 
                                            cursor: auto !important;
                                            display: none; 
                                            padding-bottom: 20px; 
                                        }
                                        .is-expanded { 
                                            color: #000; 
                                            height: auto; 
                                        }
                                        .failed, .failure, .passed, .success, .warning, .bad, .inconclusive, .skipped, .ignored, .invalid, .error, .not-run, .notrun { 
                                            text-transform: capitalize; 
                                        }
                                        .failed, .failure { 
                                            color: red; 
                                        }
                                        .error { 
                                            color: tomato; 
                                        }
                                        .passed, .success { 
                                            color: #5cb85c; 
                                        } 
                                        .warning, .bad, .inconclusive, .error { 
                                            color: #f0ad4e; 
                                        } 
                                        .skipped, .not-run, .notrun, .ignored { 
                                            color: #1e90ff; 
                                        }

                                    /* ---- [ single ] ---- */
                                    .single #slide-out {
                                        display: none !important;
                                    }
                                    .single .main {
                                        padding-left: 0 !important;
                                    }
                                    .single .main-wrap {
                                        margin: 0 auto;
                                        width: 85%;
                                    }
                                    .single .nav-main {
                                        margin: 0 auto;
                                        padding-left: 20px;
                                    }
                                    .single .page-title span:first-child {
                                        display: none !important;
                                    }
                                    .single .logo-single {
                                        display: inline-block !important;
                                    }

                                    /* ---- [ media queries ] ---- */
                                    @media (max-width: 992px) {
                                        .main {
                                            padding-left: 0;
                                        }
                                        .nav-main {
                                            padding-left: 100px;
                                        }
                                        .button-collapse {
                                            padding-left: 10px;
                                        }
                                        .single .nav-main {
                                            padding-left: 100px;
                                        }
                                        .single .main-wrap {
                                            width: 95%;
                                        }
                                    }
                                </style>
                                <!--%OPTIONALCSS%-->
                            </head>
                            <body>    
                                <div class='header'>
                                    <nav>
                                        <ul id='slide-out' class='side-nav fixed'>
                                            <li class='logo'><a href='#'><i class='mdi-hardware-desktop-mac'></i>&nbsp;&nbsp;ReportUnit</a></li>
                                            <!--%NAV%-->
                                        </ul>
                                        <a href='#' data-activates='slide-out' class='button-collapse'><i class='mdi-navigation-menu'></i></a>
                                        <div class='nav-main'>
                                            <div class='page-title'>
                                                <span><i class='mdi-file-folder-open'></i>&nbsp;&nbsp;<!--%FILENAME%--></span>
                                                <span class='logo-single'><i class='mdi-hardware-desktop-mac'></i>&nbsp;&nbsp;ReportUnit</span>
                                            </div>
                                        </div>
                                    </nav>
                                </div>
                                <div class='main'>
                                    <div class='main-wrap'>
                                        <!--%NOTESTSMESSAGE%-->
                                        <div class='row'>
                                            <div class='col s12 m4 l2'>
                                                <div class='card-panel'>
                                                    <div>Total Tests</div>
                                                    <div class='chart total-tests' data-percent=''><span class='percent'><!--%TOTALTESTS%--></span></div>
                                                </div>
                                            </div>
                                            <div class='col s12 m4 l2'>
                                                <div class='card-panel'>
                                                    <div>Passed</div>    
                                                    <div class='chart total-passed' data-percent=''><span class='percent'><!--%PASSED%--></span></div>
                                                </div>
                                            </div>
                                            <div class='col s12 m4 l2'>
                                                <div class='card-panel'>
                                                    <div>Failed</div>
                                                    <div class='chart total-failed' data-percent=''><span class='percent'><!--%FAILED%--></span></div>
                                                </div>
                                            </div>
                                            <div class='col s12 m4 l2'>
                                                <div class='card-panel'>
                                                    <div>Inconclusive</div>
                                                    <div class='chart total-inconclusive' data-percent=''><span class='percent'><!--%INCONCLUSIVE%--></span></div>
                                                </div>
                                            </div>
                                            <div class='col s12 m4 l2'>
                                                <div class='card-panel'>
                                                    <div>Errors</div>
                                                    <div class='chart total-errors' data-percent=''><span class='percent'><!--%ERRORS%--></span></div>
                                                </div>
                                            </div>
                                            <div class='col s12 m4 l2'>
                                                <div class='card-panel'>
                                                    <div>Skipped</div>
                                                    <div class='chart total-skipped' data-percent=''><span class='percent'><!--%SKIPPED%--></span></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='row'>
                                            <div class='filters'>
                                                <div class='input-field col s3 suite-toggle'>
                                                    <select>
                                                        <option value='0' selected>Choose your option</option>
                                                        <option value='1'>Passed</option>
                                                        <option value='2'>Failed</option>
                                                        <option value='3'>Error</option>
                                                        <option value='4'>Inconclusive</option>
                                                        <option value='5'>Skipped</option>
                                                        <option value='skipped'>Clear Filters</option>
                                                    </select>
                                                    <label>Filter Suites</label>
                                                </div>
                                                <div class='input-field col s3 tests-toggle'>
                                                    <select>
                                                        <option value='0' selected>Choose your option</option>
                                                        <option value='1'>Passed</option>
                                                        <option value='2'>Failed</option>
                                                        <option value='3'>Error</option>
                                                        <option value='4'>Inconclusive</option>
                                                        <option value='5'>Skipped</option>
                                                        <option value='skipped'>Clear Filters</option>
                                                    </select>
                                                    <label>Filter Tests</label>
                                                </div>
                                                <div class='switch toggle-type'>
                                                    <label>
                                                        Accordion
                                                        <input type='checkbox'>
                                                        <span class='lever'></span>
                                                        Toggle
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='row'>
                                            <div class='fixtures'>
                                                <!--%INSERTFIXTURE%-->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </body>
                            <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>
                            <script src='http://cdnjs.cloudflare.com/ajax/libs/masonry/3.2.2/masonry.pkgd.min.js' type='text/javascript' charset='utf-8'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/easy-pie-chart/2.1.4/jquery.easypiechart.min.js'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/materialize/0.96.1/js/materialize.min.js'></script>
                            <script type='text/javascript'>
                                $(document).ready(function() {
                                    $('.button-collapse').sideNav({
                                        menuWidth: 300
                                    });
                                    $('select').material_select();
                                    resetFilters();
                                    var total = $('.total-tests > span').text();
                                    var passed = $('.total-passed > span').text();
                                    var failed = $('.total-failed > span').text();
                                    var inconclusive = $('.total-inconclusive > span').text();
                                    var errors = $('.total-errors > span').text();
                                    var skipped = $('.total-skipped > span').text();
                                    $('.total-tests').easyPieChart({ lineWidth: 12,  trackColor: '#f1f2f3', barColor: '#9c27b0', lineCap: 'butt', scaleColor: '#fff', size: 100 });
                                    $('.total-tests').data('easyPieChart').update('100');
                                    $('.total-passed').easyPieChart({ lineWidth: 12,  trackColor: '#f1f2f3', barColor: '#53b657', lineCap: 'butt', scaleColor: '#fff', size: 100 });
                                    $('.total-passed').data('easyPieChart').update(passed / total * 100);
                                    $('.total-failed').easyPieChart({ lineWidth: 12,  trackColor: '#f1f2f3', barColor: '#f8576c', lineCap: 'butt', scaleColor: '#fff', size: 100 });
                                    $('.total-failed').data('easyPieChart').update(failed / total * 100);
                                    $('.total-inconclusive').easyPieChart({ lineWidth: 12,  trackColor: '#f1f2f3', barColor: '#ffc107', lineCap: 'butt', scaleColor: '#fff', size: 100 });
                                    $('.total-inconclusive').data('easyPieChart').update(inconclusive / total * 100);
                                    $('.total-errors').easyPieChart({ lineWidth: 12,  trackColor: '#f1f2f3', barColor: 'tomato', lineCap: 'butt', scaleColor: '#fff', size: 100 });
                                    $('.total-errors').data('easyPieChart').update(errors / total * 100);
                                    $('.total-skipped').easyPieChart({ lineWidth: 12,  trackColor: '#f1f2f3', barColor: 'dodgerblue', lineCap: 'butt', scaleColor: '#fff', size: 100 });
                                    $('.total-skipped').data('easyPieChart').update(skipped / total * 100);
                                    $('.nav.nav-sidebar a').filter(function(){
                                        return this.href.match(/[^\/]+$/)[0] == document.location.pathname.match(/[^\/]+$/)[0];
                                    }).parent().addClass('active');
                                    var $container = $('.fixtures').masonry({
                                        percentPosition: true
                                    });
                                    $(document).ready(sizing);
                                    $(window).resize(sizing);
                                    function sizing() {
                                        if ($(window).width() > 1650) {
                                            $('.fixtures .s4').css('width', '33.33%');
                                        } else if ($(window).width() < 1120) {
                                            $('.fixtures .s4').css('width', '99%');
                                        } else {
                                            $('.fixtures .s4').css('width', '49%');
                                        }
                                        $('.fixtures').masonry();
                                    }
                                    $container.on( 'click', '.card-panel', function(evt) {
                                        var cls = evt.target.className;
                                        cls = evt.target.nodeName.toLowerCase();
                                        if (cls.indexOf('div') >= 0 || cls.indexOf('span') >= 0) {
                                            var elm = $(this);
                                            var content = elm.find('.fixture-content');
                                            cls = '';
                                            if (content.is(':visible')) {
                                                elm.removeClass('is-expanded has-pre');
                                                content.hide(0);
                                            }
                                            else {
                                                if (!$('.toggle-type input').prop('checked'))
                                                    $('.fixtures .card-panel').removeClass('is-expanded').find('.fixture-content').hide(0);
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
                                        $('#' + $('.menu span.selected').removeClass('selected').attr('class')).hide(0);
                                        $('#' + elm.attr('class')).fadeIn(200); elm.addClass('selected'); 
                                    });
                                    $('.suite-toggle li').click(function() {
                                        var opt = $(this).text().toLowerCase();
                                        if (opt != 'choose your option') {
                                            if (opt == 'clear filters') {
                                                resetFilters();
                                            } else {
                                                $('.fixtures .card-panel').hide(0);
                                                $('.fixture-result.' + opt).closest('.card-panel').show(0);
                                            }
                                            $('.fixtures').masonry();
                                        }                                
                                    });
                                    $('.tests-toggle li').click(function() {
                                        var opt = $(this).text().toLowerCase();
                                        if (opt != 'choose your option') {
                                            if (opt == 'clear filters') {
                                                resetFilters();
                                            } else {
                                                if ($('tr.has-filter').length > 0) 
                                                    resetFilters();
                                                $('.fixture-content td:nth-child(2)').not('.' + opt).parent().addClass('has-filter').hide(0);
                                                $('.fixture-content').filter(function() {
                                                    return ($(this).find('tr.has-filter').length == $(this).find('tr').length - 1);
                                                }).closest('.card-panel').hide(0);
                                            }
                                            $('.fixtures').masonry();
                                        }
                                    });
                                    function resetFilters() {
                                        $('.fixtures .card-panel').show(0);
                                        $('tr').removeClass('has-filter').show();
                                        $('.suite-toggle li:first-child, .tests-toggle li:first-child').click();
                                    }
                                });
                            </script>
                        </html>";
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
                return @"<div class='col s4'>
                            <div class='card-panel'>
                                <div class='fixture-head'>
                                    <span class='fixture-name'><!--%FIXTURENAME%--></span>
                                    <span class='fixture-result <!--%FIXTURERESULT%-->'><!--%FIXTURERESULT%--></span>
                                </div>
                                <div class='fixture-content'>
                                    <table class='bordered'>
                                        <tr>
                                            <th>TestName</th>
                                            <th>Status</th>
                                        </tr>
                                        <!--%INSERTTEST%-->
                                    </table>
                                </div>
                                <div <!--%FOOTERDISPLAY%--> class='fixture-footer'>
                                    <span <!--%FIXTURESTARTEDATDISPLAY%--> class='startedAt'><i class='mdi-device-access-time'></i><!--%FIXTURESTARTEDAT%--></span>
                                    <span <!--%FIXTUREENDEDATDISPLAY%--> class='endedAt'><i class='mdi-device-access-time'></i><!--%FIXTUREENDEDAT%--></span>
                                </div>
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
                return @"<style>.row { display: none !important; }</style>";
            }
        }
    }
}
