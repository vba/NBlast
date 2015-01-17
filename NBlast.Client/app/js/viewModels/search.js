(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'knockout',
		'sammy',
		'moment',
		'services/markup',
		'services/search',
		'services/settings',
		'text!views/search'
	];
	define(dependencies, function(_,
								  $,
								  ko,
								  sammy,
								  moment,
								  markupService,
								  searchService,
								  settings,
								  searchView) {
		var SearchViewModel = function(page, query) {
			if (!_.isNumber(page)) {
				throw new Error('page param must be a number');
			}
			if (!_.isString(query)) {
				throw new Error('query param must be a string');
			}
			this.searchResult = ko.observable({});
			this.page = ko.observable(page);
			this.query = ko.observable(query);
			this.moment = moment;
		};

		SearchViewModel.prototype = {
			getPages: function () {
				var total = this.searchResult().total,
					amount;
				if (!_.isNumber(total)) {
					return [];
				}
				amount = Math.ceil(total / settings.getItemsPerPage());
				return _.range(1, amount + 1);
			},
			defineFoundIcon: function (level) {
				return {
					'DEBUG': 'cog',
					'INFO': 'info',
					'WARN': 'warning',
					'ERROR': 'bolt',
					'FATAL': 'fire'
				}[level.toUpperCase()] || 'asterisk';
			},
			getFoundHits: function() {
				return this.searchResult().hits || [];
			},
			getSearchResume: function() {
				var result = this.searchResult();
				return [result.total, ' record(s) found in ', result.queryDuration, ' ms'].join('');
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
				if (sammy().getLocation() === path) {
					sammy().runRoute('get', path);
				} else {
					sammy().setLocation(path);
				}
				return false;
			},
			bind: function() {
				var me = this;
				markupService.applyBindings(this, searchView);
				searchService.search(this.query())
					.done(function(data) {
						me.searchResult(data || {});
					});
			}
		};
		return SearchViewModel;
	});
})();
