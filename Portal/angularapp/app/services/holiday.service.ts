import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs/Observable';
import { Customer, Invoice, Holiday } from '../domainclasses';
import { SortDirection } from '../../common_components/components';

@Injectable()
export class HolidayService {

  constructor(private httpService: HttpService) { }

  api = 'api/admin/';

  getHolidays(year: number) {
    return this.httpService.get(this.api + 'holidays', {params: { year: year}});
  }
  saveHoliday(holiday: Holiday) {
    return this.httpService.post(this.api + 'insertHoliday', holiday);
  }
  deleteHoliday(holiday: Holiday) {
    return this.httpService.post(this.api + 'deleteHoliday', holiday);
  }
}
