(function() {
	'use strict';

	var _             = require('underscore'),
		ko            = require('knockout'),
		Class         = require('jsface').Class,
		detailsView   = require('../../views/details.html'),
		markupService = require('../services/markup'),
		searchService = require('../services/search'),
		DetailsViewModel;


	DetailsViewModel = Class(function () {
		var prototype = {}, $private = {}

		$private.onGetByIdDone = function(found) {
			this.details(_.first((found || {}).hits));
		};
		prototype.constructor = function(uuid) {
			if (!_.isString(uuid)) {
				throw new Error('uuid param must be a string');
			}

			this.uuid = ko.observable(uuid);
			this.details = ko.observable({});
		};
		prototype.bind = function() {
			searchService
				.getById(this.uuid())
				.done($private.onGetByIdDone.bind(this));

			markupService.applyBindings(this, detailsView);
		};

		return prototype;
	});

	//noinspection JSUnresolvedVariable
	module.exports = DetailsViewModel;
})();