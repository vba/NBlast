(function () {
	'use strict';

	var routes   = require('./routes'),
	    ko       = require('knockout'),
		$        = require('./config').jquery(),
		Notifier = require('./tools/notifier'),
		AjaxConfigurator;

	ko.bindingHandlers.truncatedText = {
		update: function (element, valueAccessor, allBindingsAccessor) {
			var value  = ko.utils.unwrapObservable(valueAccessor()),
			    length = ko.utils.unwrapObservable(allBindingsAccessor().length)
				    || ko.bindingHandlers.truncatedText.defaultLength,
			    truncatedValue = value.length > length
				    ? value.substring(0, Math.min(value.length, length)) + " ..."
				    : value;
			ko.bindingHandlers.text.update(element, function () {
				return truncatedValue;
			});
		},
		defaultLength: 150
	};

	AjaxConfigurator = (function () {
		function AjaxConfigurator () { }
		var notifier = new Notifier();
		AjaxConfigurator.prototype.configure = function () {
			$(document).ajaxError(function( event, request, settings ) {
				notifier.error("Error requesting page " + settings.url);
			});
		};

		return AjaxConfigurator;
	})();

	new AjaxConfigurator().configure();
	routes.run();
})();
