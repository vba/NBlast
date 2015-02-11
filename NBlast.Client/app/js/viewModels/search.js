(function () {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'knockout',
		'sammy',
		'amplify',
		'jsface',
		'services/markup',
		'services/search',
		'services/settings',
		'text!views/search',
		'viewModels/baseSearch'
	];
	define(dependencies, function (_,
	                               $,
	                               ko,
	                               sammy,
	                               amplify,
	                               jsface,
	                               markupService,
	                               searchService,
	                               settings,
	                               searchView,
	                               BaseSearchViewModel) {

		//noinspection JSUnusedGlobalSymbols
		var SearchViewModel = jsface.Class(BaseSearchViewModel, {
			constructor: function (page, expression) {

				SearchViewModel.$super.call(this);

				if (!_.isNumber(page)) {
					throw new Error('page param must be a number');
				}
				if (!_.isString(expression)) {
					throw new Error('expression param must be a string');
				}

				this.searchResult = ko.observable({});
				this.page = ko.observable(page);
				this.expression = ko.observable(expression);
				this.sammy = sammy();
			},
			getPages: function () {
				var lower, upper,
					index = 1,
					links = 10,
					totalPages = this.totalPages(),
					current = parseInt(this.page(), 10);

				lower = upper = current;

				if (!_.isNumber(this.searchResult().total)) {
					return [];
				}
                for (;index < links && index < totalPages;) {
					if (lower > 1 ) {
						lower--; index++;
					}
					if (index < links && upper < totalPages) {
						upper++; index++;
					}
				}
				return _.range(lower, upper + 1);
			},
			defineFoundIcon: function (level) {
				return {
						DEBUG: 'cog',
						INFO: 'info',
						WARN: 'warning',
						ERROR: 'bolt',
						FATAL: 'fire'
					}[level.toUpperCase()] || 'asterisk';
			},
			getFoundHits: function () {
				return this.searchResult().hits || [];
			},
			getSearchResume: function () {
				var result = this.searchResult();
				return [result.total, ' record(s) found in ', result.queryDuration, ' ms'].join('');
			},
			makeSearch : function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
				this.storeAdvancedDetails();
				if (this.sammy.getLocation() === path) {
					this.sammy.runRoute('get', path);
				} else {
					this.sammy.setLocation(path);
				}
				return false;
			},
			getSearchParams: function () {
				var dates = this.getDatesAsISO();
				return {
					expression: this.expression(),
					page: this.page(),
					sort: {
						field: this.sortField(),
						reverse: this.sortReverse()
					},
					filter: {
						from: dates.from,
						till: dates.till
					}
				};
			},
			onSearchDone: function (data) {
				var result = data || {total: 0};
				this.searchResult(result);
				this.totalPages(Math.ceil(result.total / settings.getItemsPerPage()));
			},
			bind: function () {
				markupService.applyBindings(this, searchView);
				this.initExternals();

				searchService
					.search(this.getSearchParams())
					.done(this.onSearchDone.bind(this));
			}
		});
		return SearchViewModel;
	});
})();
