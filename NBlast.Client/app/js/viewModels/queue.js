(function () {
	'use strict';

	var ko                = require('knockout'),
		queue             = require('../services/queue'),
	    views             = require('../views'),
	    config            = require('../config'),
	    Indicator         = require('../tools/indicator'),
	    markupService     = require('../services/markup'),
	    SearchViewModel   = require('../viewModels/search'),
	    QueueViewModel;

	QueueViewModel = (function () {
		function QueueViewModel () {
			this.logs   = ko.observable([]);
			this.total  = ko.observable(0);
			this.moment = config.moment();
		}

		var indicator = new Indicator(), $private = {};

		$private.onPeekTopDone = function (data) {
			this.logs(data.logs || []);
			this.total(data.total || 0);
			indicator.close();
		};

		QueueViewModel.prototype.defineFoundIcon = function() {
			var args = [].slice.call(arguments, 0);
			SearchViewModel.prototype.defineFoundIcon.apply(this, args);
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