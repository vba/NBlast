(function() {
	'use strict';
	var config = require('../config'),
		ko = require('knockout'),
		settings = require('./settings'),
		markup;

	markup = {
		applyBindings: function (viewModel, view) {
			var $ = config.jquery(),
				container = $(settings.getViewsContainer());
			container.html(view);
			ko.cleanNode(container[0]);
			ko.applyBindings(viewModel, container[0]);
		}
	};

	//noinspection JSUnresolvedVariable
	module.exports = settings.isTestEnv() ? markup : Object.freeze(markup);
})();
