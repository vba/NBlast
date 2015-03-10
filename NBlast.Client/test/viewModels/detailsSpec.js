(function() {
	'use strict';
	var DetailsViewModel = require('../../app/js/viewModels/details'),
		chai             = require('chai'),
		fakes            = require('../fakes');

	chai.should();
	describe('When details view model is in use', function () {
		it('Should fail view model creation when no params are supplied', function () {
			// Given
			// When
			// Then
			chai.expect(function () {
				new DetailsViewModel(123);
			}).to.throw('uuid param must be a string');
		});
		it('Should fail view model creation when no params are supplied', function () {
			// Given
			// When
			// Then
			chai.expect(function () {
				new DetailsViewModel();
			}).to.throw('uuid param must be a string');
		});
		it('Should call get details by id and render it via markup service', function () {
			// Given
			var mocker              = fakes.mocker(),
			    $                   = fakes.jquery(),
			    uuid                = 'uuid1',
			    markupService       = require('../../app/js/services/markup'),
			    searchService       = require('../../app/js/services/search'),
				sut                 = new DetailsViewModel(uuid),
				getByIdStub         = mocker.stub(searchService, 'getById'),
			    applyBindingsStub   = mocker.stub(markupService, 'applyBindings'),
				result              = {
					error: function() {
						return {
							done: function(callback) {
								callback({hits: ['some']});
							}
						};
					}
				};

			$.notify = mocker.stub();
			applyBindingsStub.returns(true);
			getByIdStub.withArgs(uuid).returns(result);

			// When
			sut.bind();

			// Then
			sut.details().should.equal('some');
			getByIdStub.calledOnce.should.equal(true);
			applyBindingsStub.calledOnce.should.equal(true);
		});
	});
})();