(function() {
    'use strict';

    angular.module('nblast')
        .controller('searchController', ['$scope', 'searchService', function($scope, searchService) {
            debugger
            searchService.search({q: '*:*'}).$promise.then(function(data){
                debugger
                console.log(data);
            });
        }]);
})();
