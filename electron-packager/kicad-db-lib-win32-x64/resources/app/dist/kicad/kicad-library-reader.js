"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
exports.__esModule = true;
var fs_1 = require("../fs");
var path = require("path");
var KiCadLibraryReader = /** @class */ (function () {
    function KiCadLibraryReader() {
    }
    KiCadLibraryReader.prototype.getSymbolAsync = function (library, symbol, bufferLibrary) {
        if (bufferLibrary === void 0) { bufferLibrary = true; }
        return __awaiter(this, void 0, void 0, function () {
            var _a, _b, lines, text, start, end, i, line_1, i, line, stringBuilder, result, error_1;
            return __generator(this, function (_c) {
                switch (_c.label) {
                    case 0:
                        _c.trys.push([0, 6, , 7]);
                        console.log('getSymbolAsync(...)');
                        console.log("fs.checkFileExists(" + library + ")");
                        _b = (_a = console).log;
                        return [4 /*yield*/, fs_1.fs.checkFileExists(library)];
                    case 1:
                        _b.apply(_a, [_c.sent()]);
                        return [4 /*yield*/, fs_1.fs.checkFileExists(library)];
                    case 2:
                        if (!(_c.sent())) {
                            throw new Error("Symbol library \"" + library + "\" not found!");
                        }
                        if (!(bufferLibrary && library == this._getSymbol_library)) return [3 /*break*/, 3];
                        lines = this._getSymbol_lines;
                        return [3 /*break*/, 5];
                    case 3:
                        console.log("fs.readFile(" + library + ", 'utf8')");
                        return [4 /*yield*/, fs_1.fs.readFile(library, 'utf8')];
                    case 4:
                        text = _c.sent();
                        lines = text.replace(/\r\n/g, '\n').split('\n');
                        this._getSymbol_library = bufferLibrary ? library : null;
                        this._getSymbol_lines = bufferLibrary ? lines : null;
                        _c.label = 5;
                    case 5:
                        start = 0, end = 0;
                        // Search begin of symbol (DEF ...)
                        for (i = 0; i < lines.length; i++) {
                            line_1 = lines[i];
                            if (!line_1.startsWith("DEF " + symbol))
                                continue;
                            start = i;
                            break;
                        }
                        if (start == 0) {
                            throw new Error("Symbol \"" + symbol + "\" not found in symbol library \"" + library + "\"");
                        }
                        // Search end of symbol (ENDDEF)
                        for (i = start + 1; i < lines.length; i++) {
                            line = lines[i];
                            if (!line.startsWith('ENDDEF'))
                                continue;
                            end = i + 1;
                            break;
                        }
                        if (end < start) {
                            throw new Error("Symbol library \"" + library + "\" is corrupted! \"ENDDEF\" not found!");
                        }
                        stringBuilder = [];
                        // Add comment before symbol
                        stringBuilder.push("# " + symbol);
                        stringBuilder.push('#');
                        for (i = start; i < end; i++) {
                            stringBuilder.push(lines[i]);
                        }
                        // Add comment after symbol
                        stringBuilder.push('#');
                        result = stringBuilder.join('\n');
                        //console.log(result);
                        return [2 /*return*/, Promise.resolve(result)];
                    case 6:
                        error_1 = _c.sent();
                        return [2 /*return*/, Promise.reject(error_1)];
                    case 7: return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryReader.prototype.getSymbolTemplateAsync = function (part) {
        return __awaiter(this, void 0, void 0, function () {
            var split, symbol, path, template, lines, i, format, startIndex, index, start, find, result, error_2;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 2, , 3]);
                        console.log('getSymbolTemplateAsync(part: Part)');
                        split = part.symbol.split(':');
                        if (split.length < 2) {
                            throw new Error('Symbol must be defined as "path:symbol"');
                        }
                        symbol = split[split.length - 1];
                        split.pop();
                        path = split.join(':');
                        return [4 /*yield*/, this.getSymbolAsync(path, symbol)];
                    case 1:
                        template = _a.sent();
                        lines = template
                            .replace(/\r\n/g, '\n')
                            .split('\n')
                            .filter(function (l) {
                            return l;
                        });
                        // Remove lines starting with # ( and add them later )
                        while (lines[0].startsWith('#')) {
                            lines.shift();
                        }
                        i = 0;
                        // "DEF value reference ..." to "DEF {0} {1} ..."
                        start = 'DEF ';
                        format = 0;
                        find = ' ';
                        startIndex = start.length;
                        index = lines[i].indexOf(find, startIndex);
                        lines[i] =
                            lines[i].substr(0, startIndex) + ("{" + format + "}") + lines[i].substr(index);
                        start = 'DEF {0} ';
                        format = 1;
                        startIndex = start.length;
                        index = lines[i].indexOf(find, startIndex);
                        lines[i] =
                            lines[i].substr(0, startIndex) + ("{" + format + "}") + lines[i].substr(index);
                        // Clear fields F0 - F3
                        start = 'Fx "';
                        find = '"';
                        for (i = 1; i < 5; i++) {
                            switch (i) {
                                case 1:
                                    format = 1;
                                    break;
                                case 2:
                                    format = 0;
                                    break;
                                case 3:
                                    format = 2;
                                    break;
                                case 4:
                                    format = 3;
                                    break;
                                default:
                                    break;
                            }
                            startIndex = start.length;
                            index = lines[i].indexOf(find, startIndex);
                            lines[i] =
                                lines[i].substr(0, startIndex) +
                                    ("{" + format + "}") +
                                    lines[i].substr(index);
                        }
                        // Remove F4 and following
                        while (lines[i].startsWith('F')) {
                            lines.splice(i, 1);
                        }
                        // Insert {4} for custom fields
                        lines.splice(i, 0, '{4}');
                        // Find and remove ALIAS
                        for (i = 0; i < lines.length; i++) {
                            if (!lines[i].startsWith('ALIAS'))
                                continue;
                            lines.splice(i, 1);
                            break;
                        }
                        // Restore deleted #-Lines
                        lines.splice(0, 0, '# {0}');
                        lines.splice(1, 0, '#');
                        result = lines.join('\n');
                        console.log(result);
                        return [2 /*return*/, Promise.resolve(result)];
                    case 2:
                        error_2 = _a.sent();
                        return [2 /*return*/, Promise.reject(error_2)];
                    case 3: return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryReader.prototype.getSymbolsFromDirectoryAsync = function (directory) {
        return __awaiter(this, void 0, void 0, function () {
            var files, result, _i, files_1, file, library, name, stat, symbols, _a, symbols_1, symbol, error_3;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _b.trys.push([0, 7, , 8]);
                        return [4 /*yield*/, fs_1.fs.readdir(directory)];
                    case 1:
                        files = _b.sent();
                        files = files.filter(function (f) { return f.toLowerCase().endsWith('.lib'); });
                        result = [];
                        _i = 0, files_1 = files;
                        _b.label = 2;
                    case 2:
                        if (!(_i < files_1.length)) return [3 /*break*/, 6];
                        file = files_1[_i];
                        library = path.join(directory, file);
                        name = file.replace('.lib', '');
                        return [4 /*yield*/, fs_1.fs.stat(library)];
                    case 3:
                        stat = _b.sent();
                        if (!stat.isFile)
                            return [3 /*break*/, 5];
                        return [4 /*yield*/, this.getSymbolsAsync(library)];
                    case 4:
                        symbols = _b.sent();
                        for (_a = 0, symbols_1 = symbols; _a < symbols_1.length; _a++) {
                            symbol = symbols_1[_a];
                            result.push(name + ":" + symbol);
                        }
                        _b.label = 5;
                    case 5:
                        _i++;
                        return [3 /*break*/, 2];
                    case 6:
                        result = result.sort();
                        console.log(result);
                        return [2 /*return*/, Promise.resolve(result)];
                    case 7:
                        error_3 = _b.sent();
                        return [2 /*return*/, Promise.reject(error_3)];
                    case 8: return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryReader.prototype.getSymbolsAsync = function (library) {
        return __awaiter(this, void 0, void 0, function () {
            var stat, symbols, text, lines, i, line, start, end, error_4;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 4, , 5]);
                        return [4 /*yield*/, fs_1.fs.checkFileExists(library)];
                    case 1:
                        if (!(_a.sent())) {
                            throw new Error("File \"" + library + "\" not found!");
                        }
                        return [4 /*yield*/, fs_1.fs.stat(library)];
                    case 2:
                        stat = _a.sent();
                        if (!stat.isFile()) {
                            throw new Error("\"" + library + "\" is not a file!");
                        }
                        symbols = [];
                        return [4 /*yield*/, fs_1.fs.readFile(library, 'utf8')];
                    case 3:
                        text = _a.sent();
                        lines = text.replace(/\r\n/g, '\n').split('\n');
                        for (i = 0; i < lines.length; i++) {
                            line = lines[i];
                            if (!line.startsWith('DEF'))
                                continue;
                            start = 'DEF '.length;
                            end = line.indexOf(' ', start);
                            symbols.push(line.substring(start, end));
                        }
                        return [2 /*return*/, Promise.resolve(symbols)];
                    case 4:
                        error_4 = _a.sent();
                        Promise.reject(error_4);
                        return [3 /*break*/, 5];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryReader.prototype.getFootprintsAsync = function (directory) {
        return __awaiter(this, void 0, void 0, function () {
            var stat, extension, footprints, error_5;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 4, , 5]);
                        return [4 /*yield*/, fs_1.fs.checkFileExists(directory)];
                    case 1:
                        if (!(_a.sent())) {
                            throw new Error("Directory \"" + directory + "\" not found!");
                        }
                        return [4 /*yield*/, fs_1.fs.stat(directory)];
                    case 2:
                        stat = _a.sent();
                        if (!stat.isDirectory()) {
                            throw new Error("\"" + directory + "\" is not a directory!");
                        }
                        extension = '.kicad_mod';
                        return [4 /*yield*/, fs_1.fs.readdir(directory)];
                    case 3:
                        footprints = _a.sent();
                        footprints = footprints
                            .filter(function (f) { return f.toLowerCase().endsWith(extension); })
                            .map(function (f) { return f.replace(extension, ''); });
                        return [2 /*return*/, Promise.resolve(footprints)];
                    case 4:
                        error_5 = _a.sent();
                        Promise.reject(error_5);
                        return [3 /*break*/, 5];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryReader.prototype.getFootprintsFromDirectoryAsync = function (directory) {
        return __awaiter(this, void 0, void 0, function () {
            var folders, result, _i, folders_1, folder, library, name, stat, symbols, _a, symbols_2, symbol, error_6;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _b.trys.push([0, 7, , 8]);
                        return [4 /*yield*/, fs_1.fs.readdir(directory)];
                    case 1:
                        folders = _b.sent();
                        folders = folders.filter(function (f) { return f.toLowerCase().endsWith('.pretty'); });
                        result = [];
                        _i = 0, folders_1 = folders;
                        _b.label = 2;
                    case 2:
                        if (!(_i < folders_1.length)) return [3 /*break*/, 6];
                        folder = folders_1[_i];
                        library = path.join(directory, folder);
                        name = folder.replace('.pretty', '');
                        return [4 /*yield*/, fs_1.fs.stat(library)];
                    case 3:
                        stat = _b.sent();
                        if (!stat.isDirectory)
                            return [3 /*break*/, 5];
                        return [4 /*yield*/, this.getFootprintsAsync(library)];
                    case 4:
                        symbols = _b.sent();
                        for (_a = 0, symbols_2 = symbols; _a < symbols_2.length; _a++) {
                            symbol = symbols_2[_a];
                            result.push(name + ":" + symbol);
                        }
                        _b.label = 5;
                    case 5:
                        _i++;
                        return [3 /*break*/, 2];
                    case 6:
                        result = result.sort();
                        console.log(result);
                        return [2 /*return*/, Promise.resolve(result)];
                    case 7:
                        error_6 = _b.sent();
                        return [2 /*return*/, Promise.reject(error_6)];
                    case 8: return [2 /*return*/];
                }
            });
        });
    };
    return KiCadLibraryReader;
}());
exports.KiCadLibraryReader = KiCadLibraryReader;
//# sourceMappingURL=kicad-library-reader.js.map