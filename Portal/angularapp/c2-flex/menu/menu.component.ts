import { Component, EventEmitter , OnInit, Output, Input } from '@angular/core';
import { MenuService } from '../services/menu.service';
import { ScreenService } from '../services/screen.service';

@Component({
  selector: 'c2-flex-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  @Output() showLockScreen= new EventEmitter<boolean>();
  // showLockScreen= false;
  showingLeftSideMenu = false;

  constructor(public menuService: MenuService, public screenService: ScreenService ) { }

  setShowHide(sh: boolean) {
    this.showLockScreen.emit( sh);
  }
  ngOnInit() {
  }

  toggleLeftSideMenu(): void {
    this.showingLeftSideMenu =  !this.showingLeftSideMenu;
  }

}
