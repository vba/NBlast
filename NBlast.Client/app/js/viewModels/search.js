(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'knockout',
		'sammy',
		'services/markup',
		'services/search',
		'text!views/search'
	];
	define(dependencies, function(_,
								  $,
								  ko,
								  sammy,
								  markupService,
								  searchService,
								  searchView) {

		var SearchViewModel = function(page, query) {
			if (!_.isNumber(page)) {
				throw new Error('page param must be a number');
			}
			if (!_.isString(query)) {
				throw new Error('query param must be a string');
			}

			this.foundHits = ko.observableArray([]);
			this.page = ko.observable(page);
			this.query = ko.observable(decodeURIComponent(query));
		};

		SearchViewModel.prototype = {
			enterSearch : function(data, event) {
				if (event.keyCode === 13) {
					return this.makeSearch();
				}
				return true;
			},
			makeSearch : function() {
				var query = this.query() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');


				if (sammy().getLocation() === path) {
					sammy().runRoute('get', path)
				} else {
					sammy().setLocation(path);
				}
				return false;
			},
			bind: function() {
				var me = this;
				markupService.applyBindings(this, searchView);

				console.log("Search "+ this.query());

				searchService.search(this.query())
					.done(function(data){
						console.log(data);
						me.foundHits(data.hits || []);
					});

			}
		};
		return SearchViewModel;
	});
})();
