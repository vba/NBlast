(function() {
	'use strict';

	var _                 = require('underscore'),
		ko                = require('knockout'),
		views             = require('../views'),
		markupService     = require('../services/markup'),
		config            = require('../config'),
		dashboardService  = require('../services/dashboard'),
		ActivityChart     = require('./partial/activityChart'),
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
		$private.renderMonthsChart = function() {
			return new ActivityChart().render();
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

			$private.renderMonthsChart();
		};

		return DashboardViewModel;
	})();

	//noinspection JSUnresolvedVariable
	module.exports = DashboardViewModel;
})();
