(function() {
    'use strict';
    angular.module('nblast')
        .service('searchService', ['$resource', '$http', function($resource, $http) {
            return $resource('http://localhost:9090/api/searcher/search', {}, {
                search: { 'method': 'GET' }
            });

        }]);
})();
