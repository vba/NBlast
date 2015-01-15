(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'knockout',
		'services/markup',
		'text!views/search'
	];
	define(dependencies, function(_, ko, markupService, searchView) {
		var SearchViewModel = function(page, query) {
			if (!_.isNumber(page)) {
				throw new Error('page param must be a number');
			}
			if (!_.isString(query)) {
				throw new Error('query param must be a string');
			}

			this.page = ko.observable(page);
			this.query = ko.observable(decodeURIComponent(query));
		};

		SearchViewModel.prototype = {
			enterSearch : function(data, event) {
				if (event.keyCode === 13) {
					return this.makeSearch();
				};
				return true;
			},
			makeSearch : function() {
				console.log("Search");
				return false;
			}
		};
		return {
			bind: function(page, query) {
				var viewModel = new SearchViewModel(parseInt(page, 10), query);
				markupService.applyBindings(viewModel, searchView);
			}
		}
	});
})();