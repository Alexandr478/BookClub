/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    scss = require("gulp-sass"); // добавляем модуль scss

var paths = {
    webroot: "./wwwroot/"
};
//  регистрируем задачу по преобразованию styles.scss в файл css
gulp.task("scss", function () {
    return gulp.src('scss/styles.scss')
        .pipe(scss())
        .pipe(gulp.dest(paths.webroot + '/css'))
});