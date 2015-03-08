(function () {
	'use strict';
	var QueueService;

	QueueService = (function () {
		function QueueService () {}

		QueueService.prototype.countAll = function () {
			return 0;
		};

		return QueueService;
	})();

	module.exports = new QueueService();
})();