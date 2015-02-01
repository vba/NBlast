gulp    = require 'gulp'
gutil   = require 'gulp-util'
concat  = require 'gulp-concat'
coffee  = require 'gulp-coffee'
clean   = require 'gulp-clean'
watch   = require 'gulp-watch'
less    = require 'gulp-less'
jscs    = require 'gulp-jscs'
jshint  = require 'gulp-jshint'

config =
	paths :
		app :
			js : './app/js/*.js'
			less : './app/css/*.less'

gulp.task('lint', () ->
	gulp.src([config.paths.app.js])
		.pipe(jshint('.jshintrc'))
		.pipe(jshint.reporter('jshint-stylish'))
		.pipe(jscs({configPath: '.jscsrc'}))
)

gulp.task('watch', ['lint'], () ->
	#gutil.log(gutil.colors.bgGreen('Watching for changes...'))
)

gulp.task('default', ['watch'])