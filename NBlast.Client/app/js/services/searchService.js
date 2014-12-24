(function () {
    'use strict';
    var jsonFormat = { format: 'json', callback: 'JSON_CALLBACK' };
    angular.module('nblast')
        .service('searchService', ['$resource', function ($resource) {
            return $resource('http://localhost:9090/api/searcher/search', jsonFormat, {
                search: { 'method': 'JSONP' }
            });
//            return $resource('http://localhost:9090/api/searcher/search', {}, {
//                search: { 'method': 'GET' }
//            });
        }]);
})();
