(function () {
	'use strict';
	var config = require('../config'),
		//_      = require('underscore'),
		DataType, settings;

	 DataType = Object.freeze({
		 JSON: 'json',
		 JSONP: 'jsonp'
	 });

	settings = {
		getBackendUrl: function () {
			var result = (config.amplify('settings') || {}).backendUrl;
			return result || "http://localhost:9090/api/";
		},
		getCommunicationDataType: function () {
			var result = (config.amplify('settings') || {}).communicationDataType;
			return result || DataType.JSON;
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
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings;
})();
