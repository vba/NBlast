deps = [
	'chai'
    'sinon'
	'viewModels/search'
]

define deps, (chai, sinon, SearchViewModel) ->
	mocker = null
	chai.should()
	window.beforeEach -> mocker = sinon.sandbox.create()
	window.afterEach -> mocker.restore()

	describe 'When search page is used', ->
		describe 'When search view is instantiated', ->
			it 'Should fails when init params are in wrong format', ->
				# Given
				# When
				# Then
				chai.expect( -> new SearchViewModel() )
					.to.throw('page param must be a number')
				chai.expect( -> new SearchViewModel(1) )
					.to.throw('expression param must be a string')
