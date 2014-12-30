(function() {
    'use strict';
    angular.module('nblast')
        .controller('detailsController', [
            '$scope',
            '$route',
            'searchService',
            function($scope, $route, searchService) {
                var hitId = $route.current.params.hitId;
                searchService.getById(hitId).$promise.then(function(data){
                    if (_.isEmpty(data.hits)) {
                        return; // Notify user about error
                    }
                    $scope.hit = data.hits[0];
                });
        }]);
})();