define(['angular'], function(angular) {
    'use strict';
    angular.module('nblast')
        .controller('detailsController', [
            '$scope',
            '$routeParams',
            'searchService',
            function($scope, $routeParams, searchService) {
                var hitId = $routeParams.hitId;
                searchService.getById(hitId).$promise.then(function(data){
                    if (_.isEmpty(data.hits)) {
                        return; // Notify user about error
                    }
                    $scope.hit = data.hits[0];
                });
        }]);
});
