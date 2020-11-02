import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../domainclasses';
import { UserService } from '../../services/user.service';
import { ChangePass } from '../../modelclasses';
import { CommonService } from '../../../c2-flex/services/common.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.css']
})
export class SetPasswordComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private commonService: CommonService,
    private router: Router) { }

  errorMessage = '';
  inputFocus = { '1' : true, '2': false};
  code = this.route.snapshot.params.code;
  cp: ChangePass = new ChangePass();

  ngOnInit() {
    this.userService.checkUser(this.code).subscribe( (c: ChangePass) =>
    this.cp = c,
    err => this.errorMessage = this.commonService.getError(err));
  }

  InputSetFocus(index: string, focus: boolean) {
    this.inputFocus[index] = focus;
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.userService.updatePassword(this.cp).subscribe(data => {
        this.userService.signIn(this.cp.username, this.cp.password1).subscribe(d => this.router.navigate(['/c2']),
        err => this.errorMessage = this.commonService.getError(err));
      },
      err => this.errorMessage = this.commonService.getError(err));
    }
  }

}
