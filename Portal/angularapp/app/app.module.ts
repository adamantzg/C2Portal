import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule} from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule} from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { InvoiceHistoryComponent } from './invoice-history/invoice-history.component';
import { OrderTrackingComponent } from './order-tracking/order-tracking.component';
import { OrderHistoryComponent } from './order-history/order-history.component';
import { appRoutes } from './app.routing';
 import { C2FlexModule } from '../c2-flex/c2-flex.module';

import { AccountComponent } from './account/account.component';
import { LoginComponent } from './login/login.component';
import { C2Component } from './c2/c2.component';
import { UserService } from './services/user.service';
 import { UserApi } from '../c2-flex/users/user-api';
// import { UserApi } from '@crosswater-api/user-api';
import { WelcomeComponent } from './welcome/welcome.component';
import { AuthGuard } from './services/auth-guard.service';
import { AdmGuard } from './services/adm-guard.service';
// import { StockModule } from './stock/stock.module';

import { HttpService } from './services/http.service';

import { MenuService } from '../c2-flex/services/menu.service';
import { StockComponent } from './stock/stock.component';
import { ProductService } from './services/product.service';
import { AdminUserComponent } from './admin-user/admin-user.component';
import { AdministrationComponent } from './administration/administration.component';
import { SimpleNotificationsModule } from 'angular2-notifications';
import { TypeaheadModule, BsDatepickerModule } from 'ngx-bootstrap';
import { InvoiceService } from './services/invoice.service';
import { PaginationModule } from 'ngx-bootstrap';
import { BlockUiComponent } from './blockui/block-ui.component';
import { RegisterFormModelDirective } from './registerForm.directive';
import { MessageboxComponent } from './common/messagebox/messagebox.component';
import { MessageboxService } from './common/messagebox/messagebox.service';
import { ModalModule } from 'ngx-bootstrap';
import { SetPasswordComponent } from './login/set-password/set-password.component';
import { OrderService } from './services/order.service';
import { ImpersonateboxComponent } from './common/impersonatebox/impersonatebox.component';
import { CommonAppModule } from '../common_components/common.module';
import { OrderfilterComponent } from './common/orderfilter/orderfilter.component';
import { OrderdetailmodalComponent } from './order-history/orderdetailmodal/orderdetailmodal.component';
import { AdminHolidaysComponent } from './admin-holidays/admin-holidays.component';
import { HolidayService } from './services/holiday.service';
import { BlockUIService } from '../common_components/services/block-ui.service';
import { DummyComponent } from './testmocks/routerMock';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { ClearanceStockComponent } from './clearancestock/clearancestock.component';
// import { GroupByPipe } from './pipe/group-by.pipe';


@NgModule({
  declarations: [
    AppComponent,
    InvoiceHistoryComponent,
    OrderTrackingComponent,
    OrderHistoryComponent,
    AccountComponent,
    LoginComponent,
    C2Component,
    WelcomeComponent,
    StockComponent,
    AdminUserComponent,
    AdministrationComponent,
    BlockUiComponent,
    RegisterFormModelDirective,
    MessageboxComponent,
    SetPasswordComponent,
    ImpersonateboxComponent,
    OrderfilterComponent,
    OrderdetailmodalComponent,
    AdminHolidaysComponent,
    DummyComponent,
    AdminRolesComponent,
    ClearanceStockComponent
    // GroupByPipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    C2FlexModule,
    RouterModule.forRoot(appRoutes),
    FormsModule,
    SimpleNotificationsModule.forRoot(),
    TypeaheadModule.forRoot(),
    PaginationModule.forRoot(),
    BrowserAnimationsModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    CommonAppModule
  ],
  exports: [FormsModule],
  providers: [
    UserService,
      { provide: UserApi, useExisting: UserService },
      AuthGuard,
      AdmGuard ,
      HttpService,
      BlockUIService,
      ProductService,
      InvoiceService,
      MessageboxService,
      OrderService,
      HolidayService
    ],
    entryComponents: [MessageboxComponent, OrderdetailmodalComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
