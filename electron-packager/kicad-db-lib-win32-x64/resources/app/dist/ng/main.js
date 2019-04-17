(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["main"],{

/***/ "./src/$$_lazy_route_resource lazy recursive":
/*!**********************************************************!*\
  !*** ./src/$$_lazy_route_resource lazy namespace object ***!
  \**********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncaught exception popping up in devtools
	return Promise.resolve().then(function() {
		var e = new Error("Cannot find module '" + req + "'");
		e.code = 'MODULE_NOT_FOUND';
		throw e;
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "./src/$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "./src/app/about/about.component.css":
/*!*******************************************!*\
  !*** ./src/app/about/about.component.css ***!
  \*******************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Fib3V0L2Fib3V0LmNvbXBvbmVudC5jc3MifQ== */"

/***/ }),

/***/ "./src/app/about/about.component.html":
/*!********************************************!*\
  !*** ./src/app/about/about.component.html ***!
  \********************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<section class=\"mat-typography\">\r\n  <div class=\"global\">\r\n    <mat-toolbar class=\"mat-elevation-z6\" color=\"primary\">\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"back\"\r\n        mat-icon-button\r\n        (click)=\"goBack()\"\r\n      >\r\n        <mat-icon aria-label=\"back icon\">arrow_back</mat-icon>\r\n      </button>\r\n      <span>About</span>\r\n    </mat-toolbar>\r\n    <div class=\"global-content\">\r\n      <h2>KiCad Database Library</h2>\r\n      <mat-list>\r\n        <h3 matSubheader>Info</h3>\r\n        <mat-list-item> Version {{ version }} </mat-list-item>\r\n        <mat-list-item>\r\n          Licence: MIT\r\n        </mat-list-item>\r\n        <mat-list-item *ngIf=\"userData\">\r\n          Your user data is stored here: {{ userData }}</mat-list-item\r\n        >\r\n        <h3 matSubheader>Contact</h3>\r\n        <mat-list-item\r\n          (click)=\"externalLink('https://github.com/Projektanker/kicad-db-lib')\"\r\n        >\r\n          <mat-icon>launch</mat-icon>\r\n          <span>GitHub</span>\r\n        </mat-list-item>\r\n      </mat-list>\r\n    </div>\r\n  </div>\r\n</section>\r\n"

/***/ }),

/***/ "./src/app/about/about.component.ts":
/*!******************************************!*\
  !*** ./src/app/about/about.component.ts ***!
  \******************************************/
/*! exports provided: AboutComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AboutComponent", function() { return AboutComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var ngx_electron__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ngx-electron */ "./node_modules/ngx-electron/fesm5/ngx-electron.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");





var AboutComponent = /** @class */ (function () {
    function AboutComponent(location, router, electronService) {
        this.location = location;
        this.router = router;
        this.electronService = electronService;
    }
    AboutComponent.prototype.ngOnInit = function () {
        if (this.electronService.isElectronApp) {
            this.userData = this.electronService.remote.app.getPath('userData');
            this.version = this.electronService.remote.app.getVersion();
        }
    };
    AboutComponent.prototype.goBack = function () {
        this.location.back();
    };
    AboutComponent.prototype.externalLink = function (url) {
        if (this.electronService.isElectronApp) {
            this.electronService.shell.openExternal(url);
        }
        else {
            window.location.href = url;
        }
    };
    AboutComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-about',
            template: __webpack_require__(/*! ./about.component.html */ "./src/app/about/about.component.html"),
            styles: [__webpack_require__(/*! ./about.component.css */ "./src/app/about/about.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_angular_common__WEBPACK_IMPORTED_MODULE_2__["Location"],
            _angular_router__WEBPACK_IMPORTED_MODULE_4__["Router"],
            ngx_electron__WEBPACK_IMPORTED_MODULE_3__["ElectronService"]])
    ], AboutComponent);
    return AboutComponent;
}());



/***/ }),

/***/ "./src/app/app-routing.module.ts":
/*!***************************************!*\
  !*** ./src/app/app-routing.module.ts ***!
  \***************************************/
/*! exports provided: AppRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppRoutingModule", function() { return AppRoutingModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _parts_parts_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./parts/parts.component */ "./src/app/parts/parts.component.ts");
/* harmony import */ var _part_detail_part_detail_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./part-detail/part-detail.component */ "./src/app/part-detail/part-detail.component.ts");
/* harmony import */ var _settings_settings_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./settings/settings.component */ "./src/app/settings/settings.component.ts");
/* harmony import */ var _settings_fields_settings_fields_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./settings-fields/settings-fields.component */ "./src/app/settings-fields/settings-fields.component.ts");
/* harmony import */ var _settings_paths_settings_paths_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./settings-paths/settings-paths.component */ "./src/app/settings-paths/settings-paths.component.ts");
/* harmony import */ var _about_about_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./about/about.component */ "./src/app/about/about.component.ts");









var routes = [
    { path: '', redirectTo: '/parts', pathMatch: 'full' },
    { path: 'parts', component: _parts_parts_component__WEBPACK_IMPORTED_MODULE_3__["PartsComponent"] },
    { path: 'part/:id', component: _part_detail_part_detail_component__WEBPACK_IMPORTED_MODULE_4__["PartDetailComponent"] },
    { path: 'settings', component: _settings_settings_component__WEBPACK_IMPORTED_MODULE_5__["SettingsComponent"] },
    { path: 'settings/fields', component: _settings_fields_settings_fields_component__WEBPACK_IMPORTED_MODULE_6__["SettingsFieldsComponent"] },
    { path: 'settings/paths', component: _settings_paths_settings_paths_component__WEBPACK_IMPORTED_MODULE_7__["SettingsPathsComponent"] },
    { path: 'about', component: _about_about_component__WEBPACK_IMPORTED_MODULE_8__["AboutComponent"] }
];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["NgModule"])({
            imports: [_angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"].forRoot(routes)],
            exports: [_angular_router__WEBPACK_IMPORTED_MODULE_2__["RouterModule"]]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());



/***/ }),

/***/ "./src/app/app.component.css":
/*!***********************************!*\
  !*** ./src/app/app.component.css ***!
  \***********************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "/* AppComponent's private CSS styles */\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvYXBwLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUEsc0NBQXNDIiwiZmlsZSI6InNyYy9hcHAvYXBwLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIvKiBBcHBDb21wb25lbnQncyBwcml2YXRlIENTUyBzdHlsZXMgKi9cclxuIl19 */"

/***/ }),

/***/ "./src/app/app.component.html":
/*!************************************!*\
  !*** ./src/app/app.component.html ***!
  \************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<!--<app-nav></app-nav>-->\r\n<router-outlet></router-outlet>\r\n"

/***/ }),

/***/ "./src/app/app.component.ts":
/*!**********************************!*\
  !*** ./src/app/app.component.ts ***!
  \**********************************/
/*! exports provided: AppComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppComponent", function() { return AppComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var ngx_electron__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ngx-electron */ "./node_modules/ngx-electron/fesm5/ngx-electron.js");



var AppComponent = /** @class */ (function () {
    function AppComponent(electronService) {
        this.electronService = electronService;
        this.title = 'KiCad Database Library';
        console.log('Hello from AppComponent!');
        console.log("isElectronApp: " + this.electronService.isElectronApp);
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('test');
        }
    }
    AppComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-root',
            template: __webpack_require__(/*! ./app.component.html */ "./src/app/app.component.html"),
            styles: [__webpack_require__(/*! ./app.component.css */ "./src/app/app.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [ngx_electron__WEBPACK_IMPORTED_MODULE_2__["ElectronService"]])
    ], AppComponent);
    return AppComponent;
}());



/***/ }),

/***/ "./src/app/app.module.ts":
/*!*******************************!*\
  !*** ./src/app/app.module.ts ***!
  \*******************************/
/*! exports provided: AppModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppModule", function() { return AppModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/platform-browser */ "./node_modules/@angular/platform-browser/fesm5/platform-browser.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var ngx_electron__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ngx-electron */ "./node_modules/ngx-electron/fesm5/ngx-electron.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/fesm5/forms.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/fesm5/http.js");
/* harmony import */ var angular_in_memory_web_api__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! angular-in-memory-web-api */ "./node_modules/angular-in-memory-web-api/index.js");
/* harmony import */ var _in_memory_data_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./in-memory-data.service */ "./src/app/in-memory-data.service.ts");
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./app.component */ "./src/app/app.component.ts");
/* harmony import */ var _messages_messages_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./messages/messages.component */ "./src/app/messages/messages.component.ts");
/* harmony import */ var _app_routing_module__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./app-routing.module */ "./src/app/app-routing.module.ts");
/* harmony import */ var _angular_platform_browser_animations__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/platform-browser/animations */ "./node_modules/@angular/platform-browser/fesm5/animations.js");
/* harmony import */ var _angular_cdk_layout__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @angular/cdk/layout */ "./node_modules/@angular/cdk/esm5/layout.es5.js");
/* harmony import */ var _angular_material__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/material */ "./node_modules/@angular/material/esm5/material.es5.js");
/* harmony import */ var _parts_parts_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ./parts/parts.component */ "./src/app/parts/parts.component.ts");
/* harmony import */ var _part_detail_part_detail_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ./part-detail/part-detail.component */ "./src/app/part-detail/part-detail.component.ts");
/* harmony import */ var _settings_settings_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ./settings/settings.component */ "./src/app/settings/settings.component.ts");
/* harmony import */ var _keys_pipe__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ./keys.pipe */ "./src/app/keys.pipe.ts");
/* harmony import */ var _settings_fields_settings_fields_component__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! ./settings-fields/settings-fields.component */ "./src/app/settings-fields/settings-fields.component.ts");
/* harmony import */ var _discard_changes_dialog_discard_changes_dialog_component__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! ./discard-changes-dialog/discard-changes-dialog.component */ "./src/app/discard-changes-dialog/discard-changes-dialog.component.ts");
/* harmony import */ var _settings_paths_settings_paths_component__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(/*! ./settings-paths/settings-paths.component */ "./src/app/settings-paths/settings-paths.component.ts");
/* harmony import */ var _delete_dialog_delete_dialog_component__WEBPACK_IMPORTED_MODULE_21__ = __webpack_require__(/*! ./delete-dialog/delete-dialog.component */ "./src/app/delete-dialog/delete-dialog.component.ts");
/* harmony import */ var _about_about_component__WEBPACK_IMPORTED_MODULE_22__ = __webpack_require__(/*! ./about/about.component */ "./src/app/about/about.component.ts");




 // < -- NgModel lives here


















