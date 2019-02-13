import { app, BrowserWindow, ipcMain } from 'electron';
import * as path from 'path';
import { SettingsService } from './settings/settings.service';
import { Settings } from '../../shared/settings/settings';
import { PartService } from './parts/part.service';
import { Part } from './parts/part';
import { LibraryService } from './library/library.service';
import { fs } from './fs';

let mainWindow: Electron.BrowserWindow;
let settingsService = new SettingsService();
let partService = new PartService();
let libraryService = new LibraryService();

function createWindow() {
  console.log(__dirname);
  console.log(app.getAppPath());

  // Create the browser window.
  mainWindow = new BrowserWindow({
    height: 600,
    width: 800
  });

  // and load the index.html of the app.
  mainWindow.loadFile(path.join(__dirname, '../../ng/index.html'));

  // Open the DevTools.
  mainWindow.webContents.openDevTools();

  // Emitted when the window is closed.
  mainWindow.on('closed', () => {
    // Dereference the window object, usually you would store windows
    // in an array if your app supports multi windows, this is the time
    // when you should delete the corresponding element.
    mainWindow = null;
  });
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', () => {
  createWindow();
  return;
  var lib = new LibraryService();
  console.log('build');
  lib
    .build()
    .then(() => {
      console.log('build done!');
    })
    .catch(error => {
      console.log('build error:');
      console.error(error);
    });
});

// Quit when all windows are closed.
app.on('window-all-closed', () => {
  // On OS X it is common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  // On OS X it"s common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if (mainWindow === null) {
    createWindow();
  }
});

// In this file you can include the rest of your app"s specific main process
// code. You can also put them in separate files and require them here.

function buildLibrary(event: any) {
  event.sender.send('library.build.running');
  libraryService
    .build(true)
    .then(() => event.sender.send('library.build.complete'))
    .catch(error => event.sender.send('library.build.error', error));
}

console.log('on: settings.get');
ipcMain.on('settings.get', (event: any) => {
  console.log('ipcMain: settings.get');

  var file = path.join(__dirname, '/settings.json');
  console.log(`getSettings(${file})`);
  var promise = settingsService.getSettings();
  promise
    .then(settings => {
      console.log(settings);
      event.sender.send('settings.changed', settings);
    })
    .catch(error => console.error(error));
});

console.log('on: settings.update');
ipcMain.on('settings.update', (event: any, arg: Settings) => {
  console.log('ipcMain: settings.update');
  var promise = settingsService.updateSettings(arg);
  promise
    .then(settings => console.log('settings.update: then'))
    .catch(error => console.error(error));
});

console.log('on: part.getParts');
ipcMain.on('part.getParts', (event: any) => {
  console.log('ipcMain: part.getParts');

  var promise = partService.getParts();
  promise
    .then(parts => {
      console.log('part.getParts: then');
      event.sender.send('part.partsChanged', parts);
    })
    .catch(error => console.error(error));
});

console.log('on: part.getPart');
ipcMain.on('part.getPart', (event: any, arg: number) => {
  console.log(`ipcMain: part.getPart arg: ${arg}`);

  var promise = partService.getPart(arg);
  promise
    .then(part => {
      console.log('part.getPart: then');
      event.sender.send('part.partChanged', part);
    })
    .catch(error => console.error(error));
});

console.log('on: part.addPart');
ipcMain.on('part.addPart', (event: any, arg: Part) => {
  console.log('ipcMain: part.addPart arg:');
  console.log(arg);

  var promise = partService.addPart(arg);
  promise
    .then(part => {
      console.log('part.addPart: then');
      buildLibrary(event);

      libraryService.build().then();
    })
    .catch(error => console.error(error));
});

console.log('on: part.updatePart');
ipcMain.on('part.updatePart', (event: any, arg: Part) => {
  console.log('ipcMain: part.updatePart arg:');
  console.log(arg);

  var promise = partService.updatePart(arg);
  promise
    .then(part => {
      console.log('part.updatePart: then');
      buildLibrary(event);
    })
    .catch(error => console.error(error));
});

console.log('on: part.deletePart');
ipcMain.on('part.deletePart', (event: any, arg: Part) => {
  console.log('ipcMain: part.deletePart arg:');
  console.log(arg);

  var promise = partService.deletePart(arg);
  promise
    .then(part => {
      console.log('part.deletePart: then');
      buildLibrary(event);
    })
    .catch(error => console.error(error));
});

console.log('on: part.getLibraries');
ipcMain.on(
  'part.getLibraries',
  (event: any, filter: string, reload: boolean, max?: number) => {
    console.log(`ipcMain: part.getLibraries(${filter}, ${reload}, ${max}):`);

    var promise = partService.getLibraries(filter, reload, max);
    promise
      .then(libraries => {
        console.log('part.getLibraries: then');
        event.sender.send('part.getLibraries', libraries);
      })
      .catch(error => console.error(error));
  }
);

console.log('on: library.getSymbols');
ipcMain.on(
  'library.getSymbols',
  (event: any, filter: string, reload: boolean, max?: number) => {
    console.log(`ipcMain: library.getSymbols(${filter}, ${reload}, ${max}):`);

    var promise = libraryService.getSymbols(filter, reload, max);
    promise
      .then(symbols => {
        console.log('library.getSymbols: then');
        event.sender.send('library.getSymbols', symbols);
      })
      .catch(error => console.error(error));
  }
);

console.log('on: library.getFootprints');
ipcMain.on(
  'library.getFootprints',
  (event: any, filter: string, reload: boolean, max?: number) => {
    console.log(
      `ipcMain: library.getFootprints(${filter}, ${reload}, ${max}):`
    );
    console.log(filter);
    console.log(reload);

    var promise = libraryService.getFootprints(filter, reload, max);
    promise
      .then(footprints => {
        console.log('library.getFootprints: then');
        event.sender.send('library.getFootprints', footprints);
      })
      .catch(error => console.error(error));
  }
);

console.log('on: test');
ipcMain.on('test', (event: any) => {
  console.log('ipcMain: test');
});
