import { TestBed, inject } from '@angular/core/testing';
import { UserService } from './user.service';
import {RouterTestingModule} from '@angular/router/testing';
import {Router} from '@angular/router';
import {routes} from '../routestest';
import { HttpService } from './http.service';
import { BlockUIService } from '../../common_components/services/block-ui.service';
import { HttpMock } from '../testmocks/httpmock';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';
import { HttpClient } from '@angular/common/http';
import { User, RoleId, PermissionId, Permission } from '../domainclasses';
import { RouterMock } from '../testmocks/routerMock';
import { ChangePass } from '../modelclasses';
import { initialMenuItems } from '../app.menu';

describe('UserService', () => {

  let service: UserService = null;
  let httpClient: HttpMock;
  let httpService: HttpService;
  let routerMock: RouterMock;

  setupTestBed({
    providers: [UserService, HttpService, {provide: HttpClient, useClass: HttpMock },
      {provide: BlockUIService, useClass: BlockUIServiceMock}, {provide: Router, useClass: RouterMock}]

  });

  beforeEach(() => {
    service = TestBed.get(UserService);
    httpClient = TestBed.get(HttpClient);
    httpService = TestBed.get(HttpService);
    routerMock = TestBed.get(Router);
  });

  test('should be created', () => {
    expect(service).toBeTruthy();
    expect(httpClient).toBeTruthy();

  });

  test('sign in', () => {
    const user = new User();
    user.name = 'test';
    user.token = 'token';
    httpClient.postResult = user;
    const username = 'user';
    const pass = 'pass';
    let userResult;
    service.signIn(username, pass).subscribe(u => {
      userResult = u;
    });

    expect(service.CurrentUser).toBeTruthy();
    expect(service.CurrentUser.name).toBe(user.name);
    expect(httpService.Token).toBe(user.token);
    expect(httpClient.url).toEqual(expect.stringContaining(`username=${username}&password=${pass}`));
    expect(userResult).toBeTruthy();
    expect(userResult.name).toBe(user.name);
    expect(localStorage.getItem('c2_custportal_user')).toBeTruthy();
  });

  test('getall', () => {
    const users = [ {
      name: '1'
    },
    {
      name: '2'
    }];

    httpClient.getResult = users;
    let usersResult;
    service.getAll().subscribe(u => {
      usersResult = u;
    });

    expect(usersResult).toBeTruthy();
    expect(usersResult.length).toBe(2);
  });

  test('getUsersModel', () => {
    const usersModel = {users:  [ {
      name: '1'
    },
    {
      name: '2'
    }]};

    httpClient.getResult = usersModel;
    let usersModelResult;
    service.getUsersModel().subscribe(u => {
      usersModelResult = u;
    });

    expect(usersModelResult).toBeTruthy();
    expect(usersModelResult.users).toBeTruthy();
    expect(usersModelResult.users.length).toBe(2);
  });

  test('signOut', () => {

    localStorage.setItem('c2_custportal_user', JSON.stringify({name: 'test'}));
    httpService.Token = 'token';

    service.signOut();

    expect(httpService.Token).toBeFalsy();
    expect(routerMock.navigateUrls).toEqual(expect.arrayContaining(['/login']));
    expect(localStorage.getItem('c2_custportal_user')).toBeFalsy();
    expect(localStorage.getItem('c2_cust_portal_token')).toBeFalsy();
  });

  test('saveUser', () => {

    const user = new User();
    user.name = 'test';
    user.token = 'token';

    service.saveUser(user);
    const storage = localStorage.getItem('c2_custportal_user');

    expect(storage).toBeTruthy();
    expect(httpService.Token).toBe(user.token);
  });

  test('loadUser', () => {

    localStorage.setItem('c2_custportal_user', JSON.stringify({name: 'test'}));
    const user = service.loadUser();

    expect(user).toBeTruthy();
    expect(user.name).toBe('test');
  });

  test('updateUserData', () => {

    const user = new User();
    user.name = 'test';

    httpClient.postResult = user;
    let userResult;
    service.updateUserData(user).subscribe(u => {
      userResult = u;
    });

    expect(httpClient.url).toEqual(expect.stringContaining('updateData'));
    expect(userResult).toBeTruthy();
    expect(userResult.name).toBe(user.name);
  });

  test('customerSearch', () => {

    const code = 'code';
    const role_id = RoleId.User;

    httpClient.getResult = [{code: '1'}, {code: '2'}];
    let customersResult;
    service.customerSearch(code, role_id).subscribe(data => {
      customersResult = data;
    });

    expect(customersResult).toBeTruthy();
    expect(customersResult.length).toBe(2);
    expect(httpClient.options.params.code).toBeTruthy();
    expect(httpClient.options.params.role_id).toBeTruthy();
    expect(httpClient.url).toEqual(expect.stringContaining('customerSearch'));
    expect(customersResult.length).toBe(2);
  });

  test('getCustomer', () => {

    const code = 'code';
    const role_id = RoleId.User;

    httpClient.getResult = {code: '1'};
    let customerResult;
    service.getCustomer(code).subscribe(data => {
      customerResult = data;
    });

    expect(customerResult).toBeTruthy();

    expect(httpClient.options.params.code).toBeTruthy();
    expect(httpClient.url).toEqual(expect.stringContaining('getCustomer'));

  });

  test('createUpdateUser', () => {

    const user = new User();
    user.name = 'test';
    user.id = 0;
    httpClient.postResult = {id: '1', name: 'test'};
    let userResult;
    service.createUpdateUser(user).subscribe(data => {
      userResult = data;
    });

    expect(httpClient.url).toEqual(expect.stringContaining('create'));
    expect(userResult.name).toBe(httpClient.postResult.name);

    user.id = 1;
    service.createUpdateUser(user).subscribe(data => {
      userResult = data;
    });

    expect(httpClient.url).toEqual(expect.stringContaining('update'));
    expect(userResult.id).toBe(httpClient.postResult.id);

  });

  test('deleteUser', () => {
    httpClient.postResult = {text: 'result'};
    const id = 1;
    let result;
    service.deleteUser(id).subscribe(r => result = r );

    expect(httpClient.url).toEqual(expect.stringContaining('delete?id=' + id));
    expect(result.text).toBe(httpClient.postResult.text);
  });

  test('activateUser', () => {
    httpClient.postResult = {text: 'result'};
    const id = 1;
    let result;
    service.activateUser(id).subscribe(r => result = r );

    expect(httpClient.url).toEqual(expect.stringContaining('activate?id=' + id));
    expect(result.text).toBe(httpClient.postResult.text);
  });

  test('checkUser', () => {
    httpClient.postResult = {text: 'result'};
    const code = 'test';
    let result;
    service.checkUser(code).subscribe(r => result = r );

    expect(httpClient.url).toEqual(expect.stringContaining('checkUser'));
    expect(httpClient.data.code).toBe(code);
    expect(result.text).toBe(httpClient.postResult.text);
  });

  test('updatePassword', () => {
    httpClient.postResult = {text: 'result'};
    const cp = new ChangePass();
    let result;
    service.updatePassword(cp).subscribe(r => result = r );

    expect(httpClient.url).toEqual(expect.stringContaining('updatePassword'));
    expect(result.text).toBe(httpClient.postResult.text);
  });

  test('resetPassword', () => {
    httpClient.postResult = {text: 'result'};
    const id = 1;
    let result;
    service.resetPassword(id).subscribe(r => result = r );

    expect(httpClient.url).toEqual(expect.stringContaining('resetpassword?id=' + id));
    expect(result.text).toBe(httpClient.postResult.text);
  });

  test('sendPasswordRecoveryLink', () => {
    httpClient.postResult = {text: 'result'};
    const email = 'test';
    let result;
    service.sendPasswordRecoveryLink(email).subscribe(r => result = r );

    expect(httpClient.url).toEqual(expect.stringContaining('sendPasswordRecoveryLink?email=' + email));
    expect(result.text).toBe(httpClient.postResult.text);
  });

  test('getMenuItems', () => {

    const user = new User();
    user.id = 1;
    user.permissions = [new Permission(PermissionId.ViewAccountDetails), new Permission( PermissionId.ViewInvoiceHistory)];
    httpClient.postResult = user;

    service.signIn('', '').subscribe((u) => {} );

    const items = service.getMenuItems();

    expect(items.length).toBe(user.permissions.length);


  });
});



