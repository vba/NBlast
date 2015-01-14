(function() {
	'use strict';
	var dependencies = [
		'services/markup',
		'text!views/search'
	];
	define(dependencies, function(markupService, searchView) {
		var SearchViewModel = function() {};
		return {
			bind: function() {
				markupService.applyBindings(new SearchViewModel(), searchView);
			}
		}
	});
})();