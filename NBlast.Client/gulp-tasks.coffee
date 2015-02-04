gulp        = require 'gulp'
gutil       = require 'gulp-util'
concat      = require 'gulp-concat'
coffee      = require 'gulp-coffee'
sourcemaps  = require 'gulp-sourcemaps'
clean       = require 'gulp-clean'
watch       = require 'gulp-watch'
less        = require 'gulp-less'
jscs        = require 'gulp-jscs'
jshint      = require 'gulp-jshint'
mocha       = require 'gulp-mocha-phantomjs'

config =
	paths :
		app :
			js : './app/js/*.js'
			less : './app/css/*.less'
		test :
			coffee: './tests/**/*.coffee'
			runner: './test.html'

CommonTasks =
	compileSpecs: () ->
		gulp.src(config.paths.test.coffee)
			.pipe sourcemaps.init()
			.pipe coffee({bare: true}).on('error', gutil.log)
			.pipe sourcemaps.write()
			.pipe gulp.dest('./tests')
	runSpecs: (silentMode) ->
		gulp.src(config.paths.test.runner)
#			.pipe plumber()
			.pipe mocha({silentMode: Boolean(silentMode)}).on('error', gutil.log)

gulp.task 'test', ->
	CommonTasks.compileSpecs()
	CommonTasks.runSpecs(false)

gulp.task 'lint',  ->
	gulp.src([config.paths.app.js])
		.pipe(jshint('.jshintrc'))
		.pipe(jshint.reporter('jshint-stylish'))
		.pipe(jscs({configPath: '.jscsrc'}))


gulp.task 'watch', ['lint'], ->
	#gutil.log(gutil.colors.bgGreen('Watching for changes...'))

gulp.task('default', ['watch'])