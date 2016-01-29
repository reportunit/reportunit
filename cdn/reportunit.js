var menuWidth = 260;

function showDynamicModal(heading, content) {
    var m = $('#dynamicModal');
    m.find('h4').text(heading);
    m.find('pre').text(content);
    m.openModal({ in_duration: 200 });
}

$('.details-container').click(function(evt) {
    var t = $(evt.target);
    
    if (t.is('.showStatusMessage') || t.is('i')) {
        if (t.is('i')) {
            t = t.parent();
        }
        
        showDynamicModal(t.closest('tr').find('.name').text() + ' StatusMessage', t.next().text());
    }
    
    if (t.is('.showDescription')) {
        showDynamicModal(t.text() + ' Description', t.next().text());
    }
});

/* toggle dashboard on 'Enable Dashboard' click */
$('#enableDashboard').click(function() {
    $('.suite-list, .suite-details').toggleClass('v-spacer');
    $(this).toggleClass('enabled').children('i').toggleClass('active');
    $('.dashboard').toggleClass('hide');
});

/* show suite data on click */
$('.suite').click(function() {
    var t = $(this);
    
    $('.suite').removeClass('active');
    $('.suite-name-displayed, .details-container').html('');
    
    t.toggleClass('active');
    var html = t.find('.suite-content').html();
    
    $('.suite-name-displayed').text(t.find('.suite-name').text());
    $('.details-container').append(html);
});

$('#slide-out .report-item > a').filter(function(){
    return this.href.match(/[^\/]+$/)[0] == document.location.pathname.match(/[^\/]+$/)[0];
}).parent().addClass('active');

/* filters -> by suite status */
$('#suite-toggle li').click(function() {
    var t = $(this);
    
    if (!t.hasClass('clear')) {
        resetFilters();
        
        var status = t.text().toLowerCase();
        
        $('#suites .suite').addClass('hide');
        $('#suites .suite.' + status).removeClass('hide');
        
        selectVisSuite()
    }
});

/* filters -> by test status */
$('#tests-toggle li').click(function() {
    var t = $(this);

    if (!t.hasClass('clear')) {
        resetFilters();
        
        var opt = t.text().toLowerCase();
        
        $('.suite table tr.test-status:not(.' + opt + '), .details-container tr.test-status:not(.' + opt).addClass('hide');
        $('.suite table tr.test-status.' + opt + ', .details-container tr.test-status.' + opt).removeClass('hide');
        
        hideEmptySuites();
        selectVisSuite()
    }
});

/* filters -> by category */
$('#category-toggle li').click(function() {
    var t = $(this);

    if (!t.hasClass('clear')) {
        resetFilters();
        
        filterByCategory(t.text());
        selectVisSuite()
    }
});

$('.clear').click(function() {
    resetFilters(); selectVisSuite()
});

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

function selectVisSuite() {
    $('.suite:visible').get(0).click();
}

function clickListItem(listClass, index) {
    $('#' + listClass).find('li').get(index).click();
}

$(document).ready(function() {
	/* init */
	$('select').material_select();
	$('.modal-trigger').leanModal();
	$('.tooltipped').tooltip({delay: 10});

	/* for a single report item, hide sidenav */
	if ($('.report-item').length <= 1) {
		$('#slide-out').addClass('hide');
		
		pinWidth = '56.5%';
		
		$('.pin').css('width', pinWidth);
		$('.main-wrap, nav').css('padding-left', '20px');
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
	segmentShowStroke : false, 
	percentageInnerCutout : 55, 
	animationSteps : 1,
	legendTemplate : '<ul class=\'<%=name.toLowerCase()%>-legend\'><% for (var i=0; i<segments.length; i++){%><li><span style=\'background-color:<%=segments[i].fillColor%>\'></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'
};

/* report -> suites chart */
function suitesChart() {
	if (!$('#suite-analysis').length) {
		return false;
	}

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

/* test case counts */
var total = $('.test-name').length;
var passed = $('td.passed').length;
var failed = $('td.failed').length;
var inconclusive = $('td.inconclusive').length;
var errors = $('td.error').length;
var skipped = $('td.skipped').length;

/* report -> tests chart */
function testsChart() {
	if (!$('#test-analysis').length) {
		return false;
	}

    var data = {};
    
    if ($('body.summary').length > 0) {
        total = parseInt($('#total-tests').text());
        passed = parseInt($('#total-passed').text());
        failed = parseInt($('#total-failed').text());
        others = parseInt($('#total-others').text());
        
        data = [
            { value: passed, color: '#00af00', highlight: '#32bf32', label: 'Pass' },
            { value: failed, color:'#F7464A', highlight: '#FF5A5E', label: 'Fail' },
            { value: others, color: '#1e90ff', highlight: '#4aa6ff', label: 'Others' }
        ];
        
        $('.test-others-count').text(others);
    }
    else {
        data = [
            { value: passed, color: '#00af00', highlight: '#32bf32', label: 'Pass' },
            { value: failed, color:'#F7464A', highlight: '#FF5A5E', label: 'Fail' },
            { value: errors, color:'#ff6347', highlight: '#ff826b', label: 'Error' },
            { value: inconclusive, color: '#FDB45C', highlight: '#FFC870', label: 'Warning' },
            { value: skipped, color: '#1e90ff', highlight: '#4aa6ff', label: 'Skip' }
        ];
        
        $('.test-others-count').text(errors + inconclusive + skipped);
    }
	
	$('.test-pass-count').text(passed);
	$('.test-fail-count').text(failed);
	
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
