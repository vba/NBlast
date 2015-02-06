deps = [
	'chai'
    'sinon'
	'services/markup'
	'services/search'
	'viewModels/search'
]

define deps, (chai, sinon, markupService, searchService, SearchViewModel) ->
	mocker = null
	chai.should()
	window.beforeEach -> mocker = sinon.sandbox.create()
	window.afterEach -> mocker.restore()

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

		describe 'When search view is bind', ->
			it 'Should apply binding, init externals and accomplish a search request', ->
				# Given
				actualSearchParams = {}
				actualSearchCallback = ->
				sut = new SearchViewModel(10, '*')
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
				initExternalsStub.calledOnce.should.be.true()
				searchStub.calledOnce.should.be.true()
				actualSearchParams.expression.should.be.equal('*')
				actualSearchParams.page.should.be.equal(10)

		describe 'When view model needs to interact with server', ->
			it 'Should run route during search request when location remains equal to requested path', ->
				# Given
				sut = new SearchViewModel(1, '*')
				path = '/#/search/' + encodeURIComponent('*')
				storeStub = mocker.stub(sut, 'storeAdvancedDetails', -> true)
				getLocationStub = mocker.stub(sut.sammy, 'getLocation')
				runRouteStub = mocker.stub(sut.sammy, 'runRoute')

				getLocationStub.returns(path)
				runRouteStub.withArgs('get', path).returns(true)

				# When
				actual = sut.enterSearch(null, {keyCode: 13})

				# Then
				actual.should.be.false()
				storeStub.calledOnce.should.be.true()
				sinon.assert.calledWithMatch(runRouteStub, 'get', path)

			it 'Should set location during search request when location does not remain equal to requested path', ->
				# Given
				sut = new SearchViewModel(1, '*')
				path = '/#/search/' + encodeURIComponent('*')
				storeStub = mocker.stub(sut, 'storeAdvancedDetails', -> true)
				setLocationStub = mocker.stub(sut.sammy, 'setLocation', -> true)

				# When
				actual = sut.enterSearch(null, {keyCode: 13})

				# Then
				actual.should.be.false()
				storeStub.calledOnce.should.be.true()
				sinon.assert.calledWithMatch(setLocationStub, path)