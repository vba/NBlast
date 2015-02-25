(function() {
	'use strict';
	var $        = require('../config').jquery(),
	    _        = require('underscore'),
		settings = require('./settings'),
		dashboard;

	function getLevelsPer(granularity, number) {
		var resourcePart, url;
		if (!/^(?:month|week|day)$/ig.test(granularity)) {
			throw new Error ('Unexpected granularity ' + granularity);
		}
		if (!_.isNumber(number)) {
			throw new Error ('Number expected instead of ' + number);
		}
		resourcePart = [number, '/', 'levels-per-' + granularity.toLowerCase()].join('');
		url = settings.appendToBackendUrl('dashboard/' + resourcePart);

		return $.getJSON(url);
	}

	dashboard = {
		groupBy: function (field, limit) {
			var resourcesPart = [field, '/', parseInt(limit, 10) || 10].join(''),
				url = settings.appendToBackendUrl('dashboard/group-by/' + resourcesPart);
			return $.getJSON(url);
		},
		getLevelsPerMonth: function (number) {
			return getLevelsPer('month', number);
		},
		getLevelsPerWeek: function (number) {
			return getLevelsPer('week', number);
		},
		getLevelsPerDay: function (number) {
			return getLevelsPer('day', number);
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings.isTestEnv() ? dashboard : Object.freeze(dashboard);
})();

