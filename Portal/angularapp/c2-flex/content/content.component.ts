import { Component, OnInit } from '@angular/core';
import { ScreenService } from '../services/screen.service';
import { MenuService } from '../services/menu.service';
// import { MenuService } from '@crosswater-template/services/menu.service';

@Component({
  selector: 'c2-flex-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css']
})
export class ContentComponent implements OnInit {
  showLockScreen= false;

  screenAction(ev: boolean) {
    this.showLockScreen = ev;
  }
  constructor(public screenService: ScreenService, public menuService: MenuService) { }

  ngOnInit() {
  }

}
