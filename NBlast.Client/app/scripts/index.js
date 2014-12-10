'use strict';

(function() {
    angular.module('nblast', [])
        .controller('indexController', function($scope) {
            return undefined;
        })
    .config(['$routeProvider',
        function($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: 'partials/dashboard.html',
                    controller: 'indexController'
                })
                .otherwise({
                    redirectTo: '/'
                });
        }
    ])
    ;
})();
