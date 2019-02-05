import { Injectable, NgZone } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of, Subject } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { Part } from './part/part';
import { ElectronService } from 'ngx-electron';
import { element } from '@angular/core/src/render3';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class PartService {
  private partsUrl = 'api/parts'; // URL to web api

  private onPartChangedSubject: Subject<Part> = new Subject<Part>();
  private onPartsChangedSubject: Subject<Part[]> = new Subject<Part[]>();

  get onPartChanged(): Observable<Part> {
    return this.onPartChangedSubject.asObservable();
  }

  get onPartsChanged(): Observable<Part[]> {
    return this.onPartsChangedSubject.asObservable();
  }

  constructor(
    private http: HttpClient,
    private electronService: ElectronService,
    private zone: NgZone
  ) {
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.on(
        'part.partsChanged',
        (event: any, arg: any) => this.ipcPartsChanged(event, arg)
      );

      this.electronService.ipcRenderer.on(
        'part.partChanged',
        (event: any, arg: any) => this.ipcPartChanged(event, arg)
      );
    }
  }
  private ipcPartsChanged(event: any, arg: any) {
    console.log('ipcPartsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onPartsChangedSubject.next(arg);
    });
  }

  private ipcPartChanged(event: any, arg: any) {
    console.log('ipcPartsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onPartChangedSubject.next(arg);
    });
  }

  getParts() {
    console.log('getParts()');
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('part.getParts');
    } else {
      this.http
        .get<Part[]>(this.partsUrl)
        .subscribe(
          next => this.onPartsChangedSubject.next(next),
          error => this.handleError(error)
        );
    }
  }

  /** GET part by id. Will 404 if id not found */
  getPart(id: number) {
    console.log(`getPart(${id})`);
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('part.getPart', id);
    } else {
      const url = `${this.partsUrl}/${id}`;
      this.http
        .get<Part>(url)
        .subscribe(
          next => this.onPartChangedSubject.next(next),
          error => this.handleError(error)
        );
    }
  }

  /** PUT: update the part on the server */
  updatePart(part: Part) {
    console.log('updatePart(part: Part)');
    this.http
      .put(this.partsUrl, part, httpOptions)
      .subscribe(() => {}, error => this.handleError(error));
  }

  /** POST: add a new part to the server */
  addPart(part: Part) {
    console.log('addPart(part: Part)');
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('part.addPart', part);
    } else {
      this.http
        .post<Part>(this.partsUrl, part, httpOptions)
        .subscribe(() => {}, error => this.handleError(error));
    }
  }

  /** DELETE: delete the part from the server */
  deletePart(part: Part | number) {
    console.log('deletePart(part: Part | number)');
    const id = typeof part === 'number' ? part : part.id;
    const url = `${this.partsUrl}/${id}`;

    this.http
      .delete<Part>(url, httpOptions)
      .subscribe(() => {}, error => this.handleError(error));
  }

  private handleError(error) {
    console.error(error);
  }
}
