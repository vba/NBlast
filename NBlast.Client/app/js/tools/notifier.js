(function () {
	'use strict';

	var config = require('../config'),
	    _      = require('underscore'),
		Notifier;

	config.bootstrapNotify();

	Notifier = (function () {
		function Notifier () {
			this.settings = {
				element: '#notificationZone',
				type: 'info',
				allow_dismiss: true,
				delay: 3000,
				timer: 0,
				spacing: 2,
				offset: 5,
				newest_on_top: true
			};
			this.container = {};
		}

		Notifier.prototype.display = function(message, type) {
			var $ = config.jquery();
			if (!_.isEmpty(this.container)) {
				$(this.settings.element).html('');
			}
			this.container = $.notify({message: message, type: type || 'info'}, this.settings);
		};

		Notifier.prototype.close = function() {
			if (_.isEmpty(this.container)) {
				return false;
			}
			this.container.close();
			return true;
		};

		return Notifier;
	})();

	module.exports = Notifier;
})();