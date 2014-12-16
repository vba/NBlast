(function() {
    'use strict';
    var jsonFormat = { format: 'json', jsoncallback: 'JSON_CALLBACK' };
    angular.module('nblast')
        .factory('searchService', ['$resource', function($resource) {
            return $resource('http://localhost:9090/api/searcher/search', jsonFormat, {
                search: { 'method': 'JSONP' }
            });
        }]);
})();
