(function() {
	'use strict';
	var dependencies = [
		'jquery',
		'underscore',
		'services/settings'
	];
	define(dependencies, function($, _, settings) {
		return {
			search: function(query) {
				var url = settings.appendToBackendUrl('searcher/search'),
					params = {
						q: query || '*:*'
					};
				return $.getJSON(url, params);
			},
			getById: function(uuid) {
				var url = settings.appendToBackendUrl('searcher/' + uuid + '/get');
				return $.getJSON(url);
			}
		}
	});
})();


