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
		describe 'When search view pagination is used', ->
			it 'Should paginate nothing for an empty total', ->
				# Given
				sut = new SearchViewModel(1, '*')
				# When
				actual = sut.getPages()
				# Then
				actual.should.have.length(0)

		describe 'When search view is instantiated', ->
			it 'Should fails when init params are invalid', ->
				# Given
				# When
				# Then
				chai.expect( -> new SearchViewModel() )
					.to.throw('page param must be a number')
				chai.expect( -> new SearchViewModel(1) )
					.to.throw('expression param must be a string')

			it 'Should initialize with correct data', ->
				# Given
				expectedExpression = 'level: up'
				expectedPage = 10
				# When
				sut = new SearchViewModel(expectedPage, expectedExpression)
				# Then
				sut.page().should.be.equal(expectedPage)
				sut.expression().should.be.equal(expectedExpression)
