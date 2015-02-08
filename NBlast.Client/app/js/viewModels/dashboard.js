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

		var DashboardViewModel = jsface.Class({
			constructor: function () {
				this.levelCounters = ko.observable({});
			},
			onGroupByLevelDone: function (data) {
				var counters = _.map(data.facets || [], function (counter) {
					var result = {};
					result.key = counter.name;
					result.value = counter.count;
					return result;
				});
				this.levelCounters(counters);
				debugger
			},
			bind: function() {
				markupService.applyBindings(this, dashboardView);

				dashboardService
					.groupBy('level')
					.done(this.onGroupByLevelDone.bind(this));
			}
		});
		return DashboardViewModel;
	});
})();
