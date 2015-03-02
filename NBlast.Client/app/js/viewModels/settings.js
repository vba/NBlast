(function () {
	'use strict';
	var markupService     = require('../services/markup'),
		views             = require('../views'),
		SettingsViewModel;

	SettingsViewModel = (function () {
		function SettingsViewModel () {}
		SettingsViewModel.prototype.bind = function () {
			markupService.applyBindings(this, views.getSettings());
		};

		return SettingsViewModel;
	})();

	module.exports = SettingsViewModel;
})();
