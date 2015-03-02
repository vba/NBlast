(function() {
	'use strict';
	var views = {
		getSearch: function() {
			return require('../views/search.html');
		},
		getDashboard: function() {
			return require('../views/dashboard.html');
		},
		getSettings: function() {
			return require('../views/settings.html');
		},
		getDetails: function() {
			return require('../views/details.html');
		}
	};

	module.exports = views;
})();