var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_2__["NgModule"])({
            declarations: [
                _app_component__WEBPACK_IMPORTED_MODULE_8__["AppComponent"],
                _messages_messages_component__WEBPACK_IMPORTED_MODULE_9__["MessagesComponent"],
                _parts_parts_component__WEBPACK_IMPORTED_MODULE_14__["PartsComponent"],
                _part_detail_part_detail_component__WEBPACK_IMPORTED_MODULE_15__["PartDetailComponent"],
                _settings_settings_component__WEBPACK_IMPORTED_MODULE_16__["SettingsComponent"],
                _keys_pipe__WEBPACK_IMPORTED_MODULE_17__["KeysPipe"],
                _settings_fields_settings_fields_component__WEBPACK_IMPORTED_MODULE_18__["SettingsFieldsComponent"],
                _discard_changes_dialog_discard_changes_dialog_component__WEBPACK_IMPORTED_MODULE_19__["DiscardChangesDialogComponent"],
                _settings_paths_settings_paths_component__WEBPACK_IMPORTED_MODULE_20__["SettingsPathsComponent"],
                _delete_dialog_delete_dialog_component__WEBPACK_IMPORTED_MODULE_21__["DeleteDialogComponent"],
                _about_about_component__WEBPACK_IMPORTED_MODULE_22__["AboutComponent"]
            ],
            imports: [
                _angular_platform_browser__WEBPACK_IMPORTED_MODULE_1__["BrowserModule"],
                ngx_electron__WEBPACK_IMPORTED_MODULE_3__["NgxElectronModule"],
                _angular_forms__WEBPACK_IMPORTED_MODULE_4__["FormsModule"],
                _app_routing_module__WEBPACK_IMPORTED_MODULE_10__["AppRoutingModule"],
                _angular_common_http__WEBPACK_IMPORTED_MODULE_5__["HttpClientModule"],
                // The HttpClientInMemoryWebApiModule module intercepts HTTP requests
                // and returns simulated server responses.
                // Remove it when a real server is ready to receive requests.
                angular_in_memory_web_api__WEBPACK_IMPORTED_MODULE_6__["HttpClientInMemoryWebApiModule"].forRoot(_in_memory_data_service__WEBPACK_IMPORTED_MODULE_7__["InMemoryDataService"], {
                    dataEncapsulation: false
                }),
                _angular_platform_browser_animations__WEBPACK_IMPORTED_MODULE_11__["BrowserAnimationsModule"],
                _angular_cdk_layout__WEBPACK_IMPORTED_MODULE_12__["LayoutModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatToolbarModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatButtonModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatSidenavModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatIconModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatListModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatTableModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatPaginatorModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatSortModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatInputModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatSelectModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatRadioModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatCardModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatExpansionModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatDialogModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatProgressSpinnerModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatProgressBarModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatAutocompleteModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatMenuModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatSnackBarModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_13__["MatTooltipModule"],
                _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ReactiveFormsModule"]
            ],
            entryComponents: [_discard_changes_dialog_discard_changes_dialog_component__WEBPACK_IMPORTED_MODULE_19__["DiscardChangesDialogComponent"], _delete_dialog_delete_dialog_component__WEBPACK_IMPORTED_MODULE_21__["DeleteDialogComponent"]],
            providers: [],
            bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_8__["AppComponent"]]
        })
    ], AppModule);
    return AppModule;
}());



/***/ }),

/***/ "./src/app/delete-dialog/delete-dialog.component.css":
/*!***********************************************************!*\
  !*** ./src/app/delete-dialog/delete-dialog.component.css ***!
  \***********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2RlbGV0ZS1kaWFsb2cvZGVsZXRlLWRpYWxvZy5jb21wb25lbnQuY3NzIn0= */"

/***/ }),

/***/ "./src/app/delete-dialog/delete-dialog.component.html":
/*!************************************************************!*\
  !*** ./src/app/delete-dialog/delete-dialog.component.html ***!
  \************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div mat-dialog-content><p>Delete?</p></div>\r\n<div mat-dialog-actions>\r\n  <button mat-button [mat-dialog-close]=\"true\">Yes</button>\r\n  <button mat-button [mat-dialog-close]=\"false\" cdkFocusInitial>No</button>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/delete-dialog/delete-dialog.component.ts":
/*!**********************************************************!*\
  !*** ./src/app/delete-dialog/delete-dialog.component.ts ***!
  \**********************************************************/
/*! exports provided: DeleteDialogComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DeleteDialogComponent", function() { return DeleteDialogComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var DeleteDialogComponent = /** @class */ (function () {
    function DeleteDialogComponent() {
    }
    DeleteDialogComponent.prototype.ngOnInit = function () {
    };
    DeleteDialogComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-delete-dialog',
            template: __webpack_require__(/*! ./delete-dialog.component.html */ "./src/app/delete-dialog/delete-dialog.component.html"),
            styles: [__webpack_require__(/*! ./delete-dialog.component.css */ "./src/app/delete-dialog/delete-dialog.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [])
    ], DeleteDialogComponent);
    return DeleteDialogComponent;
}());



/***/ }),

/***/ "./src/app/discard-changes-dialog/discard-changes-dialog.component.css":
/*!*****************************************************************************!*\
  !*** ./src/app/discard-changes-dialog/discard-changes-dialog.component.css ***!
  \*****************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL2Rpc2NhcmQtY2hhbmdlcy1kaWFsb2cvZGlzY2FyZC1jaGFuZ2VzLWRpYWxvZy5jb21wb25lbnQuY3NzIn0= */"

/***/ }),

/***/ "./src/app/discard-changes-dialog/discard-changes-dialog.component.html":
/*!******************************************************************************!*\
  !*** ./src/app/discard-changes-dialog/discard-changes-dialog.component.html ***!
  \******************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div mat-dialog-content><p>Discard changes?</p></div>\r\n<div mat-dialog-actions>\r\n  <button mat-button [mat-dialog-close]=\"true\">Yes</button>\r\n  <button mat-button [mat-dialog-close]=\"false\" cdkFocusInitial>No</button>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/discard-changes-dialog/discard-changes-dialog.component.ts":
/*!****************************************************************************!*\
  !*** ./src/app/discard-changes-dialog/discard-changes-dialog.component.ts ***!
  \****************************************************************************/
/*! exports provided: DiscardChangesDialogComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "DiscardChangesDialogComponent", function() { return DiscardChangesDialogComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var DiscardChangesDialogComponent = /** @class */ (function () {
    function DiscardChangesDialogComponent() {
    }
    DiscardChangesDialogComponent.prototype.ngOnInit = function () {
    };
    DiscardChangesDialogComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-discard-changes-dialog',
            template: __webpack_require__(/*! ./discard-changes-dialog.component.html */ "./src/app/discard-changes-dialog/discard-changes-dialog.component.html"),
            styles: [__webpack_require__(/*! ./discard-changes-dialog.component.css */ "./src/app/discard-changes-dialog/discard-changes-dialog.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [])
    ], DiscardChangesDialogComponent);
    return DiscardChangesDialogComponent;
}());



/***/ }),

/***/ "./src/app/in-memory-data.service.ts":
/*!*******************************************!*\
  !*** ./src/app/in-memory-data.service.ts ***!
  \*******************************************/
/*! exports provided: InMemoryDataService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InMemoryDataService", function() { return InMemoryDataService; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var InMemoryDataService = /** @class */ (function () {
    function InMemoryDataService() {
    }
    InMemoryDataService.prototype.createDb = function () {
        var heroes = [
            { id: 12, name: 'Narco' },
            { id: 13, name: 'Bombasto' },
            { id: 14, name: 'Celeritas' },
            { id: 15, name: 'Magneta' },
            { id: 16, name: 'RubberMan' },
            { id: 17, name: 'Dynama' },
            { id: 18, name: 'Dr IQ' },
            { id: 19, name: 'Magma' },
            { id: 20, name: 'Tornado' }
        ];
        var parts = [
            {
                id: 11,
                reference: 'R',
                value: '1K',
                footprint: 'Resistor_SMD:R_0603_1608Metric',
                library: 'R_0603',
                description: 'Resistor 1K 0603 75V',
                keywords: 'Res Resistor 1K 0603',
                symbol: '{lib}:R',
                datasheet: 'no datasheet',
                customFields: { OC_FARNELL: '123465', OC_MOUSER: 'm123465' }
            },
            {
                id: 12,
                reference: 'R',
                value: '10K',
                footprint: 'Resistor_SMD:R_0603_1608Metric',
                library: 'R_0603',
                description: 'Resistor 10K 0603 75V',
                keywords: 'Res Resistor 10K 0603',
                symbol: '{lib}:R',
                datasheet: 'no datasheet',
                customFields: {}
            },
            {
                id: 13,
                reference: 'R',
                value: '1K',
                footprint: 'Resistor_SMD:R_0805_2012Metric',
                library: 'R_0805',
                description: 'Resistor 1K 0603 150V',
                keywords: 'Res Resistor 1K 0603',
                symbol: '{lib}:R',
                datasheet: 'no datasheet',
                customFields: {}
            },
            {
                id: 14,
                reference: 'R',
                value: '10K',
                footprint: 'Resistor_SMD:R_0805_2012Metric',
                library: 'R_0805',
                description: 'Resistor 10K 0805 150V',
                keywords: 'Res Resistor 10K 0805',
                symbol: '{lib}:R',
                datasheet: 'no datasheet',
                customFields: {}
            },
            {
                id: 15,
                reference: 'C',
                value: '100n_50V',
                footprint: 'Capacitor_SMD:C_0603_1608Metric',
                library: 'C_0603',
                description: 'Capacitor 100nF 50V 0603',
                keywords: 'Cap Capacitor 100n 0603',
                symbol: '{lib}:C',
                datasheet: 'no datasheet',
                customFields: {}
            },
            {
                id: 16,
                reference: 'C',
                value: '10u_16V',
                footprint: 'Capacitor_SMD:C_0805_2012Metric',
                library: 'C_0805',
                description: 'Capacitor 10uF 0805 16V',
                keywords: 'Capacitor Capacitor 10u 0805',
                symbol: '{lib}:C',
                datasheet: 'no datasheet',
                customFields: {}
            }
        ];
        var libraries = ['R_0603', 'R_0805', 'C_0805', 'C_0603'].sort();
        var symbols = [
            'Devices:R',
            'Devices:C',
            'Devices:L',
            'Devices:P'
        ].sort();
        var footprints = ['F1', 'F2', 'F3', 'F4'].sort();
        return { heroes: heroes, parts: parts, libraries: libraries, symbols: symbols, footprints: footprints };
    };
    // Overrides the genId method to ensure that a hero always has an id.
    // If the heroes array is empty,
    // the method below returns the initial number (11).
    // if the heroes array is not empty, the method below returns the highest
    // hero id + 1.
    InMemoryDataService.prototype.genId = function (data) {
        return data.length > 0 ? Math.max.apply(Math, data.map(function (x) { return x.id; })) + 1 : 11;
    };
    InMemoryDataService = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Injectable"])({
            providedIn: 'root'
        })
    ], InMemoryDataService);
    return InMemoryDataService;
}());



/***/ }),

/***/ "./src/app/keys.pipe.ts":
/*!******************************!*\
  !*** ./src/app/keys.pipe.ts ***!
  \******************************/
/*! exports provided: KeysPipe */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "KeysPipe", function() { return KeysPipe; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var KeysPipe = /** @class */ (function () {
    function KeysPipe() {
    }
    KeysPipe.prototype.transform = function (value, args) {
        return Object.keys(value);
    };
    KeysPipe = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Pipe"])({
            name: 'keys'
        })
    ], KeysPipe);
    return KeysPipe;
}());



/***/ }),

/***/ "./src/app/library.service.ts":
/*!************************************!*\
  !*** ./src/app/library.service.ts ***!
  \************************************/
/*! exports provided: LibraryService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "LibraryService", function() { return LibraryService; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/fesm5/http.js");
/* harmony import */ var ngx_electron__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-electron */ "./node_modules/ngx-electron/fesm5/ngx-electron.js");





