(function() {
	'use strict';

	var _                 = require('underscore'),
		ko                = require('knockout'),
		views             = require('../views'),
		config            = require('../config'),
		markupService     = require('../services/markup'),
		dashboardService  = require('../services/dashboard'),
		Chartist          = config.chartist(),
		StorageService    = require('../services/storage'),
		DashboardViewModel;

	//noinspection JSUnusedGlobalSymbols,UnnecessaryLocalVariableJS
	DashboardViewModel = (function() {
		function DashboardViewModel () {
			this.levelCounters = ko.observable({});
			this.topSenders = ko.observableArray([]);
			this.topLoggers = ko.observableArray([]);
			this.sammy = config.sammy();
		}

		var $private = {},
		    storageService = new StorageService();

		$private.onGroupByLevelDone = function (data) {
			var counters = _.reduce(data.facets || [], function (aggregator, counter) {
				aggregator[counter.name.toLowerCase()] = counter.count;
				return aggregator;
			}, {});
			this.levelCounters(counters);
		};
		$private.onGroupBySenderDone = function (data) {
			this.topSenders(data.facets || []);
		};
		$private.onGroupByLoggerDone = function (data) {
			this.topLoggers(data.facets || []);
		};
		$private.bindMonthsChart = function() {
			if (!Chartist.Bar) return;
			var $ = config.jquery(),
				result = $.when(dashboardService.getLevelsPerMonth(0),
								dashboardService.getLevelsPerMonth(-1),
								dashboardService.getLevelsPerMonth(-2),
								dashboardService.getLevelsPerMonth(-3),
								dashboardService.getLevelsPerMonth(-4))
					.then(function (month0, month1, month2, month3, month4) {
						var data = {
							labels: ['month0', 'month1', 'month2', 'month3', 'month4'],
							series: [
								[month0[0].trace, month0[0].debug, month0[0].info, month0[0].warn, month0[0].error, month0[0].fatal],
								[month1[0].trace, month1[0].debug, month1[0].info, month1[0].warn, month1[0].error, month1[0].fatal],
								[month2[0].trace, month2[0].debug, month2[0].info, month2[0].warn, month2[0].error, month2[0].fatal],
								[month3[0].trace, month3[0].debug, month3[0].info, month3[0].warn, month3[0].error, month3[0].fatal],
								[month4[0].trace, month4[0].debug, month4[0].info, month4[0].warn, month4[0].error, month4[0].fatal]
							]
						};

						return new Chartist.Bar('#levelsActivityChart', data);
					});
			return result;
		};

		//noinspection JSUnusedGlobalSymbols
		DashboardViewModel.prototype.searchTerm = function (type, value) {
			var path = '#/search/' + encodeURIComponent(value);
			storageService.clearAll();
			storageService.storeSearch(type);
			storageService.storeSort('createdAt', true);

			this.sammy().setLocation(path);
		};
		DashboardViewModel.prototype.bind = function() {
			markupService.applyBindings(this, views.getDashboard());

			dashboardService
				.groupBy('level')
				.done($private.onGroupByLevelDone.bind(this));

			dashboardService
				.groupBy('sender', 7)
				.done($private.onGroupBySenderDone.bind(this));

			dashboardService
				.groupBy('logger', 7)
				.done($private.onGroupByLoggerDone.bind(this));

			$private.bindMonthsChart();
		};

		return DashboardViewModel;
	})();

	//noinspection JSUnresolvedVariable
	module.exports = DashboardViewModel;
})();
