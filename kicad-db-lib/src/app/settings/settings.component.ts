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
import { MessageService } from "../message.service";

@Component({
  selector: "app-settings",
  templateUrl: "./settings.component.html",
  styleUrls: ["./settings.component.css"]
})
export class SettingsComponent implements OnInit {
  settings: Settings;

  constructor(
    private settingsService: SettingsService,
    private msg: MessageService,
    private location: Location
  ) {
    this.msg.add("Settings constructor");
  }

  ngOnInit() {
    this.msg.add("Settings ngOnInit");
    this.settingsService
      .getSettings()
      .subscribe(settings => (this.settings = settings));
  }

  goBack() {
    this.location.back();
  }
}
