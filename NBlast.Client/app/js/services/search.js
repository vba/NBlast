(function () {
	'use strict';
	var _               = require('underscore'),
		settings        = require('./settings'),
		SearchService;

	SearchService = (function () {

		function SearchService() { }

		var $private = {};

		$private.prepareSearchParams = function (q) {
			var result = {};
			result.q = _.isString(q) ? q : q.expression || '*:*';
			result.p = q.page || 1;
			result.sf = q.sort && q.sort.field || '';
			result.sr = q.sort && q.sort.reverse || '';
			result.from = q.filter && q.filter.from || '';
			result.till = q.filter && q.filter.till || '';

			if (q.search && q.search.type) {
				result.k = q.search.type;
			}
			return result;
		};

		SearchService.prototype.search = function (query) {
			var url = settings.appendToBackendUrl('searcher/search'),
				params = $private.prepareSearchParams(query || {});
			return settings.makeRequest(url, params);
		};

		SearchService.prototype.searchByTerm = function (query) {
			var url = settings.appendToBackendUrl('term-searcher/search'),
				params = $private.prepareSearchParams(query || {});
			return settings.makeRequest(url, params);
		};

		SearchService.prototype.getById = function (uuid) {
			var url = settings.appendToBackendUrl('searcher/' + uuid + '/get');
			return settings.makeRequest(url);
		};

		SearchService.prototype.getPathName = function () {
			return (location && location.pathname) || '/';
		};

		return SearchService;
	})();

    //noinspection JSUnresolvedVariable
	module.exports = new SearchService();

})();