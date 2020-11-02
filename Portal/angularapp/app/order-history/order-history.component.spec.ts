import { OrderHistoryComponent } from './order-history.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ImpersonateboxComponent } from '../common/impersonatebox/impersonatebox.component';
import { UserService } from '../services/user.service';
import { HttpService } from '../services/http.service';
import { HttpClient } from '@angular/common/http';
import { HttpMock } from '../testmocks/httpmock';
import { OrderService } from '../services/order.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { BlockUIService } from 'common_components/services/block-ui.service';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { InvoiceService } from '../services/invoice.service';
import { ModalModule, BsLocaleService, BsModalService, PaginationComponent,
    TypeaheadModule, BsDatepickerModule, PaginationConfig } from 'ngx-bootstrap';
import { RouterTestingModule } from '@angular/router/testing';
import { routes } from '../routestest';
import { BlockUiComponent } from '../blockui/block-ui.component';
import { OrderfilterComponent } from '../common/orderfilter/orderfilter.component';
import { CommonAppModule } from 'common_components/common.module';
import { FormsModule } from '@angular/forms';
import { User, Customer } from '../domainclasses';
import { By } from '@angular/platform-browser';
import { GridDefinition, GridColumn, SortDirection, GridButtonEventData } from 'common_components/components';

describe('order history', () => {
    let fixture: ComponentFixture<OrderHistoryComponent>;
    let component: OrderHistoryComponent;
    let orderService;
    let userService;

    setupTestBed({
        declarations: [OrderHistoryComponent, ImpersonateboxComponent, BlockUiComponent, OrderfilterComponent, PaginationComponent],
        providers: [UserService, HttpService, {provide: HttpClient, useClass: HttpMock},
            OrderService, CommonService, BlockUIService, MessageboxService, InvoiceService, BsLocaleService,
            BsModalService,  PaginationConfig],
        imports: [ModalModule.forRoot(), RouterTestingModule.withRoutes(routes), CommonAppModule,
            FormsModule, TypeaheadModule.forRoot(), BsDatepickerModule.forRoot()]
    });

    beforeEach(() => {
        orderService = TestBed.get(OrderService);
        userService = TestBed.get(UserService);
        const u = new User();
        u.id = 1;
        jest.spyOn(userService, 'CurrentUser', 'get').mockImplementation(() => u);
        fixture = TestBed.createComponent(OrderHistoryComponent);
        component = fixture.componentInstance;
        component.gridDefinition = new GridDefinition([]);
    });

    test('it should create component', () => {
        expect(component).toBeTruthy();
        expect(fixture).toMatchSnapshot();
    });

    test('impersonate box should show ', () => {
        component.user.isTopAdmin = true;
        jest.spyOn(component, 'getOrderTotals').mockImplementationOnce((code) => {});
        jest.spyOn(component, 'getOrders').mockImplementationOnce((code) => {});
        jest.spyOn(component, 'showForCustomer').mockImplementationOnce((code) => {});
        fixture.detectChanges();
        const impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeTruthy();
    });

    test('impersonate box should not show', () => {

        jest.spyOn(component, 'getOrderTotals').mockImplementationOnce((code) => {});
        jest.spyOn(component, 'getOrders').mockImplementationOnce((code) => {});

        fixture.detectChanges();
        const impBox = fixture.debugElement.query(By.directive(ImpersonateboxComponent));
        expect(impBox).toBeNull();
    });

    test('ngOnInit', () => {
        jest.spyOn(component, 'getOrderTotals').mockImplementationOnce((code) => {});
        jest.spyOn(component, 'getOrders').mockImplementationOnce((code) => {});
        component.gridDefinition = new GridDefinition([new GridColumn('col', 'date_entered')]);
        component.ngOnInit();
        expect(component.gridDefinition.sort.column.title).toBe('col');
        expect(component.gridDefinition.sort.direction).toBe(SortDirection.Desc);
    });

    test('showForCustomer', () => {

        const fn1 = jest.spyOn(component, 'getOrderTotals').mockImplementation((code) => {});
        const fn2 = jest.spyOn(component, 'getOrders').mockImplementation((code) => {});
        component.user.customer_code = 'c1';
        let c = new Customer();
        c.code = 'c2';

        component.showForCustomer(c);
        expect(component.page).toBe(1);
        expect(component.impersonation).toBeTruthy();
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn1.mock.calls[0][0]).toBe('c2');
        expect(fn2.mock.calls.length).toBe(1);
        expect(fn2.mock.calls[0][0]).toBe('c2');

        c = null;
        component.showForCustomer(c);
        expect(component.impersonation).toBeFalsy();
        expect(fn1.mock.calls.length).toBe(2);
        expect(fn1.mock.calls[1][0]).toBe('c1');
        expect(fn2.mock.calls.length).toBe(2);
        expect(fn2.mock.calls[1][0]).toBe('c1');

        fn1.mockReset();
        fn2.mockReset();

    });

    test('pageChanged', () => {
        const e = {page: 3};
        component.lastpage = 2;
        component.impersonation = true;
        component.customer_code = 'code';
        const fn1 = jest.spyOn(component, 'getOrders').mockImplementation((code) => {});
        component.pageChanged(e);
        expect(component.page).toBe(e.page);
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn1.mock.calls[0][0]).toBe(component.customer_code);
        expect(component.lastpage).toBe(e.page);

        // NO impersonation
        component.lastpage = 2;
        component.impersonation = false;
        component.pageChanged(e);
        expect(fn1.mock.calls[1][0]).toBe(userService.CurrentUser.customer_code);

        fn1. mockReset();
    });

    test('gridButtonClicked', () => {
        const event = new GridButtonEventData('col', {reference: 'ref'});
        event.name = 'view';
        const fn1 = jest.spyOn(component, 'viewOrder').mockImplementationOnce(() => {});
        component.gridButtonClicked(event);
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn1.mock.calls[0][0]).toBe(event.row);

        event.name = 'pod';
        const fn2 = jest.spyOn(component, 'showPod').mockImplementationOnce(() => {});
        component.gridButtonClicked(event);
        expect(fn2.mock.calls[0][0]).toBe(event.row);

        event.name = 'order';
        const fn3 = jest.spyOn(component, 'goToTracking').mockImplementationOnce(() => {});
        component.gridButtonClicked(event);
        expect(fn3.mock.calls[0][0]).toBe(event.row);

    });
});
