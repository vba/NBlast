gulp        = require 'gulp'
gutil       = require 'gulp-util'
concat      = require 'gulp-concat'
coffee      = require 'gulp-coffee'
sourcemaps  = require 'gulp-sourcemaps'
clean       = require 'gulp-clean'
watch       = require 'gulp-watch'
less        = require 'gulp-less'
cssmin      = require 'gulp-minify-css'
jscs        = require 'gulp-jscs'
jshint      = require 'gulp-jshint'
uglify      = require 'gulp-uglify'
mocha       = require 'gulp-mocha'
cover       = require 'gulp-coverage'
browserify  = require 'gulp-browserify'
stringify   = require 'stringify'
debug       = require 'gulp-debug'
zip         = require 'gulp-zip'
sequence    = require 'gulp-run-sequence'

config =
	paths :
		app :
			views: './app/views/**/*.html'
			js : './app/js/**/*.js'
			less : './app/styles/*.less'
			main : './app/js/main.js'
			fonts: [
				'./bower_components/font-awesome/fonts/*'
				'./bower_components/bootstrap/fonts/*'
			]
		test :
			coffee: './tests/**/*.coffee'
			js: './test/**/*Spec.js'
			runner: './test.html'
		out :
			js: './out/bundle/js'
			fonts: './out/bundle/fonts'
			css: './out/bundle/css'
			package: './out/package/out/bundle'

gulp.task 'test', ['lint'], ->
	gulp.src([config.paths.test.js], {read: false})
		.pipe mocha()

gulp.task 'test-cover', ->
	gulp.src([config.paths.test.js], {read: false})
		.pipe cover.instrument({
			pattern: [ '**/app/**']
		})
		.pipe mocha()
		.pipe cover.gather()
		.pipe cover.format()
		.pipe gulp.dest('./out/test-reports')

gulp.task 'lint',  ->
	gulp.src([config.paths.app.js])
		.pipe(jshint('.jshintrc'))
		.pipe(jshint.reporter('jshint-stylish'))
		.pipe(jscs({configPath: '.jscsrc'}))


gulp.task 'bundle:styles', ->
	gulp.src config.paths.app.less
		.pipe sourcemaps.init()
		.pipe less()
		.pipe cssmin()
		.pipe sourcemaps.write()
		.pipe gulp.dest config.paths.out.css

gulp.task 'bundle:js', ->
	gulp.src(config.paths.app.main, {read:false})
		.pipe browserify {
			debug: true
			transform: stringify {
				extensions: ['.html'], minify: false
			}
		}
		.pipe sourcemaps.init()
		.pipe uglify {preserveComments: 'all'}
		.pipe sourcemaps.write()
		.pipe gulp.dest config.paths.out.js

gulp.task 'bundle:fonts', ->
	gulp.src(config.paths.app.fonts)
		.pipe gulp.dest config.paths.out.fonts

gulp.task 'bundle', ['test', 'bundle:js', 'bundle:styles', 'bundle:fonts']

gulp.task 'package:resources', ->
	gulp.src('./out/bundle/**').pipe gulp.dest(config.paths.out.package)

gulp.task 'package:entry', ->
	gulp.src('./index.html').pipe gulp.dest('./out/package')

gulp.task 'package:zip', ->
	gulp.src './out/package/**'
		.pipe zip('nblast.client.zip')
		.pipe gulp.dest('./out')

gulp.task 'package', ->
	sequence('clean'
			 ['bundle']
			 ['package:resources', 'package:entry']
			'package:zip')

gulp.task 'clean', ->
	gulp.src('./out', {read: false}).pipe(clean())

gulp.task 'watch', ->
	app = config.paths.app
	gulp.watch([app.js, app.views], ['bundle:js'])
	gulp.watch(app.less, ['bundle:styles', 'bundle:fonts'])

gulp.task('default', ['bundle:styles', 'bundle:fonts', 'bundle:js', 'watch'])