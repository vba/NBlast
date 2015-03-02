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

		//var indicator = null,
		//	settings = {
		//		element: '#indicationZone',
		//		type: 'info',
		//		allow_dismiss: false,
		//		delay: 0,
		//		timer: 0,
		//		spacing: 2,
		//		offset: 5,
		//		newest_on_top: true
		//	};

		//Indicator.prototype.display = function(message) {
		//	var $ = config.jquery();
		//	if (!_.isNull(indicator)) {
		//		$('#indicationZone').html('');
		//	}
		//	indicator = $.notify({ message: message }, settings);
		//};
        //
		//Indicator.prototype.close = function () {
		//	if (_.isNull(indicator)) {
		//		return false;
		//	}
		//	indicator.close();
		//	return true;
		//};

		return Indicator;
	})(Notifier);

	module.exports = new Indicator();
})();
