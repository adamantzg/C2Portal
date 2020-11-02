import { InvoiceHistoryComponent } from './invoice-history.component';
import { HttpMock } from '../testmocks/httpmock';
import { HttpClient } from '@angular/common/http';
import { HttpService } from '../services/http.service';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';

import { Router } from '@angular/router';
import { RouterMock } from '../testmocks/routerMock';
import { InvoiceService } from '../services/invoice.service';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { CommonService } from '../../c2-flex/services/common.service';
import { BsLocaleService, BsModalService, PaginationComponent, BsDatepickerModule,
    TypeaheadModule, ModalModule, PositioningService, PaginationConfig, BsDatepickerConfig } from 'ngx-bootstrap';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { BlockUIService } from 'common_components/services/block-ui.service';
import { ImpersonateboxComponent } from '../common/impersonatebox/impersonatebox.component';
import { BlockUiComponent } from '../blockui/block-ui.component';
import { OrderfilterComponent } from '../common/orderfilter/orderfilter.component';
import { GridDefinition, GridColumn,
    ColumnDataType, Sort, SortDirection, GridButtonEventData } from 'common_components/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { UserService } from '../services/user.service';
import { By } from '@angular/platform-browser';
import { RoleId, User, Customer, Invoice } from '../domainclasses';
import { UserServiceMock } from '../testmocks/userserviceMock';
import { of, throwError } from 'rxjs';
import { MockLink } from '../mocks/mocked';
import * as moment from 'moment';
import { CommonAppModule } from 'common_components/common.module';


