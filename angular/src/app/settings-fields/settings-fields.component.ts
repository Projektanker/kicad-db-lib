import { Component, OnInit } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { SettingsService } from '../settings.service';
import { Location } from '@angular/common';
import { Settings } from '../../../../shared/settings/settings';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-settings-fields',
  templateUrl: './settings-fields.component.html',
  styleUrls: ['./settings-fields.component.css']
})
export class SettingsFieldsComponent implements OnInit {
  settingsForm: FormGroup;
  settings: Settings;
  subscription: Subscription = new Subscription();

  get customFields(): FormArray {
    if (this.settingsForm)
      return this.settingsForm.get('customFields') as FormArray;
    else return null;
  }
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
    //this.ref.detectChanges();
  }

  private handleError(error) {
    console.error(error);
  }

  initForm() {
    console.log('initForm()');

    const customFields: FormControl[] = [];
    for (const field of this.settings.customFields) {
      customFields.push(new FormControl(field, Validators.required));
    }
    this.settingsForm = new FormGroup({
      customFields: new FormArray(customFields)
    });
    console.log(this.settingsForm.value);
  }

  addCustomField(): void {
    this.customFields.push(new FormControl('', Validators.required));
  }

  removeCustomField(index: number): void {
    this.customFields.removeAt(index);
  }

  onSubmit(): void {
    console.log('onSubmit()');
    console.log(this.settingsForm.value);
    this.settings.customFields = this.settingsForm.controls[
      'customFields'
    ].value;
    this.settingsService.updateSettings(this.settings);
    this.goBack();
  }

  goBack(): void {
    this.location.back();
  }
}
