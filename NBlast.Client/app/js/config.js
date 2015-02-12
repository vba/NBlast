(function () {
	'use strict';
	var dependencies = [];
	define(dependencies, function () {
		return {
			paths: {
				// Dev env
				jquery: '../../bower_components/jquery/dist/jquery',
				text: '../../bower_components/requirejs-text/text',
				noext: '../../bower_components/requirejs-plugins/src/noext',
				bootstrap: '../../bower_components/bootstrap/dist/js/bootstrap',
				'bootstrap-picker': '../../bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min',
				underscore: '../../bower_components/underscore/underscore-min',
				knockout: '../../bower_components/knockout/dist/knockout.debug',
				sammy: '../../bower_components/sammy/lib/sammy',
				moment: '../../bower_components/moment/moment',
				amplify: '../../bower_components/amplify/lib/amplify.store.min',
				mozart: '../../bower_components/mozart/mozart',
				views: '../views'
			},
			shim: {
				mozart: {
					exports: 'mozart'
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
		};
	});
})();