describe('Invoice history component', () => {
    let component: InvoiceHistoryComponent = null;
    let fixture: ComponentFixture<InvoiceHistoryComponent>;
    let invoiceService: InvoiceService = null;
    let httpClient: HttpMock;
    let userService: UserServiceMock;

    setupTestBed({
        declarations: [InvoiceHistoryComponent, ImpersonateboxComponent, BlockUiComponent, OrderfilterComponent,
             PaginationComponent],
      providers: [InvoiceService, HttpService, {provide: HttpClient, useClass: HttpMock },
        {provide: BlockUIService, useClass: BlockUIServiceMock}, {provide: Router, useClass: RouterMock},
        CommonService, BsLocaleService, MessageboxService, { provide: UserService, useClass: UserServiceMock },
        BsModalService, PositioningService, PaginationConfig, BsDatepickerConfig],
        imports: [FormsModule, ReactiveFormsModule, BsDatepickerModule.forRoot(),
            TypeaheadModule.forRoot(), ModalModule.forRoot(), CommonAppModule]
    });

    beforeEach(() => {
        invoiceService = TestBed.get(InvoiceService);
        httpClient = TestBed.get(HttpClient);
        // set current user
        userService = TestBed.get(UserService);
        userService.CurrentUser = new User();
        userService.CurrentUser.name = 'test';
        userService.CurrentUser.customer_code = 'cust';
        fixture = TestBed.createComponent(InvoiceHistoryComponent);
        component = fixture.componentInstance;
        component.gridDefinition = new GridDefinition([]);

    });

    test('should create', () => {
        expect(component).toBeTruthy();
    });

    test('get user from userService', () => {
        expect(component.user).toBeTruthy();
        expect(component.user.name).toBe('test');
    });

    test('impersonate box should show for admin', () => {
        component.user.isTopAdmin = true;
        jest.spyOn(component, 'getData').mockImplementation((code) => {});
        fixture.detectChanges();
        const impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeTruthy();
    });

    test('impersonate box should show for branch admin', () => {
        component.user.isBranchAdmin = true;
        jest.spyOn(component, 'getData').mockImplementation((code) => {});
        fixture.detectChanges();
        const impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeTruthy();
    });

    test('impersonate box should not show for user', () => {
        component.user.isTopAdmin = false;
        component.user.isBranchAdmin = false;
        jest.spyOn(component, 'getData').mockImplementation((code) => {});
        invoiceService.getInvoices = jest.fn();
        fixture.detectChanges();
        const impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeNull();
    });

    test('getPageLimits', () => {
        component.page = 2;
        component.pageSize = 50;
        const result = component.getPageLimits();
        expect(result.from).toBe(50);
        expect(result.to).toBe(99);
    });

    test('showForCustomer', () => {

        const spy = jest.spyOn(component, 'getData').mockImplementation((code) => {});
        component.user.customer_code = 'c1';
        let c = new Customer();
        c.code = 'c2';

        component.showForCustomer(c);
        expect(component.page).toBe(1);
        expect(component.impersonation).toBeTruthy();
        expect(spy.mock.calls.length).toBe(1);
        expect(spy.mock.calls[0][0]).toBe('c2');

        c = null;
        component.showForCustomer(c);
        expect(component.impersonation).toBeFalsy();
        expect(spy.mock.calls.length).toBe(2);
        expect(spy.mock.calls[1][0]).toBe('c1');
    });

    test('getData - getCustomer', () => {
        const c = new Customer();
        c.currency = 'EU1';
        const code = 'code';
        const fn = jest.spyOn(invoiceService, 'getCustomer').mockImplementation((cust) => of(c));
        component.getInvoices = jest.fn();
        component.getCustomerTotals = jest.fn();

        const col = new GridColumn('', '');
        col.dataType = ColumnDataType.Currency;
        col.format = {currencyCode: 'USD'};
        component.gridDefinition.columns[0] = col;

        component.getData(code);
        expect(fn.mock.calls[0][0]).toBe(code);
        expect(component.customer).toBe(c);
        expect(component.currency).toBe('EUR');
        expect(component.gridDefinition.columns[0].format.currencyCode).toBe('EUR');
    });

    test('getCustomerTotals', () => {
        const t = {result: {}};
        const code = 'code';
        const fn = jest.spyOn(invoiceService, 'getCustomerTotals').mockImplementation(() => of(t));
        component.getCustomerTotals(code);
        expect(fn.mock.calls[0][0]).toBe(code);
        expect(component.totals).toBe(t.result);
    });

    test('getInvoices', () => {
        const inv = new Invoice();
        inv.reference = 'test';
        inv.kind = 'CSH';
        const code = 'code';
        const d = {result: [inv]};
        const fn = jest.spyOn(invoiceService, 'getInvoices').mockImplementation(() => of(d));

        component.gridDefinition.columns.push(new GridColumn('', ''));
        component.gridDefinition.sort = new Sort(component.gridDefinition.columns[0], SortDirection.Asc);

        component.getInvoices(code);
        expect(fn.mock.calls[0][0]).toBe(code);
        expect(component.invoices).toBe(d.result);
        expect(component.invoices[0].customer_order_num).toBe('Payment on Account');
        expect(component.filteredInvoices).toBe(d.result);
    });

    test('pageChanged', () => {
        const e = {page: 3};
        component.lastpage = 2;
        component.impersonation = true;
        component.customer_code = 'code';
        const fn = jest.spyOn(component, 'getInvoices').mockImplementation(() => of({result: [{}]}));
        component.pageChanged(e);
        expect(component.page).toBe(e.page);
        expect(fn.mock.calls.length).toBe(1);
        expect(fn.mock.calls[0][0]).toBe(component.customer_code);
        expect(component.lastpage).toBe(e.page);

        // NO impersonation
        component.lastpage = 2;
        component.impersonation = false;
        component.pageChanged(e);
        expect(fn.mock.calls[1][0]).toBe(userService.CurrentUser.customer_code);
    });

    test('viewInvoice', () => {
        const data = 'data';
        const order_no = 'order';
        const fn1 = jest.spyOn(invoiceService, 'getInvoicePdf').mockImplementation(() => of(data));
        const fn2 = jest.spyOn(component, 'createPdfLink');
        component.viewInvoice(order_no);
        expect(fn1.mock.calls[0][0]).toBe(order_no);
        expect(fn2.mock.calls[0][0]).toBe(order_no);
        expect(fn2.mock.calls[0][1]).toBe(data);
    });

    test('createBlob', () => {
        const data = 'BgYG'; // 060606
        const blob = component.createBlob(data);
        expect(blob.size).toBe(3);
        expect(blob.type).toBe('application/pdf');
    });


    test('createPdfLink', () => {
        const title = 'title';
        const blob = new Blob();
        const fn1 = jest.spyOn(component, 'createBlob').mockImplementation(() => blob );
        const fn2 = jest.fn();
        navigator.msSaveOrOpenBlob = fn2;
        component.createPdfLink(title, 'data');

        expect(fn1.mock.calls.length).toBe(1);
        expect(fn2.mock.calls.length).toBe(1);
        expect(fn2.mock.calls[0][0]).toBe(blob);
        expect(fn2.mock.calls[0][1]).toBe('title.pdf');

        navigator.msSaveOrOpenBlob = null;
        let sentLink: MockLink;
        jest.spyOn(document, 'createElement').mockImplementationOnce((e) => {
            sentLink = new MockLink(e);
            return sentLink;
        });
        jest.spyOn(document.body, 'appendChild').mockImplementationOnce((link) => sentLink = link);
        jest.spyOn(document.body, 'removeChild').mockImplementationOnce(() => {});
        window.URL.createObjectURL = jest.fn(() => 'href');
        component.createPdfLink(title, 'data');
        expect(sentLink.clicked).toBeTruthy();
        expect(sentLink.href).toBe('href');
        expect(sentLink.download).toBe('title.pdf');
        expect(sentLink.target).toBe('_blank');
        jest.restoreAllMocks();
    });

    test('applyFilter', () => {
        component.impersonation = true;
        component.customer_code = 'comp_cust_code';

        const fn1 = jest.spyOn(component, 'getInvoices').mockImplementation(() => {});
        const fn2 = jest.spyOn(component, 'getCustomerTotals').mockImplementation(() => {});

        component.applyFilter();
        expect(fn1.mock.calls[0][0]).toBe(component.customer_code);
        component.impersonation = false;
        component.applyFilter();
        expect(fn1.mock.calls[1][0]).toBe(component.user.customer_code);

    });

    test('clearFilter', () => {
        const fn1 = jest.spyOn(component, 'initFilter').mockImplementationOnce(() => {});
        const fn2 = jest.spyOn(component, 'applyFilter').mockImplementationOnce(() => {});
        component.clearFilter();
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn2.mock.calls.length).toBe(1);
    });

    test('orderTracking', () => {
        const router: RouterMock = TestBed.get(Router);
        component.orderTracking('ref');
        expect(router.navigateUrls).toEqual(expect.arrayContaining(['./c2/order-tracking']));
        expect(router.params.queryParams).toBeTruthy();
        expect(router.params.queryParams.order_no).toBe('ref');
    });

    test('gridButtonClicked', () => {
        const event = new GridButtonEventData('col', {reference: 'ref'});
        event.name = 'view';
        const fn1 = jest.spyOn(component, 'viewInvoice').mockImplementationOnce(() => {});
        component.gridButtonClicked(event);
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn1.mock.calls[0][0]).toBe('ref');

        event.name = 'pod';
        const fn2 = jest.spyOn(component, 'showPod').mockImplementationOnce(() => {});
        component.gridButtonClicked(event);
        expect(fn2.mock.calls[0][0]).toBe(event.row);

    });

    test('onGridSortChange', () => {
        const fn1 = jest.spyOn(component, 'getInvoices').mockImplementation(() => {});
        component.impersonation = true;
        component.customer_code = 'ccc';
        component.onGridSortChange(null);
        expect(fn1.mock.calls[0][0]).toBe(component.customer_code);

        component.impersonation = false;
        component.onGridSortChange(null);
        expect(fn1.mock.calls[1][0]).toBe(component.user.customer_code);
    });

    test('selectedOrders', () => {
        component.invoices = [];
        component.invoices.push(new Invoice());
        component.invoices.push(new Invoice());
        component.invoices[0].selected = true;
        expect(component.selectedOrders().length).toBe(1);
    });

    test('viewSelectedOrders', () => {
        component.invoices = [];
        component.invoices.push(new Invoice());
        component.invoices.push(new Invoice());
        component.invoices[0].selected = true;
        component.invoices[0].reference = 'ref1';
        component.invoices[1].selected = true;
        component.invoices[1].reference = 'ref2';
        const order_nos = 'ref1,ref2';

        const fn1 = jest.spyOn(invoiceService, 'getInvoicePdfs').mockImplementationOnce((d) => of('data'));
        const fn2 = jest.spyOn(component, 'createPdfLink').mockImplementationOnce(null);
        component.viewSelectedOrders();
        expect(fn1.mock.calls[0][0]).toBe(order_nos);
        expect(fn2.mock.calls[0][0]).toBe(order_nos);
        expect(fn2.mock.calls[0][1]).toBe('data');
    });

    test('showPod', () => {
        component.customer_code = 'cust';
        component.impersonation = true;
        const inv = new Invoice();
        inv.reference = 'ref';
        inv.customer_order_num = 'con';
        inv.postcode = 'AA BB';

        const msgService = TestBed.get(MessageboxService);
        let fn1 = jest.spyOn(invoiceService, 'checkInvoicePod').mockImplementationOnce(() => of(true));
        const fn2 = jest.spyOn(window, 'open').mockImplementationOnce(null);

        component.showPod(inv);

        expect(fn1.mock.calls[0][0]).toBe(inv.reference);
        expect(fn1.mock.calls[0][1]).toBe(component.customer_code);
        expect(fn1.mock.calls[0][2]).toBe(inv.customer_order_num);
        expect(fn1.mock.calls[0][3]).toBe(inv.postcode);

        expect(fn2.mock.calls[0][0]).toEqual(expect.stringContaining('&ref=' + inv.reference));
        expect(fn2.mock.calls[0][0]).toEqual(expect.stringContaining('&pc=AABB'));

        fn1 = jest.spyOn(invoiceService, 'checkInvoicePod').mockImplementationOnce(() => of(false));
        const fn3 = jest.spyOn(msgService, 'openDialog').mockImplementationOnce(null);
        component.showPod(inv);
        expect(fn3.mock.calls[0][0]).toEqual(expect.stringContaining('Request for POD sent'));

        jest.spyOn(invoiceService, 'checkInvoicePod').mockImplementationOnce(() => throwError({error: 'error'}));
        component.showPod(inv);
        expect(component.errorMessage).toBe('error');

    });

    test('checkInvoiceButtonVisibility', () => {
        const inv = new Invoice();
        inv.kind = 'A';
        let buttonName = 'pod';

        expect(component.checkInvoiceButtonVisibility(inv, '', buttonName)).toBeTruthy();
        inv.kind = 'CSH';
        expect(component.checkInvoiceButtonVisibility(inv, '', buttonName)).toBeFalsy();
        inv.kind = 'CRN';
        expect(component.checkInvoiceButtonVisibility(inv, '', buttonName)).toBeFalsy();

        buttonName = 'view';
        inv.kind = 'A';
        expect(component.checkInvoiceButtonVisibility(inv, '', buttonName)).toBeTruthy();
        inv.kind = 'CSH';
        expect(component.checkInvoiceButtonVisibility(inv, '', buttonName)).toBeFalsy();

    });

    test('initFilter', () => {
        const expFrom = moment().subtract(6, 'months').toDate();
        const expTo = new Date();

        component.initFilter();
        expect(component.filter.from).toEqual(expFrom);
        expect(component.filter.to).toEqual(expTo);
        expect(component.filter.searchText).toBe('');
    });


});




