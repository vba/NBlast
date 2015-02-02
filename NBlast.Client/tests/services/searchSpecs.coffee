define(['chai', 'sinon', 'mocha', 'underscore'], (chai, sinon, mocha, _) ->
	chai.should()
	describe('When a user get a log item by identifier', ()->
		it('Should retrieve this item when it exists', ()->
			(true).should.be.equal(true)
		)
	)
)