(function () {
    'use strict';
    angular.module('nblast')
        .service('dashboardService', [
            '$resource',
            'configService',
            function ($resource, configService) {
                var searchUrl = configService.appendToBackendUrl('searcher/search'),
                    jsonFormat = configService.getJsonFormat();

                return $resource(searchUrl, jsonFormat, {
                    search: {
                        'method': 'JSONP'
                    }
                });
        }]);
})();