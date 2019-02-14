import { DataSource } from '@angular/cdk/collections';
import { MatPaginator, MatSort } from '@angular/material';
import {
  map,
  tap,
  catchError,
  finalize,
  delay,
  subscribeOn
} from 'rxjs/operators';
import {
  Observable,
  of as observableOf,
  merge,
  BehaviorSubject,
  of,
  Subscription
} from 'rxjs';
import { Part } from '../part/part';
import { PartService } from '../part.service';

/**
 * Data source for the Parts view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class PartsDataSource extends DataSource<{ [name: string]: string }> {
  data = { length: 0 };
  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = Object.keys(new Part()).slice(0, -1);
  private subscription: Subscription = new Subscription();

  private partsSubject = new BehaviorSubject<{ [name: string]: string }[]>(
    []
    /*[
    {
      id: 11,
      reference: 'R',
      value: '1K',
      footprint: 'Resistor_SMD:R_0603_1608Metric',
      library: 'R_0603',
      description: 'Resistor 1K 0603 75V',
      keywords: 'Res Resistor 1K 0603',
      symbol: '{lib}:R',
      datasheet: 'no datasheet',
      customFields: { OC_FARNELL: '123465', OC_MOUSER: 'm123465' }
    }
  ]*/
  );
  private loadingSubject = new BehaviorSubject<boolean>(false);

  public loading$ = this.loadingSubject.asObservable();

  constructor(private partService: PartService) {
    super();
    this.partsSubject.subscribe(parts => {
      console.log(parts);
    });
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect(): Observable<{ [name: string]: string }[]> {
    var sub = this.partService.onPartsChanged
      .pipe(
        catchError(error => {
          console.error(error);
          return of([]);
        }),
        map((parts: Part[]) => parts.map((part: Part) => this.mapPart(part))),
        tap(parts => {
          console.log(`tap: ${parts.length}`);
          this.data.length = parts.length;
          this.loadingSubject.next(false);
        })
      )
      .subscribe(parts => {
        console.log(`next: ${parts}`);
        this.partsSubject.next(parts);
      });

    this.subscription.add(sub);
    return this.partsSubject.asObservable();
  }

  mapPart(part: Part): { [name: string]: string } {
    if (part.datasheet) {
      var start = part.datasheet.indexOf('\\') >= 0 ? '..\\' : '../';
      part.datasheet = start + part.datasheet.split(/(\\|\/)/g).pop();
    }
    var item: { [name: string]: string } = {};
    for (const key in part) {
      if (part.hasOwnProperty(key)) {
        if (key !== 'customFields') {
          item[key] = part[key];
        } else {
          for (const customField in part.customFields) {
            if (part.customFields.hasOwnProperty(customField)) {
              item[customField] = part.customFields[customField];
              // add to displayed columns
              if (this.displayedColumns.indexOf(customField) < 0) {
                this.displayedColumns = [...this.displayedColumns, customField];
              }
            }
          }
        }
      }
    }
    return item;
  }

  loadParts() {
    this.loadingSubject.next(true);
    this.partService.getParts();
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect() {
    this.partsSubject.complete();
    this.loadingSubject.complete();
    this.subscription.unsubscribe();
  }
}
