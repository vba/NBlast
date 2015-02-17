(function () {
	'use strict';

	var chai, fakes, search, settings, sinon, _;

	sinon    = require('sinon');
	chai     = require('chai');
	fakes    = require('../fakes');
	settings = require('../../app/js/services/settings');
	search   = require('../../app/js/services/search');
	_        = require('underscore');

	chai.should();

	describe('When we use search service', function () {
		describe('When we try to get a log item by identifier', function () {
			it('Should retrieve this item when it exists', function () {
				// Given
				var actual, appendToBackendUrlStub, callUrl, expected, getJsonStub, uuid;
				uuid = 'uuid1';
				callUrl = 'http://nblast.kz/' + uuid;
				appendToBackendUrlStub = fakes.mocker().stub(settings, 'appendToBackendUrl');
				getJsonStub = fakes.mocker().stub(fakes.jquery(), 'getJSON');
				expected = { promise: true};
				appendToBackendUrlStub.withArgs('searcher/' + uuid + '/get').returns(callUrl);
				getJsonStub.withArgs(callUrl).returns(expected);

				// When
				actual = search.getById(uuid);

				// Then
				actual.should.be.equal(expected);
			});
		});
		return describe('When we try to search', function () {
			it('Should take expression param, prepare it and make a request', function () {
				// Given
				var actual, appendToBackendUrlStub, callUrl, expected, expression, getJsonStub;
				expression = '*:*';
				callUrl = 'http://nblast.kz/searcher/search';
				appendToBackendUrlStub = fakes.mocker().stub(settings, 'appendToBackendUrl');
				getJsonStub = fakes.mocker().stub(fakes.jquery(), 'getJSON');
				expected = { promise: true};
				appendToBackendUrlStub.withArgs('searcher/search').returns(callUrl);
				getJsonStub.withArgs(callUrl, sinon.match({
					q: expression, p: 1, sf: '', sr: '', from: '', till: ''
				})).returns(expected);

				// When
				actual = search.search(expression);

				// Then
				actual.should.be.equal(expected);
			});
			return it('Should take passed params, prepare them and make a request', function () {
				// Given
				var actual, appendToBackendUrlStub, callUrl, expected, getJsonStub, params;
				params = {
					expression: 'level: up',
					page: 10,
					sort: {
						field: 'createdAt',
						reverse: 'false'
					},
					filter: {
						from: '2010.01.01',
						till: '2015.01.01'
					}
				};
				callUrl = 'http://nblast.kz/searcher/search';
				appendToBackendUrlStub = fakes.mocker().stub(settings, 'appendToBackendUrl');
				getJsonStub = fakes.mocker().stub(fakes.jquery(), 'getJSON');
				expected = { promise: true};
				appendToBackendUrlStub.withArgs('searcher/search').returns(callUrl);
				getJsonStub.withArgs(callUrl, sinon.match({
					q: params.expression,
					p: params.page,
					sf: params.sort.field,
					sr: params.sort.reverse,
					from: params.filter.from,
					till: params.filter.till
				})).returns(expected);

				// When
				actual = search.search(params);

				// Then
				actual.should.be.equal(expected);
			});
		});
	});

})();