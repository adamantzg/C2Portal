import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from './user.service';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor( private userService: UserService, private router: Router) { }
  canActivate(): boolean {
    // console.log(`AuhtGuard#canActivate called ${this.userService.CurrentUser.roles[0].id}`);

    if (this.userService.CurrentUser == null) {
      console.log('NOT AUTH');
      this.router.navigate(['/login']);
    }
    return true;
  }
}
