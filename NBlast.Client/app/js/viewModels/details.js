(function() {
	'use strict';
	var dependencies = [
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
	});
})();