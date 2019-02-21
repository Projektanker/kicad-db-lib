"use strict";
exports.__esModule = true;
var oldFs = require("fs");
var util = require("util");
var fs = /** @class */ (function () {
    function fs() {
    }
    fs.checkFileExists = function (file) {
        return new Promise(function (r) {
            return oldFs.access(file, oldFs.constants.F_OK, function (e) { return r(!e); });
        });
    };
    fs.stat = util.promisify(oldFs.stat);
    fs.readFile = util.promisify(oldFs.readFile);
    fs.writeFile = util.promisify(oldFs.writeFile);
    fs.readdir = util.promisify(oldFs.readdir);
    fs.unlink = util.promisify(oldFs.unlink);
    return fs;
}());
exports.fs = fs;
//# sourceMappingURL=fs.js.map