import { Injectable, EventEmitter } from '@angular/core';

import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { UserApi } from '../../c2-flex/users/user-api';
import { Router } from '@angular/router';
import { User, RoleId, Role } from '../domainclasses';
import { HttpService } from './http.service';
import { tap } from 'rxjs/operators/tap';
import { MenuItem } from '../../c2-flex/services/menu.service';
import { initialMenuItems } from '../app.menu';
import { UsersModel, ChangePass } from '../modelclasses';

@Injectable()
export class UserService implements UserApi {

  private currentUser: User;
  isAuthenticated = false;
  api = 'api/user/';

  userSetEvent: EventEmitter<User> = new EventEmitter<User>();

  constructor(
    private router: Router,
    private httpService: HttpService
  ) { }

  logged(): Observable<User> {
    return of(this.currentUser);
  }

  get CurrentUser() {
    if (this.currentUser == null) {
      this.currentUser = this.loadUser();
    }
    return this.currentUser;
  }

  signIn(username: string, password: string, rememberMe?: boolean): Observable<any> {
    this.httpService.clearToken();
    return this.httpService.post('api/login/' + `?username=${username}&password=${password}`).pipe(
      tap(u => {
        this.currentUser = u;
        this.saveUser(u);
        this.userSetEvent.emit(u);
      })
    );


  }
  getAll(): Observable<any> {
    return this.httpService.get(`api/users`);
  }

  getUsersModel(): Observable<UsersModel> {
    return this.httpService.get('api/usersModel');
  }

  signOut(): Observable<any> {
    this.currentUser = null;
    this.clearUser();
    this.router.navigate(['/login']);
    return of({});
  }

  saveUser(u: User) {
    this.httpService.Token = u.token;
    localStorage.setItem('c2_custportal_user', JSON.stringify(u));
  }

  clearUser() {
    this.httpService.Token = null;
    localStorage.removeItem('c2_custportal_user');
  }

  loadUser(): User {
    const sUser = localStorage.getItem('c2_custportal_user');
    if (sUser != null) {
      return JSON.parse(sUser);
    }
    return null;
  }

  updateUserData(u: User) {
    this.saveUser(u);
    return this.httpService.post(this.api + 'updateData', u);
  }

  customerSearch(code: string, role_id?: number) {
    return this.httpService.get(this.api + 'customerSearch', { params: {code: code, role_id: role_id}});
  }

  getCustomer(code: string) {
    return this.httpService.get(this.api + 'getCustomer', {params: {code: code}});
  }

  createUpdateUser(u: User) {
    return this.httpService.post(this.api + (u.id > 0 ? 'update' : 'create'), u);
  }

  updateUser(u: User) {
    return this.httpService.post(this.api + 'update', u);
  }

  deleteUser(id: number) {
    return this.httpService.post(this.api + 'delete?id=' + id);
  }

  activateUser(id: number) {
    return this.httpService.post(this.api + 'activate?id=' + id);
  }

  checkUser(code: string) {
    return this.httpService.post(this.api + 'checkUser', {code: code});
  }

  updatePassword(cp: ChangePass) {
    return this.httpService.post(this.api + 'updatePassword', cp);
  }

  resetPassword(id: number) {
    return this.httpService.post(this.api + 'resetpassword?id=' + id);
  }

  sendPasswordRecoveryLink(email: string): Observable<any> {
    return this.httpService.post(this.api + 'sendPasswordRecoveryLink?email=' + email);
  }

  getRolesModel() {
    return this.httpService.get(this.api + 'getRolesModel');
  }

  createUpdateRole(r: Role) {
    return this.httpService.post(this.api + (r.id > 0 ? 'updateRole' : 'createRole'), r);
  }

  deleteRole(id: number) {
    return this.httpService.post(this.api + 'deleteRole?id=' + id);
  }

  getMenuItems(): MenuItem[] {
    const user = this.CurrentUser;
    // let userRoles = [RoleId.User];
    let userPermissions = [];
    /*if (user.roles != null) {
      userRoles = user.roles.map(r => r.id);
    }*/
    if (user.permissions != null) {
      userPermissions = user.permissions.map(p => p.id);
    }
    const items: MenuItem[] = [];
    initialMenuItems.forEach( m => {
      if ( m.permissions.find(p => userPermissions.find(x => x === p) != null )) {
        items.push(m);
      }
    });
    return items.sort((a, b) => a.text < b.text ? -1 : 1);
  }
}
