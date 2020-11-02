import { Component, OnInit } from '@angular/core';
import { UserApi } from '../user-api';
import { BsModalRef } from 'ngx-bootstrap';
import { CommonService } from '../../services/common.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(public userApi: UserApi, public bsModalRef: BsModalRef,
    public commonService: CommonService
   ) { }

  email: string;
  errorMessage = '';
  successMessage = '';
  loading = false;

  ngOnInit() {
  }

  ok() {
    this.loading = true;
    this.userApi.sendPasswordRecoveryLink(this.email).subscribe(d => {
      this.loading = false;
      this.successMessage = `Password recovery link sent to ${this.email}.`;
      setTimeout(() => {
        this.bsModalRef.hide();
      }, 2000);
    },
    err => {
      this.loading = false;
      this.errorMessage = this.commonService.getError(err);
    });
  }

  cancel() {
    this.bsModalRef.hide();
  }

}
