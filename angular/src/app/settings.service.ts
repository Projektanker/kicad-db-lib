import { Injectable, NgZone } from '@angular/core';
import { Settings } from '../../../shared/settings/settings';
import { Observable, of, Subject } from 'rxjs';
import { MessageService } from './message.service';
import { ElectronService } from 'ngx-electron';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private settings: Settings;
  private onSettingsChangedSubject: Subject<Settings> = new Subject<Settings>();

  get onSettingsChanged() {
    return this.onSettingsChangedSubject.asObservable();
  }

  constructor(private electronService: ElectronService, private zone: NgZone) {
    console.log('constructor(...)');
    this.settings = new Settings();
    this.settings.customFields = ['OC_FARNELL', 'OC_MOUSER'];
    console.log(this.onSettingsChanged);
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.on(
        'settings.changed',
        (event: any, arg: any) => this.ipcSettingsChanged(event, arg)
      );
    }
  }

  private ipcSettingsChanged(event: any, arg: any) {
    console.log('ipcSettingsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.settings = arg;
      this.onSettingsChangedSubject.next(arg);
    });
  }

  getSettings() {
    console.log('getSettings()');

    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('settings.get');
    } else {
      this.onSettingsChangedSubject.next(this.settings);
    }
  }

  updateSettings(settings: Settings) {
    console.log('updateSettings()');
    this.settings = settings;
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('settings.update', settings);
    }
  }
}
