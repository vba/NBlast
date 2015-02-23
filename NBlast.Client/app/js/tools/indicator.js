(function() {
	"use strict";

	var config = require('../config'),
		_ = require('underscore'),
		Indicator;

	config.bootstrapNotify();

	Indicator = (function(){
		function Indicator() {}

		var indicator = null,
			settings = {
				element: '#indicationZone',
				type: 'info',
				allow_dismiss: false,
				delay: 0,
				timer: 0,
				spacing: 2,
				offset: 5,
				newest_on_top: true
			};

		Indicator.prototype.display = function(message) {
			var $ = config.jquery();
			if (!_.isNull(indicator)) {
				$('#indicationZone').html('');
			}
			indicator = $.notify({ message: message }, settings);
		};

		Indicator.prototype.close = function () {
			if (_.isNull(indicator)) {
				return false;
			}
			indicator.close();
			return true;
		};

		return Indicator;
	})();

	module.exports = new Indicator();
})();
