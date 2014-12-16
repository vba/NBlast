(function() {
    'use strict';

    angular.module('nblast')
        .controller('searchController', function($scope, searchService) {
            searchService.search({q: '*:*'}).$promise.then(function(data){
                debugger
                console.log(data);
            });
        });
})();
