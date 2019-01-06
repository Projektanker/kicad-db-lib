import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { Part } from './part/part';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class PartService {
  private partsUrl = 'api/parts';  // URL to web api

  constructor(
    private http: HttpClient,
    private messageService: MessageService) { }

  /** Log a PartService message with the MessageService */
  private log(message: string) {
    this.messageService.add(`PartService: ${message}`);
  }
  getParts(): Observable<Part[]> {
    this.log('PartService: fetched parts');
    return this.http.get<Part[]>(this.partsUrl)
      .pipe(
        tap(_ => this.log('fetched parts')),
        catchError(this.handleError('getParts', []))
      );
  }

  /** GET part by id. Will 404 if id not found */
  getPart(id: number): Observable<Part> {
    const url = `${this.partsUrl}/${id}`;
    return this.http.get<Part>(url).pipe(
      tap(_ => this.log(`fetched part id=${id}`)),
      catchError(this.handleError<Part>(`getPart id=${id}`))
    );
  }

  /** PUT: update the part on the server */
  updatePart(part: Part): Observable<any> {
    return this.http.put(this.partsUrl, part, httpOptions).pipe(
      tap(_ => this.log(`updated part id=${part.id}`)),
      catchError(this.handleError<any>('updatePart'))
    );
  }

  /** POST: add a new part to the server */
  addPart(part: Part): Observable<Part> {
    return this.http.post<Part>(this.partsUrl, part, httpOptions).pipe(
      tap((addedPart: Part) => this.log(`added part w/ id=${addedPart.id}`)),
      catchError(this.handleError<Part>('addPart'))
    );
  }

  /** DELETE: delete the part from the server */
  deletePart(part: Part | number): Observable<Part> {
    const id = typeof part === 'number' ? part : part.id;
    const url = `${this.partsUrl}/${id}`;

    return this.http.delete<Part>(url, httpOptions).pipe(
      tap(_ => this.log(`deleted part id=${id}`)),
      catchError(this.handleError<Part>('deletePart'))
    );
  }

  /* GET parts whose name contains search term */
  searchParts(term: string): Observable<Part[]> {
    if (!term.trim()) {
      // if not search term, return empty part array.
      return of([]);
    }
    return this.http.get<Part[]>(`${this.partsUrl}/?value=${term}`).pipe(
      tap(_ => this.log(`found parts matching "${term}"`)),
      catchError(this.handleError<Part[]>('searchParts', []))
    );
  }

  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
