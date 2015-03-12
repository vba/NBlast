(function () {
	'use strict';

	var _                 = require('underscore'),
		ko                = require('knockout'),
		queue             = require('../services/queue'),
	    views             = require('../views'),
	    config            = require('../config'),
	    Indicator         = require('../tools/indicator'),
	    markupService     = require('../services/markup'),
	    settingsService   = require('../services/settings'),
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
			return SearchViewModel.prototype.defineFoundIcon.apply(this, args);
		};

		QueueViewModel.prototype.bind = function () {
			indicator.display('Loading ...');

			markupService.applyBindings(this, views.getQueue());

			queue.peekTop(50)
				.error(indicator.close.bind(indicator))
				.done($private.onPeekTopDone.bind(this));
		};

		if (settingsService.isTestEnv()) {
			_.extend(QueueViewModel.prototype, _.clone($private));
		}

		return QueueViewModel;
	})();

	module.exports = QueueViewModel;
})();