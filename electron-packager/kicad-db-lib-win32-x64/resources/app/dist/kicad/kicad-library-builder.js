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
var kicad_library_reader_1 = require("./kicad-library-reader");
var fs_1 = require("../fs");
var path = require("path");
var KiCadLibraryBuilder = /** @class */ (function () {
    function KiCadLibraryBuilder() {
        this._reader = new kicad_library_reader_1.KiCadLibraryReader();
    }
    KiCadLibraryBuilder.prototype.finishLibrary = function (outputDirectory, library, lib, dcm) {
        return __awaiter(this, void 0, void 0, function () {
            var libFile, dcmFile, libText, dxmText;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!lib)
                            return [2 /*return*/];
                        if (!dcm)
                            return [2 /*return*/];
                        libFile = path.join(outputDirectory, library + ".lib");
                        dcmFile = path.join(outputDirectory, library + ".dcm");
                        lib.push('#End Library\n');
                        libText = lib.join('\n');
                        console.log(libFile);
                        return [4 /*yield*/, fs_1.fs.writeFile(libFile, libText, 'utf8')];
                    case 1:
                        _a.sent();
                        dcm.push('#End Doc Library\n');
                        dxmText = dcm.join('\n');
                        console.log(dcmFile);
                        return [4 /*yield*/, fs_1.fs.writeFile(dcmFile, dxmText, 'utf8')];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryBuilder.prototype.buildAsync = function (kiCadParts, outputDirectory, clearOutputDirectory) {
        if (clearOutputDirectory === void 0) { clearOutputDirectory = false; }
        return __awaiter(this, void 0, void 0, function () {
            var files, i, file, parts, library, lib, dcm, i, part, template, values, partString, i_1, value, find, error_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, 13, , 14]);
                        console.log('buildAsync()');
                        if (!clearOutputDirectory) return [3 /*break*/, 5];
                        console.log('clear output directory:');
                        return [4 /*yield*/, fs_1.fs.readdir(outputDirectory)];
                    case 1:
                        files = _a.sent();
                        files = files
                            .filter(function (f) {
                            return (f.toLowerCase().endsWith('.lib') ||
                                f.toLowerCase().endsWith('.dcm'));
                        })
                            .map(function (f) { return path.join(outputDirectory, f); });
                        i = 0;
                        _a.label = 2;
                    case 2:
                        if (!(i < files.length)) return [3 /*break*/, 5];
                        file = files[i];
                        return [4 /*yield*/, fs_1.fs.unlink(file)];
                    case 3:
                        _a.sent();
                        _a.label = 4;
                    case 4:
                        i++;
                        return [3 /*break*/, 2];
                    case 5:
                        // Sort parts by Library and Value
                        console.log('Sort parts by Library and Value');
                        parts = kiCadParts.sort(function (a, b) {
                            if (a.library == b.library) {
                                return a.value < b.value ? -1 : a.value > b.value ? 1 : 0;
                            }
                            else {
                                return a.library < b.library ? -1 : 1;
                            }
                        });
                        library = null;
                        lib = null, dcm = null;
                        i = 0;
                        _a.label = 6;
                    case 6:
                        if (!(i < parts.length)) return [3 /*break*/, 11];
                        console.log("part[" + i + "]");
                        part = parts[i];
                        if (!(part.library != library)) return [3 /*break*/, 8];
                        // Close previous library files
                        return [4 /*yield*/, this.finishLibrary(outputDirectory, library, lib, dcm)];
                    case 7:
                        // Close previous library files
                        _a.sent();
                        // Create new library files
                        library = part.library;
                        lib = [];
                        dcm = [];
                        // Write file head
                        lib.push('EESchema-LIBRARY Version 2.4');
                        lib.push('#encoding utf-8');
                        lib.push('#');
                        dcm.push('EESchema-DOCLIB  Version 2.0');
                        dcm.push('#');
                        _a.label = 8;
                    case 8: return [4 /*yield*/, this._reader.getSymbolTemplateAsync(part)];
                    case 9:
                        template = _a.sent();
                        // Fill part and remove empty lines
                        part.customFields['ID'] = part.id.toString();
                        part.customFields['Description'] = part.description;
                        values = [
                            part.value,
                            part.reference,
                            part.footprint,
                            part.datasheet,
                            this.createCustomFields(part.customFields)
                        ];
                        partString = template;
                        for (i_1 = 0; i_1 < values.length; i_1++) {
                            value = values[i_1];
                            find = new RegExp("\\{" + i_1 + "\\}", 'g');
                            partString = partString.replace(find, value);
                        }
                        partString = partString.replace(/\n\n/g, '\n');
                        // Write .lib file
                        lib.push(partString);
                        // Write .dcm file
                        if (part.value)
                            dcm.push("$CMP " + part.value);
                        if (part.description)
                            dcm.push("D " + part.description);
                        if (part.keywords)
                            dcm.push("K " + part.keywords);
                        if (part.datasheet)
                            dcm.push("F " + part.datasheet);
                        dcm.push('$ENDCMP');
                        dcm.push('#');
                        _a.label = 10;
                    case 10:
                        i++;
                        return [3 /*break*/, 6];
                    case 11: 
                    // Close previous library files
                    return [4 /*yield*/, this.finishLibrary(outputDirectory, library, lib, dcm)];
                    case 12:
                        // Close previous library files
                        _a.sent();
                        return [2 /*return*/, Promise.resolve()];
                    case 13:
                        error_1 = _a.sent();
                        return [2 /*return*/, Promise.reject(error_1)];
                    case 14: return [2 /*return*/];
                }
            });
        });
    };
    KiCadLibraryBuilder.prototype.createCustomField = function (fieldNumber, key, value) {
        if (fieldNumber < 4) {
            throw new Error('Fieldnumber must be greater or equal 4!');
        }
        return "F" + fieldNumber + " \"" + (value ? value : '-') + "\" " + 100 *
            (fieldNumber - 3) + " " + 100 * (fieldNumber - 2) + " 50 H I C CNN \"" + key + "\"";
    };
    KiCadLibraryBuilder.prototype.createCustomFields = function (fields) {
        if (fields == null)
            return '';
        var text = [];
        var i = 4;
        for (var key in fields) {
            if (fields.hasOwnProperty(key)) {
                var value = fields[key];
                text.push(this.createCustomField(i, key, value));
                i++;
            }
        }
        return text.join('\n');
    };
    return KiCadLibraryBuilder;
}());
exports.KiCadLibraryBuilder = KiCadLibraryBuilder;
//# sourceMappingURL=kicad-library-builder.js.map