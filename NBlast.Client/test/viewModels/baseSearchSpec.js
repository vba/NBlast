(function () {
	var BaseSearchViewModel = require('../../app/js/viewModels/baseSearch'),
		fakes               = require('../fakes'),
		sinon               = require('sinon'),
		chai                = require('chai'),
		moment              = require('moment'),
		_                   = require('underscore');

	chai.should();

	describe('When common search features are in use', function () {
		describe('When UI needs to store advanced details before make a request', function () {
			it('Should initialize empty details', function () {
				// Given
				var storeStub, sut;

				storeStub = fakes.mocker().stub(fakes.amplify(), 'store');
				sut = new BaseSearchViewModel();
				storeStub.withArgs('filter').returns(null);
				storeStub.withArgs('sort').returns(null);

				// When
				sut.initAdvancedDetails();

				// Then
				sut.sortField().should.be.equal('');
				sut.sortReverse().should.be.equal(false);
				sut.filter.from().should.be.equal('');
				sut.filter.till().should.be.equal('');
			});
			it('Should initialize stored advanced details', function () {
				// Given
				var format, fromDate, storeStub, stored, sut, tillDate;

				format = BaseSearchViewModel.displayDateTimeFormat;
				storeStub = fakes.mocker().stub(fakes.amplify(), 'store');
				fromDate = moment().subtract(5, 'years');
				tillDate = moment().subtract(1, 'years');
				sut = new BaseSearchViewModel();
				stored = {
					filter: {
						from: fromDate.toISOString(),
						till: tillDate.toISOString()
					},
					sort  : {
						field  : 'logger',
						reverse: true
					}
				};
				storeStub.withArgs('filter').returns(stored.filter);
				storeStub.withArgs('sort').returns(stored.sort);

				// When
				sut.initAdvancedDetails();

				// Then
				sut.sortField().should.be.equal(stored.sort.field);
				sut.sortReverse().should.be.equal(stored.sort.reverse);
				sut.filter.from().should.be.equal(moment(new Date(stored.filter.from)).format(format));
				sut.filter.till().should.be.equal(moment(new Date(stored.filter.till)).format(format));
			});
			it('Should store filter and sort details', function () {
				// Given
				var captured, format, fromDate, sortField, sortReverse, sut, tillDate;

				format = BaseSearchViewModel.displayDateTimeFormat;
				fromDate = moment().subtract(5, 'years');
				tillDate = moment().subtract(1, 'years');
				sortField = 'sender';
				sortReverse = 'false';
				captured = [];
				fakes.mocker().stub(fakes.amplify(), 'store', function (x, y) {
					if (_.isEmpty(y) === false) {
						return captured.push(y);
					}
				});
				sut = new BaseSearchViewModel();
				sut.filter = {
					from: function () {
						return fromDate.format(format);
					},
					till: function () {
						return tillDate.format(format);
					}
				};
				sut.sortReverse = function () {
					return sortReverse;
				};
				sut.sortField = function () {
					return sortField;
				};

				// When
				sut.storeAdvancedDetails();

				// Then
				captured.length.should.be.equal(3);
				captured[0].from.split('T')[0].should.be.equal(fromDate.toISOString().split('T')[0]);
				captured[0].till.split('T')[0].should.be.equal(tillDate.toISOString().split('T')[0]);
				captured[1].reverse.should.be.equal('false');
				captured[1].field.should.be.equal('sender');
			});
		});
		describe('When data transformation methods are in use', function () {
			it('Should map search type for an existent name', function () {
				// Given
				var actual, sut;

				fakes.mocker().stub(fakes.amplify(), 'store', function () { });
				sut = new BaseSearchViewModel();

				// When
				actual = [
					sut.mapSearchTypeLabel('Id'),
					sut.mapSearchTypeLabel('SenDer'),
					sut.mapSearchTypeLabel('logger'),
					sut.mapSearchTypeLabel('level'),
					sut.mapSearchTypeLabel('')
				];

				// Then
				actual[0].should.be.equal('Identifier');
				actual[1].should.be.equal('Sender');
				actual[2].should.be.equal('Logger');
				actual[3].should.be.equal('Level');
				actual[4].should.be.equal('Any expression');
			});
			it('Should map sort field for an existent name', function () {
				// Given
				var actual, sut;
				fakes.mocker().stub(fakes.amplify(), 'store', function () { });
				sut = new BaseSearchViewModel();

				// When
				actual = [
					sut.mapSortFieldLabel('createdAt'),
					sut.mapSortFieldLabel('Sender'),
					sut.mapSortFieldLabel('logger'),
					sut.mapSortFieldLabel('Level'),
					sut.mapSortFieldLabel('')
				];

				// Then
				actual[0].should.be.equal('Date');
				actual[1].should.be.equal('Sender');
				actual[2].should.be.equal('Logger');
				actual[3].should.be.equal('Level');
				actual[4].should.be.equal('Relevance');
			});
			it('Should map not existent sort field to the value by default', function () {
				// Given
				var actual, sut;
				fakes.mocker().stub(fakes.amplify(), 'store', function () { });
				sut = new BaseSearchViewModel();

				// When
				actual = sut.mapSortFieldLabel('Some case');

				// Then
				actual.should.be.equal('Relevance');
			});
			it('Should map not existent search type to the value by default', function () {
				// Given
				var actual, sut;
				fakes.mocker().stub(fakes.amplify(), 'store', function () { });
				sut = new BaseSearchViewModel();

				// When
				actual = sut.mapSearchTypeLabel('Some case');

				// Then
				actual.should.be.equal('Any expression');
			});
		});
	});

})();