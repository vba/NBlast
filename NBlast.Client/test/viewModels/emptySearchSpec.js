(function () {
	'use strict';
	var
		EmptySearchViewModel = require('../../app/js/viewModels/emptySearch'),
		chai                 = require('chai'),
		searchService        = require('../../app/js/services/search'),
		fakes                = require('../fakes');

	chai.should();

	describe('When user interacts with empty search page', function () {
		it('Should store details and redirect to complete search page', function () {
			// Given
			var actual, setLocationStub, storeStub, sut, mocker = fakes.mocker();

            storeStub = mocker.stub(EmptySearchViewModel.prototype, 'storeAdvancedDetails', function () { });
			sut = new EmptySearchViewModel();
			setLocationStub = mocker.stub(sut.sammy(), 'setLocation', function () { });
			mocker.stub(searchService, 'getPathName').returns('/');

			// When
			actual = sut.enterSearch(null, {keyCode: 13});

			// Then
			actual.should.be.equal(false);
			setLocationStub.calledOnce.should.be.equal(true);
			storeStub.calledOnce.should.be.equal(true);
		});
	});

})();