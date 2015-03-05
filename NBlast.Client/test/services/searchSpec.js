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
				var actual, appendToBackendUrlStub, callUrl, expected, ajaxStub, uuid;
				uuid = 'uuid1';
				callUrl = 'http://nblast.kz/' + uuid;
				appendToBackendUrlStub = fakes.mocker().stub(settings, 'appendToBackendUrl');
				ajaxStub = fakes.mocker().stub(fakes.jquery(), 'ajax');
				expected = { promise: true};
				appendToBackendUrlStub.withArgs('searcher/' + uuid + '/get').returns(callUrl);
				ajaxStub.withArgs(sinon.match({url: callUrl})).returns(expected);

				// When
				actual = search.getById(uuid);

				// Then
				actual.should.be.equal(expected);
			});
		});
		return describe('When we try to search', function () {
			it('Should take expression param, prepare it and make a request', function () {
				// Given
				var actual, appendToBackendUrlStub, callUrl, expected, expression, ajaxStub;
				expression = '*:*';
				callUrl = 'http://nblast.kz/searcher/search';
				appendToBackendUrlStub = fakes.mocker().stub(settings, 'appendToBackendUrl');
				ajaxStub = fakes.mocker().stub(fakes.jquery(), 'ajax');
				expected = { promise: true};
				appendToBackendUrlStub.withArgs('searcher/search').returns(callUrl);
				ajaxStub.withArgs(sinon.match({url:callUrl, data:{
					q: expression, p: 1, sf: '', sr: '', from: '', till: ''
				}})).returns(expected);

				// When
				actual = search.search(expression);

				// Then
				actual.should.be.equal(expected);
			});
			return it('Should take passed params, prepare them and make a request', function () {
				// Given
				var actual, appendToBackendUrlStub, callUrl, expected, ajaxStub, params;
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
				ajaxStub = fakes.mocker().stub(fakes.jquery(), 'ajax');
				expected = { promise: true};
				appendToBackendUrlStub.withArgs('searcher/search').returns(callUrl);
				ajaxStub.withArgs(sinon.match({url:callUrl, data: {
					q: params.expression,
					p: params.page,
					sf: params.sort.field,
					sr: params.sort.reverse,
					from: params.filter.from,
					till: params.filter.till
				}})).returns(expected);

				// When
				actual = search.search(params);

				// Then
				actual.should.be.equal(expected);
			});
		});
	});

})();