(function () {
    'use strict';
    angular.module('nblast')
        .service('searchService', ['$resource', function ($resource) {
            return $resource('http://localhost:9090/api/searcher/search', {}, {
                search: { 'method': 'GET' }
            });

        }]);
})();
