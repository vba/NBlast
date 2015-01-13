// ReSharper disable once UseOfImplicitGlobalInFunctionScope

(function() {
    'use strict';
    var dependencies = [
        'angular', 
        'underscore',
        'services/search', 
        'services/config'
    ];
    define(dependencies, function(angular, _) {
        angular.module('nblast')
            .controller('searchController', [
                '$scope',
                '$routeParams',
                '$location',
                'searchService',
                'configService',
                function ($scope, $routeParams, $location, searchService, configService) {
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

                    $scope.getPages = function(total) {
                        if (!_.isNumber(total)) {
                            return [];
                        }
                        var amount = Math.ceil(total/configService.getItemsPerPage());
                        return _.range(1, amount+1);
                    }
                }
            ]);
    });
})();

