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
var part_1 = require("../part/part");
var path = require("path");
var settings_service_1 = require("../settings/settings.service");
var PartService = /** @class */ (function () {
    function PartService() {
        this.libraries = null;
    }
    PartService.prototype.getParts = function (sortActive, sortDirection) {
        if (sortActive === void 0) { sortActive = ''; }
        if (sortDirection === void 0) { sortDirection = ''; }
        return __awaiter(this, void 0, void 0, function () {
            var settingsService, settings, files, parts, i, file, json, part, error_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 7, , 8]);
                        settingsService = new settings_service_1.SettingsService();
                        return [4 /*yield*/, settingsService.getSettings()];
                    case 1:
                        settings = _a.sent();
                        return [4 /*yield*/, fs_1.fs.readdir(settings.paths.parts)];
                    case 2:
                        files = _a.sent();
                        parts = [];
                        i = 0;
                        _a.label = 3;
                    case 3:
                        if (!(i < files.length)) return [3 /*break*/, 6];
                        file = path.join(settings.paths.parts, files[i]);
                        console.log(file);
                        return [4 /*yield*/, fs_1.fs.readFile(file, 'utf8')];
                    case 4:
                        json = _a.sent();
                        part = JSON.parse(json);
                        console.log(part);
                        parts.push(part);
                        _a.label = 5;
                    case 5:
                        i++;
                        return [3 /*break*/, 3];
                    case 6:
                        // sort parts
                        parts = this.sortParts(parts, sortActive, sortDirection);
                        return [2 /*return*/, Promise.resolve(parts)];
                    case 7:
                        error_1 = _a.sent();
                        console.error(error_1);
                        return [2 /*return*/, Promise.resolve([])];
                    case 8: return [2 /*return*/];
                }
            });
        });
    };
    PartService.prototype.sortParts = function (parts, sortActive, sortDirection) {
        if (sortActive === void 0) { sortActive = ''; }
        if (sortDirection === void 0) { sortDirection = ''; }
        if (!sortActive || !sortDirection) {
            parts.sort(function (a, b) { return a.id - b.id; });
        }
        else {
            var asc = sortDirection == 'asc';
            switch (sortActive) {
                case 'id':
                    parts.sort(function (a, b) { return (asc ? a.id - b.id : b.id - a.id); });
                    break;
                default:
                    parts.sort(function (a, b) {
                        var key1 = a.hasOwnProperty(sortActive)
                            ? a[sortActive]
                            : a.customFields[sortActive];
                        var key2 = b.hasOwnProperty(sortActive)
                            ? b[sortActive]
                            : b.customFields[sortActive];
                        if (key1 == key2) {
                            return 0;
                        }
                        if (key1 < key2) {
                            return asc ? -1 : 1;
                        }
                        return asc ? 1 : -1;
                    });
                    break;
            }
        }
        return parts;
    };
    PartService.prototype.getPart = function (id) {
        return __awaiter(this, void 0, void 0, function () {
            var settingsService, settings, file, part, json, part, error_2, part;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 3, , 4]);
                        console.log("getPart(" + id + ")");
                        settingsService = new settings_service_1.SettingsService();
                        return [4 /*yield*/, settingsService.getSettings()];
                    case 1:
                        settings = _a.sent();
                        file = path.join(settings.paths.parts, id + ".json");
                        console.log(file);
                        part = null;
                        return [4 /*yield*/, fs_1.fs.readFile(file, 'utf8')];
                    case 2:
                        json = _a.sent();
                        part = JSON.parse(json);
                        console.log(part);
                        return [2 /*return*/, Promise.resolve(part)];
                    case 3:
                        error_2 = _a.sent();
                        console.error(error_2);
                        part = new part_1.Part();
                        part.id = id;
                        return [2 /*return*/, Promise.resolve(part)];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    PartService.prototype.addPart = function (part) {
        return __awaiter(this, void 0, void 0, function () {
            var settingsService, settings, files, allIds, file, json, error_3;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 4, , 5]);
                        console.log("addPart(part: Part)");
                        settingsService = new settings_service_1.SettingsService();
                        return [4 /*yield*/, settingsService.getSettings()];
                    case 1:
                        settings = _a.sent();
                        return [4 /*yield*/, fs_1.fs.readdir(settings.paths.parts)];
                    case 2:
                        files = _a.sent();
                        allIds = files.map(function (f) { return parseInt(f); }).filter(function (n) { return !isNaN(n); });
                        part.id = Math.max.apply(Math, allIds) + 1;
                        file = path.join(settings.paths.parts, part.id + ".json");
                        console.log(file);
                        console.log(part);
                        json = JSON.stringify(part);
                        return [4 /*yield*/, fs_1.fs.writeFile(file, json, 'utf8')];
                    case 3:
                        _a.sent();
                        return [2 /*return*/, Promise.resolve(part)];
                    case 4:
                        error_3 = _a.sent();
                        console.error(error_3);
                        return [2 /*return*/, Promise.reject(error_3)];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    PartService.prototype.updatePart = function (part) {
        return __awaiter(this, void 0, void 0, function () {
            var settingsService, settings, file, json, error_4;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 3, , 4]);
                        console.log("updatePart(part: Part)");
                        settingsService = new settings_service_1.SettingsService();
                        return [4 /*yield*/, settingsService.getSettings()];
                    case 1:
                        settings = _a.sent();
                        file = path.join(settings.paths.parts, part.id + ".json");
                        console.log(file);
                        console.log(part);
                        json = JSON.stringify(part);
                        return [4 /*yield*/, fs_1.fs.writeFile(file, json, 'utf8')];
                    case 2:
                        _a.sent();
                        return [2 /*return*/, Promise.resolve(part)];
                    case 3:
                        error_4 = _a.sent();
                        console.error(error_4);
                        return [2 /*return*/, Promise.reject(error_4)];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    PartService.prototype.deletePart = function (part) {
        return __awaiter(this, void 0, void 0, function () {
            var id, settingsService, settings, file, error_5;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 3, , 4]);
                        console.log("deletePart(part: Part | number)");
                        id = typeof part === 'number' ? part : part.id;
                        settingsService = new settings_service_1.SettingsService();
                        return [4 /*yield*/, settingsService.getSettings()];
                    case 1:
                        settings = _a.sent();
                        file = path.join(settings.paths.parts, id + ".json");
                        console.log(file);
                        console.log(part);
                        return [4 /*yield*/, fs_1.fs.unlink(file)];
                    case 2:
                        _a.sent();
                        return [2 /*return*/, Promise.resolve()];
                    case 3:
                        error_5 = _a.sent();
                        console.error(error_5);
                        return [2 /*return*/, Promise.reject(error_5)];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    PartService.prototype.getLibraries = function (filter, reload, max) {
        return __awaiter(this, void 0, void 0, function () {
            var parts, filtered, error_6;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 3, , 4]);
                        if (!(!this.libraries || reload)) return [3 /*break*/, 2];
                        return [4 /*yield*/, this.getParts()];
                    case 1:
                        parts = _a.sent();
                        this.libraries = parts
                            .map(function (p) { return p.library; })
                            .filter(function (value, index, array) { return array.indexOf(value) === index; })
                            .sort();
                        _a.label = 2;
                    case 2:
                        filtered = this.libraries
                            .filter(function (l) { return l.toLowerCase().includes(filter.toLowerCase()); })
                            .slice(0, max);
                        return [2 /*return*/, Promise.resolve(filtered)];
                    case 3:
                        error_6 = _a.sent();
                        return [2 /*return*/, Promise.reject(error_6)];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    return PartService;
}());
exports.PartService = PartService;
//# sourceMappingURL=part.service.js.map