var LibraryService = /** @class */ (function () {
    function LibraryService(http, electronService, zone) {
        var _this = this;
        this.http = http;
        this.electronService = electronService;
        this.zone = zone;
        this.symbolsUrl = 'api/symbols'; // URL to web api
        this.footprintsUrl = 'api/footprints'; // URL to web api
        this.onSymbolsChangedSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.onFootprintsChangedSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.onBuildRunningSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.onBuildCompleteSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        this.onBuildErrorSubject = new rxjs__WEBPACK_IMPORTED_MODULE_2__["Subject"]();
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.on('library.getSymbols', function (event, arg) { return _this.ipcSymbolsChanged(event, arg); });
            this.electronService.ipcRenderer.on('library.getFootprints', function (event, arg) { return _this.ipcFootprintsChanged(event, arg); });
            this.electronService.ipcRenderer.on('library.build.running', function (event, arg) { return _this.ipcBuildRunning(event, arg); });
            this.electronService.ipcRenderer.on('library.build.complete', function (event, arg) { return _this.ipcBuildComplete(event, arg); });
            this.electronService.ipcRenderer.on('library.build.error', function (event, arg) { return _this.ipcBuildError(event, arg); });
        }
    }
    Object.defineProperty(LibraryService.prototype, "onSymbolsChanged", {
        get: function () {
            return this.onSymbolsChangedSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LibraryService.prototype, "onFootprintsChanged", {
        get: function () {
            return this.onFootprintsChangedSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LibraryService.prototype, "onBuildRunning", {
        get: function () {
            return this.onBuildRunningSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LibraryService.prototype, "onBuildComplete", {
        get: function () {
            return this.onBuildCompleteSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LibraryService.prototype, "onBuildError", {
        get: function () {
            return this.onBuildErrorSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    LibraryService.prototype.ipcSymbolsChanged = function (event, arg) {
        var _this = this;
        console.log('ipcSymbolsChanged(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onSymbolsChangedSubject.next(arg);
        });
    };
    LibraryService.prototype.ipcFootprintsChanged = function (event, arg) {
        var _this = this;
        console.log('ipcFootprintsChanged(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onFootprintsChangedSubject.next(arg);
        });
    };
    LibraryService.prototype.ipcBuildRunning = function (event, arg) {
        var _this = this;
        console.log('ipcBuildRunning(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onBuildRunningSubject.next(arg);
        });
    };
    LibraryService.prototype.ipcBuildComplete = function (event, arg) {
        var _this = this;
        console.log('ipcBuildComplete(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onBuildCompleteSubject.next(arg);
        });
    };
    LibraryService.prototype.ipcBuildError = function (event, arg) {
        var _this = this;
        console.log('ipcBuildError(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onBuildErrorSubject.next(arg);
        });
    };
    LibraryService.prototype.getSymbols = function (filter, reload, max) {
        var _this = this;
        console.log('getSymbols()');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('library.getSymbols', filter, reload, max);
        }
        else {
            this.http
                .get(this.symbolsUrl)
                .subscribe(function (next) { return _this.onSymbolsChangedSubject.next(next); }, function (error) { return _this.handleError(error); });
        }
    };
    LibraryService.prototype.getFootprints = function (filter, reload, max) {
        var _this = this;
        console.log('getFootprints()');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('library.getFootprints', filter, reload, max);
        }
        else {
            this.http
                .get(this.footprintsUrl)
                .subscribe(function (next) { return _this.onFootprintsChangedSubject.next(next); }, function (error) { return _this.handleError(error); });
        }
    };
    LibraryService.prototype.build = function () {
        console.log('build()');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('library.build');
        }
        else {
            this.ipcBuildError(null, new Error('Not implemented!'));
        }
    };
    LibraryService.prototype.handleError = function (error) {
        console.error(error);
    };
    LibraryService = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Injectable"])({
            providedIn: 'root'
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_angular_common_http__WEBPACK_IMPORTED_MODULE_3__["HttpClient"],
            ngx_electron__WEBPACK_IMPORTED_MODULE_4__["ElectronService"],
            _angular_core__WEBPACK_IMPORTED_MODULE_1__["NgZone"]])
    ], LibraryService);
    return LibraryService;
}());



/***/ }),

/***/ "./src/app/message.service.ts":
/*!************************************!*\
  !*** ./src/app/message.service.ts ***!
  \************************************/
/*! exports provided: MessageService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MessageService", function() { return MessageService; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var MessageService = /** @class */ (function () {
    function MessageService() {
        this.messages = [];
    }
    MessageService.prototype.add = function (message) {
        console.log(message);
        this.messages.push(message);
    };
    MessageService.prototype.clear = function () {
        this.messages = [];
    };
    MessageService = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Injectable"])({
            providedIn: "root"
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [])
    ], MessageService);
    return MessageService;
}());



/***/ }),

/***/ "./src/app/messages/messages.component.css":
/*!*************************************************!*\
  !*** ./src/app/messages/messages.component.css ***!
  \*************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".message-card {\r\n  min-width: 120px;\r\n  margin: 20px;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvbWVzc2FnZXMvbWVzc2FnZXMuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNFLGdCQUFnQjtFQUNoQixZQUFZO0FBQ2QiLCJmaWxlIjoic3JjL2FwcC9tZXNzYWdlcy9tZXNzYWdlcy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLm1lc3NhZ2UtY2FyZCB7XHJcbiAgbWluLXdpZHRoOiAxMjBweDtcclxuICBtYXJnaW46IDIwcHg7XHJcbn1cclxuIl19 */"

/***/ }),

/***/ "./src/app/messages/messages.component.html":
/*!**************************************************!*\
  !*** ./src/app/messages/messages.component.html ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<mat-card class=\"message-card\" *ngIf=\"messageService.messages.length\">\r\n  <mat-card-header>\r\n    <mat-card-title>Messages</mat-card-title>\r\n  </mat-card-header>\r\n  <mat-card-actions>\r\n    <button mat-raised-button (click)=\"messageService.clear()\">clear</button>\r\n  </mat-card-actions>\r\n  <mat-card-content>\r\n    <mat-list dense>\r\n      <mat-list-item *ngFor='let message of messageService.messages'> {{message}} </mat-list-item>\r\n    </mat-list>\r\n  </mat-card-content>\r\n</mat-card>\r\n"

/***/ }),

/***/ "./src/app/messages/messages.component.ts":
/*!************************************************!*\
  !*** ./src/app/messages/messages.component.ts ***!
  \************************************************/
/*! exports provided: MessagesComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MessagesComponent", function() { return MessagesComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _message_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../message.service */ "./src/app/message.service.ts");



var MessagesComponent = /** @class */ (function () {
    function MessagesComponent(messageService) {
        this.messageService = messageService;
    }
    MessagesComponent.prototype.ngOnInit = function () {
    };
    MessagesComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-messages',
            template: __webpack_require__(/*! ./messages.component.html */ "./src/app/messages/messages.component.html"),
            styles: [__webpack_require__(/*! ./messages.component.css */ "./src/app/messages/messages.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_message_service__WEBPACK_IMPORTED_MODULE_2__["MessageService"]])
    ], MessagesComponent);
    return MessagesComponent;
}());



/***/ }),

/***/ "./src/app/part-detail/part-detail.component.css":
/*!*******************************************************!*\
  !*** ./src/app/part-detail/part-detail.component.css ***!
  \*******************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".full-width {\r\n  width: 100%;\r\n}\r\n\r\n.part-card {\r\n  min-width: 120px;\r\n  margin: 20px;\r\n}\r\n\r\n.mat-radio-button {\r\n  display: block;\r\n  margin: 5px 0;\r\n}\r\n\r\n.row {\r\n  display: flex;\r\n  flex-direction: row;\r\n}\r\n\r\n.col {\r\n  flex: 1;\r\n  margin-right: 20px;\r\n}\r\n\r\n.col:last-child {\r\n  margin-right: 0;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvcGFydC1kZXRhaWwvcGFydC1kZXRhaWwuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNFLFdBQVc7QUFDYjs7QUFFQTtFQUNFLGdCQUFnQjtFQUNoQixZQUFZO0FBQ2Q7O0FBRUE7RUFDRSxjQUFjO0VBQ2QsYUFBYTtBQUNmOztBQUVBO0VBQ0UsYUFBYTtFQUNiLG1CQUFtQjtBQUNyQjs7QUFFQTtFQUNFLE9BQU87RUFDUCxrQkFBa0I7QUFDcEI7O0FBRUE7RUFDRSxlQUFlO0FBQ2pCIiwiZmlsZSI6InNyYy9hcHAvcGFydC1kZXRhaWwvcGFydC1kZXRhaWwuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5mdWxsLXdpZHRoIHtcclxuICB3aWR0aDogMTAwJTtcclxufVxyXG5cclxuLnBhcnQtY2FyZCB7XHJcbiAgbWluLXdpZHRoOiAxMjBweDtcclxuICBtYXJnaW46IDIwcHg7XHJcbn1cclxuXHJcbi5tYXQtcmFkaW8tYnV0dG9uIHtcclxuICBkaXNwbGF5OiBibG9jaztcclxuICBtYXJnaW46IDVweCAwO1xyXG59XHJcblxyXG4ucm93IHtcclxuICBkaXNwbGF5OiBmbGV4O1xyXG4gIGZsZXgtZGlyZWN0aW9uOiByb3c7XHJcbn1cclxuXHJcbi5jb2wge1xyXG4gIGZsZXg6IDE7XHJcbiAgbWFyZ2luLXJpZ2h0OiAyMHB4O1xyXG59XHJcblxyXG4uY29sOmxhc3QtY2hpbGQge1xyXG4gIG1hcmdpbi1yaWdodDogMDtcclxufVxyXG4iXX0= */"

/***/ }),

