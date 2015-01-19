(function() {
    'use strict';

    requirejs.config({
        paths: {
            'jquery': '../../bower_components/jquery/dist/jquery',
            'text': '../../bower_components/requirejs-text/text',
            'bootstrap': '../../bower_components/bootstrap/dist/js/bootstrap',
            'underscore': '../../bower_components/underscore/underscore-min',
            'knockout': '../../bower_components/knockout/dist/knockout.debug',
            'sammy': '../../bower_components/sammy/lib/sammy',
            'moment': '../../bower_components/moment/moment',
            'views': '../views'
        },
        shim: {
            moment: {
                exports: 'moment'
            },
            knockout: {
                exports: 'ko'
            },
            sammy: {
                exports: 'sammy'
            },
            underscore: {
                exports: '_'
            },
            jquery: {
                exports: 'jQuery'
            },
            bootstrap: {
                deps: ['jquery']
            }
        }
    });

    require([
            'routes'
        ],
        function(routes) {
            routes.run();
        });
})();
