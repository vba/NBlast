mocker               = null
sinon                = require 'sinon'
config               = require '../../app/js/config'
chai                 = require 'chai'
sammy                = {setLocation: ->}
EmptySearchViewModel = require '../../app/js/viewModels/emptySearch'

chai.should()

describe 'When user interacts with empty search page', ->
	beforeEach( ->
		mocker = sinon.sandbox.create()
		mocker.stub(config, 'amplify', -> {store: ->})
		mocker.stub(config, 'sammy', -> -> sammy)
		mocker.stub(config, 'jquery', -> {trim: (x) -> [x].join('').trim()})
		mocker.stub(config, 'moment', -> require 'moment')
	)
	afterEach( -> mocker.restore())

	it 'Should store details and redirect to complete search page', ->
		# Given
		storeStub = mocker.stub(EmptySearchViewModel.prototype, 'storeAdvancedDetails', -> )
		sut = new EmptySearchViewModel()
		setLocationStub = mocker.stub(sammy, 'setLocation', -> )

		# When
		actual = sut.enterSearch(null, {keyCode: 13})

		# Then
		actual.should.be.equal false
		setLocationStub.calledOnce.should.be.equal true
		storeStub.calledOnce.should.be.equal true
