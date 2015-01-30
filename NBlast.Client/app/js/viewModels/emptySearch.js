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
		};

		EmptySearchViewModel.prototype = _.extend(_.clone(BaseSearchViewModel.prototype), {
			makeSearch: function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
				this.storeAdvancedDetails();
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
