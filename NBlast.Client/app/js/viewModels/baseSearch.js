(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'moment',
		'amplify',
		'knockout',
		'mozart',
		'bootstrap-picker'
	];
	define(dependencies, function(_, $, moment, amplify, ko, ctor) {
		//noinspection JSUnusedGlobalSymbols
		var BaseSearchViewModel = ctor(function(prototype, _keys, _protected) {
			//$statics: {
			//	displayDateTimeFormat: 'HH:mm DD/MM/YYYY'
			//},
			prototype.init = function() {
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
			};
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
				amplify.store('filter', null);
				amplify.store('sort', null);
				amplify.store('search', null);
				this.searchType('');
				this.sortField('');
				this.sortReverse(false);
				this.filter.from('');
				this.filter.till('');
			};
			prototype.storeAdvancedDetails = function() {
				var dates = this.getDatesAsISO(),
					filter = {
						from: dates.from,
						till: dates.till
					},
					sort = {
						reverse: this.sortReverse(),
						field: $.trim(this.sortField())
					},
					search = {
						type: this.searchType()
					};

				amplify.store('filter', filter);
				amplify.store('sort', sort);
				amplify.store('search', search);
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
			prototype.changeSortOrder = function(value) {
				this.sortReverse(value);
			};
			prototype.getFoundHits = function() {
				return [];
			};
			prototype.getPages = function() {
				return [];
			};
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
		});
		BaseSearchViewModel.displayDateTimeFormat = 'HH:mm DD/MM/YYYY';
		return BaseSearchViewModel;
	});
})();
