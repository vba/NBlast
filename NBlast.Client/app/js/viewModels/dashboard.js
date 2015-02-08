(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'knockout',
		'jsface',
		'services/dashboard',
		'services/markup',
		'text!views/dashboard'
	];
	define(dependencies, function(_,
	                              ko,
	                              jsface,
	                              dashboardService,
	                              markupService,
	                              dashboardView) {

		//noinspection JSUnusedGlobalSymbols
		var DashboardViewModel = jsface.Class({
			constructor: function () {
				this.levelCounters = ko.observable({});
				this.topSenders = ko.observableArray([]);
				this.topLoggers = ko.observableArray([]);
			},
			onGroupByLevelDone: function (data) {
				var counters = _.reduce(data.facets || [], function (aggregator, counter) {
					aggregator[counter.name.toLowerCase()] = counter.count;
					return aggregator;
				}, {});
				this.levelCounters(counters);
			},
			onGroupBySenderDone: function (data) {
				this.topSenders(data.facets || []);
			},
			onGroupByLoggerDone: function (data) {
				this.topLoggers(data.facets || []);
			},
			searchExactUri: function (name) {
				return '#/search/' + encodeURIComponent(['"', name, '"'].join(''));
			},
			bind: function() {
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
			}
		});
		return DashboardViewModel;
	});
})();
