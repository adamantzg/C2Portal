import { Routes } from '@angular/router';
import {InvoiceHistoryComponent } from './invoice-history/invoice-history.component';

// import { DashboardComponent } from './dashboard/dashboard.component';
// import { CountriesComponent } from './countries/countries.component';
// import { SettingsComponent } from './settings/settings.component';
import { OrderTrackingComponent } from './order-tracking/order-tracking.component';
import { OrderHistoryComponent } from './order-history/order-history.component';
import { AccountComponent } from './account/account.component';
// import { LoginScreenComponent } from './login-screen/login-screen.component';
import { LoginComponent } from './login/login.component';
import { C2Component } from './c2/c2.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { AuthGuard } from './services/auth-guard.service';
import { AdminUserComponent} from './admin-user/admin-user.component';
import { StockComponent } from './stock/stock.component';
import { AdmGuard } from './services/adm-guard.service';
import { SetPasswordComponent } from './login/set-password/set-password.component';
import { AdminHolidaysComponent } from './admin-holidays/admin-holidays.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { ClearanceStockComponent } from './clearancestock/clearancestock.component';

export const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'setpass/:code', component: SetPasswordComponent },
  { path: 'c2', component: C2Component, canActivate: [AuthGuard],
    children: [
      { path: 'account', component: AccountComponent },
      { path: 'invoice-history', component: InvoiceHistoryComponent },
      { path: 'order-tracking', component: OrderTrackingComponent },
      { path: 'order-history', component: OrderHistoryComponent },
      { path: 'order-history/:id', component: OrderHistoryComponent },
      { path: 'welcome', component: WelcomeComponent },
      { path: 'stock', component: StockComponent },
      { path: 'clearance', component: ClearanceStockComponent },
      { path: 'user', component: AdminUserComponent},
      { path: 'role', component: AdminRolesComponent},
      { path: '', component: WelcomeComponent },
      {
        path: 'admin-holidays', component: AdminHolidaysComponent
      },
    ]
  },
  // {
  //   path: 'admin', component: AdminNonWorkingComponent
  // },
  { path: '', component: LoginComponent },
  { path: '**', component: LoginComponent }

];
