(function () {
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
			return _.isFunction(window.describe) && _.isFunction(window.it);
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings;
})();

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
