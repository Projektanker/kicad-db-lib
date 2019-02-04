import { Component, OnInit } from '@angular/core';
import { FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { SettingsService } from '../settings.service';
import { Location } from '@angular/common';
import { Settings } from '../../../../shared/settings/settings';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-settings-paths',
  templateUrl: './settings-paths.component.html',
  styleUrls: ['./settings-paths.component.css']
})
export class SettingsPathsComponent implements OnInit {
  settingsForm: FormGroup = null;
  settings: Settings;

  constructor(
    private settingsService: SettingsService,
    private location: Location,
    private msg: MessageService
  ) {}

  ngOnInit() {
    console.log('ngOnInit');

    this.initForm();

    this.settingsService.getSettings().subscribe(
      settings => {
        console.log('ngOnInit: next');
        console.log(settings);
        this.settings = settings;
        this.settingsForm.setValue(settings.paths);
        //this.initForm();
      },
      (error: any) => {
        console.log('ngOnInit: error');
        console.error(error);
      },
      () => {
        console.log('ngOnInit: complete');
      }
    );
  }

  initForm() {
    console.log('initForm: start');

    this.settingsForm = new FormGroup({
      parts: new FormControl('this.settings.paths.parts', Validators.required),
      symbol: new FormControl(
        'this.settings.paths.symbol',
        Validators.required
      ),
      footprint: new FormControl(
        'this.settings.paths.footprint',
        Validators.required
      ),
      output: new FormControl('this.settings.paths.output', Validators.required)
    });
    console.log('initForm: complete');
    console.log(this.settingsForm.value);
  }

  setPath(field: string) {
    this.msg.add(`setPath(${field})`);
  }

  onSubmit(): void {
    console.log('onSubmit');
    console.log(this.settingsForm.value);
    this.settings.paths = this.settingsForm.value;
    this.settingsService
      .updateSettings(this.settings)
      .subscribe(() => this.goBack());
  }

  goBack(): void {
    this.location.back();
  }
}
