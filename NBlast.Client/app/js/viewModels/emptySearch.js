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
			this.page = ko.observable();
			this.query = ko.observable();
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
				var query = this.query() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
//					path = ['/#/search/', query].join('');

				console.log("Redirect to "+path);
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
