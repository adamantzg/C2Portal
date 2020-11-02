import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../services/header.service';
import { UserApi, IUser } from '../users/user-api';
// import { User } from '../../app/classes/tracking';
// 'app/classes/tracking';
import { Observable } from 'rxjs/Observable';
import { ScreenService } from '../services/screen.service';
import { MenuService } from '../services/menu.service';

@Component({
  selector: 'c2-flex-title-bar',
  templateUrl: './title-bar.component.html',
  styleUrls: ['./title-bar.component.css']
})
export class TitleBarComponent implements OnInit {
  logged: IUser;
  constructor(
    public headerService: HeaderService,
    private userApi: UserApi,
    public screenService: ScreenService,
    public menuService: MenuService
  ) { }

  ngOnInit() {
     this.userApi.logged().subscribe(
      data => {
        this.logged = data; // `${data.name} ${data.lastname}`;
        console.log(`LOGED: ${data}`);
      }
    );
  }
  signOut() {
    this.userApi.signOut();
  }

}
