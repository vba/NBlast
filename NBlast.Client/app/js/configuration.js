angular.module('nblast', ['ngRoute', 'ngResource'])
    .config([
        '$routeProvider',
        '$httpProvider',
        function($routeProvider, $httpProvider) {
            'use strict';
            $routeProvider
                .when('/', {
                    templateUrl: 'partials/dashboard.html',
                    controller: 'indexController'
                })
                .when('/search', {
                    templateUrl: 'partials/search.html',
                    controller: 'searchController'
                })
                .otherwise({
                    redirectTo: '/'
                });

            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            $httpProvider.defaults.headers.common['X-NBlast-Client'] = 'angular-client-app';
        }
    ]);
