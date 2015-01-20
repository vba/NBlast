(function() {
	'use strict';
	var dependencies = [
		'jquery',
		'underscore',
		'services/settings'
	];
	define(dependencies, function($, _, settings) {
		return {
			search: function(query, page) {
				var url = settings.appendToBackendUrl('searcher/search/');
				return $.getJSON([url, page,'/', query].join(''));
			},
			getById: function(uuid) {
				var url = settings.appendToBackendUrl('searcher/' + uuid + '/get');
				return $.getJSON(url);
			}
		}
	});
})();


