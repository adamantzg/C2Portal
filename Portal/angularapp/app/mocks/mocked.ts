import { ActivatedRoute } from '@angular/router';
import { Observable, of } from 'rxjs';

export class MockActivatedRoute extends ActivatedRoute {
    constructor() {
        super();
        this.params = of({id: '5'});
    }
}

export class MockLink {
    clicked = false;
    href = '';
    download = '';
    target = '';
    element = '';

    constructor(element: string) {
        this.element = element;
    }

    click() {
        this.clicked = true;
    }
}
