import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { PartService } from '../part.service';
import { Part } from '../part/part';
import { SettingsService } from '../settings.service';
import { Settings } from '../settings/settings';

@Component({
  selector: 'app-part-detail',
  templateUrl: './part-detail.component.html',
  styleUrls: ['./part-detail.component.css']
})
export class PartDetailComponent implements OnInit {
  part: Part;
  settings: Settings;

  partForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private partService: PartService,
    private settingsService: SettingsService,
    private location: Location) { }

  ngOnInit() {
    this.getPart();
  }

  getPart(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.partService.getPart(id)
      .subscribe((newPart: Part) => {
        this.part = newPart;
        this.getSettings();
      });
  }

  getSettings(): void {
    this.settingsService.getSettings()
      .subscribe(settings => {
        this.settings = settings;
        this.initForm();
      });
  }

  initForm(): void {
    const customFieldsGroup: FormGroup = new FormGroup({});
    this.settings.customFields.forEach(field => {
      customFieldsGroup.addControl(field, new FormControl('', Validators.required));
    });

    this.partForm = new FormGroup({
      'id': new FormControl(null),
      'reference': new FormControl('', Validators.required),
      'value': new FormControl(''),
      'footprint': new FormControl(''),
      'symbol': new FormControl(''),
      'library': new FormControl(''),
      'datasheet': new FormControl(''),
      'description': new FormControl(''),
      'keywords': new FormControl(''),
      'customFields': customFieldsGroup,
    });

    console.warn(this.partForm.value);
    this.partForm.patchValue(this.part);
  }

  onSubmit(): void {
    console.warn('SUBMIT');
    console.warn(this.partForm.value);
    this.part = this.partForm.value;
    this.partService.updatePart(this.part)
      .subscribe(() => this.goBack());
  }

  goBack(): void {
    console.warn('BACK');
    this.location.back();
  }
}
