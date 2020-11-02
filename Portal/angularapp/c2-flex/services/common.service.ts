import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import * as moment from 'moment';

@Injectable()
export class CommonService {


  constructor() { }

  getError(err: HttpErrorResponse): string {
    if (err.error != null && (err.error instanceof Error || err.error.message)) {
      return err.error.message;
    }
    if (typeof(err.error) === 'string') {
      return err.error;
    }
    if (err.statusText)  {
      return err.statusText;
    }
    return err.message;
  }

  toDateParameter(date: Date) {
    return moment(date).format('YYYY-MM-DD');
  }

}
