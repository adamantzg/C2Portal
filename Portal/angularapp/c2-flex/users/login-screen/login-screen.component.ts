import { Component, OnInit, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserApi } from '../user-api';
import { CommonService } from '../../services/common.service';
import { BsModalService } from 'ngx-bootstrap';
import { ForgotPasswordComponent } from '../forgot-password/forgot-password.component';

@Component({
  selector: 'login-screen',
  templateUrl: './login-screen.component.html',
  styleUrls: ['./login-screen.component.css']
})
export class LoginScreenComponent implements OnInit {
  submitting = false;
  formError: string;
  userInputFocus = false;
  passInputFocus = false;

  constructor(
    private userApi: UserApi,
    private router: Router,
    private commonService: CommonService,
    private bsModalService: BsModalService
  ) { }

  ngOnInit() {
  }

  userInputSetFocus(focus: boolean) {
    this.userInputFocus = focus;
  }
  passInputSetFocus(focus: boolean) {
    this.passInputFocus = focus;
  }
  onSubmit(signInForm: NgForm) {
    if (signInForm.valid) {
      console.log('SUBMITING... ', signInForm);

      this.submitting = true;
      this.formError = null;

      this.userApi.signIn(signInForm.value.username, signInForm.value.pass, signInForm.value.rememberMe)
        .subscribe((data) => {
          console.log('got valid:', data);
          this.router.navigate(['/c2']);
        },
        (err) => {
          this.submitting = false;
          console.log('got error: ', err);
          this.formError = this.commonService.getError(err);
        });


    }

  }

  forgotPass() {
    this.bsModalService.show(ForgotPasswordComponent);
  }

}
