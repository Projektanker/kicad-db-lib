import { Component, OnInit } from "@angular/core";
import { Location } from "@angular/common";
import { SettingsService } from "../settings.service";
import {
  FormBuilder,
  FormGroup,
  FormArray,
  FormControl,
  Validators
} from "@angular/forms";
import { Settings } from "./settings";

@Component({
  selector: "app-settings",
  templateUrl: "./settings.component.html",
  styleUrls: ["./settings.component.css"]
})
export class SettingsComponent implements OnInit {
  settings: Settings;

  constructor(
    private settingsService: SettingsService,
    private location: Location
  ) {
    console.log("Settings constructor");
  }

  ngOnInit() {
    console.log("Settings ngOnInit");
    this.settingsService
      .getSettings()
      .subscribe(settings => (this.settings = settings));
  }

  goBack() {
    this.location.back();
  }
}
