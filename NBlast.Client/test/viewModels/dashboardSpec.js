(function () {
	'use strict';
	var DashboardViewModel = require('../../app/js/viewModels/dashboard'),
	    chai               = require('chai'),
	    fakes              = require('../fakes');

	chai.should();

	describe('When dashboard view model is in use', function () {
		it('Should bind view model with all supplied data', function () {
			// Given
			var mocker            = fakes.mocker(),
				markupService     = require('../../app/js/services/markup'),
			    dashboardService  = require('../../app/js/services/dashboard'),
			    applyBindingsStub = mocker.stub(markupService, 'applyBindings'),
			    groupByStub       = mocker.stub(dashboardService, 'groupBy'),
			    sut               = new DashboardViewModel(),
			    loggerResult      = {
				    done: function(callback) {
					    callback({facets: [1]});
				    }
			    },
			    senderResult      = {
				    done: function(callback) {
					    callback({facets: [1, 2]});
				    }
			    },
			    levelResult      = {
				    done: function(callback) {
					    callback({facets: [{name: 'n1', count: 1}, {name: 'n2', count: 2}, {name: 'n3', count: 3}]});
				    }
			    };

			applyBindingsStub.returns(true);
			groupByStub.withArgs('level').returns(levelResult);
			groupByStub.withArgs('logger', 7).returns(loggerResult);
			groupByStub.withArgs('sender', 7).returns(senderResult);

			// When
			sut.bind();

			// Then
			applyBindingsStub.calledOnce.should.equal(true);
			groupByStub.calledWith('level').should.equal(true);
			groupByStub.calledWith('logger', 7).should.equal(true);
			groupByStub.calledWith('sender', 7).should.equal(true);
			sut.topLoggers().should.have.length(1);
			sut.topSenders().should.have.length(2);
			sut.levelCounters().n1.should.equal(1);
			sut.levelCounters().n2.should.equal(2);
			sut.levelCounters().n3.should.equal(3);
		});

		it('Should set location and prepare storage when searching by term', function () {
			// Given
			var StorageService = require('../../app/js/services/storage'),
			    mocker = fakes.mocker(),
			    clearAllStub = mocker.stub(StorageService.prototype, 'clearAll'),
			    storeSearchStub = mocker.stub(StorageService.prototype, 'storeSearch'),
			    storeSortStub = mocker.stub(StorageService.prototype, 'storeSort'),
				sut = new DashboardViewModel(),
			    sammyStub = mocker.stub(sut, 'sammy'),
				sammy = {setLocation: mocker.stub()};

			clearAllStub.returns(true);
			storeSearchStub.withArgs('logger').returns(true);
			storeSortStub.withArgs('createdAt', true).returns(true);
			sammyStub.returns(sammy);

			// When
			sut.searchTerm('logger', 'alaska');

			// Then
			clearAllStub.calledOnce.should.equal(true);
			storeSearchStub.calledWith('logger').should.equal(true);
			storeSortStub.calledWith('createdAt', true).should.equal(true);
			sammy.setLocation.calledOnce.should.equal(true);
			sammy.setLocation.calledWith('#/search/'+ encodeURIComponent('alaska')).should.equal(true);
		});
	});
})();