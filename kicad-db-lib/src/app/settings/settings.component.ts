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
    private location: Location) { }

  ngOnInit() {
    this.settingsService.getSettings().subscribe(
      settings => this.initForm(settings)
    );
  }

  initForm(settings: Settings) {
    const customFields: FormControl[] = [];
    settings.customFields.forEach(field => {
      customFields.push(new FormControl(field, Validators.required));
    });
    this.settingsForm.addControl('customFields', new FormArray(customFields));
  }

  addCustomField(): void {
    const array: FormArray = this.settingsForm.get['customFields'] as FormArray;
    array.push(new FormControl('', Validators.required));
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
