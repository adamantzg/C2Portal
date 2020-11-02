import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { C2FlexBodyComponent } from './flex-body/flex-body.component';
import { TitleBarComponent } from './title-bar/title-bar.component';
import { StatusBarComponent } from './status-bar/status-bar.component';
import { ContentComponent } from './content/content.component';
import { MenuComponent } from './menu/menu.component';
import { MenuItemComponent } from './menu-item/menu-item.component';
import { MenuService } from './services/menu.service';
import { HeaderService } from './services/header.service';
import { LockScreenComponent } from './users/lock-screen/lock-screen.component';
import { LoginScreenComponent } from './users/login-screen/login-screen.component';
import { ScreenService } from './services/screen.service';
import { CommonService } from './services/common.service';
import { ForgotPasswordComponent } from './users/forgot-password/forgot-password.component';
import { ModalModule } from 'ngx-bootstrap';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ModalModule.forRoot()
  ],
  declarations: [
    C2FlexBodyComponent,
    TitleBarComponent,
    StatusBarComponent,
    ContentComponent,
    MenuComponent,
    MenuItemComponent,
    LockScreenComponent,
    LoginScreenComponent,
    ForgotPasswordComponent
  ],
  providers: [
    MenuService,
    HeaderService,
    ScreenService,
    CommonService
  ],
  exports: [
    C2FlexBodyComponent,
    LoginScreenComponent
  ],
  entryComponents: [ForgotPasswordComponent]
})

export class C2FlexModule { }

