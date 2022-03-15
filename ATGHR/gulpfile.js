/// <binding />
// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var runSequence = require('run-sequence');

var cleanCSS = require('gulp-clean-css');

var config = {
	//Include all js files but exclude any min.js files
	srcjs: [
		'wwwroot/lib/jquery/dist/jquery.js',
		'wwwroot/lib/jquery-dateFormat/dist/jquery-dateFormat.js',
		'wwwroot/lib/datatables.net/js/jquery.dataTables.js',
		'wwwroot/lib/datatables.net-dt/js/dataTables.dataTables.js',
		'wwwroot/lib/datatables.net-se/js/dataTables.semanticui.js',
		'wwwroot/lib/semantic-ui/dist/semantic.js',
		'wwwroot/js/site.js',
		'wwwroot/lib/orgchart/dist/js/jquery.orgchart.js',
		//'wwwroot/lib/jquery/dist/jquery.js',
		//'wwwroot/lib/**/*.js',
		//'wwwroot/**/*.js',
		'!wwwroot/**/*.min.js'],
	srccss: [
		'wwwroot/lib/semantic-ui/dist/semantic.css',
		'wwwroot/lib/datatables.net-dt/css/jquery.dataTables.css',
		'wwwroot/lib/datatables.net-se/css/dataTables.semanticui.css',
		'wwwroot/css/site.css',
		'wwwroot/css/print.css',
		//'wwwroot/lib/**/*.css',
		//'wwwroot/**/*.css',
		'!wwwroot/**/*.min.css']
};

gulp.task('clean assets', function () {
	return del(['wwwroot/css/themes']);
});

gulp.task('clean', gulp.series('clean assets', function () {
	return del(['wwwroot/lib/**/*']);
}));

gulp.task('copy assets', function () {
	return gulp.src([
		'node_modules/semantic-ui/dist/themes/default/assets/**/*'
	], {
			base: 'node_modules/semantic-ui/dist'
		}).pipe(gulp.dest('wwwroot/css'));
});

gulp.task('copy', gulp.series('copy assets', function () {
	return gulp.src([
		'node_modules/jquery/dist/jquery.js',
		'node_modules/jquery-dateFormat/dist/jquery-dateFormat.js',
		'node_modules/semantic-ui/dist/semantic.css',
		'node_modules/semantic-ui/dist/components/*',
		'node_modules/semantic-ui/dist/semantic.js',
		'node_modules/semantic-ui/dist/themes/default/assets/**/*',
		'node_modules/datatables.net/js/**/*',
		'node_modules/datatables.net-dt/css/**/*',
		'node_modules/datatables.net-dt/images/**/*',
		'node_modules/datatables.net-dt/js/**/*',
		'node_modules/datatables.net-se/css/**/*',
		'node_modules/datatables.net-se/js/**/*',
		'node_modules/orgchart/dist/js/**/*',
		'node_modules/orgchart/dist/css/**/*',
		'node_modules/font-awesome/**/*',
		'!**/*.min.*',
		'!**/*.map',
		'!**/*.slim.*',
		'!**/*.bundle.*'
	], {
			base: 'node_modules'
		}).pipe(gulp.dest('wwwroot/lib'));
}));

gulp.task('scripts', function () {
	return gulp.src(config.srcjs)
		.pipe(uglify())
		.pipe(concat('site.min.js'))
		.pipe(gulp.dest('wwwroot/js/'));
});

gulp.task('style', function () {
	return gulp.src(config.srccss)
		.pipe(cleanCSS())
		.pipe(concat('site.min.css'))
		.pipe(gulp.dest('wwwroot/css/'));
});

gulp.task('bundle', gulp.series('scripts', 'style'));

gulp.task('Debug', gulp.series('clean', 'copy', function (done) { done(); }));

gulp.task('Release', gulp.series('clean', 'copy', 'bundle', function (done) { done(); }));
