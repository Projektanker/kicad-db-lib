import { app, BrowserWindow, ipcMain } from 'electron';
import * as path from 'path';
import { SettingsService } from './settings/settings.service';
import { Settings } from '../../shared/settings/settings';

let mainWindow: Electron.BrowserWindow;

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
app.on('ready', createWindow);

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

console.log('on: settings.get');
ipcMain.on('settings.get', (event: any) => {
  console.log('ipcMain: settings.get');
  var file = path.join(__dirname, '/settings.json');
  console.log(`getSettings(${file})`);
  var settingsService = new SettingsService();
  var promise = settingsService.getSettings(file);
  promise
    .then(settings => {
      console.log(settings);
      event.sender.send('settings.get', settings);
    })
    .catch(error => console.error(error));
});

console.log('on: settings.update');
ipcMain.on('settings.update', (event: any, arg: Settings) => {
  console.log('ipcMain: settings.update');

  var file = path.join(__dirname, '/settings.json');
  console.log(`updateSettings(${file}, ${arg})`);

  var settingsService = new SettingsService();
  var promise = settingsService.updateSettings(file, arg);
  promise
    .then(settings => {
      console.log(settings);
      event.sender.send('settings.update', settings);
    })
    .catch(error => console.error(error));
});

console.log('on: test');
ipcMain.on('test', (event: any) => {
  console.log('ipcMain: test');
});
