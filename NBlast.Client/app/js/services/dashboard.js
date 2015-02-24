(function() {
	'use strict';
	var $        = require('../config').jquery(),
		settings = require('./settings'),
		dashboard;

	dashboard = {
		groupBy: function (field, limit) {
			var resourcesPart = [field, '/', parseInt(limit, 10) || 10].join(''),
				url = settings.appendToBackendUrl('dashboard/group-by/' + resourcesPart);
			return $.getJSON(url);
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings.isTestEnv() ? dashboard : Object.freeze(dashboard);
})();

