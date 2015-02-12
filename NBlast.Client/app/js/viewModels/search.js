(function () {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'knockout',
		'sammy',
		'amplify',
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
	                               markupService,
	                               searchService,
	                               settings,
	                               searchView,
	                               BaseSearchViewModel) {

		//noinspection JSUnusedGlobalSymbols,UnnecessaryLocalVariableJS
		var SearchViewModel = BaseSearchViewModel.subclass(function(prototype, _keys, _protected) {
			prototype.init = function (page, expression, termKey) {

				prototype.super.init.call(this);
				//SearchViewModel.$super.call(this);

				if (!_.isNumber(page)) {
					throw new Error('page param must be a number');
				}
				if (!_.isString(expression)) {
					throw new Error('expression param must be a string');
				}
				if (_.isString(termKey) && !_.isEmpty(termKey)) {
					this.searchType = ko.observable(termKey);
				}

				this.searchResult = ko.observable({});
				this.page = ko.observable(page);
				this.expression = ko.observable(expression);
				this.sammy = sammy();
			};
			prototype.getPages = function () {
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
			};
			prototype.defineFoundIcon = function (level) {
				return {
						DEBUG: 'cog',
						INFO: 'info',
						WARN: 'warning',
						ERROR: 'bolt',
						FATAL: 'fire'
					}[level.toUpperCase()] || 'asterisk';
			};
			prototype.getFoundHits = function () {
				return this.searchResult().hits || [];
			};
			prototype.getSearchResume = function () {
				var result = this.searchResult();
				return [result.total, ' record(s) found in ', result.queryDuration, ' ms'].join('');
			};
			prototype.makeSearch  = function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
				this.storeAdvancedDetails();
				if (this.sammy.getLocation() === path) {
					this.sammy.runRoute('get', path);
				} else {
					this.sammy.setLocation(path);
				}
				return false;
			};
			prototype.getSearchParams = function () {
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
			};
			prototype.getTermSearchParams = function () {
				var result = this.getSearchParams();
				result.search = {
					type : this.searchType()
				};
				return result;
			};
			prototype.termSearchMode = function () {
				return /(?:id|sender|logger|level)/gi.test(this.searchType());
			};
			prototype.onSearchDone = function (data) {
				var result = data || {total: 0};
				this.searchResult(result);
				this.totalPages(Math.ceil(result.total / settings.getItemsPerPage()));
			};
			prototype.requestSearch = function () {
				return this.termSearchMode()
					? searchService.searchByTerm(this.getTermSearchParams())
					: searchService.search(this.getSearchParams());
			};
			prototype.bind = function () {
				markupService.applyBindings(this, searchView);
				this.initExternals();
				this.requestSearch().done(this.onSearchDone.bind(this));
			};
		});
		return SearchViewModel;
	});
})();
