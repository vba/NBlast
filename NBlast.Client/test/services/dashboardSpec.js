(function () {
	'use strict';
	var chai      = require('chai'),
	    settings  = require('../../app/js/services/settings'),
	    dashboard = require('../../app/js/services/dashboard'),
		fakes     = require('../fakes');

	chai.should();

	describe('When dashboard service is in use', function () {
		it('Should produce expected request url in group by case', function () {
			// Given
			var mocker = fakes.mocker(),
			    field = 'logger',
			    limit = 55,
			    part = 'dashboard/group-by/' + field + '/' + limit,
			    makeRequestStub = mocker.stub(settings, 'makeRequest'),
			    appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl'),
				actual;

			appendToBackendUrlStub
				.withArgs(part)
				.returns('www.api.com/' + part);

			makeRequestStub
				.withArgs('www.api.com/' + part)
				.returns(':)');

			// When
			actual = dashboard.groupBy(field, limit);

			// Then
			':)'.should.equal(actual);
		});
		it('Should produce expected request url in get-level-per-months case', function () {
			// Given
			var mocker = fakes.mocker(),
			    granularity = 'month',
			    number = -1,
			    part = 'dashboard/' + number + '/levels-per-' + granularity,
			    makeRequestStub = mocker.stub(settings, 'makeRequest'),
			    appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl'),
			    actual;

			appendToBackendUrlStub
				.withArgs(part)
				.returns('www.api.com/' + part);

			makeRequestStub
				.withArgs('www.api.com/' + part)
				.returns(':)');

			// When
			actual = dashboard.getLevelsPerMonth(number);

			// Then
			':)'.should.equal(actual);
		});
		it('Should produce expected request url in get-level-per-days case', function () {
			// Given
			var mocker = fakes.mocker(),
			    granularity = 'day',
			    number = 2,
			    part = 'dashboard/' + number + '/levels-per-' + granularity,
			    makeRequestStub = mocker.stub(settings, 'makeRequest'),
			    appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl'),
			    actual;

			appendToBackendUrlStub
				.withArgs(part)
				.returns('www.api.com/' + part);

			makeRequestStub
				.withArgs('www.api.com/' + part)
				.returns(':)');

			// When
			actual = dashboard.getLevelsPerDay(number);

			// Then
			':)'.should.equal(actual);
		});
		it('Should produce expected request url in get-level-per-week case', function () {
			// Given
			var mocker = fakes.mocker(),
			    granularity = 'week',
			    number = 2,
			    part = 'dashboard/' + number + '/levels-per-' + granularity,
			    makeRequestStub = mocker.stub(settings, 'makeRequest'),
			    appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl'),
			    actual;

			appendToBackendUrlStub
				.withArgs(part)
				.returns('www.api.com/' + part);

			makeRequestStub
				.withArgs('www.api.com/' + part)
				.returns(':)');

			// When
			actual = dashboard.getLevelsPerWeek(number);

			// Then
			':)'.should.equal(actual);
		});
		it('Should fail in get levels per day when number param is not a number', function () {
			// Given
			// When
			chai.expect(function() {
				dashboard.getLevelsPerDay('AAAA');
			}).to.throw('Number expected instead of AAAA');
			// Then
		});
	});

})();
