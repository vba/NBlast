(function () {
	'use strict';
	var config = require('../app/js/config'),
		views = require('../app/js/views'),
		sinon = require('sinon'),
		mocker = null,
		nothing, fakes, amplify, sammy, $;

	nothing = function () {
		return '';
	};
	amplify = {
		store: function () {
			return '';
		}
	};
	$ = {
		trim: function (str) {
			return [str].join('').trim();
		},
		getJSON: nothing
	};
	sammy = {
		setLocation: nothing,
		getLocation: nothing,
		runRoute: nothing
	};

	fakes = {
		mocker: function() {
			return mocker;
		},
		amplify: function () {
			return amplify;
		},
		jquery: function () {
			return $;
		},
		sammy: function () {
			return sammy;
		}
	};

	beforeEach(function() { // jshint ignore:line
		mocker = sinon.sandbox.create();
		mocker.stub(config, 'sammy', function() {
			return fakes.sammy;
		});
		mocker.stub(config, 'amplify', fakes.amplify);
		mocker.stub(config, 'jquery', fakes.jquery);
		mocker.stub(views, 'getSearch', nothing);
		mocker.stub(views, 'getDashboard', nothing);
		mocker.stub(views, 'getDetails', nothing);
	});

	afterEach(function(){ // jshint ignore:line
		if (fakes.mocker()) {
			fakes.mocker().restore();
		}
	});

	module.exports = fakes;
})();