/***/ "./src/app/part-detail/part-detail.component.html":
/*!********************************************************!*\
  !*** ./src/app/part-detail/part-detail.component.html ***!
  \********************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<section class=\"mat-typography\">\r\n  <div class=\"global\">\r\n    <mat-toolbar class=\"mat-elevation-z6\" color=\"primary\">\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"back\"\r\n        mat-icon-button\r\n        (click)=\"goBack()\"\r\n      >\r\n        <mat-icon aria-label=\"back icon\">arrow_back</mat-icon>\r\n      </button>\r\n      <span>Part</span>\r\n      <span *ngIf=\"part\">\r\n        <span *ngIf=\"part.id\">&nbsp;{{ part.id }}</span>\r\n        <span *ngIf=\"part.value != ''\"> - {{ part.value }}</span>\r\n      </span>\r\n      <span class=\"spacer\"></span>\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"submit\"\r\n        matTooltip=\"Submit\"\r\n        mat-icon-button\r\n        [disabled]=\"!partForm?.valid\"\r\n        (click)=\"onSubmit()\"\r\n      >\r\n        <mat-icon aria-label=\"submit icon\">done</mat-icon>\r\n      </button>\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"delete\"\r\n        matTooltip=\"Delete part\"\r\n        mat-icon-button\r\n        [disabled]=\"add\"\r\n        (click)=\"delete()\"\r\n      >\r\n        <mat-icon aria-label=\"delete icon\">delete</mat-icon>\r\n      </button>\r\n      <button\r\n        mat-icon-button\r\n        matTooltip=\"Duplicate part\"\r\n        type=\"button\"\r\n        aria-label=\"duplicate\"\r\n        [disabled]=\"add\"\r\n        (click)=\"duplicate()\"\r\n      >\r\n        <mat-icon aria-label=\"duplicate icon\">control_point_duplicate</mat-icon>\r\n      </button>\r\n    </mat-toolbar>\r\n    <div class=\"global-progress-container\">\r\n      <mat-progress-bar\r\n        *ngIf=\"!partForm\"\r\n        color=\"accent\"\r\n        mode=\"indeterminate\"\r\n      ></mat-progress-bar>\r\n    </div>\r\n    <div class=\"global-content\">\r\n      <form *ngIf=\"partForm\" [formGroup]=\"partForm\" (ngSubmit)=\"onSubmit()\">\r\n        <mat-divider></mat-divider>\r\n        <h3>Basic fields</h3>\r\n        <div\r\n          *ngFor=\"\r\n            let field of (partForm.controls | keys);\r\n            first as isFirst;\r\n            last as isLast;\r\n            index as i\r\n          \"\r\n        >\r\n          <mat-form-field *ngIf=\"!(isLast || isFirst)\" class=\"full-width\">\r\n            <input\r\n              matInput\r\n              placeholder=\"{{ field | uppercase }}{{ i > 4 ? '' : '*' }}\"\r\n              [formControlName]=\"field\"\r\n              [readonly]=\"isFirst\"\r\n              [matAutocomplete]=\"auto\"\r\n            />\r\n            <mat-error *ngIf=\"partForm.controls[field].hasError('required')\">\r\n              {{ field | uppercase }} is <strong>required</strong>\r\n            </mat-error>\r\n            <mat-error *ngIf=\"partForm.controls[field].hasError('pattern')\">\r\n              {{ field | uppercase }} contains\r\n              <strong>invalid characters</strong>\r\n            </mat-error>\r\n            <mat-autocomplete\r\n              autoActiveFirstOption\r\n              #auto=\"matAutocomplete\"\r\n              [ngSwitch]=\"field\"\r\n            >\r\n              <ng-container *ngSwitchCase=\"'library'\">\r\n                <mat-option *ngFor=\"let x of libraries\" [value]=\"x\">\r\n                  {{ x }}\r\n                </mat-option>\r\n              </ng-container>\r\n              <ng-container *ngSwitchCase=\"'symbol'\">\r\n                <mat-option *ngFor=\"let x of symbols\" [value]=\"x\">\r\n                  {{ x }}\r\n                </mat-option>\r\n              </ng-container>\r\n              <ng-container *ngSwitchCase=\"'footprint'\">\r\n                <mat-option *ngFor=\"let x of footprints\" [value]=\"x\">\r\n                  {{ x }}\r\n                </mat-option>\r\n              </ng-container>\r\n              <ng-container *ngSwitchDefault> </ng-container>\r\n            </mat-autocomplete>\r\n          </mat-form-field>\r\n        </div>\r\n\r\n        <mat-divider></mat-divider>\r\n\r\n        <h3>Custom Fields</h3>\r\n        <div\r\n          *ngFor=\"\r\n            let field of (partForm.get('customFields').controls | keys);\r\n            first as isFirst;\r\n            last as isLast\r\n          \"\r\n        >\r\n          <mat-form-field formGroupName=\"customFields\" class=\"full-width\">\r\n            <input\r\n              matInput\r\n              [placeholder]=\"field | uppercase\"\r\n              [formControlName]=\"field\"\r\n            />\r\n          </mat-form-field>\r\n        </div>\r\n      </form>\r\n    </div>\r\n  </div>\r\n</section>\r\n"

/***/ }),

/***/ "./src/app/part-detail/part-detail.component.ts":
/*!******************************************************!*\
  !*** ./src/app/part-detail/part-detail.component.ts ***!
  \******************************************************/
/*! exports provided: PartDetailComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PartDetailComponent", function() { return PartDetailComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/fesm5/forms.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var _part_service__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../part.service */ "./src/app/part.service.ts");
/* harmony import */ var _part_part__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../part/part */ "./src/app/part/part.ts");
/* harmony import */ var _settings_service__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../settings.service */ "./src/app/settings.service.ts");
/* harmony import */ var _angular_material__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material */ "./node_modules/@angular/material/esm5/material.es5.js");
/* harmony import */ var _discard_changes_dialog_discard_changes_dialog_component__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ../discard-changes-dialog/discard-changes-dialog.component */ "./src/app/discard-changes-dialog/discard-changes-dialog.component.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var _delete_dialog_delete_dialog_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ../delete-dialog/delete-dialog.component */ "./src/app/delete-dialog/delete-dialog.component.ts");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm5/operators/index.js");
/* harmony import */ var _library_service__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ../library.service */ "./src/app/library.service.ts");














var PartDetailComponent = /** @class */ (function () {
    function PartDetailComponent(route, partService, libraryService, settingsService, location, dialog) {
        this.route = route;
        this.partService = partService;
        this.libraryService = libraryService;
        this.settingsService = settingsService;
        this.location = location;
        this.dialog = dialog;
        this.part = null;
        this.add = false;
        this.settings = null;
        this.libraries = [];
        this.filteredLibraries$ = null;
        this.symbols = [];
        this.filteredSymbols$ = null;
        this.footprints = [];
        this.filteredFootprints$ = null;
        this.maxItems = 100;
        this.subscription = new rxjs__WEBPACK_IMPORTED_MODULE_10__["Subscription"]();
        console.log('constructor');
    }
    PartDetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        // settings - onSettingsChanged
        var sub = this.settingsService.onSettingsChanged.subscribe(function (next) {
            console.log('onSettingsChanged: next');
            _this.onSettingsChanged(next);
        }, function (error) {
            console.log('onSettingsChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onSettingsChanged: complete'); });
        this.subscription.add(sub);
        // parts - onPartChanged
        var sub = this.partService.onPartChanged.subscribe(function (next) {
            console.log('onPartChanged: next');
            _this.onPartChanged(next);
        }, function (error) {
            console.log('onPartChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onPartChanged: complete'); });
        this.subscription.add(sub);
        // parts - onLibrariesChanged
        var sub = this.partService.onLibrariesChanged.subscribe(function (next) {
            console.log('onLibrariesChanged: next');
            _this.onLibrariesChanged(next);
        }, function (error) {
            console.log('onLibrariesChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onLibrariesChanged: complete'); });
        this.subscription.add(sub);
        // library - onSymbolsChanged
        var sub = this.libraryService.onSymbolsChanged.subscribe(function (next) {
            console.log('onSymbolsChanged: next');
            _this.onSymbolsChanged(next);
        }, function (error) {
            console.log('onSymbolsChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onSymbolsChanged: complete'); });
        this.subscription.add(sub);
        // library - onFootprintsChanged
        var sub = this.libraryService.onFootprintsChanged.subscribe(function (next) {
            console.log('onFootprintsChanged: next');
            _this.onFootprintsChanged(next);
        }, function (error) {
            console.log('onFootprintsChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onFootprintsChanged: complete'); });
        this.subscription.add(sub);
        this.getPart();
        this.getSettings();
        this.getLibraries();
        this.getSymbols();
        this.getFootprints();
    };
    PartDetailComponent.prototype.ngOnDestroy = function () {
        console.log('ngOnDestroy');
        this.subscription.unsubscribe();
    };
    PartDetailComponent.prototype.onSettingsChanged = function (settings) {
        console.log('onSettingsChanged(settings: Settings)');
        console.log(settings);
        this.settings = settings;
        this.initForm();
    };
    PartDetailComponent.prototype.onPartChanged = function (part) {
        console.log('onPartChanged(part: Part)');
        console.log(part);
        this.part = part;
        this.initForm();
    };
    PartDetailComponent.prototype.onLibrariesChanged = function (libraries) {
        console.log('onLibrariesChanged(libraries: string[])');
        console.log(libraries);
        if (libraries.length == this.maxItems) {
            libraries.push('...');
        }
        this.libraries = libraries;
    };
    PartDetailComponent.prototype.onSymbolsChanged = function (symbols) {
        console.log('onSymbolsChanged(symbols: string[])');
        console.log(symbols);
        if (symbols.length == this.maxItems) {
            symbols.push('...');
        }
        this.symbols = symbols;
    };
    PartDetailComponent.prototype.onFootprintsChanged = function (footprints) {
        console.log('onFootprintsChanged(footprints: string[])');
        console.log(footprints);
        if (footprints.length == this.maxItems) {
            footprints.push('...');
        }
        this.footprints = footprints;
    };
    PartDetailComponent.prototype.handleError = function (error) {
        console.error(error);
    };
    PartDetailComponent.prototype.getPart = function () {
        var idString = this.route.snapshot.paramMap.get('id');
        if (idString == 'new') {
            this.add = true;
            this.onPartChanged(new _part_part__WEBPACK_IMPORTED_MODULE_6__["Part"]());
        }
        else {
            var id = +idString;
            this.partService.getPart(id);
        }
    };
    PartDetailComponent.prototype.getSettings = function () {
        this.settingsService.getSettings();
    };
    PartDetailComponent.prototype.getLibraries = function (filter, reload, max) {
        if (filter === void 0) { filter = ''; }
        if (reload === void 0) { reload = true; }
        this.partService.getLibraries(filter, reload, max ? max : this.maxItems);
    };
    PartDetailComponent.prototype.getSymbols = function (filter, reload, max) {
        if (filter === void 0) { filter = ''; }
        if (reload === void 0) { reload = true; }
        this.libraryService.getSymbols(filter, reload, max ? max : this.maxItems);
    };
    PartDetailComponent.prototype.getFootprints = function (filter, reload, max) {
        if (filter === void 0) { filter = ''; }
        if (reload === void 0) { reload = true; }
        this.libraryService.getFootprints(filter, reload, max ? max : this.maxItems);
    };
    PartDetailComponent.prototype.initForm = function () {
        var _this = this;
        if (!this.part)
            return;
        if (!this.settings)
            return;
        //if (!this.libraries) return;
        //if (!this.footprints) return;
        //if (!this.symbols) return;
        var pattern_library = '^[a-zA-Z0-9_\\-\\.:]*$';
        var pattern_default = '^[a-zA-Z0-9_\\-\\.:\\~\\*\\?]*$';
        var patter_value = '^[a-zA-Z0-9_\\-\\.,]*$';
        var customFieldsGroup = new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroup"]({});
        this.settings.customFields.forEach(function (field) {
            customFieldsGroup.addControl(field, new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''));
        });
        this.partForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroup"]({
            id: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](null),
            library: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required,
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].pattern(pattern_library)
            ]),
            reference: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required,
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].pattern('^[A-Z#]*$')
            ]),
            value: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required,
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].pattern(patter_value)
            ]),
            symbol: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', [
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required,
                _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].pattern(pattern_default)
            ]),
            footprint: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].pattern(pattern_default)),
            description: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
            keywords: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
            datasheet: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](''),
            customFields: customFieldsGroup
        });
        this.subscription.add(this.partForm.controls['library'].valueChanges
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["startWith"])(this.part.library), 
        // wait 300ms after each keystroke before considering the term
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["debounceTime"])(300), 
        // To lower case and trim
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["map"])(function (value) { return value.toLowerCase().trim(); }), 
        // ignore new term if same as previous term
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["distinctUntilChanged"])(), 
        // filter
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["tap"])(function (x) { return _this.getLibraries(x, false); }))
            .subscribe());
        this.subscription.add(this.partForm.controls['symbol'].valueChanges
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["startWith"])(this.part.symbol), 
        // wait 300ms after each keystroke before considering the term
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["debounceTime"])(300), 
        // To lower case and trim
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["map"])(function (value) { return value.toUpperCase().trim(); }), 
        // ignore new term if same as previous term
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["distinctUntilChanged"])(), 
        // filter
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["tap"])(function (x) { return _this.getSymbols(x, false); }))
            .subscribe());
        this.subscription.add(this.partForm.controls['footprint'].valueChanges
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["startWith"])(this.part.footprint), 
        // wait 300ms after each keystroke before considering the term
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["debounceTime"])(300), 
        // trim
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["map"])(function (value) { return value.trim(); }), 
        // ignore new term if same as previous term
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["distinctUntilChanged"])(), 
        // filter
        Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_12__["tap"])(function (x) { return _this.getFootprints(x, false); }))
            .subscribe());
        console.log(this.partForm.value);
        this.partForm.patchValue(this.part);
    };
    PartDetailComponent.prototype.onSubmit = function () {
        console.log('onSubmit()');
        console.log(this.partForm.value);
        this.part = this.partForm.value;
        if (this.add) {
            this.partService.addPart(this.part);
            this.location.back();
        }
        else {
            this.partService.updatePart(this.part);
            this.location.back();
        }
    };
    PartDetailComponent.prototype.delete = function () {
        var _this = this;
        console.log('delete()');
        var dialogRef = this.dialog.open(_delete_dialog_delete_dialog_component__WEBPACK_IMPORTED_MODULE_11__["DeleteDialogComponent"]);
        dialogRef.afterClosed().subscribe(function (result) {
            console.log("The dialog was closed " + result);
            if (result) {
                _this.partService.deletePart(_this.part);
                _this.location.back();
            }
        });
    };
    PartDetailComponent.prototype.duplicate = function () {
        console.log('duplicate()');
        this.add = true;
        this.part = null;
        this.partForm.controls['id'].setValue(null);
        this.partForm.markAsDirty();
    };
    PartDetailComponent.prototype.goBack = function () {
        var _this = this;
        if (!this.partForm.dirty) {
            this.location.back();
        }
        else {
            var dialogRef = this.dialog.open(_discard_changes_dialog_discard_changes_dialog_component__WEBPACK_IMPORTED_MODULE_9__["DiscardChangesDialogComponent"]);
            dialogRef.afterClosed().subscribe(function (result) {
                console.log("The dialog was closed " + result);
                if (result) {
                    _this.location.back();
                }
            });
        }
    };
    PartDetailComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-part-detail',
            template: __webpack_require__(/*! ./part-detail.component.html */ "./src/app/part-detail/part-detail.component.html"),
            styles: [__webpack_require__(/*! ./part-detail.component.css */ "./src/app/part-detail/part-detail.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_angular_router__WEBPACK_IMPORTED_MODULE_3__["ActivatedRoute"],
            _part_service__WEBPACK_IMPORTED_MODULE_5__["PartService"],
            _library_service__WEBPACK_IMPORTED_MODULE_13__["LibraryService"],
            _settings_service__WEBPACK_IMPORTED_MODULE_7__["SettingsService"],
            _angular_common__WEBPACK_IMPORTED_MODULE_4__["Location"],
            _angular_material__WEBPACK_IMPORTED_MODULE_8__["MatDialog"]])
    ], PartDetailComponent);
    return PartDetailComponent;
}());



