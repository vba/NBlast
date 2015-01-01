// ReSharper disable once UseOfImplicitGlobalInFunctionScope
(function() {
    'use strict';

    angular.module('nblast')
        .controller('searchController', [
            '$scope',
            '$routeParams',
            '$location',
            'searchService',
            function ($scope, $routeParams, $location, searchService) {
                var query = $routeParams.query;

                if (!_.isEmpty(query)) {
                    var searchQuery = { q: decodeURIComponent(query) };
                    $scope.searchQuery = searchQuery.q;
                    $scope.searchResult = searchService.search(searchQuery);
                }

                $scope.search = function () {
                    var searchQuery = encodeURIComponent($scope.searchQuery || '*:*');
                    $location.path(['/search/', searchQuery].join(''));
                };

                $scope.defineFoundIcon = function(level) {
                    return {
                        'DEBUG' : 'cog',
                        'INFO' : 'info',
                        'WARN' : 'warning',
                        'ERROR' : 'bolt',
                        'FATAL' : 'fire'
                    }[level.toUpperCase()] || 'asterisk';
                };

                $scope.onHitClick = function(hit) {
                    $location.path(['/details/', hit.id].join(''));
                };
            }
        ]);
})();
