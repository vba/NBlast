(function() {
	'use strict';
	var views               = require('../views'),
		object              = require('../tools/object'),
		markupService       = require('../services/markup'),
		searchService       = require('../services/search'),
		BaseSearchViewModel = require('./baseSearch'),
		EmptySearchViewModel;

	//noinspection UnnecessaryLocalVariableJS
	EmptySearchViewModel = (function($super) {
		function EmptySearchViewModel() {
			$super.call(this);
			this.searchType('');
		}

		object.extends(EmptySearchViewModel, $super);

		EmptySearchViewModel.prototype.makeSearch = function () {
			var query = this.expression() || '*:*',
				path = [searchService.getPathName(), '#/search/', encodeURIComponent(query)].join('');
			this.storeAdvancedDetails();
			this.sammy().setLocation(path);
			return false;
		};

		EmptySearchViewModel.prototype.bind = function () {
			var searchView = views.getSearch();
			markupService.applyBindings(this, searchView);
			this.initExternals();
		};

		return EmptySearchViewModel;
	})(BaseSearchViewModel);

	//noinspection JSUnresolvedVariable
	module.exports = EmptySearchViewModel;

})();
