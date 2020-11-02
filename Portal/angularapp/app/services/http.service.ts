import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';

import { HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { HttpErrorResponse } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { BlockUIService } from '../../common_components/services/block-ui.service';



@Injectable()
export class HttpService {

    constructor(private http: HttpClient, private blockUIService: BlockUIService) {
    }

    private token = '';

    set Token(value: string) {
        this.token = value;
        if (value == null) {
            localStorage.removeItem('c2_cust_portal_token');
        } else {
            localStorage.setItem('c2_cust_portal_token', value);
        }
    }
    get Token(): string {
        return localStorage.getItem('c2_cust_portal_token');
    }

    clearToken() {
        localStorage.removeItem('c2_cust_portal_token');
    }

    public post<T>(url: string, object?: any): Observable<T> {

        this.blockUIService.startBlock();


        return this.http.post<T>(url, object, {headers: this.BuildHeaders()}).pipe(
          tap(
            obj => this.blockUIService.stopBlock(),
            obj => this.blockUIService.stopBlock()
          )
        );

    }


    public postNoBlock<T>(url: string, object: any): Observable<T> {

      return this.http.post<T>(url, object, {headers: this.BuildHeaders()});
    }

    public get<T>(url: string, options?: any): Observable<any> {


      this.blockUIService.startBlock();

      if (options == null) {
          options = {};
      }


      options.headers = this.BuildHeaders();

      return this.http.get<T>(url, options).pipe(
        tap(
          obj => this.blockUIService.stopBlock(),
          obj => this.blockUIService.stopBlock()
        ));
    }

    public getNoBlock<T>(url: string, options?: any): Observable<any> {
        if (options == null) {
            options = {};
        }

        options.headers = this.BuildHeaders();
        return this.http.get<T>(url, options);
    }


    private handleError(error: any, blockUIService: BlockUIService, blocking: Boolean) {

        const body = error.json();

        if (blocking) {
            blockUIService.blockUIEvent.emit({
                value: false
            });
        }

        return Observable.throw(body);

    }

    private parseResponse(response: Response, blockUIService: BlockUIService, blocking: Boolean) {

        const authorizationToken = response.headers.get('Authorization');
        if (authorizationToken != null) {

            if (typeof (Storage) !== 'undefined') {
                localStorage.setItem('authToken', authorizationToken);
            }
        }

        if (blocking) {
            blockUIService.blockUIEvent.emit({
                value: false
            });
        }

        const body = response.json();

        return body;
    }

    private BuildHeaders(): HttpHeaders {
      let headers = new HttpHeaders();
      headers = headers.append('Content-Type', 'application/json; charset=utf-8');
      headers = headers.append('Accept', 'q=0.8;application/json;q=0.9');
      const token = this.Token;
      if (token != null && token.length > 0) {
          headers = headers.append('Authorization', 'Bearer ' + token);
      }

      return headers;
    }


}
