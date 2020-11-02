import { Injectable } from '@angular/core';

export class MenuItem {
  text: string;
  icon: string;
  route: string;
  submenu: MenuItem[];
}

@Injectable()
export class MenuService {

  items: Array<MenuItem>;
  isVertical= true;
  showingLeftSideMenu= false;

  constructor() { }

  toggleLeftSideMenu(): void {
    this.isVertical = true;
    this.showingLeftSideMenu = !this.showingLeftSideMenu;
  }
}
