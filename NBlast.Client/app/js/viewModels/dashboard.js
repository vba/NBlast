(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'services/markup',
		'text!views/dashboard'
	];
	define(dependencies, function(_, markupService, dashboardView) {
		var DashboardViewModel = function() {};

		DashboardViewModel.prototype = _.extend({
			bind: function() {
				markupService.applyBindings(this, dashboardView);
			}
		});
		return DashboardViewModel;
	});
})();
