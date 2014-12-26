// ReSharper disable once UseOfImplicitGlobalInFunctionScope
(function() {
    'use strict';

    angular.module('nblast')
        .controller('searchController', [
            '$scope',
            'searchService',
            function ($scope, searchService) {
                $scope.search = function () {
                    var params = { q: $scope.searchQuery || '*:*' };
                    $scope.searchResult = searchService.search(params);
//                    searchService.search(params).$promise.then(function (data) {
//                        $scope.searchResult = _.omit(data, function (value, key) {
//                            return key.slice(0, 1) === '$' || key.toUpperCase() === 'TOJSON';
//                        });
//                    });
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
            }
        ]);
})();
