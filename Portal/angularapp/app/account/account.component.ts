import { Component, OnInit } from '@angular/core';
import { User, Customer } from '../domainclasses';
import { HeaderService } from '../../c2-flex/services/header.service';
import { UserService } from '../services/user.service';
import { CommonService } from '../../c2-flex/services/common.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})

export class AccountComponent implements OnInit {

  constructor( private headerService: HeaderService,
  private userService: UserService,
  private commonService: CommonService
  ) {
    headerService.title = 'Account';
   }

   user: User = new User();
   successMessage = '';
   errorMessage = '';


  ngOnInit() {
    this.user = this.userService.CurrentUser;

  }

  update() {
    this.userService.updateUserData(this.user).subscribe( (d: any) => this.successMessage = d,
    err => this.errorMessage = this.commonService.getError(err));
  }



}
