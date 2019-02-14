import { Component, OnInit, ViewChild } from '@angular/core';
import {
  MatPaginator,
  MatSort,
  MatSnackBar,
  MatSnackBarRef,
  SimpleSnackBar
} from '@angular/material';
import { PartsDataSource } from './parts-datasource';
import { PartService } from '../part.service';
import { Part } from '../part/part';
import { Router } from '@angular/router';
import { MessageService } from '../message.service';
import { LibraryService } from '../library.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-parts',
  templateUrl: './parts.component.html',
  styleUrls: ['./parts.component.css']
})
export class PartsComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: PartsDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = Object.keys(new Part()).slice(0, -1);

  private buildSnackBar?: MatSnackBarRef<SimpleSnackBar>;
  private subscription: Subscription = new Subscription();

  constructor(
    private partService: PartService,
    private libraryService: LibraryService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    console.log('ngOnInit');
    this.dataSource = new PartsDataSource(this.partService);
    this.dataSource.loadParts();

    this.subscription.add(
      this.libraryService.onBuildRunning.subscribe(
        next => {
          console.log('onBuildRunning: next');
          this.onBuildRunning(next);
        },
        error => {
          console.log('onBuildRunning: error');
          this.handleError(error);
        },
        () => console.log('onBuildRunning: complete')
      )
    );

    this.subscription.add(
      this.libraryService.onBuildComplete.subscribe(
        next => {
          console.log('onBuildComplete: next');
          this.onBuildComplete(next);
        },
        error => {
          console.log('onBuildComplete: error');
          this.handleError(error);
        },
        () => console.log('onBuildComplete: complete')
      )
    );

    this.subscription.add(
      this.libraryService.onBuildError.subscribe(
        next => {
          console.log('onBuildError: next');
          this.onBuildError(next);
        },
        error => {
          console.log('onBuildError: error');
          this.handleError(error);
        },
        () => console.log('onBuildError: complete')
      )
    );
  }
  onBuildError(next: any): any {
    if (this.buildSnackBar) {
      this.buildSnackBar.dismiss();
    }
    console.error(next);
    var error = '';
    if (next.message) {
      error = next.message;
    } else if (next.code) {
      switch (next.code) {
        case 'ENOENT':
          error = `No such file or directory: ${next.path}`;
          break;

        default:
          error = 'Unknown error. Refer to console!';
          break;
      }
    } else {
      error = 'Unknown error. Refer to console!';
    }

    this.buildSnackBar = this.snackBar.open(`Build error: ${error}`, 'OK');
  }
  onBuildComplete(next: string): any {
    if (this.buildSnackBar) {
      this.buildSnackBar.dismiss();
    }
    this.buildSnackBar = this.snackBar.open('Build complete.', 'OK', {
      duration: 3000
    });
  }
  onBuildRunning(next: string): any {
    this.buildSnackBar = this.snackBar.open('Build running.');
  }

  ngOnDestroy() {
    console.log('ngOnDestroy');
    this.subscription.unsubscribe();
  }
  private handleError(error) {
    console.error(error);
  }
  onRowClicked(row: Part) {
    this.router.navigateByUrl(`/part/${row.id}`);
  }

  addPart() {
    this.router.navigateByUrl(`/part/new`);
  }

  refresh() {
    this.dataSource.loadParts();
  }

  build() {
    this.libraryService.build();
  }
}
