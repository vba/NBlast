(function () {
	'use strict';
	var markupService     = require('../services/markup'),
		ko                = require('knockout'),
		views             = require('../views'),
		Notifier          = require('../tools/notifier'),
		StorageService    = require('../services/storage'),
		SettingsViewModel, DataType;

	DataType = Object.freeze({
		JSON: 'json',
		JSONP: 'jsonp'
	});

	SettingsViewModel = (function () {
		function SettingsViewModel () {
			this.storageService = new StorageService();
			this.backendUrl     = ko.observable(this.storageService.getBackendUrl() || "http://localhost:9090/api/");
			//noinspection JSUnusedGlobalSymbols
			this.dataType       = ko.observable(this.storageService.getCommunicationDataType() || DataType.JSON);
		}
		SettingsViewModel.prototype.bind = function () {
			markupService.applyBindings(this, views.getSettings());
		};
		//noinspection JSUnusedGlobalSymbols
		SettingsViewModel.prototype.save = function (d, e) {
			e.stopPropagation();
			new Notifier().success('Saved');
			this.storageService.storeSettings(this.backendUrl(), this.dataType());
		};

		return SettingsViewModel;
	})();

	module.exports = SettingsViewModel;
})();
