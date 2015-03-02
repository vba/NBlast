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
uglify      = require 'gulp-uglify'
mocha       = require 'gulp-mocha-phantomjs'
browserify  = require 'gulp-browserify'
stringify   = require 'stringify'

config =
	paths :
		app :
			views: './app/views/**/*.html'
			js : './app/js/**/*.js'
			less : './app/css/*.less'
			main : './app/js/main.js'
		test :
			coffee: './tests/**/*.coffee'
			runner: './test.html'
		out :
			bundle: './out/bundle'

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

gulp.task 'bundle', ->
	gutil.log(gutil.colors.bgGreen('Start bundling...'))
	gulp.src(config.paths.app.main, {read:false})
		.pipe browserify {
			debug: true
			transform: stringify {
				extensions: ['.html'], minify: false
			}
		}
#		.pipe uglify {preserveComments: 'all'}
		.pipe gulp.dest config.paths.out.bundle

gulp.task 'watch', ->
	app = config.paths.app
	gulp.watch([app.js, app.views], ['bundle'])

gulp.task('default', ['bundle', 'watch'])