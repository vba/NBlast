angular.module('nblast', ['ngRoute', 'ngResource', 'angular-underscore'])
    .config([
        '$routeProvider',
        '$httpProvider',
        function ($routeProvider, $httpProvider) {
            'use strict';
            $routeProvider
                .when('/', {
                    templateUrl: 'app/views/dashboard.html',
                    controller: 'indexController'
                })
                .when('/search', {
                    templateUrl: 'app/views/search.html',
                    controller: 'searchController'
                })
                .when('/search/:query', {
                    templateUrl: 'app/views/search.html',
                    controller: 'searchController'
                })
                .when('/details/:hitId', {
                    templateUrl: 'app/views/details.html',
                    controller: 'detailsController'
                })
                .otherwise({
                    redirectTo: '/'
                });
            $httpProvider.defaults.headers.common['X-NBlast-Client'] = 'angular-client-app';
        }
    ]);