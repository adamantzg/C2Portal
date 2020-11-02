import { Component } from '@angular/core';

export class RouterMock {
    navigateUrls: string[];
    params: any;

    navigate(urls: string[], params?: any) {
        this.navigateUrls = urls;
        this.params = params;
    }
}

export class ActivatedRouteMock {

}


export class RouterLinkMock {

}

@Component({
    template: ''
  })
  export class DummyComponent {
  }
