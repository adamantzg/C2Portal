import { TestBed, inject, getTestBed, fakeAsync } from '@angular/core/testing';

import { HttpService } from './http.service';
import { HttpClientTestingModule, HttpTestingController, TestRequest } from '@angular/common/http/testing';
import { BlockUIService } from '../../common_components/services/block-ui.service';
import { HttpRequest } from '@angular/common/http';
import { doesNotThrow } from 'assert';
import { Observable, of } from 'rxjs';
import { HttpMock } from '../testmocks/httpmock';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';

describe('HttpService', () => {

  let blockUIServiceMock;
  let httpMock: HttpMock;
  let httpService: HttpService;



  beforeEach(() => {
    /*TestBed.configureTestingModule({
      providers: [HttpService]

    });*/

    httpMock = new HttpMock();

    blockUIServiceMock = new BlockUIServiceMock();

    httpService = new HttpService(httpMock as any, blockUIServiceMock as any);


  });

  afterEach(() => {
    // httpMock.verify();
  });

  test('should be created', () => {
    expect(httpService).toBeTruthy();
  });

  test('get/set token', () => {
    httpService.Token = 'test';
    expect(localStorage.getItem('c2_cust_portal_token')).toBeTruthy();

    httpService.Token = null;
    expect(localStorage.getItem('c2_cust_portal_token')).toBeFalsy();

    httpService.Token = 'test';
    expect(localStorage.getItem('c2_cust_portal_token')).toBeTruthy();
    httpService.clearToken();
    expect(localStorage.getItem('c2_cust_portal_token')).toBeFalsy();
  });

  test('post', () => {
    const data = { text: 'test'};
    let postResult = null;
    const url = 'http://org';
    httpService.Token = 'test';
    httpMock.postResult = 'test';

    httpService.post<any>(url, data).subscribe((d) => {
      postResult = d;
    });

    expect(httpMock.url).toBe(url);
    expect(httpMock.data).toBe(data);
    expect(httpMock.params).toBeTruthy();
    checkHeaders(httpMock.params);

    expect(postResult).toBe(httpMock.postResult);
    expect(blockUIServiceMock.blockTicks).toBe(1);
    expect(blockUIServiceMock.unBlockTicks).toBe(1);

  });

  test('post no block', () => {
    const data = { text: 'test'};
    let postResult = null;
    const url = 'http://org';
    httpService.Token = 'test';
    httpMock.postResult = 'test';

    httpService.postNoBlock<any>(url, data).subscribe((d) => {
      postResult = d;
    });

    expect(httpMock.url).toBe(url);
    expect(httpMock.data).toBe(data);
    expect(httpMock.params).toBeTruthy();
    checkHeaders(httpMock.params);

    expect(postResult).toBe(httpMock.postResult);
    expect(blockUIServiceMock.blockTicks).toBe(0);
    expect(blockUIServiceMock.unBlockTicks).toBe(0);

  });

  test('get', () => {
    let getResult = null;
    const url = 'http://org';
    httpService.Token = 'test';
    httpMock.getResult = 'test';

    httpService.get<any>(url).subscribe((d) => {
      getResult = d;
    });

    expect(httpMock.url).toBe(url);
    expect(httpMock.options).toBeTruthy();
    checkHeaders(httpMock.options);

    expect(getResult).toBe(httpMock.getResult);
    expect(blockUIServiceMock.blockTicks).toBe(1);
    expect(blockUIServiceMock.unBlockTicks).toBe(1);

  });

  test('get no block', () => {
    let getResult = null;
    const url = 'http://org';
    httpService.Token = 'test';
    httpMock.getResult = 'test';

    httpService.getNoBlock<any>(url).subscribe((d) => {
      getResult = d;
    });

    expect(httpMock.url).toBe(url);
    expect(httpMock.options).toBeTruthy();
    checkHeaders(httpMock.options);

    expect(getResult).toBe(httpMock.getResult);
    expect(blockUIServiceMock.blockTicks).toBe(0);
    expect(blockUIServiceMock.unBlockTicks).toBe(0);

  });

});

function checkHeaders(params: any) {
  expect(params.headers).toBeTruthy();
  expect(params.headers.has('Content-Type')).toBeTruthy();
  expect(params.headers.has('Accept')).toBeTruthy();
  expect(params.headers.has('Authorization')).toBeTruthy();
}








