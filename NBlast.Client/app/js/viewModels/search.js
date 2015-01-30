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

		var SearchViewModel = function (page, expression) {

			BaseSearchViewModel.apply(this);

			if (!_.isNumber(page)) {
				throw new Error('page param must be a number');
			}
			if (!_.isString(expression)) {
				throw new Error('expression param must be a string');
			}

			this.searchResult = ko.observable({});
			this.page = ko.observable(page);
			this.expression = ko.observable(expression);
		};

		//noinspection JSUnusedGlobalSymbols
		SearchViewModel.prototype = _.extend(_.clone(BaseSearchViewModel.prototype), {
			getPages: function () {
				var total = this.searchResult().total,
					links = 10 + this.page(),
					amount = this.totalPages() + 1;
				if (!_.isNumber(total)) {
					return [];
				}
				return _
					.chain(_.range(this.page() - 5, amount > links ? links : amount))
					.filter(function (n) {
						return n > 0;
					})
					.take(10)
					.value();
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
				if (sammy().getLocation() === path) {
					sammy().runRoute('get', path);
				} else {
					sammy().setLocation(path);
				}
				return false;
			},
			bind: function () {
				var me = this,
					filter = amplify.store('filter') || {},
					sort = amplify.store('sort') || {};

				markupService.applyBindings(this, searchView);
				me.initExternals();

				searchService.search({
					expression: this.expression(),
					page: this.page(),
					sort: {
						field: sort.field,
						reverse: sort.reverse
					},
					filter: {
						from: filter.from,
						till: filter.till
					}
				}).done(function (data) {
					var result = data || {total: 0};
					me.searchResult(result);
					me.totalPages(Math.ceil(result.total / settings.getItemsPerPage()));
				});
			}
		});
		return SearchViewModel;
	});
})();
