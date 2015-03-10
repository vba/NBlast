(function () {
	'use strict';

	var chai, fakes, queue, settings, sinon;

	sinon    = require('sinon');
	chai     = require('chai');
	fakes    = require('../fakes');
	settings = require('../../app/js/services/settings');
	queue    = require('../../app/js/services/queue');

	chai.should();

	describe('When queue service is in use', function () {
		describe('When it consults top messages', function () {
			it('Should work as expected without precision of top number', function () {
				// Given
				var actual,
				    mocker = fakes.mocker(),
					makeRequestStub = mocker.stub(settings, 'makeRequest'),
					appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl');

				appendToBackendUrlStub
					.withArgs('queue/50/top')
					.returns('www.final.com');

				makeRequestStub
					.withArgs('www.final.com')
					.returns(true);

				// When
				 actual = queue.peekTop();

				// Then
				appendToBackendUrlStub.calledOnce.should.equal(true);
				makeRequestStub.calledOnce.should.equal(true);
				actual.should.equal(true);
			});

			it('Should work as expected with specified top number', function () {
				// Given
				var actual,
				    mocker = fakes.mocker(),
				    makeRequestStub = mocker.stub(settings, 'makeRequest'),
				    appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl');

				appendToBackendUrlStub
					.withArgs('queue/23/top')
					.returns('www.final.com');

				makeRequestStub
					.withArgs('www.final.com')
					.returns(true);

				// When
				actual = queue.peekTop(23);

				// Then
				appendToBackendUrlStub.calledOnce.should.equal(true);
				makeRequestStub.calledOnce.should.equal(true);
				actual.should.equal(true);
			});
		});
	});

})();