import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImpersonateboxComponent } from './impersonatebox.component';
import { FormsModule } from '@angular/forms';
import {RouterTestingModule} from '@angular/router/testing';
import {Router} from '@angular/router';

import { HttpClientModule, HttpClient } from '@angular/common/http';

import { routes } from '../../routestest';
import { UserService } from '../../services/user.service';
import { HttpService } from '../../services/http.service';
import { BlockUIService } from '../../../common_components/services/block-ui.service';
import { detectChanges } from '@angular/core/src/render3';
import { TypeaheadModule, ComponentLoaderFactory } from 'ngx-bootstrap';
import { HttpMock } from '../../testmocks/httpmock';
import { of } from 'rxjs';
import { Customer } from '../../domainclasses';

describe('ImpersonateboxComponent', () => {
  let component: ImpersonateboxComponent;
  let fixture: ComponentFixture<ImpersonateboxComponent>;
  let userService;

  setupTestBed({
    providers: [UserService, HttpService, {provide: HttpClient, useClass: HttpMock},  BlockUIService, ComponentLoaderFactory],
    imports: [RouterTestingModule.withRoutes(routes), FormsModule, TypeaheadModule.forRoot()],
    declarations: [ ImpersonateboxComponent ]

  });

  beforeEach(async(() => {
    fixture = TestBed.createComponent(ImpersonateboxComponent);
    component = fixture.componentInstance;
    userService = TestBed.get(UserService);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImpersonateboxComponent);
    component = fixture.componentInstance;

  });

  test('should create', () => {
    expect(component).toBeTruthy();
    expect(fixture).toMatchSnapshot();
  });

  test('properties', () => {
    component.showLabel = true;
    fixture.detectChanges();

    expect(fixture.debugElement.nativeElement.querySelector('label')).toBeTruthy();
    const btn = fixture.debugElement.nativeElement.querySelector('.btn-primary');
    expect(btn).toBeTruthy();
    expect(btn.disabled).toBeTruthy();

    component.customer_code = 'a';
    fixture.detectChanges();
    expect(btn.disabled).toBeFalsy();

    const input = fixture.debugElement.nativeElement.querySelector('input');
    expect(input).toBeTruthy();
    const attrs = input.attributes;
    expect(attrs.getNamedItem('typeaheadoptionfield').value).toBe('combined_name');
    expect(attrs.getNamedItem('placeholder').value).toBe('Type customer code');

    expect(fixture.debugElement.nativeElement.querySelector('.alert-danger')).toBeNull();
    component.showWarning = true;
    fixture.detectChanges();
    expect(fixture.debugElement.nativeElement.querySelector('.alert-danger')).toBeTruthy();

  });

  test('buttonClicked', () => {
    component.customer_code = 'code name1 name2';
    let emitted;
    component.CustomerChange.subscribe(c => emitted = c );
    let returnedCust = {code: 'r1'};
    const fn = jest.spyOn(userService, 'getCustomer').mockImplementation(() => of(returnedCust));
    component.clicked();
    expect(fn.mock.calls[0][0]).toBe('code');
    expect(component.customer.code).toBe(returnedCust.code);
    expect(localStorage.getItem('impersonated_customer')).toEqual(JSON.stringify(returnedCust));
    expect(emitted.code).toBe(returnedCust.code);

    // No customer
    returnedCust = null;
    component.clicked();
    expect(component.customer_code).toBe('');
    expect(component.showWarning).toBeTruthy();
    expect(localStorage.getItem('impersonated_customer')).toBeNull();

  });

  test('onInit', () => {
    const cust = new Customer();
    cust.code = 'code';
    let emmited;
    localStorage.setItem('impersonated_customer', JSON.stringify(cust) );
    component.CustomerChange.subscribe(c => emmited = c);
    component.ngOnInit();
    expect(component.customer).toBeTruthy();
    expect(component.customer.code).toBe(cust.code);
    expect(component.customer_code).toBe(cust.code);
    expect(emmited).toBeTruthy();
    expect(emmited.code).toBe(cust.code);

  });

  test('clear', () => {
    component.customer_code = 'aaa';
    let emmited = {};
    localStorage.setItem('impersonated_customer', JSON.stringify({}) );
    component.CustomerChange.subscribe(c => emmited = c);
    component.customer = new Customer();
    component.clear();
    expect(component.customer_code).toBe('');
    expect(component.customer).toBeNull();
    expect(localStorage.getItem('impersonated_customer')).toBeNull();
    expect(emmited).toBeNull();
  });
});
