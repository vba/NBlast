deps = ['chai',
        'sinon',
        'underscore']

define deps, (chai, sinon) ->
	mocker = null
	chai.should()
	window.beforeEach -> mocker = sinon.sandbox.create()
	window.afterEach -> mocker.restore()

	describe 'When empty search page is in use', ->