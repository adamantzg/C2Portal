import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'lock-screen',
  templateUrl: './lock-screen.component.html',
  styleUrls: ['./lock-screen.component.css']
})
export class LockScreenComponent implements OnInit {
  @Output() unlockScreen = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }

  unlock(show: boolean) {
    this.unlockScreen.emit(show);
  }

}
