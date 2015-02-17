EmptySearchViewModel = require '../../app/js/viewModels/emptySearch'
sinon                = require 'sinon'
chai                 = require 'chai'
fakes                = require '../fakes'

chai.should()

describe 'When user interacts with empty search page', ->

	it 'Should store details and redirect to complete search page', ->
		# Given
		storeStub = fakes.mocker().stub(EmptySearchViewModel.prototype, 'storeAdvancedDetails', -> )
		sut = new EmptySearchViewModel()
		setLocationStub = fakes.mocker().stub(sut.sammy(), 'setLocation', -> )

		# When
		actual = sut.enterSearch(null, {keyCode: 13})

		# Then
		actual.should.be.equal false
		setLocationStub.calledOnce.should.be.equal true
		storeStub.calledOnce.should.be.equal true
