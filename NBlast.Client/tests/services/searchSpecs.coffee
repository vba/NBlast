define(['chai', 'sinon', 'underscore', 'jquery', 'services/settings', 'services/search'], (chai, sinon, _, $, settings, searchService) ->
	mocker = null

	window.beforeEach(() -> mocker = sinon.sandbox.create())
	window.afterEach(() -> mocker.restore())

	describe('When we use search service', () ->
		describe('When we try to get a log item by identifier', () ->
			it('Should retrieve this item when it exists', () ->
				# Given
				chai.should()
				uuid = 'uuid1'
				callUrl = 'http://nblast.kz/'+uuid
				appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl')
				getJsonStub = mocker.stub($, 'getJSON')
				expected = {promise: true}

				appendToBackendUrlStub
					.withArgs('searcher/' + uuid + '/get')
					.returns(callUrl)

				getJsonStub
					.withArgs(callUrl)
					.returns(expected)

				# When
				actual = searchService.getById(uuid)

				# Then
				(actual).should.be.equal(expected)
			)
		)
		describe('When we try to search', ()->
			it('Should take expression param, prepare it and make a request', () ->
				# Given
				expression = '*:*'
				callUrl = 'http://nblast.kz/searcher/search'
				appendToBackendUrlStub = mocker.stub(settings, 'appendToBackendUrl')
				getJsonStub = mocker.stub($, 'getJSON')
				expected = {promise: true}

				appendToBackendUrlStub
					.withArgs('searcher/search')
					.returns(callUrl)

				getJsonStub
					.withArgs(callUrl, {
						q: expression
						p: 1
						sf: '',
						sr: '',
						from: '',
						till: ''
					})
					.returns(expected)

				# When
				actual = searchService.search(expression)

				# Then
				chai.expect(actual).to.equal(expected)
			)
		)
	)
)