(function() {
	'use strict';
	var config               = require('./config'),
		DetailsViewModel     = require('./viewModels/details'),
		DashboardViewModel   = require('./viewModels/dashboard'),
		EmptySearchViewModel = require('./viewModels/emptySearch'),
		SettingsViewModel    = require('./viewModels/settings'),
		SearchViewModel      = require('./viewModels/search'),
		sammy                = config.sammy(),
		routes;

	routes = sammy(function () {
		var me = this;
		me.get('#/details/:uuid', function () {
			var params = this.params;
			new DetailsViewModel(params.uuid).bind();
		});
		me.get('#/search/:page/:expression', function () {
			var params = this.params;
			new SearchViewModel(parseInt(params.page, 10), params.expression).bind();
		});
		me.get('#/dashboard', function () {
			new DashboardViewModel().bind();
		});
		me.get('#/search', function () {
			new EmptySearchViewModel().bind();
		});
		me.get('#/settings', function () {
			new SettingsViewModel().bind();
		});
		me.get('#/search/:expression', function () {
			var path = ['#/search/1/', encodeURIComponent(this.params.expression || '*:*')].join('');
			me.runRoute('get', path);
		});
		me.get('', function () {
			me.runRoute('get', '#/dashboard');
		});
	});

	//noinspection JSUnresolvedVariable
	module.exports = routes;


/*	var dependencies = [
		'sammy'
	];
	define(dependencies, function(sammy) {
		var routes;
		routes = sammy(function () {
			var me = this;
			me.get('#/details/:uuid', function () {
				var params = this.params;
				require(['viewModels/details'], function (DetailsViewModel) {
					new DetailsViewModel(params.uuid).bind();
				});
			});
			me.get('#/search/:page/:expression', function () {
				var params = this.params;
				require(['viewModels/search'], function(SearchViewModel) {
					new SearchViewModel(parseInt(params.page, 10), params.expression).bind();
				});
			});
			me.get('#/dashboard', function () {
				require(['viewModels/dashboard'], function (DashboardViewModel) {
					new DashboardViewModel().bind();
				});
			});
			me.get('#/search', function () {
				require(['viewModels/emptySearch'], function (EmptySearchViewModel) {
					new EmptySearchViewModel().bind();
				});
			});
			me.get('#/search/:expression', function () {
				var path = ['#/search/1/', encodeURIComponent(this.params.expression || '*:*')].join('');
				me.runRoute('get', path);
			});
			me.get('', function () {
				me.runRoute('get', '#/dashboard');
			});
		});
		return routes;
	});*/
})();
