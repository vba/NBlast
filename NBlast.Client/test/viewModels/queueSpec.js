(function () {
	'use strict';
	var QueueViewModel     = require('../../app/js/viewModels/queue'),
		SearchViewModel    = require('../../app/js/viewModels/search'),
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
			it('Should bind callback data to the view after peek is over', function () {
				// Given
				var mocker = fakes.mocker(),
				    closeStub = mocker.stub(Indicator.prototype, 'close'),
				    sut = new QueueViewModel();

				sut.logs = mocker.stub();
				sut.total = mocker.stub();

				// When
				sut.onPeekTopDone({logs: ':)', total: 1});

				// Then
				closeStub.calledOnce.should.equal(true);
				sut.logs.calledWith(':)');
				sut.total.calledWith(1);
			});
		});
		describe('When user interact with queue view', function () {
			it('Should call search view model function that defines found icon', function () {
				// Given
				var mocker = fakes.mocker(),
				    defineFoundIconStub = mocker.stub(SearchViewModel.prototype, 'defineFoundIcon'),
					sut = new QueueViewModel(),
					actual;

				defineFoundIconStub.withArgs(1, 2, 3).returns(':)');

				// When
				actual = sut.defineFoundIcon(1, 2, 3);

				// Then
				actual.should.equal(':)');
			});
		});
	});
})();