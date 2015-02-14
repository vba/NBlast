sinon       = require 'sinon'
config      = require '../../app/js/config'
mocker      = sinon.sandbox.create()
chai        = require 'chai'
getSutType  = -> require '../../app/js/viewModels/baseSearch'
amplify     = {store: -> ''}
moment      = require 'moment'
$           = {trim: (x) -> [x].join('').trim()}
_           = require 'underscore'


chai.should()

describe 'When common search features are in use', ->
	beforeEach( ->
		mocker = sinon.sandbox.create()
		mocker.stub(config, 'amplify', -> amplify)
		mocker.stub(config, 'jquery', -> $)
		mocker.stub(config, 'moment', -> moment)
	)
	afterEach( -> mocker.restore())
	describe 'When UI needs to store advanced details before make a request', ->
		it 'Should initialize empty details', ->
			# Given
			storeStub = mocker.stub(amplify, 'store')
			Sut = getSutType()
			sut = new Sut()
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
			format = getSutType().displayDateTimeFormat
			storeStub = mocker.stub(amplify, 'store')
			fromDate = moment().subtract(5, 'years')
			tillDate = moment().subtract(1, 'years')
			Sut = getSutType()
			sut = new Sut()
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
			format = getSutType().displayDateTimeFormat
			fromDate = moment().subtract(5, 'years')
			tillDate = moment().subtract(1, 'years')
			sortField = 'sender'
			sortReverse = 'false'
			captured = []
			mocker.stub(amplify, 'store', (x, y) ->if _.isEmpty(y) is false then captured.push(y))
#			initDetailsStub = mocker.stub(sut, 'initAdvancedDetails')
			Sut = getSutType()
			sut = new Sut()

			sut.filter =
				from: -> fromDate.format(format)
				till: -> tillDate.format(format)
			sut.sortReverse = -> sortReverse
			sut.sortField = -> sortField
#			initDetailsStub.returns(true)

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
			mocker.stub(amplify, 'store', ->)
			Sut = getSutType()
			sut = new Sut()

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
			mocker.stub(amplify, 'store', ->)
			Sut = getSutType()
			sut = new Sut()

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
			mocker.stub(amplify, 'store', ->)
			Sut = getSutType()
			sut = new Sut()

			# When
			actual = sut.mapSortFieldLabel('Some case')

			# Then
			actual.should.be.equal('Relevance')

		it 'Should map not existent search type to the value by default', ->
			# Given
			mocker.stub(amplify, 'store', ->)
			Sut = getSutType()
			sut = new Sut()

			# When
			actual = sut.mapSearchTypeLabel('Some case')

			# Then
			actual.should.be.equal('Any expression')