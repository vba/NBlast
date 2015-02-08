(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'moment',
		'amplify',
		'knockout',
		'jsface',
		'bootstrap-picker'
	];
	define(dependencies, function(_, $, moment, amplify, ko, jsface) {
		//noinspection JSUnusedGlobalSymbols
		var BaseSearchViewModel = jsface.Class({
			$statics: {
				displayDateTimeFormat: 'HH:mm DD/MM/YYYY'
			},
			constructor: function() {
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
			},
			mapSortFieldLabel: function(value) {
				return {
						CREATEDAT: 'Date',
						SENDER: 'Sender',
						LOGGER: 'Logger',
						LEVEL: 'Level'
					}[value.toUpperCase()] || 'Relevance';
			},
			getDatesAsISO: function () {
				var format = BaseSearchViewModel.displayDateTimeFormat,
					fromDate = this.moment(this.filter.from(), format),
					tillDate = this.moment(this.filter.till(), format);

				return {
					from: fromDate.isValid() ? fromDate.toISOString() : '',
					till: tillDate.isValid() ? tillDate.toISOString()  : ''
				};
			},
			clearAdvancedDetails: function () {
				amplify.store('filter', null);
				amplify.store('sort', null);
				this.sortField('');
				this.sortReverse(false);
				this.filter.from('');
				this.filter.till('');
			},
			storeAdvancedDetails: function() {
				var dates = this.getDatesAsISO(),
					filter = {
						from: dates.from,
						till: dates.till
					},
					sort = {
						reverse: this.sortReverse(),
						field: $.trim(this.sortField())
					};

				amplify.store('filter', filter);
				amplify.store('sort', sort);
			},
			initAdvancedDetails: function() {
				var format = BaseSearchViewModel.displayDateTimeFormat,
					filter = amplify.store('filter') || {},
					sort = amplify.store('sort') || {};

				this.sortField = ko.observable(sort.field || '');
				this.sortReverse = ko.observable(sort.reverse || false);
				this.filter = {
					from : ko.observable(!filter.from ? '' : moment(new Date(filter.from)).format(format)),
					till : ko.observable(!filter.till ? '' : moment(new Date(filter.till)).format(format))
				};
			},
			changeSortOrder: function(value) {
				this.sortReverse(value);
			},
			getFoundHits: function() {
				return [];
			},
			getPages: function() {
				return [];
			},
			getSearchResume: function() {
				return "";
			},
			enterSearch : function(data, event) {
				if (event.keyCode === 13) {
					return this.makeSearch();
				}
				return true;
			},
			makeSearch: function() {
				throw new Error("[Not yet implemented]");
			},
			bind: function() {
				throw new Error("[Not yet implemented]");
			},
			initExternals: function () {
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
			}
		});
		return BaseSearchViewModel;
	});
})();
