import { Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { Product } from '../domainclasses';
import { tap } from 'rxjs/operators';
import { of } from 'rxjs/observable/of';
import { Observable } from 'rxjs/Observable';
import { SortDirection } from '../../common_components/components';
import { Order } from './../domainclasses';

@Injectable()
export class ProductService {

  constructor(private httpService: HttpService) { }
  api = 'api/product/';
  cachedProducts: Product[] = [];
  lastCode = '';

  searchProduct(code: string): Observable<Product[]> {
    console.log('SEARCH STARTED');
    if (this.lastCode.length > 0 && code.length >= this.lastCode.length) {
      if (code.toLowerCase().startsWith(this.lastCode.toLowerCase())) {
        console.log('Inside IF');
        return of(this.cachedProducts
          .sort((a, b) => (a.code > b.code) ? 1 : ((b.code > a.code) ? -1 : 0))
          .filter(p => p.code.toLowerCase().indexOf(code.toLowerCase()) >= 0
           || p.name.toLowerCase().indexOf(code.toLowerCase()) >= 0));
      }
    }
    this.lastCode = code;
    console.log(`${code} kod`);
     return this.httpService.get(this.api + 'search', {params: {code: encodeURIComponent(code)}}).pipe(
      tap((data) => {
        const temp =  data.filter(item => item.code.toLowerCase() === code.toLowerCase() );
        // let dt = dataCopy.sort((a, b) => (a.code > b.code) ? 1 : ((b.code > a.code) ? -1 : 0));

        data = data.sort((a, b) => (a.code > b.code) ? 1 : ((b.code > a.code) ? -1 : 0));
        data.unshift(...temp);
        this.cachedProducts = data;
       })
    );
  }



  getFreeStock(code: string) {
    return this.httpService.get(this.api + 'getFreeStock', {params: {code: encodeURIComponent(code)}});
  }

  getProductPrice(customer: string, code: string) {
    return this.httpService.get(this.api + 'getPrice', {params: {customer: customer, code: encodeURIComponent(code)}});
  }

  getProduct(code: string) {
    return this.httpService.get(this.api + 'get', {params: {code: encodeURIComponent(code)}});
  }

  getClearanceStock(code: string, description: string, range: string, recFrom: number, recTo: number,
    sortBy: string, direction: SortDirection) {
      return this.httpService.get(this.api + 'getClearanceStock',
      {
        params:
        {
          code: code,
          description: description,
          range: range,
          recFrom: recFrom,
          recTo: recTo,
          sortBy: sortBy,
          direction: direction
        }
      });
    }

  getClearanceStockCount(code: string, description: string, range: string) {
    return this.httpService.get(this.api + 'getClearanceStockCount', { params: {
      code: code,
      description: description,
      range: range
    }});
  }

}
