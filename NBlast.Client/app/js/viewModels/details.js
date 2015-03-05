(function() {
	'use strict';

	var _             = require('underscore'),
		ko            = require('knockout'),
		views         = require('../views'),
		markupService = require('../services/markup'),
		searchService = require('../services/search'),
		Indicator     = require('../tools/indicator'),
		DetailsViewModel;


	DetailsViewModel = (function () {
		function DetailsViewModel (uuid) {
			if (!_.isString(uuid)) {
				throw new Error('uuid param must be a string');
			}
			this.uuid = ko.observable(uuid);
			this.details = ko.observable({});
		}
		var indicator = new Indicator(), $private = {};

		$private.onGetByIdDone = function(found) {
			this.details(_.first((found || {}).hits));
			indicator.close();
		};
		DetailsViewModel.prototype.bind = function() {
			indicator.display('Loading ...');
			searchService
				.getById(this.uuid())
				.error(indicator.close.bind(indicator))
				.done($private.onGetByIdDone.bind(this));

			markupService.applyBindings(this, views.getDetails());
		};
		return DetailsViewModel;
	})();

	//noinspection JSUnresolvedVariable
	module.exports = DetailsViewModel;
})();