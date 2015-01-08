(function() {
    'use strict';

    requirejs.config({
        paths: {
            'angular': '../../bower_components/angular/angular',
            'angular-route': '../../bower_components/angular-route/angular-route.min',
            'angular-resource': '../../bower_components/angular-resource/angular-resource.min',
            'angular-underscore': '../../bower_components/angular-underscore/angular-underscore.min'
        },
        shim: {
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
                deps: ["angular"]
            }
        }
    });

    /*

    <script src="bower_components/angular-route/angular-route.min.js"></script>
    <script src="bower_components/angular-resource/angular-resource.min.js"></script>
    <script src="bower_components/angular-underscore/angular-underscore.min.js"></script>

    <script src="app/js/configuration.js"></script>
    <script src="app/js/services/config.js"></script>
    <script src="app/js/services/search.js"></script>
    <script src="app/js/services/dashboard.js"></script>
    <script src="app/js/controllers/index.js"></script>
    <script src="app/js/controllers/search.js"></script>
    <script src="app/js/controllers/details.js"></script>

*/

    require([
            'angular',
            'angular-route',
            'angular-resource',
            'angular-underscore',
            'configuration',
            'services/config',
            'services/search',
            'services/dashboard',
            'controllers/index',
            'controllers/search',
            'controllers/details'
        ],
        function(angular) {
            angular.bootstrap(document, ['nblast']);
        });
})();
