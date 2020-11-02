import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { MenuService } from '../../c2-flex/services/menu.service';
import { initialMenuItems } from '../app.menu';
import { MenuItem } from '../../c2-flex/services/menu.service';
import { RoleId } from '../domainclasses';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private userService: UserService,
  private menuService: MenuService) { }

  ngOnInit() {

  }



}
