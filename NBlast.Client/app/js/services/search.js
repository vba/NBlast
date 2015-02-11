(function () {
	'use strict';
	var dependencies = [
		'jquery',
		'underscore',
		'moment',
		'services/settings'
	];
	define(dependencies, function ($, _, moment, settings) {
		var prepareSearchParams = function (q) {
			var result  = {};
			result.q    = _.isString(q) ? q : q.expression || '*:*';
			result.p    = q.page || 1;
			result.sf   = q.sort && q.sort.field || '';
			result.sr   = q.sort && q.sort.reverse || '';
			result.from = q.filter && q.filter.from || '';
			result.till = q.filter && q.filter.till || '';

			if (q.search && q.search.type) {
				result.k = q.search.type;
			}
			return result;
		};
		return {
			search: function (query) {
				var url    = settings.appendToBackendUrl('searcher/search'),
					params = prepareSearchParams(query || {});

				return $.getJSON(url, params);
			},
			searchByTerm: function (query) {
				var url    = settings.appendToBackendUrl('term-searcher/search'),
					params = prepareSearchParams(query || {});

				return $.getJSON(url, params);
			},
			getById: function (uuid) {
				var url = settings.appendToBackendUrl('searcher/' + uuid + '/get');
				return $.getJSON(url);
			}
		};
	});
})();