(function() {
	'use strict';
	var config          = require('../config'),
		ko              = require('knockout'),
		StorageService  = require('../services/storage'),
		BaseSearchViewModel;

	//noinspection JSUnresolvedFunction
	config.bootstrapComponents();

	//noinspection JSUnusedGlobalSymbols
	BaseSearchViewModel = (function() {

		var $private = {}, storageService = new StorageService();

		function BaseSearchViewModel() {
			this.moment         = config.moment();
			this.totalPages     = ko.observable(0);
			this.page           = ko.observable();
			this.expression     = ko.observable();
			this.searchRssLink  = ko.observable({});
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
			var dates = this.getDatesAsISO();
			storageService.storeFilters(dates.from, dates.till);
		};
		$private.storeSort = function () {
			var $ = config.jquery();
			storageService.storeSort($.trim(this.sortField()), this.sortReverse());
		};
		$private.storeSearch = function () {
			storageService.storeSearch(this.searchType());
		};
		$private.clearFilters = function () {
			storageService.clearFilters();
			this.filter.from('');
			this.filter.till('');
		};
		$private.clearSort = function () {
			storageService.clearSort();
			this.sortField('');
			this.sortReverse(false);
		};
		$private.clearSearch = function () {
			storageService.clearSearch();
			this.searchType('');
		};

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
			return []; // cover:false
		};
		BaseSearchViewModel.prototype.getPages = function() {
			return []; // cover:false
		};
		//noinspection JSUnusedGlobalSymbols
		BaseSearchViewModel.prototype.getSearchResume = function() {
			return ""; // cover:false
		};
		BaseSearchViewModel.prototype.enterSearch = function(data, event) {
			if (event.keyCode === 13) {
				return this.makeSearch();
			}
			return true;
		};
		BaseSearchViewModel.prototype.makeSearch = function() {
			throw new Error("[Not yet implemented]"); // cover:false
		};
		BaseSearchViewModel.prototype.bind = function() {
			throw new Error("[Not yet implemented]"); // cover:false
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
