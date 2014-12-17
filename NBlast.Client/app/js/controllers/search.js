(function() {
    'use strict';

    angular.module('nblast')
        .controller('searchController', ['$scope', 'searchService', function($scope, searchService) {
            searchService.search({q: '*:*'}).$promise.then(function(data){
                console.log(data);
            });
        }]);
})();
