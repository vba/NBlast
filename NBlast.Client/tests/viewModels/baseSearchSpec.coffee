deps = ['chai',
        'sinon',
        'amplify',
        'moment',
		'viewModels/baseSearch']

define deps, (chai, sinon, amplify, moment, BaseSearchViewModel) ->
	mocker = null
	chai.should()
	window.beforeEach -> mocker = sinon.sandbox.create()
	window.afterEach -> mocker.restore()

	describe 'When common search features are in use', ->
		describe 'When UI needs to store advanced details before make a request', ->
			it 'Should initialize empty details', ->
				# Given
				storeStub = mocker.stub(amplify, 'store')
				sut = new BaseSearchViewModel()
				storeStub.withArgs('filter').returns null
				storeStub.withArgs('sort').returns null

				# When
				sut.initAdvancedDetails()

				#Then
				sut.sortField().should.be.equal('')
				sut.sortReverse().should.be.equal(false)
				sut.filter.from().should.be.equal('')
				sut.filter.till().should.be.equal('')

			it 'Should initialize stored advanced details', ->
				# Given
				format = BaseSearchViewModel.displayDateTimeFormat
				storeStub = mocker.stub(amplify, 'store')
				fromDate = moment().subtract(5, 'years')
				tillDate = moment().subtract(1, 'years')
				sut = new BaseSearchViewModel()
				stored =
					filter:
						from: fromDate.toISOString()
						till: tillDate.toISOString()
					sort:
						field: 'logger'
						reverse: true

				storeStub.withArgs('filter').returns stored.filter
				storeStub.withArgs('sort').returns stored.sort

				# When
				sut.initAdvancedDetails()

				#Then
				sut.sortField().should.be.equal(stored.sort.field)
				sut.sortReverse().should.be.equal(stored.sort.reverse)
				sut.filter.from().should.be
					.equal(moment(new Date(stored.filter.from)).format(format))
				sut.filter.till().should.be
					.equal(moment(new Date(stored.filter.till)).format(format))

			it 'Should store filter and sort details', ->
				# Given
				format = BaseSearchViewModel.displayDateTimeFormat
				fromDate = moment().subtract(5, 'years')
				tillDate = moment().subtract(1, 'years')
				sortField = 'sender'
				sortReverse = 'false'
				captured = []
				sut = new BaseSearchViewModel()
				initDetailsStub = mocker.stub(sut, 'initAdvancedDetails')
				sut.filter =
					from: -> fromDate.format(format)
					till: -> tillDate.format(format)
				sut.sortReverse = -> sortReverse
				sut.sortField = -> sortField

				initDetailsStub.returns(true)
				mocker.stub(amplify, 'store', (x, y) -> captured.push(y))

				# When
				sut.storeAdvancedDetails()

				# Then
				captured.length.should.be.equal(3)
				captured[0].from.split('T')[0]
					.should.be.equal(fromDate.toISOString().split('T')[0])
				captured[0].till.split('T')[0]
					.should.be.equal(tillDate.toISOString().split('T')[0])
				captured[1].reverse.should.be.equal('false')
				captured[1].field.should.be.equal('sender')

		describe 'When data transformation methods are in use', ->
			it 'Should map search type for an existent name', ->
				# Given
				sut = new BaseSearchViewModel()

				# When
				actual = [
					sut.mapSearchTypeLabel('Id')
					sut.mapSearchTypeLabel('SenDer')
					sut.mapSearchTypeLabel('logger')
					sut.mapSearchTypeLabel('level')
					sut.mapSearchTypeLabel('')
				]

				# Then
				actual[0].should.be.equal('Identifier')
				actual[1].should.be.equal('Sender')
				actual[2].should.be.equal('Logger')
				actual[3].should.be.equal('Level')
				actual[4].should.be.equal('Any expression')

			it 'Should map sort field for an existent name', ->
				# Given
				sut = new BaseSearchViewModel()

				# When
				actual = [
					sut.mapSortFieldLabel('createdAt')
					sut.mapSortFieldLabel('Sender')
					sut.mapSortFieldLabel('logger')
					sut.mapSortFieldLabel('Level')
					sut.mapSortFieldLabel('')
				]

				# Then
				actual[0].should.be.equal('Date')
				actual[1].should.be.equal('Sender')
				actual[2].should.be.equal('Logger')
				actual[3].should.be.equal('Level')
				actual[4].should.be.equal('Relevance')

			it 'Should map not existent sort field to the value by default', ->
				# Given
				sut = new BaseSearchViewModel()

				# When
				actual = sut.mapSortFieldLabel('Some case')

				# Then
				actual.should.be.equal('Relevance')

			it 'Should map not existent search type to the value by default', ->
				# Given
				sut = new BaseSearchViewModel()

				# When
				actual = sut.mapSearchTypeLabel('Some case')

				# Then
				actual.should.be.equal('Any expression')