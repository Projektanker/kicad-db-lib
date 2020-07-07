import { Injectable, NgZone } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
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

  private onBuildRunningSubject: Subject<string> = new Subject<string>();
  private onBuildCompleteSubject: Subject<string> = new Subject<string>();
  private onBuildErrorSubject: Subject<any> = new Subject<any>();

  get onBuildRunning(): Observable<string> {
    return this.onBuildRunningSubject.asObservable();
  }

  get onBuildComplete(): Observable<string> {
    return this.onBuildCompleteSubject.asObservable();
  }

  get onBuildError(): Observable<any> {
    return this.onBuildErrorSubject.asObservable();
  }

  constructor(
    private http: HttpClient,
    private zone: NgZone
  ) {    
  }

  private ipcSymbolsChanged(event: any, arg: any): any {
    console.log('ipcSymbolsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onSymbolsChangedSubject.next(arg);
    });
  }

  private ipcFootprintsChanged(event: any, arg: any): any {
    console.log('ipcFootprintsChanged(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onFootprintsChangedSubject.next(arg);
    });
  }

  private ipcBuildRunning(event: any, arg: any): any {
    console.log('ipcBuildRunning(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onBuildRunningSubject.next(arg);
    });
  }

  private ipcBuildComplete(event: any, arg: any): any {
    console.log('ipcBuildComplete(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onBuildCompleteSubject.next(arg);
    });
  }

  private ipcBuildError(event: any, arg: any): any {
    console.log('ipcBuildError(event, arg)');
    console.log(event);
    console.log(arg);
    this.zone.run(() => {
      this.onBuildErrorSubject.next(arg);
    });
  }

  getSymbols(filter: string, reload: boolean, max?: number): void {
    console.log('getSymbols()');    
    this.http
      .get<string[]>(this.symbolsUrl)
      .subscribe(
        next => this.onSymbolsChangedSubject.next(next),
        error => this.handleError(error)
      );
    
  }

  getFootprints(filter: string, reload: boolean, max?: number): void {
    console.log('getFootprints()');
    this.http
      .get<string[]>(this.footprintsUrl)
      .subscribe(
        next => this.onFootprintsChangedSubject.next(next),
        error => this.handleError(error)
      );
    
  }

  build() {
    console.log('build()');    
      this.ipcBuildError(null, new Error('Not implemented!'));    
  }

  private handleError(error) {
    console.error(error);
  }
}
