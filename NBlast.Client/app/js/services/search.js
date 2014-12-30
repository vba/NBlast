(function () {
    'use strict';
    angular.module('nblast')
        .service('searchService', [
            '$resource',
            'configService',
            function ($resource, configService) {
                var searchUrl = configService.appendToBackendUrl('searcher/search'),
                    jsonFormat = configService.getJsonFormat(),
                    xhr = $resource(searchUrl, jsonFormat, {
                        search: {
                            'method': 'JSONP'
                        }
                    });

                return _.extend({}, xhr);
        }]);
})();