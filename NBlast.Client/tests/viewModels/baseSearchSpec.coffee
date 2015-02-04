deps = ['chai',
        'sinon',
        'underscore',
		'viewModels/baseSearch']

define deps, (chai, sinon, _, BaseSearchViewModel) ->
	mocker = null
	chai.should()
	window.beforeEach -> mocker = sinon.sandbox.create()
	window.afterEach -> mocker.restore()

	describe 'When common search features are in use', ->
		describe 'When data transformation methods are in use', ->
			it 'Should map sort field for an existent name', ->
				# Given
				chai.should()
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
				chai.should()
				sut = new BaseSearchViewModel()

				# When
				actual = sut.mapSortFieldLabel('Some case')

				# Then
				actual.should.be.equal('Relevance')

