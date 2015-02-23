(function () {
	'use strict';
	var settings;

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
		}
	};
	//noinspection JSUnresolvedVariable
	module.exports = settings;
})();
