"use strict";
exports.__esModule = true;
var Settings = /** @class */ (function () {
    function Settings() {
        this.customFields = [];
        this.paths = new SettingsPaths();
    }
    return Settings;
}());
exports.Settings = Settings;
var SettingsPaths = /** @class */ (function () {
    function SettingsPaths() {
        this.parts = '';
        this.symbol = '';
        this.footprint = '';
        this.output = '';
    }
    return SettingsPaths;
}());
exports.SettingsPaths = SettingsPaths;
//# sourceMappingURL=settings.js.map