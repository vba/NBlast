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
				delay: 2000,
				timer: 0,
				spacing: 2,
				offset: 5,
				newest_on_top: true
			};
			this.container = {};
		}

		Notifier.prototype.display = function(message, type) {
			var $ = config.jquery(),
				local = _.extend(this.settings, {type: type || 'info'});
			if (!_.isEmpty(this.container)) {
				$(this.settings.element).html('');
			}
			this.container = $.notify({message: message}, local);
		};

		Notifier.prototype.success = function (message) {
			this.display(message, 'success');
		};
		Notifier.prototype.info = function (message) {
			this.display(message, 'info');
		};
		Notifier.prototype.warning = function (message) {
			this.display(message, 'warning');
		};
		Notifier.prototype.error = function (message) {
			this.display(message, 'danger');
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