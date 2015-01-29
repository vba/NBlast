(function () {
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
		'text!views/search',
		'bootstrap-picker'
	];
	define(dependencies, function (_,
	                               $,
	                               ko,
	                               sammy,
	                               moment,
	                               markupService,
	                               searchService,
	                               settings,
	                               searchView) {

		var SearchViewModel = function (page,
		                                expression,
		                                sortField,
		                                sortReverse,
		                                filterFrom,
		                                filterTill) {
			if (!_.isNumber(page)) {
				throw new Error('page param must be a number');
			}
			if (!_.isString(expression)) {
				throw new Error('expression param must be a string');
			}

			this.searchResult = ko.observable({});
			this.page = ko.observable(page);
			this.totalPages = ko.observable(0);
			this.expression = ko.observable(expression);
			this.sortField = ko.observable(sortField || '');
			this.sortReverse = ko.observable(sortReverse || 'false');
			this.filterFrom = ko.observable(filterFrom || '');
			this.filterTill = ko.observable(filterTill || '');
			this.moment = moment;
		};

		//noinspection JSUnusedGlobalSymbols
		SearchViewModel.prototype = {
			selectHit: function (hit) {
				if (_.isEmpty(hit)) {
					throw new Error('selected hit cannot be empty');
				}
				sammy().setLocation(['/#/details/', hit.id].join(''));
			},
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
			enterSearch : function (data, event) {
				// if (event.keyCode === 27) {
				// 	sammy().setLocation('/#/search');
				// 	return false;
				// }
				if (event.keyCode === 13) {
					return this.makeSearch();
				}
				return true;
			},
			makeSearch : function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
				if (sammy().getLocation() === path) {
					sammy().runRoute('get', path);
				} else {
					sammy().setLocation(path);
				}
				return false;
			},
			initExternals: function () {
				var fromPicker = $('#filterFromDate'),
					tillPicker = $('#filterTillDate'),
					options = { format: 'HH:mm DD/MM/YYYY' };

				fromPicker.datetimepicker(options).on("dp.change", function (e) {
					tillPicker.data("DateTimePicker").minDate(e.date);
				});
				tillPicker.datetimepicker(options).on("dp.change", function (e) {
					fromPicker.data("DateTimePicker").maxDate(e.date);
				});
			},
			bind: function () {
				var me = this;
				markupService.applyBindings(this, searchView);
				me.initExternals();
				searchService.search({
					expression: this.expression(),
					page: this.page()
				}).done(function (data) {
					var result = data || {total: 0};
					me.searchResult(result);
					me.totalPages(Math.ceil(result.total / settings.getItemsPerPage()));
				});
			}
		};
		return SearchViewModel;
	});
})();
