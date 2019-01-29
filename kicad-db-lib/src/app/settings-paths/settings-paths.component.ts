import { Component, OnInit } from "@angular/core";
import { FormGroup, FormArray, FormControl, Validators } from "@angular/forms";
import { SettingsService } from "../settings.service";
import { Location } from "@angular/common";
import { Settings } from "../settings/settings";

@Component({
  selector: "app-settings-paths",
  templateUrl: "./settings-paths.component.html",
  styleUrls: ["./settings-paths.component.css"]
})
export class SettingsPathsComponent implements OnInit {
  settingsForm: FormGroup;
  settings: Settings;

  constructor(
    private settingsService: SettingsService,
    private location: Location
  ) {}

  ngOnInit() {
    this.settingsService.getSettings().subscribe(settings => {
      this.settings = settings;
      this.initForm();
    });
  }

  initForm() {
    console.log("Settings initForm");

    this.settingsForm = new FormGroup({
      symbol: new FormControl(null),
      footprint: new FormControl(null, Validators.required),
      output: new FormControl(null)
    });

    this.settingsForm.setValue(this.settings.paths);
    console.log(this.settingsForm.value);
  }

  setPath(field: string) {
    console.log(`setPath(${field})`);
  }

  onSubmit(): void {
    console.warn(this.settingsForm.value);
    this.settings.paths = this.settingsForm.value;
    this.settingsService
      .updateSettings(this.settings)
      .subscribe(() => this.goBack());
  }

  goBack(): void {
    this.location.back();
  }
}
