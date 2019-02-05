import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SettingsService } from '../settings.service';
import { Location } from '@angular/common';
import { Settings } from '../../../../shared/settings/settings';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-settings-paths',
  templateUrl: './settings-paths.component.html',
  styleUrls: ['./settings-paths.component.css']
})
export class SettingsPathsComponent implements OnInit {
  settingsForm: FormGroup = null;
  settings: Settings;
  subscription: Subscription = new Subscription();
  constructor(
    private settingsService: SettingsService,
    private location: Location
  ) {}

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
    this.initForm();
  }

  private handleError(error) {
    console.error(error);
  }

  initForm() {
    console.log('initForm()');

    this.settingsForm = new FormGroup({
      parts: new FormControl(this.settings.paths.parts, Validators.required),
      symbol: new FormControl(this.settings.paths.symbol, Validators.required),
      footprint: new FormControl(
        this.settings.paths.footprint,
        Validators.required
      ),
      output: new FormControl(this.settings.paths.output, Validators.required)
    });
    console.log('initForm: complete');
    console.log(this.settingsForm.value);
  }

  setPath(field: string) {
    console.log(`setPath(${field})`);
  }

  onSubmit(): void {
    console.log('onSubmit()');
    console.log(this.settingsForm.value);
    this.settings.paths = this.settingsForm.value;
    this.settingsService.updateSettings(this.settings);
    this.goBack();
  }

  goBack(): void {
    this.location.back();
  }
}
