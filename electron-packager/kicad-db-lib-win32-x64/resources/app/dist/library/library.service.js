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
var settings_service_1 = require("../settings/settings.service");
var part_service_1 = require("../parts/part.service");
var path = require("path");
var kicad_library_builder_1 = require("../kicad/kicad-library-builder");
var kicad_library_reader_1 = require("../kicad/kicad-library-reader");
var LibraryService = /** @class */ (function () {
    function LibraryService() {
        this.symbols = null;
        this.footprints = null;
        this.settingsService = new settings_service_1.SettingsService();
        this.partService = new part_service_1.PartService();
        this.builder = new kicad_library_builder_1.KiCadLibraryBuilder();
        this.reader = new kicad_library_reader_1.KiCadLibraryReader();
    }
    LibraryService.prototype.build = function (clearBeforeBuild) {
        if (clearBeforeBuild === void 0) { clearBeforeBuild = true; }
        return __awaiter(this, void 0, void 0, function () {
            var parts, settings, i, part, split, library, symbol, error_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 4, , 5]);
                        console.log('Build()');
                        console.log('read parts');
                        return [4 /*yield*/, this.partService.getParts()];
                    case 1:
                        parts = _a.sent();
                        console.log('read settings');
                        return [4 /*yield*/, this.settingsService.getSettings()];
                    case 2:
                        settings = _a.sent();
                        console.log('prepare parts');
                        for (i = 0; i < parts.length; i++) {
                            part = parts[i];
                            split = part.symbol.split(':');
                            if (split.length != 2) {
                                throw new Error('Symbol must be defined as "library name:symbol"');
                            }
                            library = split[0];
                            symbol = split[1];
                            part.symbol = path.join(settings.paths.symbol, library + ".lib") + ":" + symbol;
                            parts[i] = part;
                        }
                        console.log('build...');
                        return [4 /*yield*/, this.builder.buildAsync(parts, settings.paths.output, clearBeforeBuild)];
                    case 3:
                        _a.sent();
                        console.log('done');
                        return [2 /*return*/, Promise.resolve()];
                    case 4:
                        error_1 = _a.sent();
                        console.error(error_1);
                        return [2 /*return*/, Promise.reject(error_1)];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    LibraryService.prototype.regExpEscape = function (s) {
        /*
        Source:
        https://stackoverflow.com/questions/3561493/is-there-a-regexp-escape-function-in-javascript/3561711#3561711
         */
        return s.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
    };
    LibraryService.prototype.wildCardToRegExp = function (s) {
        var exp = this.regExpEscape(s)
            .replace(/\\\*/g, '.*?')
            .replace(/\\\?/g, '.?');
        return exp;
    };
    LibraryService.prototype.getSymbols = function (filter, reload, max) {
        return __awaiter(this, void 0, void 0, function () {
            var settings, directory, _a, regex, filtered, error_2;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _b.trys.push([0, 4, , 5]);
                        if (!(!this.symbols || reload)) return [3 /*break*/, 3];
                        return [4 /*yield*/, this.settingsService.getSettings()];
                    case 1:
                        settings = _b.sent();
                        directory = settings.paths.symbol;
                        _a = this;
                        return [4 /*yield*/, this.reader.getSymbolsFromDirectoryAsync(directory)];
                    case 2:
                        _a.symbols = _b.sent();
                        _b.label = 3;
                    case 3:
                        regex = this.wildCardToRegExp(filter.toLowerCase().trim());
                        filtered = this.symbols
                            .filter(function (s) { return s.toLowerCase().match(regex); })
                            .slice(0, max);
                        return [2 /*return*/, Promise.resolve(filtered)];
                    case 4:
                        error_2 = _b.sent();
                        return [2 /*return*/, Promise.reject(error_2)];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    LibraryService.prototype.getFootprints = function (filter, reload, max) {
        return __awaiter(this, void 0, void 0, function () {
            var settings, directory, _a, regex, filtered, error_3;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _b.trys.push([0, 4, , 5]);
                        if (!(!this.footprints || reload)) return [3 /*break*/, 3];
                        return [4 /*yield*/, this.settingsService.getSettings()];
                    case 1:
                        settings = _b.sent();
                        directory = settings.paths.footprint;
                        _a = this;
                        return [4 /*yield*/, this.reader.getFootprintsFromDirectoryAsync(directory)];
                    case 2:
                        _a.footprints = _b.sent();
                        _b.label = 3;
                    case 3:
                        regex = this.wildCardToRegExp(filter.toLowerCase().trim());
                        filtered = this.footprints
                            .filter(function (s) { return s.toLowerCase().match(regex); })
                            .slice(0, max);
                        return [2 /*return*/, Promise.resolve(filtered)];
                    case 4:
                        error_3 = _b.sent();
                        return [2 /*return*/, Promise.reject(error_3)];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    return LibraryService;
}());
exports.LibraryService = LibraryService;
//# sourceMappingURL=library.service.js.map