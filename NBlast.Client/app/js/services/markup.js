(function() {
	'use strict';
	var $ = require('../config').jquery(),
		ko = require('knockout'),
		settings = require('./settings'),
		markup;

	markup = {
		applyBindings: function (viewModel, view) {
			var container = $(settings.getViewsContainer());
			container.html(view);
			ko.cleanNode(container[0]);
			ko.applyBindings(viewModel, container[0]);
		}
	};

	//noinspection JSUnresolvedVariable
	module.exports = settings.isTestEnv() ? markup : Object.freeze(markup);
})();

/*

define(['knockout', 'jquery', 'services/settings'], function (ko, $, settings) {
	'use strict';

	var markup = {
		applyBindings: function (viewModel, view) {
			var container = $(settings.getViewsContainer());
			container.html(view);
			ko.cleanNode(container[0]);
			ko.applyBindings(viewModel, container[0]);
        }
    };
	return settings.isTestEnv() ? markup : Object.freeze(markup);
});*/
