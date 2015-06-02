(function () {
	'use strict';
	var _                   = require('underscore'),
		ko                  = require('knockout'),
		views               = require('../views'),
		markupService       = require('../services/markup'),
		searchService       = require('../services/search'),
		settings            = require('../services/settings'),
		object              = require('../tools/object'),
		Indicator           = require('../tools/indicator'),
		BaseSearchViewModel = require('./baseSearch'),
		SearchViewModel;

	SearchViewModel = (function($super) {
		function SearchViewModel (page, expression, termKey) {
			$super.call(this);

			if (!_.isNumber(page)) {
				throw new Error('page param must be a number');
			}
			if (!_.isString(expression)) {
				throw new Error('expression param must be a string');
			}
			if (_.isString(termKey) && !_.isEmpty(termKey)) {
				this.searchType = ko.observable(termKey);
			}

			this.searchRssLink  = ko.observable({});
			this.searchResult   = ko.observable({});
			this.expression     = ko.observable(expression);
			this.page           = ko.observable(page);
		}

		var indicator = new Indicator();
		object.extends(SearchViewModel, $super);

		SearchViewModel.prototype.getPages = function () {
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
		SearchViewModel.prototype.defineFoundIcon = function (level) {
			return {
				DEBUG: 'cog',
				INFO: 'info',
				WARN: 'warning',
				ERROR: 'bolt',
				FATAL: 'fire'
			}[level.toUpperCase()] || 'asterisk';
		};
		//noinspection JSUnusedGlobalSymbols
		SearchViewModel.prototype.getFoundHits = function () {
			return this.searchResult().hits || [];
		};
		//noinspection JSUnusedGlobalSymbols
		SearchViewModel.prototype.getSearchResume = function () {
			var result = this.searchResult();
			return [result.total, ' record(s) found in ', result.queryDuration, ' ms'].join('');
		};
		SearchViewModel.prototype.makeSearch  = function () {
			var query = this.expression() || '*:*',
				path  = [searchService.getPathName(), '#/search/', encodeURIComponent(query)].join('');
			this.storeAdvancedDetails();
			if (this.sammy().getLocation() === path) {
				this.sammy().runRoute('get', path);
			} else {
				this.sammy().setLocation(path);
			}
			return false;
		};
		SearchViewModel.prototype.getSearchParams = function () {
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
		SearchViewModel.prototype.getTermSearchParams = function () {
			var result = this.getSearchParams();
			result.search = {
				type : this.searchType()
			};
			return result;
		};
		SearchViewModel.prototype.termSearchMode = function () {
			return /(?:id|sender|logger|level)/gi.test(this.searchType());
		};
		SearchViewModel.prototype.onSearchError = function () {
			indicator.close();
		};
		SearchViewModel.prototype.onSearchDone = function (data, url) {
			var result = data || {total: 0};
			indicator.close();
			this.searchRssLink(url);
			this.searchResult(result);
			this.totalPages(Math.ceil(result.total / settings.getItemsPerPage()));
		};
		SearchViewModel.prototype.requestSearch = function () {
			indicator.display('Searching ...');
			return this.termSearchMode()
				? searchService.searchByTerm(this.getTermSearchParams())
				: searchService.search(this.getSearchParams());
		};
		SearchViewModel.prototype.bind = function () {
			var me         = this,
				searchView = views.getSearch();

			markupService.applyBindings(this, searchView);

			this.initExternals();
			this.requestSearch()
				.done(function (data) {
					me.onSearchDone.call(me, data, this.url);
				})
				.error(this.onSearchError.bind(this));
		};

		return SearchViewModel;
	})(BaseSearchViewModel);

	//noinspection JSUnresolvedVariable
	module.exports = SearchViewModel;

})();