/***/ }),

/***/ "./src/app/part.service.ts":
/*!*********************************!*\
  !*** ./src/app/part.service.ts ***!
  \*********************************/
/*! exports provided: PartService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PartService", function() { return PartService; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common/http */ "./node_modules/@angular/common/fesm5/http.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var ngx_electron__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-electron */ "./node_modules/ngx-electron/fesm5/ngx-electron.js");





var httpOptions = {
    headers: new _angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpHeaders"]({ 'Content-Type': 'application/json' })
};
var PartService = /** @class */ (function () {
    function PartService(http, electronService, zone) {
        var _this = this;
        this.http = http;
        this.electronService = electronService;
        this.zone = zone;
        this.partsUrl = 'api/parts'; // URL to web api
        this.librariesUrl = 'api/libraries'; // URL to web api
        this.onPartChangedSubject = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this.onPartsChangedSubject = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        this.onLibrariesChangedSubject = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.on('part.partsChanged', function (event, arg) { return _this.ipcPartsChanged(event, arg); });
            this.electronService.ipcRenderer.on('part.partChanged', function (event, arg) { return _this.ipcPartChanged(event, arg); });
            this.electronService.ipcRenderer.on('part.getLibraries', function (event, arg) { return _this.ipcLibrariesChanged(event, arg); });
        }
    }
    Object.defineProperty(PartService.prototype, "onPartChanged", {
        get: function () {
            return this.onPartChangedSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartService.prototype, "onPartsChanged", {
        get: function () {
            return this.onPartsChangedSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PartService.prototype, "onLibrariesChanged", {
        get: function () {
            return this.onLibrariesChangedSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    PartService.prototype.ipcPartsChanged = function (event, arg) {
        var _this = this;
        console.log('ipcPartsChanged(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onPartsChangedSubject.next(arg);
        });
    };
    PartService.prototype.ipcPartChanged = function (event, arg) {
        var _this = this;
        console.log('ipcPartChanged(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onPartChangedSubject.next(arg);
        });
    };
    PartService.prototype.ipcLibrariesChanged = function (event, arg) {
        var _this = this;
        console.log('ipcLibrariesChanged(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.onLibrariesChangedSubject.next(arg);
        });
    };
    PartService.prototype.getParts = function (sortActive, sortDirection) {
        var _this = this;
        if (sortActive === void 0) { sortActive = ''; }
        if (sortDirection === void 0) { sortDirection = ''; }
        console.log("getParts('" + sortActive + "','" + sortDirection + "')");
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('part.getParts', sortActive, sortDirection);
        }
        else {
            this.http
                .get(this.partsUrl)
                .subscribe(function (next) { return _this.onPartsChangedSubject.next(next); }, function (error) { return _this.handleError(error); });
        }
    };
    /** GET part by id. Will 404 if id not found */
    PartService.prototype.getPart = function (id) {
        var _this = this;
        console.log("getPart(" + id + ")");
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('part.getPart', id);
        }
        else {
            var url = this.partsUrl + "/" + id;
            this.http
                .get(url)
                .subscribe(function (next) { return _this.onPartChangedSubject.next(next); }, function (error) { return _this.handleError(error); });
        }
    };
    /** PUT: update the part on the server */
    PartService.prototype.updatePart = function (part) {
        var _this = this;
        console.log('updatePart(part: Part)');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('part.updatePart', part);
        }
        else {
            this.http
                .put(this.partsUrl, part, httpOptions)
                .subscribe(function () { }, function (error) { return _this.handleError(error); });
        }
    };
    /** POST: add a new part to the server */
    PartService.prototype.addPart = function (part) {
        var _this = this;
        console.log('addPart(part: Part)');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('part.addPart', part);
        }
        else {
            this.http
                .post(this.partsUrl, part, httpOptions)
                .subscribe(function () { }, function (error) { return _this.handleError(error); });
        }
    };
    /** DELETE: delete the part from the server */
    PartService.prototype.deletePart = function (part) {
        var _this = this;
        console.log('deletePart(part: Part | number)');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('part.deletePart', part);
        }
        else {
            var id = typeof part === 'number' ? part : part.id;
            var url = this.partsUrl + "/" + id;
            this.http
                .delete(url, httpOptions)
                .subscribe(function () { }, function (error) { return _this.handleError(error); });
        }
    };
    PartService.prototype.getLibraries = function (filter, reload, max) {
        var _this = this;
        console.log('getLibraries()');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('part.getLibraries', filter, reload, max);
        }
        else {
            this.http
                .get(this.librariesUrl)
                .subscribe(function (next) { return _this.onLibrariesChangedSubject.next(next); }, function (error) { return _this.handleError(error); });
        }
    };
    PartService.prototype.handleError = function (error) {
        console.error(error);
    };
    PartService = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Injectable"])({
            providedIn: 'root'
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_angular_common_http__WEBPACK_IMPORTED_MODULE_2__["HttpClient"],
            ngx_electron__WEBPACK_IMPORTED_MODULE_4__["ElectronService"],
            _angular_core__WEBPACK_IMPORTED_MODULE_1__["NgZone"]])
    ], PartService);
    return PartService;
}());



/***/ }),

/***/ "./src/app/part/part.ts":
/*!******************************!*\
  !*** ./src/app/part/part.ts ***!
  \******************************/
/*! exports provided: Part */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Part", function() { return Part; });
var Part = /** @class */ (function () {
    function Part() {
        this.id = 0;
        // Library the part have be stored in. <see cref="Value"/> must be unique per library.
        this.library = '';
        // Schematic reference. E.g. "R" for resistors
        this.reference = '';
        // Part value. E.g. "10K". Must be unique per <see cref="Library"/>
        this.value = '';
        // Reference to symbol. ../lib1.lib:symbol1
        this.symbol = '';
        // Footprint of part. Have to match KiCad footprint libraries. E.g. "Resistor_SMD:R_0603_1608Metric"
        this.footprint = '';
        // Description of part.
        this.description = '';
        // Datasheet link
        this.datasheet = '';
        // Keywords to find part
        this.keywords = '';
        // Custom fields like manufacturer etc.
        this.customFields = {};
    }
    return Part;
}());



/***/ }),

/***/ "./src/app/parts/parts-datasource.ts":
/*!*******************************************!*\
  !*** ./src/app/parts/parts-datasource.ts ***!
  \*******************************************/
/*! exports provided: PartsDataSource */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PartsDataSource", function() { return PartsDataSource; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_cdk_collections__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/cdk/collections */ "./node_modules/@angular/cdk/esm5/collections.es5.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm5/operators/index.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var _part_part__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../part/part */ "./src/app/part/part.ts");





/**
 * Data source for the Parts view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
var PartsDataSource = /** @class */ (function (_super) {
    tslib__WEBPACK_IMPORTED_MODULE_0__["__extends"](PartsDataSource, _super);
    function PartsDataSource(partService) {
        var _this = _super.call(this) || this;
        _this.partService = partService;
        _this.data = { length: 0 };
        /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
        _this.displayedColumns = Object.keys(new _part_part__WEBPACK_IMPORTED_MODULE_4__["Part"]()).slice(0, -1);
        _this.subscription = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subscription"]();
        _this.partsSubject = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"]([]
        /*[
        {
          id: 11,
          reference: 'R',
          value: '1K',
          footprint: 'Resistor_SMD:R_0603_1608Metric',
          library: 'R_0603',
          description: 'Resistor 1K 0603 75V',
          keywords: 'Res Resistor 1K 0603',
          symbol: '{lib}:R',
          datasheet: 'no datasheet',
          customFields: { OC_FARNELL: '123465', OC_MOUSER: 'm123465' }
        }
      ]*/
        );
        _this.loadingSubject = new rxjs__WEBPACK_IMPORTED_MODULE_3__["BehaviorSubject"](false);
        _this.loading$ = _this.loadingSubject.asObservable();
        _this.partsSubject.subscribe(function (parts) {
            console.log(parts);
        });
        return _this;
    }
    /**
     * Connect this data source to the table. The table will only update when
     * the returned stream emits new items.
     * @returns A stream of the items to be rendered.
     */
    PartsDataSource.prototype.connect = function () {
        var _this = this;
        var sub = this.partService.onPartsChanged
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["catchError"])(function (error) {
            console.error(error);
            return Object(rxjs__WEBPACK_IMPORTED_MODULE_3__["of"])([]);
        }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["map"])(function (parts) { return parts.map(function (part) { return _this.mapPart(part); }); }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_2__["tap"])(function (parts) {
            console.log("tap: " + parts.length);
            _this.data.length = parts.length;
            _this.loadingSubject.next(false);
        }))
            .subscribe(function (parts) {
            console.log("next: " + parts);
            _this.partsSubject.next(parts);
        });
        this.subscription.add(sub);
        return this.partsSubject.asObservable();
    };
    PartsDataSource.prototype.mapPart = function (part) {
        if (part.datasheet) {
            var start = part.datasheet.indexOf('\\') >= 0 ? '..\\' : '../';
            part.datasheet = start + part.datasheet.split(/(\\|\/)/g).pop();
        }
        var item = {};
        for (var key in part) {
            if (part.hasOwnProperty(key)) {
                if (key !== 'customFields') {
                    item[key] = part[key];
                }
                else {
                    for (var customField in part.customFields) {
                        if (part.customFields.hasOwnProperty(customField)) {
                            item[customField] = part.customFields[customField];
                            // add to displayed columns
                            if (this.displayedColumns.indexOf(customField) < 0) {
                                this.displayedColumns = this.displayedColumns.concat([customField]);
                            }
                        }
                    }
                }
            }
        }
        return item;
    };
    PartsDataSource.prototype.loadParts = function (sortActive, sortDirection) {
        if (sortActive === void 0) { sortActive = ''; }
        if (sortDirection === void 0) { sortDirection = ''; }
        console.log("sort active: " + sortActive);
        console.log("sort direction: " + sortDirection);
        this.loadingSubject.next(true);
        this.partService.getParts(sortActive, sortDirection);
    };
    /**
     *  Called when the table is being destroyed. Use this function, to clean up
     * any open connections or free any held resources that were set up during connect.
     */
    PartsDataSource.prototype.disconnect = function () {
        this.partsSubject.complete();
        this.loadingSubject.complete();
        this.subscription.unsubscribe();
    };
    return PartsDataSource;
}(_angular_cdk_collections__WEBPACK_IMPORTED_MODULE_1__["DataSource"]));



