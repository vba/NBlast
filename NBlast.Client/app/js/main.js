(function () {
	'use strict';

	var routes   = require('./routes'),
		$        = require('./config').jquery(),
		Notifier = require('./tools/notifier'),
		AjaxConfigurator;

	AjaxConfigurator = (function () {
		function AjaxConfigurator () { }
		var notifier = new Notifier();
		AjaxConfigurator.prototype.configure = function () {
			$(document ).ajaxError(function( event, request, settings ) {
				notifier.error("Error requesting page " + settings.url);
			});
		};

		return AjaxConfigurator;
	})();

	new AjaxConfigurator().configure();
	routes.run();
})();
