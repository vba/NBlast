(function() {
    'use strict';

    requirejs(['config'], function(config) {
	    requirejs.config(config);
	    requirejs(['routes'], function(routes) {
	        routes.run();
	    });
    });
})();
