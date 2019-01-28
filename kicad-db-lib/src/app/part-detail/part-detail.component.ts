import { Component, OnInit } from "@angular/core";
import {
  FormBuilder,
  Validators,
  FormGroup,
  FormControl
} from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";
import { PartService } from "../part.service";
import { Part } from "../part/part";
import { SettingsService } from "../settings.service";
import { Settings } from "../settings/settings";
import { MatDialog } from "@angular/material";
import { DiscardChangesDialogComponent } from "../discard-changes-dialog/discard-changes-dialog.component";

@Component({
  selector: "app-part-detail",
  templateUrl: "./part-detail.component.html",
  styleUrls: ["./part-detail.component.css"]
})
export class PartDetailComponent implements OnInit {
  part: Part;
  add: Boolean = false;
  settings: Settings;

  partForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private partService: PartService,
    private settingsService: SettingsService,
    private location: Location,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.getPart();
  }

  getPart(): void {
    let idString = this.route.snapshot.paramMap.get("id");
    if (idString == "new") {
      this.add = true;
      this.part = new Part();
      this.getSettings();
    } else {
      const id = +idString;
      this.partService.getPart(id).subscribe((newPart: Part) => {
        this.part = newPart;
        this.getSettings();
      });
    }
  }

  getSettings(): void {
    this.settingsService.getSettings().subscribe(settings => {
      this.settings = settings;
      this.initForm();
    });
  }

  initForm(): void {
    const customFieldsGroup: FormGroup = new FormGroup({});
    this.settings.customFields.forEach(field => {
      customFieldsGroup.addControl(
        field,
        new FormControl("", Validators.required)
      );
    });

    this.partForm = new FormGroup({
      id: new FormControl(null),
      reference: new FormControl("", Validators.required),
      value: new FormControl(""),
      footprint: new FormControl(""),
      symbol: new FormControl(""),
      library: new FormControl(""),
      datasheet: new FormControl(""),
      description: new FormControl(""),
      keywords: new FormControl(""),
      customFields: customFieldsGroup
    });

    console.warn(this.partForm.value);
    this.partForm.patchValue(this.part);
  }

  onSubmit(): void {
    console.warn("SUBMIT");
    console.warn(this.partForm.value);
    this.part = this.partForm.value;
    if (this.add) {
      this.partService.addPart(this.part).subscribe(() => this.location.back());
    } else {
      this.partService
        .updatePart(this.part)
        .subscribe(() => this.location.back());
    }
  }

  goBack(): void {
    if (!this.partForm.dirty) {
      this.location.back();
    } else {
      const dialogRef = this.dialog.open(DiscardChangesDialogComponent);
      dialogRef.afterClosed().subscribe(result => {
        console.log(`The dialog was closed ${result}`);
        if (result) {
          this.location.back();
        }
      });
    }
  }
}
