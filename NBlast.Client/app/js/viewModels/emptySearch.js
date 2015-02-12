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
		//noinspection UnnecessaryLocalVariableJS
		var EmptySearchViewModel = BaseSearchViewModel.subclass(function(prototype, _keys, _protected) {
			prototype.init = function() {
				prototype.super.init.call(this);
				this.searchType('');
				this.sammy = sammy();
			};
			prototype.makeSearch = function () {
				var query = this.expression() || '*:*',
					path = ['/#/search/', encodeURIComponent(query)].join('');
				this.storeAdvancedDetails();
				this.sammy.setLocation(path);
				return false;
			};
			prototype.bind = function () {
				markupService.applyBindings(this, searchView);
				this.initExternals();
			};
		});
		
		return EmptySearchViewModel;
	});
})();
