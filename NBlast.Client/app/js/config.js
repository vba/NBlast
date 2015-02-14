(function () {
	'use strict';

	//noinspection UnnecessaryLocalVariableJS
	var config = {
		jquery: function() {
			return jQuery;
		},
		sammy: function() {
			//noinspection JSUnresolvedVariable
			return Sammy;
		},
		amplify: function() {
			return require('../../bower_components/amplify/lib/amplify.store.min').amplify;
		},
		moment: function() {
			return moment;
		}
	};

	//noinspection JSUnresolvedVariable
	module.exports = config;

})();