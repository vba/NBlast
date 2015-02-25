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
						var m0 = month0[0], m1 = month1[0], m2 = month2[0],
						    m3 = month3[0], m4 = month4[0],
							data = {
							labels: ['month0', 'month1', 'month2', 'month3', 'month4'],
							series: [
								[m0.trace, m1.trace, m2.trace, m3.trace, m4.trace],
								[m0.debug, m1.debug, m2.debug, m3.debug, m4.debug],
								[m0.info, m1.info, m2.info, m3.info, m4.info],
								[m0.warn, m1.warn, m2.warn, m3.warn, m4.warn],
								[m0.error, m1.error, m2.error, m3.error, m4.error],
								[m0.fatal, m1.fatal, m2.fatal, m3.fatal, m4.fatal]
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
