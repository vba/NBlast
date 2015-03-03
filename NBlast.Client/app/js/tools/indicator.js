(function() {
	"use strict";

	var config   = require('../config'),
		Notifier = require('./notifier'),
		object   = require('./object'),
		//_        = require('underscore'),
		Indicator;

	config.bootstrapNotify();

	Indicator = (function($super) {
		function Indicator() {
			$super.call(this);
			this.settings.element       = '#indicationZone';
			this.settings.delay         = 0;
			this.settings.allow_dismiss = false;
		}

		object.extends(Indicator, $super);

		return Indicator;
	})(Notifier);

	module.exports = Indicator;
})();
