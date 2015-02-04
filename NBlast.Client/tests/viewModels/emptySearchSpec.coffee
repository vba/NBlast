deps = [
	'chai'
    'sinon'
	'sammy'
    'viewModels/emptySearch'
]

define deps, (chai, sinon, sammy, EmptySearchViewModel) ->
	mocker = null
	chai.should()
	window.beforeEach -> mocker = sinon.sandbox.create()
	window.afterEach -> mocker.restore()

	describe 'When user interacts with empty search page', ->
		it 'Should store details and redirect to complete search page', ->
			# Given
			sut = new EmptySearchViewModel()
			setLocationStub = mocker.stub(sut.sammy, 'setLocation', -> )
			storeStub = mocker.stub(sut, 'storeAdvancedDetails', -> )

			# When
			actual = sut.enterSearch(null, {keyCode: 13})

			# Then
			actual.should.be.equal false
			setLocationStub.calledOnce.should.be.equal true
			storeStub.calledOnce.should.be.equal true
