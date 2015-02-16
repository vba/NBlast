(function () {
	'use strict';
	var requireOrFalse = function(loader, path) {
		if (loader()) {
			require(path);
			return true;
		}
		return false;
	}, config;
	//noinspection UnnecessaryLocalVariableJS
	config = {
		jquery: function() {
			if (typeof window !== 'undefined') {
				window.jQuery = require('jquery');
				return window.jQuery;
			}
			return false;
		},
		sammy: function() {
			//noinspection JSUnresolvedVariable
			if (this.jquery()) {
				require('../../bower_components/sammy/lib/sammy');
				return typeof Sammy !== 'undefined' ? Sammy : {};
			}
			return false;
		},
		bootstrap: function() {
			if (this.jquery()) {
				require('../../bower_components/bootstrap/dist/js/bootstrap.min');
				return true;
			}
			return false;
		},
		bootstrapComponents: function() {
			if (this.bootstrap()) {
				require('../../bower_components/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min');
				return true;
			}
			return false;
		},
		amplify: function() {
			if (typeof window !== 'undefined') {
				return require('../../bower_components/amplify/lib/amplify.store.min').amplify;
			}
			return {};
		},
		moment: function() {
			var moment = require('moment');
			if (typeof window !== 'undefined') {
				window.moment = moment;
			}
            return moment;
		}
	};

	//noinspection JSUnresolvedVariable
	module.exports = config;
})();