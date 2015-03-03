(function () {
	'use strict';
	var StorageService  = require('./storage'),
		storageService  = new StorageService(),
		settings;

	settings = {
		appendToBackendUrl: function (path) {
			return [storageService.getBackendUrl(), path].join('');
		},
		getItemsPerPage: function () {
			return 10;
		},
		getViewsContainer: function () {
			return '#pageWrapper';
		},
		isTestEnv: function () {
			return (typeof describe) !== 'undefined' && (typeof it) !== 'undefined';
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings;
})();
