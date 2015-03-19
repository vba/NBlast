var CommonTasks,
    gulp = require('gulp'),
    gutil = require('gulp-util'),
    concat = require('gulp-concat'),
    coffee = require('gulp-coffee'),
    sourcemaps = require('gulp-sourcemaps'),
    clean = require('gulp-clean'),
    watch = require('gulp-watch'),
    less = require('gulp-less'),
    jscs = require('gulp-jscs'),
    jshint = require('gulp-jshint'),
    uglify = require('gulp-uglify'),
    mocha = require('gulp-mocha'),
    cover = require('gulp-coverage'),
    browserify = require('gulp-browserify'),
    stringify = require('stringify'),
    debug = require('gulp-debug');

config = {
	paths: {
		app: {
			views: './app/views/**/*.html',
			js: './app/js/**/*.js',
			less: './app/css/*.less',
			main: './app/js/main.js'
		},
		test: {
			coffee: './tests/**/*.coffee',
			js: './test/**/*Spec.js',
			runner: './test.html'
		},
		out: {
			bundle: './out/bundle'
		}
	}
};

CommonTasks = {
	compileSpecs: function () {
		return gulp.src(config.paths.test.coffee).pipe(sourcemaps.init()).pipe(coffee({
			bare: true
		}).on('error', gutil.log)).pipe(sourcemaps.write()).pipe(gulp.dest('./tests'));
	},
	runSpecs: function (silentMode) {
		return gulp.src(config.paths.test.runner).pipe(mocha({
			silentMode: Boolean(silentMode)
		}).on('error', gutil.log));
	}
};

gulp.task('test', ['lint'], function () {
	return gulp.src([config.paths.test.js], {
		read: false
	}).pipe(mocha());
});

gulp.task('test-cover', function () {
	return gulp.src([config.paths.test.js], {
		read: false
	}).pipe(cover.instrument({
		pattern: ['**/app/**']
	})).pipe(mocha()).pipe(cover.gather()).pipe(cover.format()).pipe(gulp.dest('./out/test-reports'));
});

gulp.task('lint', function () {
	return gulp.src([config.paths.app.js]).pipe(jshint('.jshintrc')).pipe(jshint.reporter('jshint-stylish')).pipe(jscs({
		configPath: '.jscsrc'
	}));
});

gulp.task('bundle', function () {
	gutil.log(gutil.colors.bgGreen('Start bundling...'));
	return gulp.src(config.paths.app.main, {
		read: false
	}).pipe(browserify({
		debug: true,
		transform: stringify({
			extensions: ['.html'],
			minify: false
		})
	})).pipe(gulp.dest(config.paths.out.bundle));
});

gulp.task('watch', function () {
	var app;
	app = config.paths.app;
	return gulp.watch([app.js, app.views], ['bundle']);
});

gulp.task('default', ['bundle', 'watch']);
