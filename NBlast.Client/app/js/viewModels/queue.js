(function () {
	'use strict';

	var queue             = require('../services/queue'),
	    ko                = require('knockout'),
	    views             = require('../views'),
	    markupService     = require('../services/markup'),
	    Indicator         = require('../tools/indicator'),
	    QueueViewModel;

	QueueViewModel = (function () {
		function QueueViewModel () {
			this.logs = ko.observable([]);
			this.total = ko.observable(0);
		}

		var indicator = new Indicator(), $private = {};

		$private.onPeekTopDone = function (data) {
			this.logs(data.logs || []);
			this.total(data.total || 0);
			indicator.close();
		};
		QueueViewModel.prototype.bind = function () {
			indicator.display('Loading ...');

			markupService.applyBindings(this, views.getQueue());

			queue.peekTop(50)
				.error(indicator.close.bind(indicator))
				.done($private.onPeekTopDone.bind(this));
		};

		return QueueViewModel;
	})();

	module.exports = QueueViewModel;
})();