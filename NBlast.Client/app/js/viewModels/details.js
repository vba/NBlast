(function() {
	'use strict';

	var _ = require('underscore'),
		ko = require('knockout'),
		ctor = require('mozart'),
		markupService = require('../services/markup'),
		searchService = require('../services/search'),
		detailsView = require('../../views/details.html'),
		DetailsViewModel;


	DetailsViewModel = ctor(function (prototype, $keys, $protected) {
		prototype.init = function(uuid) {
			if (!_.isString(uuid)) {
				throw new Error('uuid param must be a string');
			}

			this.uuid = ko.observable(uuid);
			this.details = ko.observable({});
		};
		$protected.onGetByIdDone = function(found) {
			this.details(_.first((found || {}).hits));
		};
		prototype.bind = function() {
			searchService
				.getById(this.uuid())
				.done($protected.onGetByIdDone.bind(this));

			markupService.applyBindings(this, detailsView);
		};
	});

	//noinspection JSUnresolvedVariable
	module.exports = DetailsViewModel;

/*	var dependencies = [
		'underscore',
		'knockout',
		'services/markup',
		'services/search',
		'text!views/details'
	];
	define(dependencies, function(_, 
								  ko, 
								  markupService, 
								  searchService,
								  detailsView) {

		var DetailsViewModel = function(uuid) {
			if (!_.isString(uuid)) {
				throw new Error('uuid param must be a string');
			}

			this.uuid = ko.observable(uuid);
			this.details = ko.observable({});
		};

		DetailsViewModel.prototype = _.extend({
			bind: function() {
				var me = this;
				
				searchService
					.getById(this.uuid())
					.done(function(found) {
						me.details(_.first((found || {}).hits));
					});

				markupService.applyBindings(this, detailsView);
			}
		});
		return DetailsViewModel;
	});*/
})();