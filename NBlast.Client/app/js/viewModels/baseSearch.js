(function() {
	'use strict';
	var config   = require('../config'),
		ko       = require('knockout'),
		BaseSearchViewModel;

	//noinspection JSUnresolvedFunction
	config.bootstrapComponents();

	//noinspection JSUnusedGlobalSymbols
	BaseSearchViewModel = (function() {

		var $private = {};

		function BaseSearchViewModel() {
			this.moment         = config.moment();
			this.totalPages     = ko.observable(0);
			this.page           = ko.observable();
			this.expression     = ko.observable();
			this.searchResult   = false;
			this.sammy          = config.sammy();
			this.initAdvancedDetails();
			//noinspection JSUnusedGlobalSymbols
			this.sortFieldLabel = ko.computed(function() {
				return this.mapSortFieldLabel(this.sortField());
			}.bind(this));
			//noinspection JSUnusedGlobalSymbols
			this.searchTypeLabel = ko.computed(function() {
				return this.mapSearchTypeLabel(this.searchType());
			}.bind(this));
		}

		$private.storeFilters = function () {
			var store = config.amplify().store,
				dates = this.getDatesAsISO(),
				filter = {
					from: dates.from,
					till: dates.till
				};
			store('filter', filter);
		};
		$private.storeSort = function () {
			var store = config.amplify().store,
				$ = config.jquery(),
				sort = {
				reverse: this.sortReverse(),
				field: $.trim(this.sortField())
			};
			store('sort', sort);
		};
		$private.storeSearch = function () {
			var search = {
				type: this.searchType()
			};
			config.amplify().store('search', search);
		};
		$private.clearFilters = function () {
			config.amplify().store('filter', null);
			this.filter.from('');
			this.filter.till('');
		};
		$private.clearSort = function () {
			config.amplify().store('sort', null);
			this.sortField('');
			this.sortReverse(false);
		};
		$private.clearSearch = function () {
			config.amplify().store('search', null);
			this.searchType('');
		};

/*		prototype.constructor = function() {
			this.moment = config.moment();
			this.totalPages = ko.observable(0);
			this.page = ko.observable();
			this.expression = ko.observable();
			this.searchResult = false;
			this.initAdvancedDetails();
			//noinspection JSUnusedGlobalSymbols
			this.sortFieldLabel = ko.computed(function() {
				return this.mapSortFieldLabel(this.sortField());
			}.bind(this));
			//noinspection JSUnusedGlobalSymbols
			this.searchTypeLabel = ko.computed(function() {
				return this.mapSearchTypeLabel(this.searchType());
			}.bind(this));
		}*/

		BaseSearchViewModel.prototype.mapSortFieldLabel = function(value) {
			return {
				CREATEDAT: 'Date',
				SENDER: 'Sender',
				LOGGER: 'Logger',
				LEVEL: 'Level'
			}[value.toUpperCase()] || 'Relevance';
		};
		BaseSearchViewModel.prototype.mapSearchTypeLabel = function (value) {
			return {
				ID: 'Identifier',
				SENDER: 'Sender',
				LOGGER: 'Logger',
				LEVEL: 'Level'
			}[value.toUpperCase()] || 'Any expression';
		};
		BaseSearchViewModel.prototype.getDatesAsISO = function () {
			var format = BaseSearchViewModel.displayDateTimeFormat,
				fromDate = this.moment(this.filter.from(), format),
				tillDate = this.moment(this.filter.till(), format);

			return {
				from: fromDate.isValid() ? fromDate.toISOString() : '',
				till: tillDate.isValid() ? tillDate.toISOString()  : ''
			};
		};
		BaseSearchViewModel.prototype.clearAdvancedDetails = function () {
			$private.clearFilters.call(this);
			$private.clearSort.call(this);
			$private.clearSearch.call(this);
		};
		BaseSearchViewModel.prototype.storeAdvancedDetails = function() {
			$private.storeFilters.call(this);
			$private.storeSort.call(this);
			$private.storeSearch.call(this);
		};
		BaseSearchViewModel.prototype.initAdvancedDetails = function() {
			var format = BaseSearchViewModel.displayDateTimeFormat,
				filter = config.amplify().store('filter') || {},
				sort = config.amplify().store('sort') || {},
				search = config.amplify().store('search') || {};

			this.searchType = ko.observable(search.type || '');
			this.sortField = ko.observable(sort.field || '');
			this.sortReverse = ko.observable(sort.reverse || false);
			this.filter = {
				from : ko.observable(!filter.from ? '' : this.moment(new Date(filter.from)).format(format)),
				till : ko.observable(!filter.till ? '' : this.moment(new Date(filter.till)).format(format))
			};
		};
		//noinspection JSUnusedGlobalSymbols
		BaseSearchViewModel.prototype.changeSortOrder = function(value) {
			this.sortReverse(value);
		};
		//noinspection JSUnusedGlobalSymbols
		BaseSearchViewModel.prototype.getFoundHits = function() {
			return [];
		};
		BaseSearchViewModel.prototype.getPages = function() {
			return [];
		};
		//noinspection JSUnusedGlobalSymbols
		BaseSearchViewModel.prototype.getSearchResume = function() {
			return "";
		};
		BaseSearchViewModel.prototype.enterSearch = function(data, event) {
			if (event.keyCode === 13) {
				return this.makeSearch();
			}
			return true;
		};
		BaseSearchViewModel.prototype.makeSearch = function() {
			throw new Error("[Not yet implemented]");
		};
		BaseSearchViewModel.prototype.bind = function() {
			throw new Error("[Not yet implemented]");
		};
		BaseSearchViewModel.prototype.initExternals = function () {
			var $ = config.jquery(),
				fromPicker = $('#filterFromDate'),
				tillPicker = $('#filterTillDate'),
				options = {
					format: BaseSearchViewModel.displayDateTimeFormat,
					maxDate: this.moment().add(1, 'hour')
				};

			fromPicker.datetimepicker(options).on("dp.change", function () {
				this.filter.from(fromPicker.data('date'));
			}.bind(this));

			tillPicker.datetimepicker(options).on("dp.change", function (e) {
				if (e.date !== null) {
					fromPicker.data("DateTimePicker").maxDate(e.date);
				}
				this.filter.till(tillPicker.data('date'));
			}.bind(this));
		};
		BaseSearchViewModel.displayDateTimeFormat = 'HH:mm DD/MM/YYYY';
		return BaseSearchViewModel;
	})();
	module.exports = BaseSearchViewModel;
})();
