import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { SettingsService } from '../settings.service';
import { Subscription } from 'rxjs';
import { Settings } from './settings';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settings: Settings = JSON.parse(
    '{ "customFields": [ "OC_FARNELL", "OC_MOUSER" ], "paths": { "parts": "parts", "symbol": "symbol", "footprint": "footprint", "output": "footprint" } }'
  );

  subscription: Subscription = new Subscription();

  constructor(
    private settingsService: SettingsService,
    private location: Location
  ) {
    console.log('constructor');
  }

  ngOnInit() {
    console.log('ngOnInit');
    var sub = this.settingsService.onSettingsChanged.subscribe(
      (next: Settings) => {
        console.log('onSettingsChanged: next');
        this.onSettingsChanged(next);
      },
      error => {
        console.log('onSettingsChanged: error');
        this.handleError(error);
      },
      () => console.log('onSettingsChanged: complete')
    );
    this.subscription.add(sub);
    this.settingsService.getSettings();
  }

  ngOnDestroy() {
    console.log('ngOnDestroy');
    this.subscription.unsubscribe();
  }

  private onSettingsChanged(settings: Settings) {
    console.log('onSettingsChanged(settings: Settings)');
    console.log(settings);
    this.settings = settings;
  }

  private handleError(error) {
    console.error(error);
  }

  goBack() {
    this.location.back();
  }
}
