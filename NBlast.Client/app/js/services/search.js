(function () {
	'use strict';
	var dependencies = [
		'jquery',
		'underscore',
		'moment',
		'services/settings'
	];
	define(dependencies, function ($, _, moment, settings) {
		var prepareSearchParams = function (query) {
			var result  = {};
			result.q    = _.isString(query) ? query : query.expression || '*:*';
			result.p    = query.page || 1;
			result.sf   = query.sort.field;
			result.sr   = query.sort.reverse;
			result.from = query.filter.from;
			result.till = query.filter.till;
			return result;
		};
		return {
			search: function (query) {
				var url    = settings.appendToBackendUrl('searcher/search/'),
					params = prepareSearchParams(query);

				return $.getJSON(url, params);
			},
			getById: function (uuid) {
				var url = settings.appendToBackendUrl('searcher/' + uuid + '/get');
				return $.getJSON(url);
			}
		};
	});
})();