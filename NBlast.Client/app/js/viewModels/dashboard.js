(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'knockout',
		'mozart',
		'services/dashboard',
		'services/markup',
		'text!views/dashboard'
	];
	define(dependencies, function(_,
	                              ko,
	                              ctor,
	                              dashboardService,
	                              markupService,
	                              dashboardView) {

		//noinspection JSUnusedGlobalSymbols,UnnecessaryLocalVariableJS
		var DashboardViewModel = ctor(function(prototype){
			prototype.init = function () {
				this.levelCounters = ko.observable({});
				this.topSenders = ko.observableArray([]);
				this.topLoggers = ko.observableArray([]);
			};
			prototype.onGroupByLevelDone = function (data) {
				var counters = _.reduce(data.facets || [], function (aggregator, counter) {
					aggregator[counter.name.toLowerCase()] = counter.count;
					return aggregator;
				}, {});
				this.levelCounters(counters);
			};
			prototype.onGroupBySenderDone = function (data) {
				this.topSenders(data.facets || []);
			};
			prototype.onGroupByLoggerDone = function (data) {
				this.topLoggers(data.facets || []);
			};
			prototype.searchExactUri = function (name) {
				return '#/search/' + encodeURIComponent(['"', name, '"'].join(''));
			};
			prototype.bind = function() {
				markupService.applyBindings(this, dashboardView);

				dashboardService
					.groupBy('level')
					.done(this.onGroupByLevelDone.bind(this));

				dashboardService
					.groupBy('sender', 7)
					.done(this.onGroupBySenderDone.bind(this));

				dashboardService
					.groupBy('logger', 7)
					.done(this.onGroupByLoggerDone.bind(this));
			};
		});
		return DashboardViewModel;
	});
})();
