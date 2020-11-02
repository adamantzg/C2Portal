import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Observable } from 'rxjs/Observable';
import { Customer, Invoice } from '../domainclasses';
import { SortDirection } from '../../common_components/components';
import { CommonService } from '../../c2-flex/services/common.service';

@Injectable()
export class InvoiceService {

  constructor(private httpService: HttpService, private commonService: CommonService) { }

  api = 'api/invoicehistory/';

  getCustomerTotals(customer: string, dateFrom: Date, dateTo: Date, searchText: string) {
    return this.httpService.get(this.api + 'getCustomerTotals', { params: {customer: customer,
      dateFrom: this.commonService.toDateParameter(dateFrom),
      dateTo: this.commonService.toDateParameter(dateTo),
      searchText: searchText}});
  }

  getInvoices(customer: string, dateFrom: Date, dateTo: Date, from: number, to: number,
    sortBy: string, direction: SortDirection, searchText: string) {
    return this.httpService.get(this.api + 'invoices', { params: { customer: customer,
      dateFrom: this.commonService.toDateParameter(dateFrom),
      dateTo: this.commonService.toDateParameter(dateTo),
      from: from, to: to, sortBy: sortBy, direction: direction, searchText: searchText }});
  }

  getCustomer(customer: string) {
    return this.httpService.get(this.api + 'customer', { params: {code: customer}});
  }

  getInvoicePdf(order_no: string) {
    return this.httpService.post(this.api + 'invoicepdf?order_no=' + order_no);
  }

  getInvoicePdfs(order_nos: string) {
    return this.httpService.post(this.api + 'invoicepdfs?order_nos=' + order_nos);
  }

  checkInvoicePod(order_no: string, customer: string, customer_order_no: string, postcode: string) {
    return this.httpService.get(this.api + 'checkPod', {params: {order_no: order_no, postcode: postcode,
      customer: customer, customer_order_no: customer_order_no}});
  }

}
