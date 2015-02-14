sinon       = require 'sinon'
config      = require '../../app/js/config'
mocker      = sinon.sandbox.create()
chai        = require 'chai'
getSutType  = -> require '../../app/js/viewModels/emptySearch'
sammy       = -> {setLocation: ->}

chai.should()

describe 'When user interacts with empty search page', ->
	beforeEach( ->
		mocker = sinon.sandbox.create()
		mocker.stub(config, 'sammy', -> sammy)
	)
	afterEach( -> mocker.restore())

	it 'Should store details and redirect to complete search page', ->
		# Given
		Sut = getSutType()
		storeStub = mocker.stub(Sut.prototype, 'storeAdvancedDetails', -> )
		sut = new Sut()
		setLocationStub = mocker.stub(sut.sammy, 'setLocation', -> )

		# When
		actual = sut.enterSearch(null, {keyCode: 13})

		# Then
		actual.should.be.equal false
		setLocationStub.calledOnce.should.be.equal true
		storeStub.calledOnce.should.be.equal true
