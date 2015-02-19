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
	});
})();