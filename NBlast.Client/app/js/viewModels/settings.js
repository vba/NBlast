(function () {
	'use strict';
	var markupService     = require('../services/markup'),
		settingsService   = require('../services/settings'),
		ko                = require('knockout'),
		views             = require('../views'),
		SettingsViewModel;

	SettingsViewModel = (function () {
		function SettingsViewModel () {
			this.backendUrl = ko.observable(settingsService.getBackendUrl());
			this.dataType   = ko.observable(settingsService.getCommunicationDataType());
		}
		SettingsViewModel.prototype.bind = function () {
			markupService.applyBindings(this, views.getSettings());
		};

		return SettingsViewModel;
	})();

	module.exports = SettingsViewModel;
})();
