(function() {
    'use strict';
    angular.module('nblast')
        .controller('detailsController', [
            '$scope',
            '$route',
            function($scope, $route) {
                var hitId = $route.current.params.hitId,
                    hit = JSON.parse(localStorage.getItem(hitId));
                localStorage.removeItem(hitId);

                $scope.hit = hit;
        }]);
})();