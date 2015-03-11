(function () {
	'use strict';
	var QueueViewModel     = require('../../app/js/viewModels/queue'),
	    Indicator          = require('../../app/js/tools/indicator'),
	    markupService      = require('../../app/js/services/markup'),
	    queueService       = require('../../app/js/services/queue'),
	    chai               = require('chai'),
	    fakes              = require('../fakes');

	chai.should();

	describe('When queue view model is in use', function () {
		describe('When queue view model is initialized', function () {
			it('Should indicate loading and apply binidngs', function () {
				// Given
				var mocker = fakes.mocker(),
					displayStub = mocker.stub(Indicator.prototype, 'display'),
				    applyBindingsStub = mocker.stub(markupService, 'applyBindings'),
					peekTopStub = mocker.stub(queueService, 'peekTop'),
					peekErrorStub = mocker.stub(),
					peekDoneStub = mocker.stub(),
					sut = new QueueViewModel();

				peekErrorStub.returns({done: peekDoneStub});
				peekTopStub.withArgs(50).returns({error: peekErrorStub});

				// When
				sut.bind();

				// Then
				displayStub.calledWith('Loading ...').should.equal(true);
				applyBindingsStub.calledWith(sut, '').should.equal(true);
				peekErrorStub.calledOnce.should.equal(true);
				peekDoneStub.calledOnce.should.equal(true);
			});
		});
	});
})();