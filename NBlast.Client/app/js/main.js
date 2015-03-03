(function () {
	'use strict';

	var routes = require('./routes'),
		$      = require('./config').jquery();

	$( document ).ajaxError(function( event, request, settings ) {
		console.error("Error requesting page " + settings.url);
	});

	routes.run();
})();