/***/ }),

/***/ "./src/app/parts/parts.component.css":
/*!*******************************************!*\
  !*** ./src/app/parts/parts.component.css ***!
  \*******************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".full-width-table {\r\n  width: 100%;\r\n  margin-top: 20px;\r\n  margin-bottom: 20px;\r\n}\r\n\r\ntd.mat-cell {\r\n  padding-right: 20px;\r\n}\r\n\r\nth.mat-header-cell {\r\n  padding-right: 20px;\r\n}\r\n\r\n.content {\r\n  padding: 0px;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvcGFydHMvcGFydHMuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNFLFdBQVc7RUFDWCxnQkFBZ0I7RUFDaEIsbUJBQW1CO0FBQ3JCOztBQUVBO0VBQ0UsbUJBQW1CO0FBQ3JCOztBQUVBO0VBQ0UsbUJBQW1CO0FBQ3JCOztBQUVBO0VBQ0UsWUFBWTtBQUNkIiwiZmlsZSI6InNyYy9hcHAvcGFydHMvcGFydHMuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5mdWxsLXdpZHRoLXRhYmxlIHtcclxuICB3aWR0aDogMTAwJTtcclxuICBtYXJnaW4tdG9wOiAyMHB4O1xyXG4gIG1hcmdpbi1ib3R0b206IDIwcHg7XHJcbn1cclxuXHJcbnRkLm1hdC1jZWxsIHtcclxuICBwYWRkaW5nLXJpZ2h0OiAyMHB4O1xyXG59XHJcblxyXG50aC5tYXQtaGVhZGVyLWNlbGwge1xyXG4gIHBhZGRpbmctcmlnaHQ6IDIwcHg7XHJcbn1cclxuXHJcbi5jb250ZW50IHtcclxuICBwYWRkaW5nOiAwcHg7XHJcbn1cclxuIl19 */"

/***/ }),

/***/ "./src/app/parts/parts.component.html":
/*!********************************************!*\
  !*** ./src/app/parts/parts.component.html ***!
  \********************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"global\">\r\n  <mat-toolbar class=\"mat-elevation-z6\" color=\"primary\">\r\n    <mat-toolbar-row>\r\n      <span>Parts</span> <span class=\"spacer\"></span>\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"add\"\r\n        mat-icon-button\r\n        (click)=\"addPart()\"\r\n      >\r\n        <mat-icon aria-label=\"add icon\">add</mat-icon>\r\n      </button>\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"settings\"\r\n        mat-icon-button\r\n        routerLink=\"/settings\"\r\n      >\r\n        <mat-icon aria-label=\"settings icon\">settings</mat-icon>\r\n      </button>\r\n      <button mat-icon-button [matMenuTriggerFor]=\"menu\">\r\n        <mat-icon>more_vert</mat-icon>\r\n      </button>\r\n      <mat-menu #menu=\"matMenu\">\r\n        <button mat-menu-item (click)=\"refresh()\">\r\n          <mat-icon>refresh</mat-icon>\r\n          <span>Refresh</span>\r\n        </button>\r\n        <button mat-menu-item (click)=\"build()\">\r\n          <mat-icon>build</mat-icon>\r\n          <span>Build</span>\r\n        </button>\r\n        <button mat-menu-item routerLink=\"/about\">\r\n          <mat-icon>info</mat-icon>\r\n          <span>About</span>\r\n        </button>\r\n      </mat-menu>\r\n    </mat-toolbar-row>\r\n  </mat-toolbar>\r\n  <div class=\"global-progress-container\">\r\n    <mat-progress-bar\r\n      *ngIf=\"(dataSource.loading$ | async)\"\r\n      color=\"accent\"\r\n      mode=\"indeterminate\"\r\n    ></mat-progress-bar>\r\n  </div>\r\n  <div class=\"global-content content\">\r\n    <table\r\n      mat-table\r\n      class=\"full-width-table\"\r\n      [dataSource]=\"dataSource\"\r\n      matSort\r\n      [matSortDirection]=\"sortDirection\"\r\n      [matSortActive]=\"sortActive\"\r\n      aria-label=\"Elements\"\r\n    >\r\n      <ng-container\r\n        *ngFor=\"let col of dataSource.displayedColumns\"\r\n        matColumnDef=\"{{ col }}\"\r\n      >\r\n        <th mat-header-cell *matHeaderCellDef mat-sort-header>\r\n          {{ col | uppercase }}\r\n        </th>\r\n        <td mat-cell *matCellDef=\"let element\">{{ element[col] }}</td>\r\n      </ng-container>\r\n\r\n      <tr\r\n        mat-header-row\r\n        *matHeaderRowDef=\"dataSource.displayedColumns; sticky: true\"\r\n      ></tr>\r\n      <tr\r\n        mat-row\r\n        *matRowDef=\"let row; columns: dataSource.displayedColumns\"\r\n        (click)=\"onRowClicked(row)\"\r\n      ></tr>\r\n    </table>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/parts/parts.component.ts":
/*!******************************************!*\
  !*** ./src/app/parts/parts.component.ts ***!
  \******************************************/
/*! exports provided: PartsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PartsComponent", function() { return PartsComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_material__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/material */ "./node_modules/@angular/material/esm5/material.es5.js");
/* harmony import */ var _parts_datasource__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./parts-datasource */ "./src/app/parts/parts-datasource.ts");
/* harmony import */ var _part_service__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../part.service */ "./src/app/part.service.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _library_service__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../library.service */ "./src/app/library.service.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var rxjs_operators__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! rxjs/operators */ "./node_modules/rxjs/_esm5/operators/index.js");









var PartsComponent = /** @class */ (function () {
    function PartsComponent(partService, libraryService, router, snackBar) {
        this.partService = partService;
        this.libraryService = libraryService;
        this.router = router;
        this.snackBar = snackBar;
        this.subscription = new rxjs__WEBPACK_IMPORTED_MODULE_7__["Subscription"]();
        this.sortActive = PartsComponent_1.sortActive;
        this.sortDirection = PartsComponent_1.sortDirection;
    }
    PartsComponent_1 = PartsComponent;
    PartsComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        this.dataSource = new _parts_datasource__WEBPACK_IMPORTED_MODULE_3__["PartsDataSource"](this.partService);
        this.refresh();
        this.subscription.add(this.libraryService.onBuildRunning.subscribe(function (next) {
            console.log('onBuildRunning: next');
            _this.onBuildRunning(next);
        }, function (error) {
            console.log('onBuildRunning: error');
            _this.handleError(error);
        }, function () { return console.log('onBuildRunning: complete'); }));
        this.subscription.add(this.libraryService.onBuildComplete.subscribe(function (next) {
            console.log('onBuildComplete: next');
            _this.onBuildComplete(next);
        }, function (error) {
            console.log('onBuildComplete: error');
            _this.handleError(error);
        }, function () { return console.log('onBuildComplete: complete'); }));
        this.subscription.add(this.libraryService.onBuildError.subscribe(function (next) {
            console.log('onBuildError: next');
            _this.onBuildError(next);
        }, function (error) {
            console.log('onBuildError: error');
            _this.handleError(error);
        }, function () { return console.log('onBuildError: complete'); }));
    };
    PartsComponent.prototype.ngOnDestroy = function () {
        console.log('ngOnDestroy');
        PartsComponent_1.sortActive = this.sortActive;
        PartsComponent_1.sortDirection = this.sortDirection;
        this.subscription.unsubscribe();
    };
    PartsComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.sort.sortChange
            .pipe(Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["tap"])(function () {
            _this.sortActive = _this.sort.active;
            _this.sortDirection = _this.sort.direction;
        }), Object(rxjs_operators__WEBPACK_IMPORTED_MODULE_8__["tap"])(function () { return _this.refresh(); }))
            .subscribe();
    };
    PartsComponent.prototype.onBuildError = function (next) {
        if (this.buildSnackBar) {
            this.buildSnackBar.dismiss();
        }
        console.error(next);
        var error = '';
        if (next.message) {
            error = next.message;
        }
        else if (next.code) {
            switch (next.code) {
                case 'ENOENT':
                    error = "No such file or directory: " + next.path;
                    break;
                default:
                    error = 'Unknown error. Refer to console!';
                    break;
            }
        }
        else {
            error = 'Unknown error. Refer to console!';
        }
        this.buildSnackBar = this.snackBar.open("Build error: " + error, 'OK');
    };
    PartsComponent.prototype.onBuildComplete = function (next) {
        if (this.buildSnackBar) {
            this.buildSnackBar.dismiss();
        }
        this.buildSnackBar = this.snackBar.open('Build complete.', 'OK', {
            duration: 3000
        });
    };
    PartsComponent.prototype.onBuildRunning = function (next) {
        this.buildSnackBar = this.snackBar.open('Build running.');
    };
    PartsComponent.prototype.handleError = function (error) {
        console.error(error);
    };
    PartsComponent.prototype.onRowClicked = function (row) {
        this.router.navigateByUrl("/part/" + row.id);
    };
    PartsComponent.prototype.addPart = function () {
        this.router.navigateByUrl("/part/new");
    };
    PartsComponent.prototype.refresh = function () {
        this.dataSource.loadParts(this.sortActive, this.sortDirection);
    };
    PartsComponent.prototype.build = function () {
        this.libraryService.build();
    };
    var PartsComponent_1;
    tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ViewChild"])(_angular_material__WEBPACK_IMPORTED_MODULE_2__["MatSort"]),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:type", _angular_material__WEBPACK_IMPORTED_MODULE_2__["MatSort"])
    ], PartsComponent.prototype, "sort", void 0);
    PartsComponent = PartsComponent_1 = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-parts',
            template: __webpack_require__(/*! ./parts.component.html */ "./src/app/parts/parts.component.html"),
            styles: [__webpack_require__(/*! ./parts.component.css */ "./src/app/parts/parts.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_part_service__WEBPACK_IMPORTED_MODULE_4__["PartService"],
            _library_service__WEBPACK_IMPORTED_MODULE_6__["LibraryService"],
            _angular_router__WEBPACK_IMPORTED_MODULE_5__["Router"],
            _angular_material__WEBPACK_IMPORTED_MODULE_2__["MatSnackBar"]])
    ], PartsComponent);
    return PartsComponent;
}());



/***/ }),

