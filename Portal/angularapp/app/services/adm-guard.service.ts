import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from './user.service';
import { User, RoleId } from '../domainclasses';

@Injectable()
export class AdmGuard implements CanActivate {

  constructor(private userService: UserService, private router: Router) {}

  canActivate(): boolean {
    this.userService.CurrentUser.roles.forEach(r => {

      console.log(`RoleId: ${RoleId.Admin} id: ${r.id}`);

      if (+r.id === RoleId.Admin) {
        console.log(`Id: ${r.id}`);
        console.log(`TEST: ${+r.id === RoleId.Admin} `);
        return true;
      }else {
        this.router.navigate(['/c2/welcome']);
      }
    });
    return false;
  }
}
