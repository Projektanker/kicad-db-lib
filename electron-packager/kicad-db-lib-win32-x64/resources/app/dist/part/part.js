"use strict";
exports.__esModule = true;
var Part = /** @class */ (function () {
    function Part() {
        this.id = 0;
        // Schematic reference. E.g. "R" for resistors
        this.reference = "";
        // Part value. E.g. "10K". Must be unique per <see cref="Library"/>
        this.value = "";
        // Footprint of part. Have to match KiCad footprint libraries. E.g. "Resistor_SMD:R_0603_1608Metric"
        this.footprint = "";
        // Reference to symbol. ../lib1.lib:symbol1
        this.symbol = "";
        // Library the part have be stored in. <see cref="Value"/> must be unique per library.
        this.library = "";
        // Datasheet link
        this.datasheet = "";
        // Description of part.
        this.description = "";
        // Keywords to find part
        this.keywords = "";
        // Custom fields like manufacturer etc.
        this.customFields = {};
    }
    return Part;
}());
exports.Part = Part;
//# sourceMappingURL=part.js.map