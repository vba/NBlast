(function () {
	'use strict';

	var config            = require('../../config'),
	    dashboardService  = require('../../services/dashboard'),
	    Chartist          = config.chartist(),
	    moment            = config.moment(),
		ActivityChart;

	ActivityChart = (function () {

		function ActivityChart() {
			var $ = config.jquery();
			this.tooltip = $('#activityChartTooltip');
		}

		var $ = config.jquery(), $private = {
			onMouseEnter: function (e) {
				var point = $(e.target),
				    value = point.attr('ct:value'),
				    seriesName = point.parent().attr('ct:series-name');

				this.tooltip.html(seriesName + ': ' + value).removeClass('hide');
			},
			onMouseLeave: function () {
				this.tooltip.addClass('hide');
			},
			onMouseMove: function (e) {
				this.tooltip.css({
					left: (e.offsetX || e.originalEvent.layerX) + (this.tooltip.width() / 4),
					top : (e.offsetY || e.originalEvent.layerY) - this.tooltip.height() + 45
				});
			},
			initEvents: function () {
				$('#levelsActivityChart')
					.on('mouseenter', '.ct-point', $private.onMouseEnter.bind(this))
					.on('mouseleave', '.ct-point', $private.onMouseLeave.bind(this))
					.on('mousemove', $private.onMouseMove.bind(this));
			},
			afterRequest: function (month0, month1, month2, month3, month4) {
				var m0 = month0[0], m1 = month1[0], m2 = month2[0],
				    m3 = month3[0], m4 = month4[0],
				    data = {
					    labels: [
						    moment().subtract(4, 'months').format('MMMM'),
						    moment().subtract(3, 'months').format('MMMM'),
						    moment().subtract(2, 'months').format('MMMM'),
						    moment().subtract(1, 'months').format('MMMM'),
						    moment().format('MMMM')
					    ],
					    series: [
						    {name: "Trace", data: [m4.trace, m3.trace, m2.trace, m1.trace, m0.trace]},
						    {name: "Debug", data: [m4.debug, m3.debug, m2.debug, m1.debug, m0.debug]},
						    {name: "Info", data:  [m4.info,  m3.info,  m2.info,  m1.info,  m0.info]},
						    {name: "Warn", data:  [m4.warn,  m3.warn,  m2.warn,  m1.warn,  m0.warn]},
						    {name: "Error", data: [m4.error, m3.error, m2.error, m1.error, m0.error]},
						    {name: "Fatal", data: [m4.fatal, m3.fatal, m2.fatal, m1.fatal, m0.fatal]}
					    ]
				    },
				    line = new Chartist.Line('#levelsActivityChart', data);
					$private.initEvents.call(this);
				return line;
			}
		};

		ActivityChart.prototype.render = function () {
			var request = dashboardService.getLevelsPerMonth;

            return $.when(request(0),
			              request(-1),
			              request(-2),
			              request(-3),
			              request(-4)).then($private.afterRequest.bind(this));
		};
		return ActivityChart;
	})();

	module.exports = ActivityChart;
})();