/***/ "./src/app/settings-fields/settings-fields.component.css":
/*!***************************************************************!*\
  !*** ./src/app/settings-fields/settings-fields.component.css ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".full-width {\r\n  width: 100%;\r\n}\r\n\r\n.global-content {\r\n  margin-top: 40px;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2V0dGluZ3MtZmllbGRzL3NldHRpbmdzLWZpZWxkcy5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0VBQ0UsV0FBVztBQUNiOztBQUVBO0VBQ0UsZ0JBQWdCO0FBQ2xCIiwiZmlsZSI6InNyYy9hcHAvc2V0dGluZ3MtZmllbGRzL3NldHRpbmdzLWZpZWxkcy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmZ1bGwtd2lkdGgge1xyXG4gIHdpZHRoOiAxMDAlO1xyXG59XHJcblxyXG4uZ2xvYmFsLWNvbnRlbnQge1xyXG4gIG1hcmdpbi10b3A6IDQwcHg7XHJcbn1cclxuIl19 */"

/***/ }),

/***/ "./src/app/settings-fields/settings-fields.component.html":
/*!****************************************************************!*\
  !*** ./src/app/settings-fields/settings-fields.component.html ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"div\">\r\n  <mat-toolbar class=\"mat-elevation-z6\" color=\"primary\">\r\n    <mat-toolbar-row>\r\n      <button\r\n        type=\"submit\"\r\n        aria-label=\"submit\"\r\n        mat-icon-button\r\n        [disabled]=\"!settingsForm?.valid\"\r\n        (click)=\"onSubmit()\"\r\n      >\r\n        <mat-icon aria-label=\"submit icon\">arrow_back</mat-icon>\r\n      </button>\r\n      <span>Fields</span>\r\n      <span class=\"spacer\"></span>\r\n      <button\r\n        type=\"button\"\r\n        aria-label=\"add\"\r\n        mat-icon-button\r\n        (click)=\"addCustomField()\"\r\n      >\r\n        <mat-icon aria-label=\"add icon\">add</mat-icon>\r\n      </button>\r\n    </mat-toolbar-row>\r\n  </mat-toolbar>\r\n  <div class=\"global-progress-container\">\r\n    <mat-progress-bar\r\n      *ngIf=\"!settingsForm\"\r\n      color=\"accent\"\r\n      mode=\"indeterminate\"\r\n    ></mat-progress-bar>\r\n  </div>\r\n  <div class=\"global-content\">\r\n    <form\r\n      *ngIf=\"settingsForm\"\r\n      [formGroup]=\"settingsForm\"\r\n      (ngSubmit)=\"onSubmit()\"\r\n    >\r\n      <mat-form-field\r\n        *ngFor=\"let field of customFields.controls; index as i\"\r\n        formArrayName=\"customFields\"\r\n        class=\"full-width\"\r\n      >\r\n        <input\r\n          matInput\r\n          placeholder=\"Custom field {{ i + 1 }}\"\r\n          [formControlName]=\"i\"\r\n        />\r\n        <button\r\n          mat-icon-button\r\n          matSuffix\r\n          aria-label=\"Delete\"\r\n          type=\"button\"\r\n          (click)=\"removeCustomField(i)\"\r\n        >\r\n          <mat-icon>delete</mat-icon>\r\n        </button>\r\n        <mat-error\r\n          *ngIf=\"\r\n            settingsForm\r\n              .get('customFields')\r\n              .at(i)\r\n              .hasError('required')\r\n          \"\r\n        >\r\n          Value is <strong>required</strong>\r\n        </mat-error>\r\n      </mat-form-field>\r\n    </form>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/settings-fields/settings-fields.component.ts":
/*!**************************************************************!*\
  !*** ./src/app/settings-fields/settings-fields.component.ts ***!
  \**************************************************************/
/*! exports provided: SettingsFieldsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SettingsFieldsComponent", function() { return SettingsFieldsComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/fesm5/forms.js");
/* harmony import */ var _settings_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../settings.service */ "./src/app/settings.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");






var SettingsFieldsComponent = /** @class */ (function () {
    function SettingsFieldsComponent(settingsService, location) {
        this.settingsService = settingsService;
        this.location = location;
        this.subscription = new rxjs__WEBPACK_IMPORTED_MODULE_5__["Subscription"]();
    }
    Object.defineProperty(SettingsFieldsComponent.prototype, "customFields", {
        get: function () {
            if (this.settingsForm)
                return this.settingsForm.get('customFields');
            else
                return null;
        },
        enumerable: true,
        configurable: true
    });
    SettingsFieldsComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        var sub = this.settingsService.onSettingsChanged.subscribe(function (next) {
            console.log('onSettingsChanged: next');
            _this.onSettingsChanged(next);
        }, function (error) {
            console.log('onSettingsChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onSettingsChanged: complete'); });
        this.subscription.add(sub);
        this.settingsService.getSettings();
    };
    SettingsFieldsComponent.prototype.ngOnDestroy = function () {
        console.log('ngOnDestroy');
        this.subscription.unsubscribe();
    };
    SettingsFieldsComponent.prototype.onSettingsChanged = function (settings) {
        console.log('onSettingsChanged(settings: Settings)');
        console.log(settings);
        this.settings = settings;
        this.initForm();
        //this.ref.detectChanges();
    };
    SettingsFieldsComponent.prototype.handleError = function (error) {
        console.error(error);
    };
    SettingsFieldsComponent.prototype.initForm = function () {
        console.log('initForm()');
        var customFields = [];
        for (var _i = 0, _a = this.settings.customFields; _i < _a.length; _i++) {
            var field = _a[_i];
            customFields.push(new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](field, _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required));
        }
        this.settingsForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroup"]({
            customFields: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormArray"](customFields)
        });
        console.log(this.settingsForm.value);
    };
    SettingsFieldsComponent.prototype.addCustomField = function () {
        this.customFields.push(new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"]('', _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required));
        //this.ref.detectChanges();
    };
    SettingsFieldsComponent.prototype.removeCustomField = function (index) {
        this.customFields.removeAt(index);
    };
    SettingsFieldsComponent.prototype.onSubmit = function () {
        console.log('onSubmit()');
        console.log(this.settingsForm.value);
        this.settings.customFields = this.settingsForm.controls['customFields'].value;
        this.settingsService.updateSettings(this.settings);
        this.goBack();
    };
    SettingsFieldsComponent.prototype.goBack = function () {
        this.location.back();
    };
    SettingsFieldsComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-settings-fields',
            template: __webpack_require__(/*! ./settings-fields.component.html */ "./src/app/settings-fields/settings-fields.component.html"),
            styles: [__webpack_require__(/*! ./settings-fields.component.css */ "./src/app/settings-fields/settings-fields.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_settings_service__WEBPACK_IMPORTED_MODULE_3__["SettingsService"],
            _angular_common__WEBPACK_IMPORTED_MODULE_4__["Location"]])
    ], SettingsFieldsComponent);
    return SettingsFieldsComponent;
}());



/***/ }),

/***/ "./src/app/settings-paths/settings-paths.component.css":
/*!*************************************************************!*\
  !*** ./src/app/settings-paths/settings-paths.component.css ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".full-width {\r\n  width: 100%;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2V0dGluZ3MtcGF0aHMvc2V0dGluZ3MtcGF0aHMuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNFLFdBQVc7QUFDYiIsImZpbGUiOiJzcmMvYXBwL3NldHRpbmdzLXBhdGhzL3NldHRpbmdzLXBhdGhzLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIuZnVsbC13aWR0aCB7XHJcbiAgd2lkdGg6IDEwMCU7XHJcbn1cclxuIl19 */"

/***/ }),

/***/ "./src/app/settings-paths/settings-paths.component.html":
/*!**************************************************************!*\
  !*** ./src/app/settings-paths/settings-paths.component.html ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"global\">\r\n  <mat-toolbar class=\"mat-elevation-z6\" color=\"primary\">\r\n    <mat-toolbar-row>\r\n      <button\r\n        type=\"submit\"\r\n        aria-label=\"submit\"\r\n        mat-icon-button\r\n        [disabled]=\"!settingsForm?.valid\"\r\n        (click)=\"onSubmit()\"\r\n      >\r\n        <mat-icon aria-label=\"submit icon\">arrow_back</mat-icon>\r\n      </button>\r\n      <span>Paths</span>\r\n    </mat-toolbar-row>\r\n  </mat-toolbar>\r\n  <div class=\"global-progress-container\">\r\n    <mat-progress-bar\r\n      *ngIf=\"!settingsForm\"\r\n      color=\"accent\"\r\n      mode=\"indeterminate\"\r\n    ></mat-progress-bar>\r\n  </div>\r\n  <div class=\"global-content\">\r\n    <form\r\n      *ngIf=\"settingsForm\"\r\n      [formGroup]=\"settingsForm\"\r\n      (ngSubmit)=\"onSubmit()\"\r\n    >\r\n      <div\r\n        *ngFor=\"\r\n          let field of (settingsForm.controls | keys);\r\n          first as isFirst;\r\n          last as isLast\r\n        \"\r\n      >\r\n        <mat-form-field class=\"full-width\">\r\n          <input\r\n            matInput\r\n            [placeholder]=\"field | uppercase\"\r\n            [formControlName]=\"field\"\r\n          />\r\n          <button\r\n            mat-icon-button\r\n            matSuffix\r\n            aria-label=\"folder open\"\r\n            type=\"button\"\r\n            (click)=\"setPath(field)\"\r\n          >\r\n            <mat-icon>folder_open</mat-icon>\r\n          </button>\r\n          <mat-error *ngIf=\"settingsForm.controls[field].hasError('required')\">\r\n            {{ field | uppercase }} is <strong>required</strong>\r\n          </mat-error>\r\n        </mat-form-field>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/settings-paths/settings-paths.component.ts":
/*!************************************************************!*\
  !*** ./src/app/settings-paths/settings-paths.component.ts ***!
  \************************************************************/
/*! exports provided: SettingsPathsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SettingsPathsComponent", function() { return SettingsPathsComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/fesm5/forms.js");
/* harmony import */ var _settings_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../settings.service */ "./src/app/settings.service.ts");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");






var SettingsPathsComponent = /** @class */ (function () {
    function SettingsPathsComponent(settingsService, location) {
        this.settingsService = settingsService;
        this.location = location;
        this.settingsForm = null;
        this.subscription = new rxjs__WEBPACK_IMPORTED_MODULE_5__["Subscription"]();
    }
    SettingsPathsComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        var sub = this.settingsService.onSettingsChanged.subscribe(function (next) {
            console.log('onSettingsChanged: next');
            _this.onSettingsChanged(next);
        }, function (error) {
            console.log('onSettingsChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onSettingsChanged: complete'); });
        this.subscription.add(sub);
        this.settingsService.getSettings();
    };
    SettingsPathsComponent.prototype.ngOnDestroy = function () {
        console.log('ngOnDestroy');
        this.subscription.unsubscribe();
    };
    SettingsPathsComponent.prototype.onSettingsChanged = function (settings) {
        console.log('onSettingsChanged(settings: Settings)');
        console.log(settings);
        this.settings = settings;
        this.initForm();
    };
    SettingsPathsComponent.prototype.handleError = function (error) {
        console.error(error);
    };
    SettingsPathsComponent.prototype.initForm = function () {
        console.log('initForm()');
        this.settingsForm = new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormGroup"]({
            parts: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.settings.paths.parts, _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            symbol: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.settings.paths.symbol, _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            footprint: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.settings.paths.footprint, _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required),
            output: new _angular_forms__WEBPACK_IMPORTED_MODULE_2__["FormControl"](this.settings.paths.output, _angular_forms__WEBPACK_IMPORTED_MODULE_2__["Validators"].required)
        });
        console.log('initForm: complete');
        console.log(this.settingsForm.value);
    };
    SettingsPathsComponent.prototype.setPath = function (field) {
        console.log("setPath(" + field + ")");
    };
    SettingsPathsComponent.prototype.onSubmit = function () {
        console.log('onSubmit()');
        console.log(this.settingsForm.value);
        this.settings.paths = this.settingsForm.value;
        this.settingsService.updateSettings(this.settings);
        this.goBack();
    };
    SettingsPathsComponent.prototype.goBack = function () {
        this.location.back();
    };
    SettingsPathsComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-settings-paths',
            template: __webpack_require__(/*! ./settings-paths.component.html */ "./src/app/settings-paths/settings-paths.component.html"),
            styles: [__webpack_require__(/*! ./settings-paths.component.css */ "./src/app/settings-paths/settings-paths.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_settings_service__WEBPACK_IMPORTED_MODULE_3__["SettingsService"],
            _angular_common__WEBPACK_IMPORTED_MODULE_4__["Location"]])
    ], SettingsPathsComponent);
    return SettingsPathsComponent;
}());



