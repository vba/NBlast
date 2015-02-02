define(['chai', 'sinon', 'underscore', 'jquery', 'services/settings', 'services/search'], (chai, sinon, _, $, settings, searchService) ->
	chai.should()
	mocker = null

	window.beforeEach(() -> mocker = sinon.sandbox.create())
	window.afterEach(() -> mocker.restore())

	describe('When user gets a log item by identifier', () ->
		it('Should retrieve this item when it exists', () ->
			# Given
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
)