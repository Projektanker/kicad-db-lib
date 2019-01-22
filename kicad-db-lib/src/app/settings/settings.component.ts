import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { SettingsService } from '../settings.service';
import { FormBuilder, FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { Settings } from './settings';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  settingsForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private settingsService: SettingsService,
    private location: Location) {
    console.log('Settiings constructor');
  }

  ngOnInit() {
    console.log('Settings ngOnInit');
    this.settingsService.getSettings().subscribe(
      settings => this.initForm(settings)
    );
  }

  initForm(settings: Settings) {
    console.log('Settings initForm');
    const customFields: FormControl[] = [];
    settings.customFields.forEach(field => {
      customFields.push(new FormControl(field, Validators.required));
    });
    this.settingsForm = new FormGroup({
      'customFields': new FormArray(customFields)
    });
    console.log(this.settingsForm.value);
  }

  addCustomField(): void {
    console.log('addCustomField()');
    const array: FormArray = this.settingsForm.get('customFields') as FormArray;
    array.push(new FormControl('', Validators.required));
  }

  removeCustomField(index: number): void {
    const array: FormArray = this.settingsForm.get('customFields') as FormArray;
    array.removeAt(index);
  }

  onSubmit(): void {
    console.warn(this.settingsForm.value);
    this.settingsService.updateSettings(this.settingsForm.value)
      .subscribe(() => this.goBack());
  }

  goBack(): void {
    this.location.back();
  }

}
