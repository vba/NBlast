(function() {
	'use strict';
	var sammy               = require('../config').sammy(),
		Class               = require('jsface').Class,
		markupService       = require('../services/markup'),
		BaseSearchViewModel = require('./baseSearch'),
		EmptySearchViewModel;

	//noinspection UnnecessaryLocalVariableJS
	EmptySearchViewModel = Class(BaseSearchViewModel, function() {
		var prototype = {};

		prototype.constructor = function() {
			EmptySearchViewModel.$super.call(this);
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
			var searchView = require('../../views/search.html')
			markupService.applyBindings(this, searchView);
			this.initExternals();
		};

		return prototype;
	});

	//noinspection JSUnresolvedVariable
	module.exports = EmptySearchViewModel;

})();
