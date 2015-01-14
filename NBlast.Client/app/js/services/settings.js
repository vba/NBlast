define(['knockout', 'jquery'], function(ko, $) {
	'use strict';
	// angular.module('nblast')
	// 	.service('configService', [function() {
	// 		return {
	// 			getBackendUrl: function() {
	// 				return "http://localhost:9090/api/";
	// 			},
	// 			appendToBackendUrl: function(path) {
	// 				return [this.getBackendUrl(), path].join('');
	// 			},
	// 			getJsonFormat: function() {
	// 				return { format: 'json', callback: 'JSON_CALLBACK' };
	// 			},
	// 			getItemsPerPage: function() {
	// 				return 15;
	// 			}
	// 		};
	// 	}]);


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
