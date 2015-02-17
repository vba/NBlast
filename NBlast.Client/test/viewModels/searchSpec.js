(function () {
	var sinon           = require('sinon'),
	    chai            = require('chai'),
	    searchService   = require('../../app/js/services/search'),
	    markupService   = require('../../app/js/services/markup'),
	    SearchViewModel = require('../../app/js/viewModels/search'),
	    fakes           = require('../fakes');

	chai.should();

	describe('When search page is used', function () {
		describe('When search view pagination is used', function () {
			var check_12_pages, check_pages;
			check_12_pages = function (page, expected) {
				return check_pages(page, expected, 12);
			};
			check_pages = function (page, expected, totalPages) {
				// Given
				var actual, sut;
				if (totalPages === null) {
					totalPages = 12;
				}
				sut = new SearchViewModel(page, '*');
				sut.totalPages(totalPages);
				sut.searchResult({
					total: 120
				});

				// When
				actual = sut.getPages();

				// Then
				actual.should.be.eql(expected);
			};
			it('Should paginate nothing for an empty total', function () {
				// Given
				var actual, sut;
				sut = new SearchViewModel(1, '*');

				// When
				actual = sut.getPages();

				// Then
				actual.should.have.length(0);
			});
			it('Should paginate 50 results and select relative range starting with 1st page', function () {
				// Given
				var actual, sut;
				sut = new SearchViewModel(1, '*');
				sut.totalPages(5);
				sut.searchResult({ total: 50});

				// When
				actual = sut.getPages();

				// Then
				actual.should.be.eql([1, 2, 3, 4, 5]);
			});
			it('Should paginate 50 results and select relative range starting with 2nd page', function () {
				// Given
				var actual, sut;
				sut = new SearchViewModel(2, '*');
				sut.totalPages(5);
				sut.searchResult({ total: 50});

				// When
				actual = sut.getPages();

				// Then
				actual.should.be.eql([1, 2, 3, 4, 5]);
			});
			it('Should paginate 120 results and select relative range starting with 12th page', function () {
				return check_12_pages(12, [3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
			});
			it('Should paginate 120 results and select relative range starting with 11th page', function () {
				return check_12_pages(11, [3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
			});
			it('Should paginate 120 results and select relative range starting with 10th page', function () {
				return check_12_pages(10, [3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
			});
			it('Should paginate 120 results and select relative range starting with 9th page', function () {
				return check_12_pages(9, [3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
			});
			it('Should paginate 120 results and select relative range starting with 8th page', function () {
				return check_12_pages(8, [3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);
			});
			it('Should paginate 120 results and select relative range starting with 7th page', function () {
				return check_12_pages(7, [2, 3, 4, 5, 6, 7, 8, 9, 10, 11]);
			});
			it('Should paginate 120 results and select relative range starting with 6th page', function () {
				return check_12_pages(6, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
			});
			it('Should paginate 120 results and select relative range starting with 5th page', function () {
				return check_12_pages(5, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
			});
			it('Should paginate 120 results and select relative range starting with 4th page', function () {
				return check_12_pages(4, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
			});
			it('Should paginate 120 results and select relative range starting with 3rd page', function () {
				return check_12_pages(3, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
			});
			it('Should paginate 120 results and select relative range starting with 2nd page', function () {
				return check_12_pages(2, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
			});
			return it('Should paginate 120 results and select relative range starting with 1st page', function () {
				return check_12_pages(1, [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
			});
		});
		describe('When search view displays UI elements', function () {
			it('Should define found icon by level', function () {
				// Given
				var actualIcons, sut;
				sut = new SearchViewModel(1, '*');

				// When
				actualIcons = [
					sut.defineFoundIcon('Debug'),
					sut.defineFoundIcon('info'),
					sut.defineFoundIcon('Warn'),
					sut.defineFoundIcon('ERRor'),
					sut.defineFoundIcon('Fatal'),
					sut.defineFoundIcon('bullshit'),
					sut.defineFoundIcon('')
				];

				// Then
				actualIcons[0].should.be.equal('cog');
				actualIcons[1].should.be.equal('info');
				actualIcons[2].should.be.equal('warning');
				actualIcons[3].should.be.equal('bolt');
				actualIcons[4].should.be.equal('fire');
				actualIcons[5].should.be.equal('asterisk');
				actualIcons[6].should.be.equal('asterisk');
			});
			return it('Should clear advanced details every time when user clicks the button', function () {
				// Given
				var fromSpy, sortFieldSpy, sortReverseSpy, storeStub, sut, tillSpy;
				sut = new SearchViewModel(10, '*');
				storeStub = fakes.mocker().stub(fakes.amplify(), 'store', function () {});
				sortFieldSpy = fakes.mocker().spy(sut, 'sortField');
				sortReverseSpy = fakes.mocker().spy(sut, 'sortReverse');
				fromSpy = fakes.mocker().spy(sut.filter, 'from');
				tillSpy = fakes.mocker().spy(sut.filter, 'till');

				// When
				sut.clearAdvancedDetails();

				// Then
				storeStub.callCount.should.equal(3);
				storeStub.calledWith('filter', null).should.equal(true);
				storeStub.calledWith('sort', null).should.equal(true);
				storeStub.calledWith('search', null).should.equal(true);
				sortFieldSpy.calledWith('').should.equal(true);
				sortReverseSpy.calledOnce.should.equal(true);
				fromSpy.calledOnce.should.equal(true);
				tillSpy.calledOnce.should.equal(true);
			});
		});
		describe('When search view is instantiated', function () {
			it('Should fails when init params are invalid', function () {
				chai.expect(function () {
					new SearchViewModel();
				}).to.throw('page param must be a number');

				chai.expect(function () {
					new SearchViewModel(1);
				}).to.throw('expression param must be a string');
			});
			it('Should initialize with correct data', function () {
				// Given
				var expectedExpression, expectedPage, sut;
				expectedExpression = 'level: up';
				expectedPage = 10;

				// When
				sut = new SearchViewModel(expectedPage, expectedExpression);

				// Then
				sut.page().should.be.equal(expectedPage);
				sut.expression().should.be.equal(expectedExpression);
			});
			it('Should detected terms search type correctly', function () {
				// Given
				var expected, expectedParams, getTermSearchParamsStub, searchByTermStub, sut, termSearchModeStub;
				expectedParams = { val: true};
				sut = new SearchViewModel(1, '*');
				termSearchModeStub = fakes.mocker().stub(sut, 'termSearchMode', function () {
					return true;
				});
				getTermSearchParamsStub = fakes.mocker().stub(sut, 'getTermSearchParams');
				searchByTermStub = fakes.mocker().stub(searchService, 'searchByTerm');
				getTermSearchParamsStub.returns(expectedParams);
				searchByTermStub.withArgs(expectedParams).returns('yeah!');

				// When
				expected = sut.requestSearch();
				expected.should.be.eql('yeah!');

				// Then
				termSearchModeStub.calledOnce.should.equal(true);
			});
			return it('Should detected simple search type correctly', function () {
				// Given
				var expected, expectedParams, getSearchParamsStub, searchStub, sut, termSearchModeStub;
				expectedParams = { val: true};
				sut = new SearchViewModel(1, '*');
				termSearchModeStub = fakes.mocker().stub(sut, 'termSearchMode', function () {
					return false;
				});

				getSearchParamsStub = fakes.mocker().stub(sut, 'getSearchParams');
				searchStub = fakes.mocker().stub(searchService, 'search');

				getSearchParamsStub
					.returns(expectedParams);

				searchStub
					.withArgs(expectedParams)
					.returns('yeah!');

				// When
				expected = sut.requestSearch();

				// Then
				expected.should.be.eql('yeah!');
				termSearchModeStub.calledOnce.should.equal(true);
			});
		});
		describe('When search view is bind', function () {
			return it('Should apply binding, init externals and accomplish a search request', function () {
				// Given
				var actualSearchCallback,
				    actualSearchParams,
				    applyBindingsStub,
				    initExternalsStub,
				    searchStub,
				    sut,
				    termSearchModeStub;

				actualSearchParams = {};
				actualSearchCallback = function () {};
				sut = new SearchViewModel(10, '*');
				termSearchModeStub = fakes.mocker().stub(sut, 'termSearchMode', function () {
					return false;
				});
				applyBindingsStub = fakes.mocker().stub(markupService, 'applyBindings', function () {});
				initExternalsStub = fakes.mocker().stub(sut, 'initExternals', function () {});
				searchStub = fakes.mocker().stub(searchService, 'search', function (p) {
					actualSearchParams = p;
					return {
						done: function (cb) {
							return actualSearchCallback = cb;
						}
					};
				});

				// When
				sut.bind();

				// Then
				applyBindingsStub.calledWith(sut, sinon.match.string);
				termSearchModeStub.calledOnce.should.equal(true);
				initExternalsStub.calledOnce.should.equal(true);
				searchStub.calledOnce.should.equal(true);
				actualSearchParams.expression.should.be.equal('*');
				actualSearchParams.page.should.be.equal(10);
			});
		});
		describe('When view model needs to interact with server', function () {
			it('Should run route during search request when location remains equal to requested path', function () {
				// Given
				var actual, getLocationStub, path, runRouteStub, storeStub, sut;

				sut = new SearchViewModel(1, '*');
				path = '/#/search/' + encodeURIComponent('*');
				storeStub = fakes.mocker().stub(sut, 'storeAdvancedDetails', function () {
					return true;
				});
				getLocationStub = fakes.mocker().stub(sut.sammy(), 'getLocation');
				runRouteStub = fakes.mocker().stub(sut.sammy(), 'runRoute');
				getLocationStub.returns(path);
				runRouteStub.withArgs('get', path).returns(true);

				// When
				actual = sut.enterSearch(null, { keyCode: 13});

				// Then
				actual.should.equal(false);
				storeStub.calledOnce.should.equal(true);
				sinon.assert.calledWithMatch(runRouteStub, 'get', path);
			});
			it('Should set location during search request when location does not remain equal to requested path', function () {
				// Given
				var actual, path, setLocationStub, storeStub, sut;

				sut = new SearchViewModel(1, '*');
				path = '/#/search/' + encodeURIComponent('*');
				storeStub = fakes.mocker().stub(sut, 'storeAdvancedDetails', function () {
					return true;
				});
				setLocationStub = fakes.mocker().stub(sut.sammy(), 'setLocation', function () {
					return true;
				});

				// When
				actual = sut.enterSearch(null, { keyCode: 13});

				// Then
				actual.should.equal(false);
				storeStub.calledOnce.should.equal(true);
				sinon.assert.calledWithMatch(setLocationStub, path);
			});
		});
	});

})();