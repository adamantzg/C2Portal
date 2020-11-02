import { OrderTrackingComponent } from './order-tracking.component';
import { UserService } from '../services/user.service';
import { OrderService } from '../services/order.service';
import { HttpClient } from '@angular/common/http';
import { HttpMock } from '../testmocks/httpmock';
import { HolidayService } from '../services/holiday.service';
import { RouterTestingModule } from '@angular/router/testing';
import { routes } from '../routestest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { User, Order, Customer, OrderStatus, StockData } from '../domainclasses';
import { CommonAppModule } from 'common_components/common.module';
import { BlockUiComponent } from '../blockui/block-ui.component';
import { BlockUIService } from 'common_components/services/block-ui.service';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';
import { ImpersonateboxComponent } from '../common/impersonatebox/impersonatebox.component';
import { FormsModule } from '@angular/forms';
import { TypeaheadModule } from 'ngx-bootstrap';
import { HttpService } from '../services/http.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import * as moment from 'moment';
import * as $ from 'jquery';
import { RouterMock } from '../testmocks/routerMock';
import { By } from '@angular/platform-browser';
import { exec } from 'child_process';
import { containsElement } from '@angular/animations/browser/src/render/shared';
import { Settings } from '../settings';

describe('order tracking', () => {

    let fixture: ComponentFixture<OrderTrackingComponent>;
    let component: OrderTrackingComponent;
    let userService, orderService;

    setupTestBed({
        declarations: [OrderTrackingComponent, BlockUiComponent, ImpersonateboxComponent],
        providers: [UserService, OrderService, HttpService,  {provide: HttpClient, useClass: HttpMock}, HolidayService,
            {provide: BlockUIService, useClass: BlockUIServiceMock}, CommonService],
        imports: [ RouterTestingModule.withRoutes(routes), CommonAppModule, FormsModule, TypeaheadModule.forRoot()]
    });

    beforeEach(() => {
        const u = new User();
        u.id = 1;
        u.isTopAdmin = false;
        u.isBranchAdmin = false;
        userService = TestBed.get(UserService);
        jest.spyOn(userService, 'CurrentUser', 'get').mockImplementation(() => u);
        orderService = TestBed.get(OrderService);

        fixture = TestBed.createComponent(OrderTrackingComponent);
        component = fixture.componentInstance;
    });

    test('should create', () => {
        expect(component).toBeTruthy();
        expect(fixture).toMatchSnapshot();
    });

    test('checkParams', () => {
        const route = TestBed.get(ActivatedRoute);
        route.queryParams = of({return_to: 'return', order_no: 'order'});
        const fn = jest.spyOn(component, 'searchOrder').mockImplementation(() => {});
        component.checkParams();
        expect(component.return_to).toBe('return');
        expect(component.param).toBe('order');
        expect(component.order_no).toBe('order');
        expect(fn.mock.calls.length).toBe(1);

        route.queryParams = of({order_no: 'order'});
        component.checkParams();
        expect(component.return_to).toBe('invoices');
        expect(component.param).toBe('order');
        expect(component.order_no).toBe('order');
        expect(fn.mock.calls.length).toBe(2);
        fn.mockReset();
    });

    test('searchOrder', () => {
        const returned = { result: new Order()};
        returned.result.customer = 'cust';
        const fn1 = jest.spyOn(orderService, 'getOrder').mockImplementation(() => of(returned) );
        const fn2 = jest.spyOn(component, 'getCustomer').mockImplementation(() => {});
        component.customer_code = 'cust';
        component.order_no = 'order  ';
        component.searchOrder();
        expect(fn1.mock.calls[0][0]).toBe('order');
        expect(fn1.mock.calls[0][1]).toBe(component.customer_code);
        expect(component.order).toBe(returned.result);
        expect(fn2.mock.calls[0][0]).toBe(returned.result.customer);

        returned.result = null;
        component.searchOrder();
        expect(component.errorMessage).toBe('Order not found or you don\'t have access.');
        expect(fn2.mock.calls.length).toBe(1);
        fn1.mockReset();
        fn2.mockReset();

    });

    test('getCustomer', () => {
        const returned = new Customer();
        returned.code = 'code';
        component.order = new Order();
        component.order.status = OrderStatus.Delivered;
        const fn1 = jest.spyOn(orderService, 'getCustomer').mockImplementation(() => of(returned));
        const fn2 = jest.spyOn(component, 'getExpectedDelivery').mockImplementation(() => {});
        component.getCustomer('cust');
        expect(component.customer).toBe(returned);
        expect(fn1.mock.calls[0][0]).toBe('cust');
        expect(fn2.mock.calls.length).toBe(0);

        component.order.status = OrderStatus.InProgress;
        component.getCustomer('cust');
        expect(fn2.mock.calls.length).toBe(1);
        fn1.mockReset();
        fn2.mockReset();

    });

    test('getExpectedDelivery', () => {
        const holidayService = TestBed.get(HolidayService);
        const holidays = [];
        const fn1 = jest.spyOn(holidayService, 'getHolidays').mockImplementation(() => of(holidays));

        const o = new Order();
        o.productStockData = [];
        let d = new StockData();
        d.ship_date = moment('2018-09-01').toDate();
        o.productStockData.push(d);
        d = new StockData();
        d.ship_date = moment('2018-09-15').toDate();
        o.productStockData.push(d);
        component.order = o;

        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate).toBeTruthy();
        // 15/09 is saturday, 17/09 monday
        expect(component.expectedDeliveryDate.format('YYYY-MM-DD')).toBe('2018-09-17');

        holidays.push({date: moment('2018-09-17')});
        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate.format('YYYY-MM-DD')).toBe('2018-09-18');

        // check when no product stock data
        component.order.productStockData = [];
        const cust = new Customer();
        cust.code = 'xxx';
        component.customer = cust;
        component.order.audits = [
            {audit_date : moment('2018-09-01').toDate(), audit_key : '', character_value : '6'},
            {audit_date : moment('2018-09-15').toDate(), audit_key : '', character_value : '5'},
            {audit_date : moment('2018-09-19').toDate(), audit_key : '', character_value : '6'}
        ];
        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate.format('YYYY-MM-DD')).toBe('2018-09-20');
        cust.analysis_codes1 = 'GMCCROSSAN';
        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate.format('YYYY-MM-DD')).toBe('2018-09-21');
        cust.analysis_codes1 = 'CWALSH';
        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate.format('YYYY-MM-DD')).toBe('2018-09-21');
        cust.analysis_codes1 = 'SANDERSON';
        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate.format('YYYY-MM-DD')).toBe('2018-09-21');

        component.order.audits = [];
        component.getExpectedDelivery();
        expect(component.expectedDeliveryDate).toBeNull();

        fn1.mockReset();
    });

    test('getStatus', () => {

        component.order = new Order();
        component.order.status = OrderStatus.Cancelled;
        let result = component.getStatus();
        expect(result).toBe('Cancelled');
        component.order.status = OrderStatus.Delivered;
        result = component.getStatus();
        expect(result).toBe('Delivered');
        component.order.status = 6;
        result = component.getStatus();
        expect(result).toBe('Processed');
        component.order.status = 5;
        result = component.getStatus();
        expect(result).toBe('Received');

    });

    test('getAuditDate', () => {
        const o = new Order();
        o.audits = [];
        let result = component.getAuditDate(o);
        expect(result).toBeNull();

        o.audits = [];
        o.audits.push({audit_date : moment('2018-10-01').toDate(), audit_key: '', character_value: ''});
        result = component.getAuditDate(o);
        expect(moment(result).format('YYYY-MM-DD')).toBe('2018-10-01');

    });

    test('getProcessedDate', () => {
        const o = new Order();
        o.audits = [];
        let result = component.getProcessedDate(o);
        expect(result).toBeNull();

        o.audits = [];
        o.audits.push({audit_date : moment('2018-10-01').toDate(), audit_key: '', character_value: '5'});
        result = component.getProcessedDate(o);
        expect(result).toBeNull();
        o.audits[0].character_value = '6';
        result = component.getProcessedDate(o);
        expect(moment(result).format('YYYY-MM-DD')).toBe('2018-10-01');
    });

    test('navigateToCaller', () => {
        const router = TestBed.get(Router);
        const fn = jest.spyOn(router, 'navigate').mockImplementation(() => {});
        component.return_to = 'invoices';
        component.navigateToCaller();
        expect(fn.mock.calls[0][0]).toEqual(expect.arrayContaining([expect.stringContaining('invoice-history')]));
        component.return_to = 'orders';
        component.navigateToCaller();
        expect(fn.mock.calls[1][0]).toEqual(expect.arrayContaining([expect.stringContaining('order-history')]));

        fn.mockReset();
    });

    test('showForCustomer', () => {
        const c = new Customer();
        c.code = 'cust';

        const fn1 = jest.spyOn(component, 'checkParams').mockImplementationOnce(() => {});
        const fn2 = jest.spyOn(component, 'searchOrder').mockImplementationOnce(() => {});
        component.paramsChecked = false;
        component.showForCustomer(c);
        expect(component.customer_code).toBe(c.code);
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn2.mock.calls.length).toBe(0);
        component.paramsChecked = true;
        component.showForCustomer(c);
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn2.mock.calls.length).toBe(1);

    });

    test('impersonate box', () => {
        component.user.isTopAdmin = true;
        jest.spyOn(component, 'showForCustomer').mockImplementation(() => {});
        jest.spyOn(component, 'searchOrder').mockImplementation((code) => {});
        fixture.detectChanges();
        let impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeTruthy();

        component.user.isTopAdmin = false;
        component.user.isBranchAdmin = true;
        fixture.detectChanges();
        impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeTruthy();

        component.user.isBranchAdmin = false;
        fixture.detectChanges();
        impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeNull();

        jest.resetAllMocks();
    });

    test('ui - Search button', () => {
        jest.spyOn(component, 'showForCustomer').mockImplementation(() => {});
        jest.spyOn(component, 'searchOrder').mockImplementation((code) => {});
        jest.spyOn(component, 'checkParams').mockImplementationOnce(() => {});
        fixture.detectChanges();
        expect(component.order_no.length).toBe(0);
        expect($('button:contains("Search")').prop('disabled')).toBeTruthy();
        component.order_no = 'ord';
        fixture.detectChanges();
        expect($('button:contains("Search")').prop('disabled')).toBeFalsy();

    });

    test('ui - expected delivery date', () => {
        jest.spyOn(component, 'showForCustomer').mockImplementation(() => {});
        jest.spyOn(component, 'searchOrder').mockImplementation((code) => {});
        jest.spyOn(component, 'checkParams').mockImplementationOnce(() => {});
        fixture.detectChanges();
        expect($('div.hidden-xs:contains(Expected Delivery)').length).toBe(0);

        component.expectedDeliveryDate = moment('2018-10-01');
        component.order = new Order();
        component.order.status = OrderStatus.InProgress;
        fixture.detectChanges();
        expect($('div.hidden-xs:contains(Expected Delivery)').length).toBe(1);

        expect($('span:contains(Expected Delivery)').length).toBe(2);
        let formattedData: string = $('span:contains(Expected Delivery)').first().text();

        formattedData = formattedData.replace('Expected Delivery:', '').trim();
        expect(formattedData).toBe(component.expectedDeliveryDate.format(Settings.datePattern.toUpperCase()));

    });

    test('ui - test circles', () => {

        jest.spyOn(component, 'showForCustomer').mockImplementation(() => {});
        jest.spyOn(component, 'searchOrder').mockImplementation((code) => {});
        jest.spyOn(component, 'checkParams').mockImplementationOnce(() => {});
        component.expectedDeliveryDate = moment('2018-10-01');
        component.order = new Order();
        component.order.status = OrderStatus.InProgress;
        fixture.detectChanges();

        // Test circles
        // visible-xs first
        // 1st circle Received
        expect($('div.visible-xs .circle:eq(0)').hasClass('alert-success')).toBeTruthy();
        expect($('div.visible-xs .fa-xs-holder:eq(0)').hasClass('text-success')).toBeTruthy();

        // 2nd circle Processed
        expect($('div.visible-xs .circle:eq(1)').hasClass('alert-success')).toBeTruthy();
        expect($('div.visible-xs .fa-xs-holder:eq(1)').hasClass('text-warning')).toBeTruthy();

        // 3rd circle Delivered
        expect($('div.visible-xs .circle:eq(2)').hasClass('alert-danger')).toBeTruthy();

        // hidden-xs
        // 1st circle Received
        expect($('div.hidden-xs .circle:eq(0)').hasClass('alert-success')).toBeTruthy();
        expect($('div.hidden-xs .arrow-holder:eq(0)').hasClass('text-success')).toBeTruthy();

        // 2nd circle Processed
        expect($('div.hidden-xs .circle:eq(1)').hasClass('alert-success')).toBeTruthy();
        expect($('div.hidden-xs .arrow-holder:eq(1)').hasClass('text-warning')).toBeTruthy();

        // 3rd circle Delivered
        expect($('div.hidden-xs .circle:eq(2)').hasClass('alert-danger')).toBeTruthy();

        // try with delivered
        component.order.status = OrderStatus.Delivered;
        fixture.detectChanges();

        // visible-xs first
        // 1st circle Received
        expect($('div.visible-xs .circle:eq(0)').hasClass('alert-success')).toBeTruthy();
        expect($('div.visible-xs .fa-xs-holder:eq(0)').hasClass('text-success')).toBeTruthy();

        // 2nd circle Processed
        expect($('div.visible-xs .circle:eq(1)').hasClass('alert-success')).toBeTruthy();
        expect($('div.visible-xs .fa-xs-holder:eq(1)').hasClass('text-success')).toBeTruthy();

        // 3rd circle Delivered
        expect($('div.visible-xs .circle:eq(2)').hasClass('alert-success')).toBeTruthy();

        // hidden-xs
        // 1st circle Received
        expect($('div.hidden-xs .circle:eq(0)').hasClass('alert-success')).toBeTruthy();
        expect($('div.hidden-xs .arrow-holder:eq(0)').hasClass('text-success')).toBeTruthy();

        // 2nd circle Processed
        expect($('div.hidden-xs .circle:eq(1)').hasClass('alert-success')).toBeTruthy();
        expect($('div.hidden-xs .arrow-holder:eq(1)').hasClass('text-success')).toBeTruthy();

        // 3rd circle Delivered
        expect($('div.hidden-xs .circle:eq(2)').hasClass('alert-success')).toBeTruthy();


    });

    test('ui - check order history panel', () => {
        jest.spyOn(component, 'showForCustomer').mockImplementation(() => {});
        jest.spyOn(component, 'searchOrder').mockImplementation((code) => {});
        jest.spyOn(component, 'checkParams').mockImplementationOnce(() => {});
        component.expectedDeliveryDate = moment('2018-10-01');
        const o = new Order();
        o.status = OrderStatus.Delivered;
        o.date_entered = moment('2018-08-01').toDate();
        o.audits = [];
        o.audits.push({audit_date : moment('2018-09-01').toDate(), audit_key: '', character_value: '6'});
        o.invoice_date = moment('2018-09-05').toDate();
        component.order = o;

        fixture.detectChanges();

        let orderDelivered = $('div.list-group-item:contains(Order Delivered)').text();
        expect(orderDelivered).toBeTruthy();
        orderDelivered = orderDelivered.replace('Order Delivered:', '').trim();
        expect(orderDelivered).toBe(moment(o.invoice_date).format(Settings.datePattern.toUpperCase()));

        let orderProcessed = $('div.list-group-item:contains(Order Processed)').text();
        expect(orderProcessed).toBeTruthy();
        orderProcessed = orderProcessed.replace('Order Processed:', '').trim();
        expect(orderProcessed).toBe(moment(o.audits[0].audit_date).format(Settings.datePattern.toUpperCase()));

        let orderReceived = $('div.list-group-item:contains(Order Received)').text();
        expect(orderReceived).toBeTruthy();
        orderReceived = orderReceived.replace('Order Received:', '').trim();
        expect(orderReceived).toBe(moment(o.date_entered).format(Settings.datePattern.toUpperCase()));
    });

});

