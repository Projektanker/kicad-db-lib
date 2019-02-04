import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { SettingsService } from '../settings.service';
import { Settings } from '../../../../shared/settings/settings';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settings: Settings = JSON.parse(
    '{ "customFields": [ "OC_FARNELL", "OC_MOUSER" ], "paths": { "parts": "parts", "symbol": "symbol", "footprint": "footprint", "output": "footprint" } }'
  );

  constructor(
    private settingsService: SettingsService,
    private msg: MessageService,
    private location: Location
  ) {
    console.log('Settings constructor');
  }

  ngOnInit() {
    console.log('ngOnInit');
    this.settingsService.getSettings().subscribe(
      settings => {
        console.log('ngOnInit: next');
        this.settings = settings;
      },
      error => {
        console.log(`ngOnInit: ${error}`);
      },
      () => {
        console.log('ngOnInit: complete');
      }
    );
  }

  goBack() {
    this.location.back();
  }
}
