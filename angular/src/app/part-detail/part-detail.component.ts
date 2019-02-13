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
import { Subscription, of, Observable } from 'rxjs';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  filter,
  startWith,
  tap
} from 'rxjs/operators';
import { LibraryService } from '../library.service';

@Component({
  selector: 'app-part-detail',
  templateUrl: './part-detail.component.html',
  styleUrls: ['./part-detail.component.css']
})
export class PartDetailComponent implements OnInit {
  part: Part = null;
  add: Boolean = false;
  settings: Settings = null;
  libraries: string[] = [];
  filteredLibraries$: Observable<string[]> = null;
  symbols: string[] = [];
  filteredSymbols$: Observable<string[]> = null;
  footprints: string[] = [];
  filteredFootprints$: Observable<string[]> = null;

  private readonly maxItems = 100;

  subscription: Subscription = new Subscription();

  partForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private partService: PartService,
    private libraryService: LibraryService,
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

    // parts - onLibrariesChanged
    var sub = this.partService.onLibrariesChanged.subscribe(
      (next: string[]) => {
        console.log('onLibrariesChanged: next');
        this.onLibrariesChanged(next);
      },
      error => {
        console.log('onLibrariesChanged: error');
        this.handleError(error);
      },
      () => console.log('onLibrariesChanged: complete')
    );
    this.subscription.add(sub);

    // library - onSymbolsChanged
    var sub = this.libraryService.onSymbolsChanged.subscribe(
      (next: string[]) => {
        console.log('onSymbolsChanged: next');
        this.onSymbolsChanged(next);
      },
      error => {
        console.log('onSymbolsChanged: error');
        this.handleError(error);
      },
      () => console.log('onSymbolsChanged: complete')
    );
    this.subscription.add(sub);

    // library - onFootprintsChanged
    var sub = this.libraryService.onFootprintsChanged.subscribe(
      (next: string[]) => {
        console.log('onFootprintsChanged: next');
        this.onFootprintsChanged(next);
      },
      error => {
        console.log('onFootprintsChanged: error');
        this.handleError(error);
      },
      () => console.log('onFootprintsChanged: complete')
    );
    this.subscription.add(sub);

    this.getPart();
    this.getSettings();
    this.getLibraries();
    this.getSymbols();
    this.getFootprints();
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
  }

  private onPartChanged(part: Part) {
    console.log('onPartChanged(part: Part)');
    console.log(part);
    this.part = part;
    this.initForm();
  }

  private onLibrariesChanged(libraries: string[]): any {
    console.log('onLibrariesChanged(libraries: string[])');
    console.log(libraries);
    if (libraries.length == this.maxItems) {
      libraries.push('...');
    }
    this.libraries = libraries;
  }

  onSymbolsChanged(symbols: string[]): any {
    console.log('onSymbolsChanged(symbols: string[])');
    console.log(symbols);
    if (symbols.length == this.maxItems) {
      symbols.push('...');
    }
    this.symbols = symbols;
  }

  onFootprintsChanged(footprints: string[]): any {
    console.log('onFootprintsChanged(footprints: string[])');
    console.log(footprints);
    if (footprints.length == this.maxItems) {
      footprints.push('...');
    }
    this.footprints = footprints;
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

  getLibraries(
    filter: string = '',
    reload: boolean = true,
    max?: number
  ): void {
    this.partService.getLibraries(filter, reload, max ? max : this.maxItems);
  }

  getSymbols(filter: string = '', reload: boolean = true, max?: number): void {
    this.libraryService.getSymbols(filter, reload, max ? max : this.maxItems);
  }

  getFootprints(
    filter: string = '',
    reload: boolean = true,
    max?: number
  ): void {
    this.libraryService.getFootprints(
      filter,
      reload,
      max ? max : this.maxItems
    );
  }

  initForm(): void {
    if (!this.part) return;
    if (!this.settings) return;
    //if (!this.libraries) return;
    //if (!this.footprints) return;
    //if (!this.symbols) return;

    var pattern_default = '^[a-zA-Z0-9_\\-\\.:]*$';
    var patter_value = '^[a-zA-Z0-9_\\-\\.]*$';

    const customFieldsGroup: FormGroup = new FormGroup({});
    this.settings.customFields.forEach(field => {
      customFieldsGroup.addControl(field, new FormControl(''));
    });

    this.partForm = new FormGroup({
      id: new FormControl(null),
      library: new FormControl('', [
        Validators.required,
        Validators.pattern(pattern_default)
      ]),
      reference: new FormControl('', [
        Validators.required,
        Validators.pattern('^[A-Z]*$')
      ]),
      value: new FormControl('', [
        Validators.required,
        Validators.pattern(patter_value)
      ]),
      symbol: new FormControl('', [
        Validators.required,
        Validators.pattern(pattern_default)
      ]),
      footprint: new FormControl('', [
        Validators.required,
        Validators.pattern(pattern_default)
      ]),
      description: new FormControl('', Validators.required),
      keywords: new FormControl(''),
      datasheet: new FormControl(''),
      customFields: customFieldsGroup
    });

    this.subscription.add(
      this.partForm.controls['library'].valueChanges
        .pipe(
          startWith(this.part.library),
          // wait 300ms after each keystroke before considering the term
          debounceTime(300),
          // To lower case and trim
          map((value: string) => value.toUpperCase().trim()),
          // ignore new term if same as previous term
          distinctUntilChanged(),
          // filter
          tap((x: string) => this.getLibraries(x, false))
        )
        .subscribe()
    );

    this.subscription.add(
      this.partForm.controls['symbol'].valueChanges
        .pipe(
          startWith(this.part.symbol),
          // wait 300ms after each keystroke before considering the term
          debounceTime(300),
          // To lower case and trim
          map((value: string) => value.toUpperCase().trim()),
          // ignore new term if same as previous term
          distinctUntilChanged(),
          // filter
          tap((x: string) => this.getSymbols(x, false))
        )
        .subscribe()
    );
    this.subscription.add(
      this.partForm.controls['footprint'].valueChanges
        .pipe(
          startWith(this.part.footprint),
          // wait 300ms after each keystroke before considering the term
          debounceTime(300),
          // trim
          map((value: string) => value.trim()),
          // ignore new term if same as previous term
          distinctUntilChanged(),
          // filter
          tap((x: string) => this.getFootprints(x, false))
        )
        .subscribe()
    );

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
