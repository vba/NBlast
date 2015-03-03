(function() {
	'use strict';
	//#JSCOVERAGE_IF
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
		getNotification: function() {
			return require('../views/notification.html');
		},
		getDetails: function() {
			return require('../views/details.html');
		}
	};
	module.exports = views;
	//#JSCOVERAGE_ENDIF
})();