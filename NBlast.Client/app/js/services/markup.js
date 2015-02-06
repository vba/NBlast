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
});