// ReSharper disable once UseOfImplicitGlobalInFunctionScope
define(['angular', 'services/search'], function(angular) {
    'use strict';
    angular.module('nblast')
        .controller('searchController', [
            '$scope',
            '$routeParams',
            '$location',
            'searchService',
            function ($scope, $routeParams, $location, searchService) {
                var query = $routeParams.query,
                    page = $routeParams.page,
                    searchQuery;

                if (!_.isEmpty(query)) {
                    searchQuery = { q: decodeURIComponent(query) };
                    $scope.searchQuery = searchQuery.q;
                    $scope.searchResult = searchService.search(searchQuery);
                }

                $scope.search = function () {
                    var encodedQuery = encodeURIComponent($scope.searchQuery || '*:*');
                    $location.path(['/search/1/', encodedQuery].join(''));
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
});
