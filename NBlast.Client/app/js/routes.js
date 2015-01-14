(function() {
	'use strict';
	var dependencies = [
		'sammy', 
		'knockout'
	];
	define(dependencies, function(sammy, ko) {
		var routes = sammy(function(context) {
			var me = this;
		});

		return routes;
	});

})();