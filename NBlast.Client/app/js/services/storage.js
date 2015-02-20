(function () {
	'use strict';
	var config = require('../config'),
		StorageService;

	StorageService = (function () {
		function StorageService() { }

		StorageService.prototype.storeFilters = function (from, till) {
			var store = config.amplify().store,
			    filter = {
				    from: from,
				    till: till
			    };
			store('filter', filter);
			return this;
		};
		StorageService.prototype.storeSort = function (field, reverse) {
			var store = config.amplify().store,
			    sort = {
				    reverse: reverse,
				    field: field
			    };
			store('sort', sort);
			return this;
		};
		StorageService.prototype.storeSearch = function (type) {
			var search = {
				type: type
			};
			config.amplify().store('search', search);
			return this;
		};
		StorageService.prototype.clearFilters = function () {
			config.amplify().store('filter', null);
			return this;
		};
		StorageService.prototype.clearSort = function () {
			config.amplify().store('sort', null);
			return this;
		};
		StorageService.prototype.clearSearch = function () {
			config.amplify().store('search', null);
			return this;
		};
		StorageService.prototype.clearAll = function () {
			return this
				.clearFilters()
				.clearSearch()
				.clearSort();
		};

		return StorageService;
	})();

	module.exports = StorageService;
})();
