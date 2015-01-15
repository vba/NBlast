(function() {
	'use strict';
	var dependencies = [
		'sammy',
		'knockout',
		'services/settings',
		'viewModels/search'
	];
	define(dependencies, function(sammy, ko, settings, search) {
		var routes = sammy(function() {
			var me = this;
			//me.get('#/dashboard', function(context) {});
			me.get('#/do-nothing', function() {
				return false;
			});
			me.get('#/search/:page/:query', function() {
				var params = this.params;
				require(['viewModels/search'], function(SearchViewModel) {
					new SearchViewModel(parseInt(params.page, 10), params.query)
						.bind()
						.makeSearch();
				});
			});
			me.get('#/dashboard', function() {
				require(['viewModels/dashboard'], function(DashboardViewModel) {
					new DashboardViewModel().bind();
				});
			});
			me.get('#/search', function() {
				require(['viewModels/emptySearch'], function(EmptySearchViewModel) {
					new EmptySearchViewModel().bind();
				});
				// var path = ['#/search/', encodeURIComponent('*:*')].join('');
				// me.runRoute('get', path);
			});
			me.get('#/search/:query', function() {
				var path = ['#/search/1/', this.params.query || encodeURIComponent('*:*')].join('');
				me.runRoute('get', path);
			});
			me.get('', function() {
				me.runRoute('get', '#/dashboard');
			});
		});

		return routes;
	});

})();
