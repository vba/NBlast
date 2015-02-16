sinon         = require 'sinon'
config        = require '../../app/js/config'
mocker        = sinon.sandbox.create()
chai          = require 'chai'
searchService = require '../../app/js/services/search'
markupService = require '../../app/js/services/markup'
views         = require('../../app/js/views')
getSutType    = -> require '../../app/js/viewModels/search'
amplify       = {store: -> ''}
$             = {trim: (x) -> [x].join('').trim()}
sammy =
	setLocation: ->
	getLocation: ->
	runRoute: ->

chai.should()

describe 'When search page is used', ->
	beforeEach ->
		mocker = sinon.sandbox.create()
		mocker.stub(config, 'sammy', -> -> sammy)
		mocker.stub(config, 'amplify', -> amplify)
		mocker.stub(config, 'jquery', -> $)
		mocker.stub(views, 'getSearch', -> '')

	afterEach( -> mocker.restore())

	describe 'When search view pagination is used', ->
		Sut = getSutType()
		check_12_pages = (page, expected) ->
			check_pages(page, expected, 12)

		check_pages = (page, expected, totalPages = 12) ->
			# Given
			sut = new Sut(page, '*')
			# When
			sut.totalPages(totalPages)
			sut.searchResult({total:120})
			actual = sut.getPages()
			# Then
			actual.should.be.eql(expected)

		it 'Should paginate nothing for an empty total', ->
			# Given
			sut = new Sut(1, '*')
			# When
			actual = sut.getPages()
			# Then
			actual.should.have.length(0)

		it 'Should paginate 50 results and select relative range starting with 1st page', ->
			# Given
			sut = new Sut(1, '*')
			# When
			sut.totalPages(5)
			sut.searchResult({total:50})
			actual = sut.getPages()
			# Then
			actual.should.be.eql([1,2,3,4,5])

		it 'Should paginate 50 results and select relative range starting with 2nd page', ->
			# Given
			sut = new Sut(2, '*')
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
		Sut = getSutType()
		it 'Should define found icon by level', ->
			# Given
			sut = new Sut(1, '*')

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
			sut = new Sut(10, '*')
			storeStub = mocker.stub(amplify, 'store', ->)
			sortFieldSpy = mocker.spy(sut, 'sortField')
			sortReverseSpy = mocker.spy(sut, 'sortReverse')
			fromSpy = mocker.spy(sut.filter, 'from')
			tillSpy = mocker.spy(sut.filter, 'till')

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
		Sut = getSutType()
		it 'Should fails when init params are invalid', ->
			# Given
			# When
			# Then
			chai.expect( -> new Sut() )
				.to.throw('page param must be a number')
			chai.expect( -> new Sut(1) )
				.to.throw('expression param must be a string')

		it 'Should initialize with correct data', ->
			# Given
			expectedExpression = 'level: up'
			expectedPage = 10
			# When
			sut = new Sut(expectedPage, expectedExpression)
			# Then
			sut.page().should.be.equal(expectedPage)
			sut.expression().should.be.equal(expectedExpression)

		it 'Should detected terms search type correctly', ->
			# Given
			expectedParams = {val: true}
			sut = new Sut(1, '*')
			termSearchModeStub = mocker.stub(sut, 'termSearchMode', -> true)
			getTermSearchParamsStub = mocker.stub(sut, 'getTermSearchParams')
			searchByTermStub = mocker.stub(searchService, 'searchByTerm')

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
			sut = new Sut(1, '*')
			termSearchModeStub = mocker.stub(sut, 'termSearchMode', -> false)
			getSearchParamsStub = mocker.stub(sut, 'getSearchParams')
			searchStub = mocker.stub(searchService, 'search')

			getSearchParamsStub.returns(expectedParams)
			searchStub.withArgs(expectedParams).returns('yeah!')

			# When
			expected = sut.requestSearch()

			# Then
			expected.should.be.eql('yeah!')
			termSearchModeStub.calledOnce.should.equal(true)

	describe 'When search view is bind', ->
		Sut = getSutType()
		it 'Should apply binding, init externals and accomplish a search request', ->
			# Given
			actualSearchParams = {}
			actualSearchCallback = ->
			sut = new Sut(10, '*')
			termSearchModeStub = mocker.stub(sut, 'termSearchMode', -> false)
			applyBindingsStub = mocker.stub(markupService, 'applyBindings', ->)
			initExternalsStub = mocker.stub(sut, 'initExternals', ->)
			searchStub = mocker.stub(searchService, 'search', (p) ->
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
		Sut = getSutType()
		it 'Should run route during search request when location remains equal to requested path', ->
			# Given
			sut = new Sut(1, '*')
			path = '/#/search/' + encodeURIComponent('*')
			storeStub = mocker.stub(sut, 'storeAdvancedDetails', -> true)
			getLocationStub = mocker.stub(sut.sammy(), 'getLocation')
			runRouteStub = mocker.stub(sut.sammy(), 'runRoute')

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
			sut = new Sut(1, '*')
			path = '/#/search/' + encodeURIComponent('*')
			storeStub = mocker.stub(sut, 'storeAdvancedDetails', -> true)
			setLocationStub = mocker.stub(sut.sammy(), 'setLocation', -> true)

			# When
			actual = sut.enterSearch(null, {keyCode: 13})

			# Then
			actual.should.equal(false)
			storeStub.calledOnce.should.equal(true)
			sinon.assert.calledWithMatch(setLocationStub, path)