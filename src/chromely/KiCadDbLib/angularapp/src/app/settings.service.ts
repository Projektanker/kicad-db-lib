import { Injectable, NgZone } from '@angular/core';
import { Settings } from './settings/settings';
import { Observable, of, Subject } from 'rxjs';
import { MessageService } from './message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private settings: Settings;
  private onSettingsChangedSubject: Subject<Settings> = new Subject<Settings>();

  get onSettingsChanged() {
    return this.onSettingsChangedSubject.asObservable();
  }

  constructor(private http: HttpClient, private zone: NgZone) {
    console.log('constructor(...)');
    this.settings = new Settings();
    this.settings.customFields = ['OC_FARNELL', 'OC_MOUSER'];
    console.log(this.onSettingsChanged);    
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
      this.onSettingsChangedSubject.next(this.settings);    
  }

  updateSettings(settings: Settings) {
    console.log('updateSettings()');
    this.settings = settings;    
  }
}
