import { Component, OnInit } from '@angular/core';
import { User, PermissionId } from '../domainclasses';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit {

  constructor(private userService: UserService) {

   }

   user: User = this.userService.CurrentUser;
   permissions = Object.assign({}, PermissionId);

  ngOnInit() {
  }
  test() {
    alert();
    console.log('test');
  }
/***new style text  ** */
  userHasPermission(p: PermissionId) {
    return this.user.permissions.find(x => x.id === p);
  }
}
