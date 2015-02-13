(function() {
	'use strict';
	var sammy = require('../config').sammy(),
		markupService = require('../services/markup'),
		BaseSearchViewModel = require('./baseSearch'),
		searchView = require('../../views/search.html'),
		EmptySearchViewModel;

	//noinspection UnnecessaryLocalVariableJS
	EmptySearchViewModel = BaseSearchViewModel.subclass(function(prototype) {
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
			debugger
			markupService.applyBindings(this, searchView);
			this.initExternals();
		};
	});

	//noinspection JSUnresolvedVariable
	module.exports = EmptySearchViewModel;


/*	var dependencies = [
		'knockout',
		'sammy',
		'services/markup',
		'text!views/search',
		'viewModels/baseSearch'
	];
	define(dependencies, function(ko, sammy, markupService, searchView, BaseSearchViewModel) {
		//noinspection UnnecessaryLocalVariableJS
		var EmptySearchViewModel = BaseSearchViewModel.subclass(function(prototype) {
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
	});*/
})();
