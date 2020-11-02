import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs/Observable';
import { Order } from '../domainclasses';
import { SortDirection } from '../../common_components/components';
import { CommonService } from '../../c2-flex/services/common.service';

@Injectable()
export class OrderService {

  constructor(private httpService: HttpService, private commonService: CommonService) { }

  api = 'api/order/';

  getOrder(order_no: string, customer_code?: string) {
    return this.httpService.get(this.api, { params: {order_no: order_no, customer_code: customer_code}});
  }
  getCustomer(cust: string) {
    return this.httpService.get(this.api + '/getSlCustomer', {params: {customer: cust}});
  }
  getOrders(customer: string, dateFrom: Date, dateTo: Date, from: number, to: number,
    sortBy: string, direction: SortDirection, searchText: string) {
    return this.httpService.get(this.api + 'orders', { params: { customer: customer,
      dateFrom: this.commonService.toDateParameter(dateFrom),
      dateTo: this.commonService.toDateParameter(dateTo),
      from: from, to: to, sortBy: sortBy, direction: direction, searchText: searchText }});
  }

  getDetails(order_no: string) {
    return this.httpService.get(this.api + 'details', { params: {order_no: order_no}});
  }

  getOrderTotals(customer: string, dateFrom: Date, dateTo: Date, searchText: string) {
    return this.httpService.get(this.api + 'getOrderTotals', { params: { customer: customer,
      dateFrom: this.commonService.toDateParameter(dateFrom),
      dateTo: this.commonService.toDateParameter(dateTo),
      searchText: searchText }});
  }

}
