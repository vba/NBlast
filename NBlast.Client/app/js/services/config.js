define(['angular'], function(angular) {
    'use strict';
    angular.module('nblast')
        .service('configService', [function() {
            return {
                getBackendUrl: function() {
                    return "http://localhost:9090/api/";
                },
                appendToBackendUrl: function(path) {
                    return [this.getBackendUrl(), path].join('');
                },
                getJsonFormat: function() {
                    return { format: 'json', callback: 'JSON_CALLBACK' };
                }
            };
        }]);
});