/***/ }),

/***/ "./src/app/settings.service.ts":
/*!*************************************!*\
  !*** ./src/app/settings.service.ts ***!
  \*************************************/
/*! exports provided: SettingsService */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SettingsService", function() { return SettingsService; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _settings_settings__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./settings/settings */ "./src/app/settings/settings.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");
/* harmony import */ var ngx_electron__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-electron */ "./node_modules/ngx-electron/fesm5/ngx-electron.js");





var SettingsService = /** @class */ (function () {
    function SettingsService(electronService, zone) {
        var _this = this;
        this.electronService = electronService;
        this.zone = zone;
        this.onSettingsChangedSubject = new rxjs__WEBPACK_IMPORTED_MODULE_3__["Subject"]();
        console.log('constructor(...)');
        this.settings = new _settings_settings__WEBPACK_IMPORTED_MODULE_2__["Settings"]();
        this.settings.customFields = ['OC_FARNELL', 'OC_MOUSER'];
        console.log(this.onSettingsChanged);
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.on('settings.changed', function (event, arg) { return _this.ipcSettingsChanged(event, arg); });
        }
    }
    Object.defineProperty(SettingsService.prototype, "onSettingsChanged", {
        get: function () {
            return this.onSettingsChangedSubject.asObservable();
        },
        enumerable: true,
        configurable: true
    });
    SettingsService.prototype.ipcSettingsChanged = function (event, arg) {
        var _this = this;
        console.log('ipcSettingsChanged(event, arg)');
        console.log(event);
        console.log(arg);
        this.zone.run(function () {
            _this.settings = arg;
            _this.onSettingsChangedSubject.next(arg);
        });
    };
    SettingsService.prototype.getSettings = function () {
        console.log('getSettings()');
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('settings.get');
        }
        else {
            this.onSettingsChangedSubject.next(this.settings);
        }
    };
    SettingsService.prototype.updateSettings = function (settings) {
        console.log('updateSettings()');
        this.settings = settings;
        if (this.electronService.isElectronApp) {
            this.electronService.ipcRenderer.send('settings.update', settings);
        }
    };
    SettingsService = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Injectable"])({
            providedIn: 'root'
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [ngx_electron__WEBPACK_IMPORTED_MODULE_4__["ElectronService"], _angular_core__WEBPACK_IMPORTED_MODULE_1__["NgZone"]])
    ], SettingsService);
    return SettingsService;
}());



/***/ }),

/***/ "./src/app/settings/settings.component.css":
/*!*************************************************!*\
  !*** ./src/app/settings/settings.component.css ***!
  \*************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ".full-width {\r\n  width: 100%;\r\n}\r\n\r\n.settings-card {\r\n  min-width: 120px;\r\n  margin: 20px;\r\n}\r\n\r\n.mat-radio-button {\r\n  display: block;\r\n  margin: 5px 0;\r\n}\r\n\r\n.row {\r\n  display: flex;\r\n  flex-direction: row;\r\n}\r\n\r\n.col {\r\n  flex: 1;\r\n  margin-right: 20px;\r\n}\r\n\r\n.col:last-child {\r\n  margin-right: 0;\r\n}\r\n\r\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNyYy9hcHAvc2V0dGluZ3Mvc2V0dGluZ3MuY29tcG9uZW50LmNzcyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQTtFQUNFLFdBQVc7QUFDYjs7QUFFQTtFQUNFLGdCQUFnQjtFQUNoQixZQUFZO0FBQ2Q7O0FBRUE7RUFDRSxjQUFjO0VBQ2QsYUFBYTtBQUNmOztBQUVBO0VBQ0UsYUFBYTtFQUNiLG1CQUFtQjtBQUNyQjs7QUFFQTtFQUNFLE9BQU87RUFDUCxrQkFBa0I7QUFDcEI7O0FBRUE7RUFDRSxlQUFlO0FBQ2pCIiwiZmlsZSI6InNyYy9hcHAvc2V0dGluZ3Mvc2V0dGluZ3MuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5mdWxsLXdpZHRoIHtcclxuICB3aWR0aDogMTAwJTtcclxufVxyXG5cclxuLnNldHRpbmdzLWNhcmQge1xyXG4gIG1pbi13aWR0aDogMTIwcHg7XHJcbiAgbWFyZ2luOiAyMHB4O1xyXG59XHJcblxyXG4ubWF0LXJhZGlvLWJ1dHRvbiB7XHJcbiAgZGlzcGxheTogYmxvY2s7XHJcbiAgbWFyZ2luOiA1cHggMDtcclxufVxyXG5cclxuLnJvdyB7XHJcbiAgZGlzcGxheTogZmxleDtcclxuICBmbGV4LWRpcmVjdGlvbjogcm93O1xyXG59XHJcblxyXG4uY29sIHtcclxuICBmbGV4OiAxO1xyXG4gIG1hcmdpbi1yaWdodDogMjBweDtcclxufVxyXG5cclxuLmNvbDpsYXN0LWNoaWxkIHtcclxuICBtYXJnaW4tcmlnaHQ6IDA7XHJcbn1cclxuIl19 */"

/***/ }),

/***/ "./src/app/settings/settings.component.html":
/*!**************************************************!*\
  !*** ./src/app/settings/settings.component.html ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<div class=\"global\">\r\n  <mat-toolbar class=\"mat-elevation-z6\" color=\"primary\">\r\n    <button type=\"button\" aria-label=\"back\" mat-icon-button (click)=\"goBack()\">\r\n      <mat-icon aria-label=\"back icon\">arrow_back</mat-icon>\r\n    </button>\r\n    <span>Settings</span>\r\n  </mat-toolbar>\r\n  <div class=\"global-content\">\r\n    <mat-nav-list>\r\n      <a mat-list-item routerLink=\"/settings/fields\">Fields</a>\r\n      <mat-divider></mat-divider>\r\n      <a mat-list-item routerLink=\"/settings/paths\">Paths</a>\r\n      <mat-divider></mat-divider>\r\n    </mat-nav-list>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "./src/app/settings/settings.component.ts":
/*!************************************************!*\
  !*** ./src/app/settings/settings.component.ts ***!
  \************************************************/
/*! exports provided: SettingsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SettingsComponent", function() { return SettingsComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var _settings_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../settings.service */ "./src/app/settings.service.ts");
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs */ "./node_modules/rxjs/_esm5/index.js");





var SettingsComponent = /** @class */ (function () {
    function SettingsComponent(settingsService, location) {
        this.settingsService = settingsService;
        this.location = location;
        this.settings = JSON.parse('{ "customFields": [ "OC_FARNELL", "OC_MOUSER" ], "paths": { "parts": "parts", "symbol": "symbol", "footprint": "footprint", "output": "footprint" } }');
        this.subscription = new rxjs__WEBPACK_IMPORTED_MODULE_4__["Subscription"]();
        console.log('constructor');
    }
    SettingsComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        var sub = this.settingsService.onSettingsChanged.subscribe(function (next) {
            console.log('onSettingsChanged: next');
            _this.onSettingsChanged(next);
        }, function (error) {
            console.log('onSettingsChanged: error');
            _this.handleError(error);
        }, function () { return console.log('onSettingsChanged: complete'); });
        this.subscription.add(sub);
        this.settingsService.getSettings();
    };
    SettingsComponent.prototype.ngOnDestroy = function () {
        console.log('ngOnDestroy');
        this.subscription.unsubscribe();
    };
    SettingsComponent.prototype.onSettingsChanged = function (settings) {
        console.log('onSettingsChanged(settings: Settings)');
        console.log(settings);
        this.settings = settings;
    };
    SettingsComponent.prototype.handleError = function (error) {
        console.error(error);
    };
    SettingsComponent.prototype.goBack = function () {
        this.location.back();
    };
    SettingsComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-settings',
            template: __webpack_require__(/*! ./settings.component.html */ "./src/app/settings/settings.component.html"),
            styles: [__webpack_require__(/*! ./settings.component.css */ "./src/app/settings/settings.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [_settings_service__WEBPACK_IMPORTED_MODULE_3__["SettingsService"],
            _angular_common__WEBPACK_IMPORTED_MODULE_2__["Location"]])
    ], SettingsComponent);
    return SettingsComponent;
}());



/***/ }),

/***/ "./src/app/settings/settings.ts":
/*!**************************************!*\
  !*** ./src/app/settings/settings.ts ***!
  \**************************************/
/*! exports provided: Settings, SettingsPaths */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Settings", function() { return Settings; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SettingsPaths", function() { return SettingsPaths; });
var Settings = /** @class */ (function () {
    function Settings() {
        this.customFields = [];
        this.paths = new SettingsPaths();
    }
    return Settings;
}());

var SettingsPaths = /** @class */ (function () {
    function SettingsPaths() {
        this.parts = '';
        this.symbol = '';
        this.footprint = '';
        this.output = '';
    }
    return SettingsPaths;
}());



/***/ }),

/***/ "./src/environments/environment.ts":
/*!*****************************************!*\
  !*** ./src/environments/environment.ts ***!
  \*****************************************/
/*! exports provided: environment */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "environment", function() { return environment; });
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
var environment = {
    production: false
};
/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.


/***/ }),

/***/ "./src/main.ts":
/*!*********************!*\
  !*** ./src/main.ts ***!
  \*********************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var hammerjs__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! hammerjs */ "./node_modules/hammerjs/hammer.js");
/* harmony import */ var hammerjs__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(hammerjs__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/platform-browser-dynamic */ "./node_modules/@angular/platform-browser-dynamic/fesm5/platform-browser-dynamic.js");
/* harmony import */ var _app_app_module__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./app/app.module */ "./src/app/app.module.ts");
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./environments/environment */ "./src/environments/environment.ts");





if (_environments_environment__WEBPACK_IMPORTED_MODULE_4__["environment"].production) {
    Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["enableProdMode"])();
}
Object(_angular_platform_browser_dynamic__WEBPACK_IMPORTED_MODULE_2__["platformBrowserDynamic"])().bootstrapModule(_app_app_module__WEBPACK_IMPORTED_MODULE_3__["AppModule"])
    .catch(function (err) { return console.error(err); });


/***/ }),

/***/ 0:
/*!***************************!*\
  !*** multi ./src/main.ts ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(/*! C:\workspace\kicad\kicad-db-lib\angular\src\main.ts */"./src/main.ts");


/***/ })

},[[0,"runtime","vendor"]]]);
//# sourceMappingURL=main.js.map