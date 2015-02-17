sinon           = require 'sinon'
chai            = require 'chai'
searchService   = require '../../app/js/services/search'
markupService   = require '../../app/js/services/markup'
SearchViewModel = require '../../app/js/viewModels/search'
fakes           = require '../fakes'

chai.should()

describe 'When search page is used', ->
	describe 'When search view pagination is used', ->
		check_12_pages = (page, expected) ->
			check_pages(page, expected, 12)

		check_pages = (page, expected, totalPages = 12) ->
			# Given
			sut = new SearchViewModel(page, '*')
			# When
			sut.totalPages(totalPages)
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

	describe 'When search view displays UI elements', ->
		it 'Should define found icon by level', ->
			# Given
			sut = new SearchViewModel(1, '*')

			# When
			actualIcons = [
				sut.defineFoundIcon('Debug')
				sut.defineFoundIcon('info')
				sut.defineFoundIcon('Warn')
				sut.defineFoundIcon('ERRor')
				sut.defineFoundIcon('Fatal')
				sut.defineFoundIcon('bullshit')
				sut.defineFoundIcon('')
			]

			# Then
			actualIcons[0].should.be.equal('cog')
			actualIcons[1].should.be.equal('info')
			actualIcons[2].should.be.equal('warning')
			actualIcons[3].should.be.equal('bolt')
			actualIcons[4].should.be.equal('fire')
			actualIcons[5].should.be.equal('asterisk')
			actualIcons[6].should.be.equal('asterisk')

		it 'Should clear advanced details every time when user clicks the button', ->
			# Given
			sut = new SearchViewModel(10, '*')
			storeStub = fakes.mocker().stub(fakes.amplify(), 'store', ->)
			sortFieldSpy = fakes.mocker().spy(sut, 'sortField')
			sortReverseSpy = fakes.mocker().spy(sut, 'sortReverse')
			fromSpy = fakes.mocker().spy(sut.filter, 'from')
			tillSpy = fakes.mocker().spy(sut.filter, 'till')

			# When
			sut.clearAdvancedDetails()

			# Then
			storeStub.callCount.should.equal(3)
			storeStub.calledWith('filter', null).should.equal(true)
			storeStub.calledWith('sort', null).should.equal(true)
			storeStub.calledWith('search', null).should.equal(true)
			sortFieldSpy.calledWith('').should.equal(true)
			sortReverseSpy.calledOnce.should.equal(true)
			fromSpy.calledOnce.should.equal(true)
			tillSpy.calledOnce.should.equal(true)

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

		it 'Should detected terms search type correctly', ->
			# Given
			expectedParams = {val: true}
			sut = new SearchViewModel(1, '*')
			termSearchModeStub = fakes.mocker().stub(sut, 'termSearchMode', -> true)
			getTermSearchParamsStub = fakes.mocker().stub(sut, 'getTermSearchParams')
			searchByTermStub = fakes.mocker().stub(searchService, 'searchByTerm')

			getTermSearchParamsStub.returns(expectedParams)
			searchByTermStub.withArgs(expectedParams).returns('yeah!')

			# When
			expected = sut.requestSearch()

			# Then
			expected.should.be.eql('yeah!')
			termSearchModeStub.calledOnce.should.equal(true)

		it 'Should detected simple search type correctly', ->
			# Given
			expectedParams = {val: true}
			sut = new SearchViewModel(1, '*')
			termSearchModeStub = fakes.mocker().stub(sut, 'termSearchMode', -> false)
			getSearchParamsStub = fakes.mocker().stub(sut, 'getSearchParams')
			searchStub = fakes.mocker().stub(searchService, 'search')

			getSearchParamsStub.returns(expectedParams)
			searchStub.withArgs(expectedParams).returns('yeah!')

			# When
			expected = sut.requestSearch()

			# Then
			expected.should.be.eql('yeah!')
			termSearchModeStub.calledOnce.should.equal(true)

	describe 'When search view is bind', ->
		it 'Should apply binding, init externals and accomplish a search request', ->
			# Given
			actualSearchParams = {}
			actualSearchCallback = ->
			sut = new SearchViewModel(10, '*')
			termSearchModeStub = fakes.mocker().stub(sut, 'termSearchMode', -> false)
			applyBindingsStub = fakes.mocker().stub(markupService, 'applyBindings', ->)
			initExternalsStub = fakes.mocker().stub(sut, 'initExternals', ->)
			searchStub = fakes.mocker().stub(searchService, 'search', (p) ->
				actualSearchParams = p
				done: (cb) ->
					actualSearchCallback = cb
			)

			# When
			sut.bind()

			# Then

			applyBindingsStub.calledWith(sut, sinon.match.string)
			termSearchModeStub.calledOnce.should.equal(true)
			initExternalsStub.calledOnce.should.equal(true)
			searchStub.calledOnce.should.equal(true)
			actualSearchParams.expression.should.be.equal('*')
			actualSearchParams.page.should.be.equal(10)

	describe 'When view model needs to interact with server', ->
		it 'Should run route during search request when location remains equal to requested path', ->
			# Given
			sut = new SearchViewModel(1, '*')
			path = '/#/search/' + encodeURIComponent('*')
			storeStub = fakes.mocker().stub(sut, 'storeAdvancedDetails', -> true)
			getLocationStub = fakes.mocker().stub(sut.sammy(), 'getLocation')
			runRouteStub = fakes.mocker().stub(sut.sammy(), 'runRoute')

			getLocationStub.returns(path)
			runRouteStub.withArgs('get', path).returns(true)

			# When
			actual = sut.enterSearch(null, {keyCode: 13})

			# Then
			actual.should.equal(false)
			storeStub.calledOnce.should.equal(true)
			sinon.assert.calledWithMatch(runRouteStub, 'get', path)

		it 'Should set location during search request when location does not remain equal to requested path', ->
			# Given
			sut = new SearchViewModel(1, '*')
			path = '/#/search/' + encodeURIComponent('*')
			storeStub = fakes.mocker().stub(sut, 'storeAdvancedDetails', -> true)
			setLocationStub = fakes.mocker().stub(sut.sammy(), 'setLocation', -> true)

			# When
			actual = sut.enterSearch(null, {keyCode: 13})

			# Then
			actual.should.equal(false)
			storeStub.calledOnce.should.equal(true)
			sinon.assert.calledWithMatch(setLocationStub, path)