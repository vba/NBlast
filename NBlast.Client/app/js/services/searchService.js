(function() {
    'use strict';
//    var jsonFormat = { format: 'json', jsoncallback: 'JSON_CALLBACK' };
    angular.module('nblast')
        .service('searchService', ['$resource', '$http', function($resource, $http) {
//            return $resource('http://localhost:9090/api/searcher/search', jsonFormat, {
//                search: { 'method': 'JSONP' }
//            });
            //$http.defaults.useXDomain = true;
            debugger
            return $resource('http://localhost:9090/api/searcher/search', {}, {
                search: { 'method': 'GET' }
            });

        }]);
})();
