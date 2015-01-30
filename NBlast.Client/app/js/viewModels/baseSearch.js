(function() {
	'use strict';
	var dependencies = [
		'underscore',
		'jquery',
		'moment',
		'bootstrap-picker'
	];
	define(dependencies, function(_, $, moment) {
		var BaseSearchViewModel = function() {
			this.moment = moment;
		};

		BaseSearchViewModel.prototype = {
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
					options = { format: 'HH:mm DD/MM/YYYY' };

				fromPicker.datetimepicker(options).on("dp.change", function (e) {
					tillPicker.data("DateTimePicker").minDate(e.date);
				});
				tillPicker.datetimepicker(options).on("dp.change", function (e) {
					fromPicker.data("DateTimePicker").maxDate(e.date);
				});
			},
		};
		return BaseSearchViewModel;
	});
})();
