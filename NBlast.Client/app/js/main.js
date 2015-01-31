(function() {
    'use strict';

    requirejs.config({
        paths: {
            jquery: '../../bower_components/jquery/dist/jquery',
            text: '../../bower_components/requirejs-text/text',
            bootstrap: '../../bower_components/bootstrap/dist/js/bootstrap',
            'bootstrap-picker': '../../bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min',
            underscore: '../../bower_components/underscore/underscore-min',
            knockout: '../../bower_components/knockout/dist/knockout.debug',
            sammy: '../../bower_components/sammy/lib/sammy',
            moment: '../../bower_components/moment/moment',
            amplify: '../../bower_components/amplify/lib/amplify.store.min',
            jsface: '../../bower_components/jsface/jsface',
            views: '../views'
        },
        shim: {
            jsface: {
                exports: 'jsface'
            },
            amplify: {
                exports: 'amplify'
            },
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
            },
            'bootstrap-picker': {
                deps: ['bootstrap']
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
