import { TestBed, inject } from '@angular/core/testing';
import { ProductService } from './product.service';
import {RouterTestingModule} from '@angular/router/testing';
import {Router} from '@angular/router';
import {routes} from '../routestest';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HttpService } from './http.service';
import { BlockUIService } from '../../common_components/services/block-ui.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { HttpMock } from '../testmocks/httpmock';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';
import { RouterMock } from '../testmocks/routerMock';

describe('ProductService', () => {
  let service: ProductService = null;
  let httpClient: HttpMock;
  let httpService: HttpService;

  setupTestBed({
    providers: [ProductService, HttpService, {provide: HttpClient, useClass: HttpMock },
      {provide: BlockUIService, useClass: BlockUIServiceMock}, {provide: Router, useClass: RouterMock}]

  });

  beforeEach(() => {
    service = TestBed.get(ProductService);
    httpClient = TestBed.get(HttpClient);
    httpService = TestBed.get(HttpService);
  });

  test('should be created', () => {
    expect(service).toBeTruthy();
  });

  test('searchProducts', () => {
    const products = [{code: 'c11', name: 'c11'}, {code: 'code', name: 'c112'}, {code: 'c2', name: 'c2'}];
    httpClient.getResult = products;
    let result;
    let code = 'c';

    service.searchProduct(code).subscribe(data => result = data);
    expect(httpClient.url).toEqual(expect.stringContaining('search'));
    expect(httpClient.options.params.code).toBe(code);
    expect(result.length).toBe(httpClient.getResult.length);

    httpClient.url = null;
    code = 'c1';
    // should use cache
    service.searchProduct(code).subscribe(data => result = data);
    expect(httpClient.url).toBeNull();
    expect(result.length).toBe(2);

    code = 'C1';  // should be case insensitive
    // should use cache
    service.searchProduct(code).subscribe(data => result = data);
    expect(httpClient.url).toBeNull();
    expect(result.length).toBe(2);

    code = 'C2';
    // cache
    service.searchProduct(code).subscribe(data => result = data);
    expect(httpClient.url).toBeNull();
    expect(result.length).toBe(1);

    code = 'd';
    // no cache
    httpClient.getResult = [{code: 'd1', name: 'd1'}];
    service.searchProduct(code).subscribe(data => result = data);
    expect(httpClient.url).toBeTruthy();
    expect(result.length).toBe(1);

  });

  test('getFreeStock', () => {
    httpClient.getResult = {text: 'test'};
    let result;
    const code = 'c';

    service.getFreeStock(code).subscribe(data => result = data );

    expect(httpClient.url).toEqual(expect.stringContaining('getFreeStock'));
    expect(httpClient.options.params.code).toBe(code);
  });

});
