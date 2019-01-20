import { Injectable } from '@angular/core';
import { Settings } from './settings/settings';
import { Observable, of } from 'rxjs';
import { MessageService } from './message.service';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  private settings: Settings;

  constructor(
    private messageService: MessageService) {
    this.settings = new Settings();
    this.settings.customFields = [
      'OC_FARNELL', 'OC_MOUSER'
    ];
  }

  getSettings(): Observable<Settings> {
    this.messageService.add('SettingsService: getSettings');
    return of(this.settings);
  }

  updateSettings(settings: Settings): Observable<any> {
    this.messageService.add('SettingsService: updateSettings');
    return of(this.settings = settings);
  }
}
