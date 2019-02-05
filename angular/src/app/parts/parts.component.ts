import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { PartsDataSource } from './parts-datasource';
import { PartService } from '../part.service';
import { Part } from '../part/part';
import { Router } from '@angular/router';
import { MessageService } from '../message.service';

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

  constructor(private partService: PartService, private router: Router) {}

  ngOnInit() {
    this.dataSource = new PartsDataSource(this.partService);
    this.dataSource.loadParts();
  }

  onRowClicked(row: Part) {
    this.router.navigateByUrl(`/part/${row.id}`);
  }

  addPart() {
    this.router.navigateByUrl(`/part/new`);
  }
}
