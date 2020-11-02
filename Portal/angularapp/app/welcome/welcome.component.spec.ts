import { TestBed, ComponentFixture } from '@angular/core/testing';
import { WelcomeComponent } from './welcome.component';
import { FormsModule } from '@angular/forms';
import { UserService } from '../services/user.service';
import { HttpClient } from '@angular/common/http';
import { HttpMock } from '../testmocks/httpmock';
import { User, PermissionId, Permission } from '../domainclasses';
import { UserServiceMock } from '../testmocks/userserviceMock';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { DummyComponent } from '../testmocks/routerMock';


describe('welcome component', () => {
    let component: WelcomeComponent;
    let fixture: ComponentFixture<WelcomeComponent>;
    let userService: UserServiceMock;

    setupTestBed({
        declarations: [WelcomeComponent, DummyComponent],
        providers: [UserService, {provide: HttpClient, useClass: HttpMock}, {provide: UserService, useClass: UserServiceMock}],
        imports: [FormsModule,
            RouterTestingModule.withRoutes( [
                { path: 'account', component: DummyComponent },
                { path: 'invoice-history', component: DummyComponent },
                { path: 'order-history', component: DummyComponent },
                { path: 'stock', component: DummyComponent }
            ])]
    });

    beforeEach(() => {

        userService = TestBed.get(UserService);
        userService.CurrentUser = new User();
        userService.CurrentUser.name = 'test';
        userService.CurrentUser.customer_code = 'cust';
        fixture = TestBed.createComponent(WelcomeComponent);
        component = fixture.componentInstance;
    });

    test('it should create component', () => {
        expect(component).toBeTruthy();
        expect(fixture).toMatchSnapshot();
    });

    test('links', () => {
        component.user.permissions = [new Permission(PermissionId.ViewAccountDetails),
        new Permission(PermissionId.ViewOrderHistory)];
        fixture.detectChanges();
        let elem = fixture.debugElement.nativeElement.querySelector('a[href*="account"]');
        expect(elem).toBeTruthy();
        elem = fixture.debugElement.nativeElement.querySelector('a[href*="invoice-history"]');
        expect(elem).toBeNull();
        elem = fixture.debugElement.nativeElement.querySelector('a[href*="order-history"]');
        expect(elem).toBeTruthy();
        elem = fixture.debugElement.nativeElement.querySelector('a[href*="stock"]');
        expect(elem).toBeNull();
    });
});
