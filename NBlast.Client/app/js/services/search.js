(function() {
	'use strict';
	var dependencies = [
		'jquery',
		'underscore',
		'services/settings'
	];
	define(dependencies, function($, _, settings) {
	/*
		'use strict';
		angular.module('nblast')
			.service('searchService', [
				'$resource',
				'configService',
				function ($resource, configService) {
					var searchUrl = configService.appendToBackendUrl('searcher/search'),
						jsonFormat = configService.getJsonFormat(),
						xhr = $resource(searchUrl, null, {
							search: {
								'method': 'GET'
							}
						});

					return _.extend({
						getById: function(id) {
							return $resource(
								configService.appendToBackendUrl('searcher/' + id + '/get'),
								null,
								{ get: { 'method': 'GET' } }
							).get();
						}
					}, xhr);
			}]);
	*/
		return {
			search: function(query) {
				var searchUrl = settings.appendToBackendUrl('searcher/search'),
					params = {
						q: query = encodeURIComponent(query || '*:*')
					};
				return $.getJSON(searchUrl, params);
			},
			getById: function(id) {}
		}
	});
})();


