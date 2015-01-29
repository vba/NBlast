(function() {
	'use strict';
	var dependencies = [
		'knockout',
		'sammy',
		'services/markup',
		'text!views/search'
	];
	define(dependencies, function(ko, sammy, markupService, searchView) {
		var EmptySearchViewModel = function() {
			this.totalPages = ko.observable(0);
			this.page = ko.observable();
			this.expression = ko.observable();
			this.sortField = ko.observable('');
			this.sortReverse = ko.observable('false');
			this.filterFrom = ko.observable('');
			this.filterTill = ko.observable('');
			this.searchResult = false;
		};

		EmptySearchViewModel.prototype = {
			getFoundHits: function() {
				return [];
			},
			getPages: function() {
				return [];
			},
			getSearchResume: function() {
				return "";
			},
			enterSearch : function(data, event) {
				if (event.keyCode === 13) {
					return this.makeSearch();
				}
				return true;
			},
			makeSearch : function() {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');

				sammy().setLocation(path);
				return false;
			},
			bind: function() {
				markupService.applyBindings(this, searchView);
			}
		};

		return EmptySearchViewModel;
	});
})();
