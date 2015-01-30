(function() {
	'use strict';
	var dependencies = [
		'knockout',
		'sammy',
		'services/markup',
		'text!views/search',
		'viewModels/baseSearch'
	];
	define(dependencies, function(ko, sammy, markupService, searchView, BaseSearchViewModel) {

		var EmptySearchViewModel = function() {
			BaseSearchViewModel.apply(this);
			this.totalPages = ko.observable(0);
			this.page = ko.observable();
			this.expression = ko.observable();
			this.sortField = ko.observable('');
			this.sortReverse = ko.observable('false');
			this.filterFrom = ko.observable('');
			this.filterTill = ko.observable('');
			this.searchResult = false;
		};

		EmptySearchViewModel.prototype = _.extend(BaseSearchViewModel.prototype, {
			makeSearch: function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');

				sammy().setLocation(path);
				return false;
			},
			bind: function () {
				markupService.applyBindings(this, searchView);
				this.initExternals();
			}
		});

		return EmptySearchViewModel;
	});
})();
