// ReSharper disable once UseOfImplicitGlobalInFunctionScope
(function() {
    'use strict';

    angular.module('nblast')
        .controller('searchController', [
            '$scope',
            '$route',
            '$location',
            'searchService',
            function ($scope, $route, $location, searchService) {
                var query = $route.current.params.query;

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
                    localStorage.setItem(hit.id, JSON.stringify(hit));
                    $location.path(['/details/', hit.id].join(''));
                };
            }
        ]);
})();
