(function() {
	'use strict';
	var dependencies = [
		'knockout',
		'sammy',
		'jsface',
		'services/markup',
		'text!views/search',
		'viewModels/baseSearch'
	];
	define(dependencies, function(ko, sammy, jsface, markupService, searchView, BaseSearchViewModel) {

		var EmptySearchViewModel = jsface.Class(BaseSearchViewModel, {
			constructor: function() {
				EmptySearchViewModel.$super.call(this);
				this.sammy = sammy();
			},
			makeSearch: function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
				this.storeAdvancedDetails();
				this.sammy.setLocation(path);
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
