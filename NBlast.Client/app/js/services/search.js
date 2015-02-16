(function () {
	'use strict';
	var _        = require('underscore'),
		config   = require('../config'),
		settings = require('./settings'),
		Class    = require('jsface').Class,
		SearchService;


	SearchService = Class(function () {
		var $private = {}, $public;

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

		$private.getRequester = function() {
			var $ = config.jquery();
			return $.getJSON;
		};

		$public = {
			$singleton: true,
			search: function (query) {
				var url = settings.appendToBackendUrl('searcher/search'),
					params = $private.prepareSearchParams(query || {});

				return $private.getRequester()(url, params);
			},
			searchByTerm: function (query) {
				var url = settings.appendToBackendUrl('term-searcher/search'),
					params = $private.prepareSearchParams(query || {});

				return $private.getRequester()(url, params);
			},
			getById: function (uuid) {
				var url = settings.appendToBackendUrl('searcher/' + uuid + '/get');
				return $private.getRequester()(url);
			}
		};
		return $public;
	});

    //noinspection JSUnresolvedVariable
	module.exports = SearchService;

})();