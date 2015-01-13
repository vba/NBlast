(function() {
    'use strict';

    requirejs.config({
        paths: {
            'angular': '../../bower_components/angular/angular',
            'angular-route': '../../bower_components/angular-route/angular-route.min',
            'angular-resource': '../../bower_components/angular-resource/angular-resource.min',
            'angular-underscore': '../../bower_components/angular-underscore/angular-underscore.min',
            'jquery': '../../bower_components/jquery/dist/jquery',
            'bootstrap': '../../bower_components/bootstrap/dist/js/bootstrap',
            'underscore': '../../bower_components/underscore/underscore-min',
        },
        shim: {
            underscore: {
                exports: '_'
            },
            jquery: {
                exports: 'jQuery'
            },
            bootstrap: {
                deps: ['jquery']
            },
            angular: {
                exports: 'angular'
            },
            'angular-route': {
                deps: ["angular"]
            },
            'angular-resource': {
                deps: ["angular"]
            },
            'angular-underscore': {
                deps: ["angular", 'underscore']
            }
        }
    });

    require([
            'angular',
            'angular-route',
            'angular-resource',
            'angular-underscore',
            'configuration',
            'controllers/index',
            'controllers/search',
            'controllers/details'
        ],
        function(angular) {
            angular.bootstrap(document, ['nblast']);
        });
})();
