import { Injectable, NgZone } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ElectronService } from 'ngx-electron';

@Injectable({
  providedIn: 'root'
})
export class LibraryService {
  private symbolsUrl = 'api/symbols'; // URL to web api
  private footprintsUrl = 'api/footprints'; // URL to web api

  private onSymbolsChangedSubject: Subject<string[]> = new Subject<string[]>();
  private onFootprintsChangedSubject: Subject<string[]> = new Subject<
    string[]
  >();

  get onSymbolsChanged(): Observable<string[]> {
    return this.onSymbolsChangedSubject.asObservable();
  }

  get onFootprintsChanged(): Observable<string[]> {
    return this.onFootprintsChangedSubject.asObservable();
  }

  constructor(
    private http: HttpClient,
    private electronService: ElectronService,
    private zone: NgZone
  ) {
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.on(
        'library.getSymbols',
        (event: any, arg: any) => this.ipcSymbolsChanged(event, arg)
      );

      this.electronService.ipcRenderer.on(
        'library.getFootprints',
        (event: any, arg: any) => this.ipcFootprintsChanged(event, arg)
      );
    }
  }

  ipcSymbolsChanged(event: any, arg: any): any {
    console.log('ipcSymbolsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onSymbolsChangedSubject.next(arg);
    });
  }

  ipcFootprintsChanged(event: any, arg: any): any {
    console.log('ipcFootprintsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onFootprintsChangedSubject.next(arg);
    });
  }

  getSymbols(): void {
    console.log('getSymbols()');
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('library.getSymbols');
    } else {
      this.http
        .get<string[]>(this.symbolsUrl)
        .subscribe(
          next => this.onSymbolsChangedSubject.next(next),
          error => this.handleError(error)
        );
    }
  }

  getFootprints(): void {
    console.log('getFootprints()');
    if (this.electronService.isElectronApp) {
      this.electronService.ipcRenderer.send('library.getFootprints');
    } else {
      this.http
        .get<string[]>(this.footprintsUrl)
        .subscribe(
          next => this.onFootprintsChangedSubject.next(next),
          error => this.handleError(error)
        );
    }
  }

  private handleError(error) {
    console.error(error);
  }
}
