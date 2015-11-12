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

	/* control content container position for vertical scroll */
	$(window).scroll(function() {
		var scrollTop = $('.dashboard').is(':visible') ? 330 : 85;
		
		if ($(window).scrollTop() > scrollTop) {
			var margin = $('.dashboard').is(':visible') ? '-' + scrollTop + 'px' : '-45px';
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
	if ($('.report-item').length <= 1) {
		$('#slide-out').addClass('hide');
		
		pinWidth = '56%';
		
		$('.pin').css('width', pinWidth);
		$('.main-wrap, nav').css('padding-left', '20px');
	}

	/* toggle dashboard on 'Enable Dashboard' click */
	$('#enableDashboard').click(function() {
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
	
	function selectVisSuite() {
		$('.suite:visible').get(0).click();
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