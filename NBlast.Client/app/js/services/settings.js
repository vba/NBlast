define(['knockout', 'jquery'], function(ko, $) {
	'use strict';

	return Object.freeze({
		getBackendUrl: function() {
			return "http://localhost:9090/api/";
		},
		appendToBackendUrl: function(path) {
			return [this.getBackendUrl(), path].join('');
		},
		getItemsPerPage: function() {
			return 15;
		},
		getViewsContainer: function() {
			return '#pageWrapper';
		}
	});
});
