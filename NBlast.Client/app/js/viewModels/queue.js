(function () {
	'use strict';

	var queue             = require('../services/queue'),
	    ko                = require('knockout'),
	    views             = require('../views'),
	    markupService     = require('../services/markup'),
	    QueueViewModel;

	QueueViewModel = (function () {
		function QueueViewModel () {}

		QueueViewModel.prototype.bind = function () {
			markupService.applyBindings(this, views.getQueue());
		};

		return QueueViewModel;
	})();

	module.exports = QueueViewModel;
})();