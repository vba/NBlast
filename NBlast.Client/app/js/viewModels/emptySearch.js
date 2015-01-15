(function() {
	'use strict';
	var dependencies = [
		'knockout',
		'sammy',
		'services/markup',
		'text!views/search'
	];
	define(dependencies, function(ko, sammy, markupService, searchView) {
		var EmptySearchViewModel = function() {
			this.page = ko.observable();
			this.query = ko.observable();
		};

		EmptySearchViewModel.prototype = {
			enterSearch : function(data, event) {
				if (event.keyCode === 13) {
					return this.makeSearch();
				};
				return true;
			},
			makeSearch : function() {
				// window.location.assign(['#/search/', encodeURIComponent('*:*')].join(''));
				sammy().setLocation(['#/search/', encodeURIComponent('*:*')].join(''));
				return false;
			}
		}

		return {
			bind: function() {
				var viewModel = new EmptySearchViewModel();
				markupService.applyBindings(viewModel, searchView);
			}
		}
	});
})();