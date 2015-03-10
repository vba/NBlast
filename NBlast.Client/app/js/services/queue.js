(function () {
	'use strict';
	var settings = require('./settings'),
		QueueService;

	QueueService = (function () {
		function QueueService () {}

		QueueService.prototype.peekTop = function (top) {
			var resourcesPart = [top || 50, '/top'].join(''),
			    url = settings.appendToBackendUrl('queue/' + resourcesPart);
			return settings.makeRequest(url);
		};

		return QueueService;
	})();

	module.exports = new QueueService();
})();