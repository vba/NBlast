(function() {
	'use strict';
	var dependencies = [
		'services/markup',
		'text!views/dashboard'
	];
	define(dependencies, function(markupService, dashboardView) {
		var DashboardViewModel = function() {};
		return {
			bind: function() {
				markupService.applyBindings(new DashboardViewModel(), dashboardView);
			}
		}
	})
})();