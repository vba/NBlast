(function() {
	'use strict';
	var dependencies = [
		'sammy', 
		'knockout',
		'viewModels/dashboard',
		'viewModels/search'
	];
	define(dependencies, function(sammy, ko, dashboard, search) {
		var routes = sammy(function(context) {
			var me = this;

/*
                    .when('/search', {
                        templateUrl: 'app/views/search.html',
                        controller: 'searchController'
                    })
                    .when('/search/:page/:query', {
                        templateUrl: 'app/views/search.html',
                        controller: 'searchController'
                    })
                    .when('/search/:query', {
                        redirectTo: function(routeParams) {
                            return ['/search/1/', routeParams.query || encodeURIComponent('*:*')].join('');
                        }
                    })

*/

			//me.get('#/dashboard', function(context) {});
			me.get('#/search/:page/:query', function(context) {
				search.bind(this.params['page'], this.params['query']);
			});
			me.get('#/dashboard', function(context) {
				dashboard.bind();
			});
			me.get('#/search', function(context) {});
			me.get('#/search/:query', function(context) {
				var path = ['#/search/1/', this.params['query'] || encodeURIComponent('*:*')].join('');
				me.runRoute('get', path);
			});
			me.get('', function(context) {
				me.runRoute('get', '#/dashboard');
			});
		});

		return routes;
	});

})();