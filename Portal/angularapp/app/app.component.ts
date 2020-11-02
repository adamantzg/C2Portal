import { Component, OnInit } from '@angular/core';
// import { FrameworkConfigService, FrameworkConfigSettings } from '../fw/services/framework-config.service';
import { initialMenuItems } from './app.menu';
import { UserService } from './services/user.service';
import { MenuService } from '../c2-flex/services/menu.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  ngOnInit(): void {
    if (this.userService.CurrentUser != null) {
      this.setUpMenu();
    }
  }

  constructor(private userService: UserService,
  private menuService: MenuService) {
    this.userService.userSetEvent.subscribe(u => this.setUpMenu());
  }

  setUpMenu() {
    this.menuService.items = this.userService.getMenuItems();
  }


}
