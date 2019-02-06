import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  Validators,
  FormGroup,
  FormControl
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { PartService } from '../part.service';
import { Part } from '../part/part';
import { SettingsService } from '../settings.service';
import { Settings } from '../../../../shared/settings/settings';
import { MatDialog } from '@angular/material';
import { DiscardChangesDialogComponent } from '../discard-changes-dialog/discard-changes-dialog.component';
import { MessageService } from '../message.service';
import { Subscription } from 'rxjs';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-part-detail',
  templateUrl: './part-detail.component.html',
  styleUrls: ['./part-detail.component.css']
})
export class PartDetailComponent implements OnInit {
  part: Part = null;
  add: Boolean = false;
  settings: Settings = null;

  subscription: Subscription = new Subscription();

  partForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private partService: PartService,
    private settingsService: SettingsService,
    private location: Location,
    public dialog: MatDialog
  ) {
    console.log('constructor');
  }

  ngOnInit() {
    // settings - onSettingsChanged
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
    // parts - onPartChanged
    var sub = this.partService.onPartChanged.subscribe(
      (next: Part) => {
        console.log('onPartChanged: next');
        this.onPartChanged(next);
      },
      error => {
        console.log('onPartChanged: error');
        this.handleError(error);
      },
      () => console.log('onPartChanged: complete')
    );
    this.subscription.add(sub);

    this.getPart();
    this.getSettings();
  }

  ngOnDestroy() {
    console.log('ngOnInit');
    console.log('ngOnDestroy');
    this.subscription.unsubscribe();
  }

  private onSettingsChanged(settings: Settings) {
    console.log('onSettingsChanged(settings: Settings)');
    console.log(settings);
    this.settings = settings;
    this.initForm();
  }

  private onPartChanged(part: Part) {
    console.log('onPartChanged(part: Part)');
    console.log(part);
    this.part = part;
    this.initForm();
  }

  private handleError(error) {
    console.error(error);
  }

  getPart(): void {
    let idString = this.route.snapshot.paramMap.get('id');
    if (idString == 'new') {
      this.add = true;
      this.onPartChanged(new Part());
    } else {
      const id = +idString;
      this.partService.getPart(id);
    }
  }

  getSettings(): void {
    this.settingsService.getSettings();
  }

  initForm(): void {
    if (!this.part) return;
    if (!this.settings) return;

    const customFieldsGroup: FormGroup = new FormGroup({});
    this.settings.customFields.forEach(field => {
      customFieldsGroup.addControl(field, new FormControl(''));
    });

    this.partForm = new FormGroup({
      id: new FormControl(null),
      reference: new FormControl('', Validators.required),
      value: new FormControl('', Validators.required),
      footprint: new FormControl('', Validators.required),
      symbol: new FormControl('', Validators.required),
      library: new FormControl('', Validators.required),
      datasheet: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      keywords: new FormControl('', Validators.required),
      customFields: customFieldsGroup
    });

    console.log(this.partForm.value);
    this.partForm.patchValue(this.part);
  }

  onSubmit(): void {
    console.log('onSubmit()');
    console.log(this.partForm.value);
    this.part = this.partForm.value;
    if (this.add) {
      this.partService.addPart(this.part);
      this.location.back();
    } else {
      this.partService.updatePart(this.part);
      this.location.back();
    }
  }

  delete(): void {
    console.log('delete()');
    const dialogRef = this.dialog.open(DeleteDialogComponent);
    dialogRef.afterClosed().subscribe(result => {
      console.log(`The dialog was closed ${result}`);
      if (result) {
        this.partService.deletePart(this.part);
        this.location.back();
      }
    });
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
