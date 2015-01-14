(function() {
	'use strict';
	var dependencies = [
		'services/markup',
		'text!views/search'
	];
	define(dependencies, function(markupService, searchView) {
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

		var SearchViewModel = function() {};
		return {
			bind: function() {
				markupService.applyBindings(new SearchViewModel(), searchView);
			}
		}
	});
})();


