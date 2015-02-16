(function() {
	'use strict';

	var _                 = require('underscore'),
		ko                = require('knockout'),
		views             = require('../views'),
		Class             = require('jsface').Class,
		markupService     = require('../services/markup'),
		dashboardService  = require('../services/dashboard'),
		DashboardViewModel;

	//noinspection JSUnusedGlobalSymbols,UnnecessaryLocalVariableJS
	DashboardViewModel = Class(function() {
		var prototype = {}, $private = {};

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

		prototype.constructor = function () {
			this.levelCounters = ko.observable({});
			this.topSenders = ko.observableArray([]);
			this.topLoggers = ko.observableArray([]);
		};
		//noinspection JSUnusedGlobalSymbols
		prototype.searchExactUri = function (name) {
			return '#/search/' + encodeURIComponent(['"', name, '"'].join(''));
		};
		prototype.bind = function() {
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
		};

		return prototype;
	});

	//noinspection JSUnresolvedVariable
	module.exports = DashboardViewModel;
})();
