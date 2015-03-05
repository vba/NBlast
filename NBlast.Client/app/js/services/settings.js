(function () {
	'use strict';
	var StorageService  = require('./storage'),
		storageService  = new StorageService(),
		config          = require('../config'),
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
		makeRequest: function (url, params) {
			var $ = config.jquery();
			return $.ajax({
				data: params || null,
				dataType: storageService.getCommunicationDataType(),
				jsonp: 'callback',
				url: url
			});
	},
		isTestEnv: function () {
			return (typeof describe) !== 'undefined' && (typeof it) !== 'undefined';
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings;
})();
