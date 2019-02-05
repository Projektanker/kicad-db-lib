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
export class PartsDataSource extends DataSource<Part> {
  data = { length: 0 };

  private subscription: Subscription = new Subscription();

  private partsSubject = new BehaviorSubject<Part[]>(
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
  connect(): Observable<Part[]> {
    var sub = this.partService.onPartsChanged
      .pipe(
        catchError(error => {
          console.error(error);
          return of([]);
        }),
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
