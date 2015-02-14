(function() {
	'use strict';
	var $       = require('../config').jquery(),
		moment  = require('../config').moment(),
		amplify = require('../config').amplify(),
		ko      = require('knockout'),
		Class   = require('jsface').Class,
		BaseSearchViewModel;

	//noinspection JSUnusedGlobalSymbols
	BaseSearchViewModel = Class(function() {
		var $private = {},
			prototype = {$statics: {}};

		$private.storeFilters = function () {
			var dates = this.getDatesAsISO(),
				filter = {
					from: dates.from,
					till: dates.till
				};
			amplify.store('filter', filter);
		};
		$private.storeSort = function () {
			var sort = {
				reverse: this.sortReverse(),
				field: $.trim(this.sortField())
			};
			amplify.store('sort', sort);
		};
		$private.storeSearch = function () {
			var search = {
				type: this.searchType()
			};
			amplify.store('search', search);
		};
		$private.clearFilters = function () {
			amplify.store('filter', null);
			this.filter.from('');
			this.filter.till('');
		};
		$private.clearSort = function () {
			amplify.store('sort', null);
			this.sortField('');
			this.sortReverse(false);
		};
		$private.clearSearch = function () {
			amplify.store('search', null);
			this.searchType('');
		};

		prototype.constructor = function() {
			this.moment = moment;
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
		}

		prototype.mapSortFieldLabel = function(value) {
			return {
				CREATEDAT: 'Date',
				SENDER: 'Sender',
				LOGGER: 'Logger',
				LEVEL: 'Level'
			}[value.toUpperCase()] || 'Relevance';
		};
		prototype.mapSearchTypeLabel = function (value) {
			return {
				ID: 'Identifier',
				SENDER: 'Sender',
				LOGGER: 'Logger',
				LEVEL: 'Level'
			}[value.toUpperCase()] || 'Any expression';
		};
		prototype.getDatesAsISO = function () {
			var format = BaseSearchViewModel.displayDateTimeFormat,
				fromDate = this.moment(this.filter.from(), format),
				tillDate = this.moment(this.filter.till(), format);

			return {
				from: fromDate.isValid() ? fromDate.toISOString() : '',
				till: tillDate.isValid() ? tillDate.toISOString()  : ''
			};
		};
		prototype.clearAdvancedDetails = function () {
			$private.clearFilters.call(this);
			$private.clearSort.call(this);
			$private.clearSearch.call(this);
		};
		prototype.storeAdvancedDetails = function() {
			$private.storeFilters.call(this);
			$private.storeSort.call(this);
			$private.storeSearch.call(this);
		};
		prototype.initAdvancedDetails = function() {
			var format = BaseSearchViewModel.displayDateTimeFormat,
				filter = amplify.store('filter') || {},
				sort = amplify.store('sort') || {},
				search = amplify.store('search') || {};

			this.searchType = ko.observable(search.type || '');
			this.sortField = ko.observable(sort.field || '');
			this.sortReverse = ko.observable(sort.reverse || false);
			this.filter = {
				from : ko.observable(!filter.from ? '' : moment(new Date(filter.from)).format(format)),
				till : ko.observable(!filter.till ? '' : moment(new Date(filter.till)).format(format))
			};
		};
		//noinspection JSUnusedGlobalSymbols
		prototype.changeSortOrder = function(value) {
			this.sortReverse(value);
		};
		//noinspection JSUnusedGlobalSymbols
		prototype.getFoundHits = function() {
			return [];
		};
		prototype.getPages = function() {
			return [];
		};
		//noinspection JSUnusedGlobalSymbols
		prototype.getSearchResume = function() {
			return "";
		};
		prototype.enterSearch = function(data, event) {
			if (event.keyCode === 13) {
				return this.makeSearch();
			}
			return true;
		};
		prototype.makeSearch = function() {
			throw new Error("[Not yet implemented]");
		};
		prototype.bind = function() {
			throw new Error("[Not yet implemented]");
		};
		prototype.initExternals = function () {
			var fromPicker = $('#filterFromDate'),
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
		prototype.$statics.displayDateTimeFormat = 'HH:mm DD/MM/YYYY';
		return prototype;
	});
	module.exports = BaseSearchViewModel;
})();
