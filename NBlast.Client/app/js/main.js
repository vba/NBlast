(function () {
	'use strict';

	var routes = require('./routes');
	routes.run();
/*	requirejs(['config'], function (config) {
		requirejs.config(config);
		requirejs(['routes'], function (routes) {
			routes.run();
		});
	});*/
})();
