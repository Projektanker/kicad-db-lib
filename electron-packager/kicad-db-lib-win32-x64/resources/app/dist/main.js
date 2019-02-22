"use strict";
exports.__esModule = true;
var electron_1 = require("electron");
var path = require("path");
var settings_service_1 = require("./settings/settings.service");
var part_service_1 = require("./parts/part.service");
var library_service_1 = require("./library/library.service");
var mainWindow;
var settingsService = new settings_service_1.SettingsService();
var partService = new part_service_1.PartService();
var libraryService = new library_service_1.LibraryService();
function createWindow() {
    console.log(__dirname);
    console.log(electron_1.app.getAppPath());
    // Create the browser window.
    mainWindow = new electron_1.BrowserWindow({
        height: 600,
        width: 800
    });
    // and load the index.html of the app.
    mainWindow.loadFile(path.join(__dirname, 'ng/index.html'));
    // Open the DevTools.
    //mainWindow.webContents.openDevTools();
    // Remove menu
    //mainWindow.setMenu(null);
    // Emitted when the window is closed.
    mainWindow.on('closed', function () {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        mainWindow = null;
    });
}
// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
electron_1.app.on('ready', function () {
    createWindow();
});
// Quit when all windows are closed.
electron_1.app.on('window-all-closed', function () {
    // On OS X it is common for applications and their menu bar
    // to stay active until the user quits explicitly with Cmd + Q
    if (process.platform !== 'darwin') {
        electron_1.app.quit();
    }
});
electron_1.app.on('activate', function () {
    // On OS X it"s common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (mainWindow === null) {
        createWindow();
    }
});
// In this file you can include the rest of your app"s specific main process
// code. You can also put them in separate files and require them here.
function buildLibrary(event) {
    event.sender.send('library.build.running');
    libraryService
        .build(true)
        .then(function () { return event.sender.send('library.build.complete'); })["catch"](function (error) { return event.sender.send('library.build.error', error); });
}
console.log('on: settings.get');
electron_1.ipcMain.on('settings.get', function (event) {
    console.log('ipcMain: settings.get');
    var file = path.join(__dirname, '/settings.json');
    console.log("getSettings(" + file + ")");
    var promise = settingsService.getSettings();
    promise
        .then(function (settings) {
        console.log(settings);
        event.sender.send('settings.changed', settings);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: settings.update');
electron_1.ipcMain.on('settings.update', function (event, arg) {
    console.log('ipcMain: settings.update');
    var promise = settingsService.updateSettings(arg);
    promise
        .then(function (settings) { return console.log('settings.update: then'); })["catch"](function (error) { return console.error(error); });
});
console.log('on: part.getParts');
electron_1.ipcMain.on('part.getParts', function (event, sortActive, sortDirection) {
    console.log("ipcMain: part.getParts('" + sortActive + "','" + sortDirection + "')");
    var promise = partService.getParts(sortActive, sortDirection);
    promise
        .then(function (parts) {
        console.log('part.getParts: then');
        event.sender.send('part.partsChanged', parts);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: part.getPart');
electron_1.ipcMain.on('part.getPart', function (event, arg) {
    console.log("ipcMain: part.getPart arg: " + arg);
    var promise = partService.getPart(arg);
    promise
        .then(function (part) {
        console.log('part.getPart: then');
        event.sender.send('part.partChanged', part);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: part.addPart');
electron_1.ipcMain.on('part.addPart', function (event, arg) {
    console.log('ipcMain: part.addPart arg:');
    console.log(arg);
    var promise = partService.addPart(arg);
    promise
        .then(function (part) {
        console.log('part.addPart: then');
        buildLibrary(event);
        libraryService.build().then();
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: part.updatePart');
electron_1.ipcMain.on('part.updatePart', function (event, arg) {
    console.log('ipcMain: part.updatePart arg:');
    console.log(arg);
    var promise = partService.updatePart(arg);
    promise
        .then(function (part) {
        console.log('part.updatePart: then');
        buildLibrary(event);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: part.deletePart');
electron_1.ipcMain.on('part.deletePart', function (event, arg) {
    console.log('ipcMain: part.deletePart arg:');
    console.log(arg);
    var promise = partService.deletePart(arg);
    promise
        .then(function (part) {
        console.log('part.deletePart: then');
        buildLibrary(event);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: part.getLibraries');
electron_1.ipcMain.on('part.getLibraries', function (event, filter, reload, max) {
    console.log("ipcMain: part.getLibraries(" + filter + ", " + reload + ", " + max + "):");
    var promise = partService.getLibraries(filter, reload, max);
    promise
        .then(function (libraries) {
        console.log('part.getLibraries: then');
        event.sender.send('part.getLibraries', libraries);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: library.getSymbols');
electron_1.ipcMain.on('library.getSymbols', function (event, filter, reload, max) {
    console.log("ipcMain: library.getSymbols(" + filter + ", " + reload + ", " + max + "):");
    var promise = libraryService.getSymbols(filter, reload, max);
    promise
        .then(function (symbols) {
        console.log('library.getSymbols: then');
        event.sender.send('library.getSymbols', symbols);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: library.getFootprints');
electron_1.ipcMain.on('library.getFootprints', function (event, filter, reload, max) {
    console.log("ipcMain: library.getFootprints(" + filter + ", " + reload + ", " + max + "):");
    console.log(filter);
    console.log(reload);
    var promise = libraryService.getFootprints(filter, reload, max);
    promise
        .then(function (footprints) {
        console.log('library.getFootprints: then');
        event.sender.send('library.getFootprints', footprints);
    })["catch"](function (error) { return console.error(error); });
});
console.log('on: library.build');
electron_1.ipcMain.on('library.build', function (event, arg) {
    console.log('ipcMain: library.getFootprints:');
    buildLibrary(event);
});
console.log('on: test');
electron_1.ipcMain.on('test', function (event) {
    console.log('ipcMain: test');
});
//# sourceMappingURL=main.js.map