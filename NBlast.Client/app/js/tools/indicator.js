(function() {
	"use strict";

	var _        = require('underscore'),
		config   = require('../config'),
		Notifier = require('./notifier'),
		object   = require('./object'),
		Indicator;

	config.bootstrapNotify();

	Indicator = (function($super) {
		function Indicator() {
			$super.call(this);
			this.settings.delay         = 0;
			this.element                = '#indicationZone';
			this.settings.allow_dismiss = false;
		}

		object.extends(Indicator, $super);

		Indicator.prototype.display = function (message, type) {
			var $ = config.jquery();
			if (!_.isEmpty(this.container)) {
				$(this.settings.element).html('');
			}
			return $super.prototype.display.call(this, message, type);
		};
		return Indicator;
	})(Notifier);

	module.exports = Indicator;
})();
