import { Injectable } from '@angular/core';
import { Settings } from '../../../shared/settings/settings';
import { Observable, of, Subject } from 'rxjs';
import { MessageService } from './message.service';
import { ElectronService } from 'ngx-electron';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private settings: Settings;

  constructor(
    private messageService: MessageService,
    private electronService: ElectronService
  ) {
    this.settings = new Settings();
    this.settings.customFields = ['OC_FARNELL', 'OC_MOUSER'];
  }

  getSettings(): Observable<Settings> {
    var channel = 'settings.get';
    if (!this.electronService.isElectronApp) {
      this.messageService.add('SettingsService: getSettings');
      return of(this.settings);
    }
    console.log(channel);
    var subject = new Subject<Settings>();
    this.electronService.ipcRenderer.once(channel, (event: any, arg: any) => {
      console.log(`once ${channel}`);
      console.log(arg);
      this.settings = arg;
      subject.next(arg);
      subject.complete();
    });
    this.electronService.ipcRenderer.send(channel);
    return subject;
  }

  updateSettings(settings: Settings): Observable<any> {
    var channel = 'settings.update';
    if (!this.electronService.isElectronApp) {
      this.messageService.add('SettingsService: updateSettings');
      return of((this.settings = settings));
    }
    console.log(channel);
    console.log(settings);
    var subject = new Subject<Settings>();
    this.electronService.ipcRenderer.once(channel, (event: any, arg: any) => {
      console.log(`once ${channel}`);
      console.log(arg);
      this.settings = arg;
      subject.next(arg);
      subject.complete();
    });
    this.electronService.ipcRenderer.send(channel, settings);
    return subject;
  }
}
