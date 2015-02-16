(function (global) {
	'use strict';
	var _ = require('underscore'), settings;

	settings = {
		getBackendUrl: function () {
			return "http://localhost:9090/api/";
		},
		appendToBackendUrl: function (path) {
			return [this.getBackendUrl(), path].join('');
		},
		getItemsPerPage: function () {
			return 10;
		},
		getViewsContainer: function () {
			return '#pageWrapper';
		},
		isTestEnv: function () {
			return (typeof describe) !== 'undefined' && (typeof it) !== 'undefined';
		},
		loadConfig: function () {
			if (this.isTestEnv()) {
				throw Error("Needs to be a stub");
			}
			return require('../config');
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings;
})(this);

/*define(['underscore'], function(_) {
	'use strict';

	var settings = {
		getBackendUrl: function () {
			return "http://localhost:9090/api/";
		},
		appendToBackendUrl: function (path) {
			return [this.getBackendUrl(), path].join('');
		},
		getItemsPerPage: function () {
			return 10;
		},
		getViewsContainer: function () {
            return '#pageWrapper';
        },
		isTestEnv: function () {
			return _.isFunction(window.describe) && _.isFunction(window.it);
		}
    };

    return settings.isTestEnv() ? settings : Object.freeze(settings);
});*/
