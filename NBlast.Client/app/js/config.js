(function () {
	'use strict';
	//#JSCOVERAGE_IF
	var config;
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
		bootstrapNotify: function() {
			if (this.bootstrap()) {
				require('../../bower_components/remarkable-bootstrap-growl/bootstrap-notify.min');
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
		chartist: function() {
			if (typeof window !== 'undefined') {
				return require('../../bower_components/chartist/dist/chartist.min');
			}
			return Object;
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
	//#JSCOVERAGE_ENDIF
})();
