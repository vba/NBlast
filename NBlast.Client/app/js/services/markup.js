define(['knockout', 'jquery', 'services/settings'], function (ko, $, settingsService) {
	'use strict';

	return Object.freeze({
		applyBindings: function (viewModel, view) {
			var container = $(settingsService.getViewsContainer());
			container.html(view);
			ko.cleanNode(container[0]);
			ko.applyBindings(viewModel, container[0]);
		}
	});
});