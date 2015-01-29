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
			var result = {};
			result.q = _.isString(query) ? query : query.expression || '*:*';
			result.p = query.page || 1;
			result.sf = query.sortField || "";
			result.sr = !!query.sortReverse || "";
			result.from = query.from || "";
			result.till = query.from || "";
			return result;
		};
		return {
			search: function (query) {
				var url = settings.appendToBackendUrl('searcher/search/'),
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