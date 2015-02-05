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
			check_5_pages = (page, expected) -> check_pages(page, expected, 5)
			check_12_pages = (page, expected) -> check_pages(page, expected, 12)
			check_pages = (page, expected, total = 12) ->
				# Given
				sut = new SearchViewModel(page, '*')
				# When
				sut.totalPages(total)
				sut.searchResult({total:120})
				actual = sut.getPages()
				# Then
				actual.should.be.eql(expected)

			it 'Should paginate nothing for an empty total', ->
				# Given
				sut = new SearchViewModel(1, '*')
				# When
				actual = sut.getPages()
				# Then
				actual.should.have.length(0)

			it 'Should paginate 50 results and select relative range starting with 1st page', ->
				# Given
				sut = new SearchViewModel(1, '*')
				# When
				sut.totalPages(5)
				sut.searchResult({total:50})
				actual = sut.getPages()
				# Then
				actual.should.be.eql([1,2,3,4,5])

			it 'Should paginate 50 results and select relative range starting with 2nd page', ->
				# Given
				sut = new SearchViewModel(2, '*')
				# When
				sut.totalPages(5)
				sut.searchResult({total:50})
				actual = sut.getPages()
				# Then
				actual.should.be.eql([1,2,3,4,5])

			it 'Should paginate 120 results and select relative range starting with 12th page', ->
				check_12_pages(12, [3,4,5,6,7,8,9,10,11,12])

			it 'Should paginate 120 results and select relative range starting with 11th page', ->
				check_12_pages(11, [3,4,5,6,7,8,9,10,11,12])

			it 'Should paginate 120 results and select relative range starting with 10th page', ->
				check_12_pages(10, [3,4,5,6,7,8,9,10,11,12])

			it 'Should paginate 120 results and select relative range starting with 9th page', ->
				check_12_pages(9, [3,4,5,6,7,8,9,10,11,12])

			it 'Should paginate 120 results and select relative range starting with 8th page', ->
				check_12_pages(8, [3,4,5,6,7,8,9,10,11,12])

			it 'Should paginate 120 results and select relative range starting with 7th page', ->
				check_12_pages(7, [2,3,4,5,6,7,8,9,10,11])

			it 'Should paginate 120 results and select relative range starting with 6th page', ->
				check_12_pages(6, [1,2,3,4,5,6,7,8,9,10])

			it 'Should paginate 120 results and select relative range starting with 5th page', ->
				check_12_pages(5, [1,2,3,4,5,6,7,8,9,10])

			it 'Should paginate 120 results and select relative range starting with 4th page', ->
				check_12_pages(4, [1,2,3,4,5,6,7,8,9,10])

			it 'Should paginate 120 results and select relative range starting with 3rd page', ->
				check_12_pages(3, [1,2,3,4,5,6,7,8,9,10])

			it 'Should paginate 120 results and select relative range starting with 2nd page', ->
				check_12_pages(2, [1,2,3,4,5,6,7,8,9,10])
			it 'Should paginate 120 results and select relative range starting with 1st page', ->
				check_12_pages(1, [1,2,3,4,5,6,7,8,9,10])

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
