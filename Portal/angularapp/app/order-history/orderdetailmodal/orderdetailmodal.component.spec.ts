import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderdetailmodalComponent } from './orderdetailmodal.component';
import { BsModalRef, ModalModule } from 'ngx-bootstrap';

describe('OrderdetailmodalComponent', () => {
  let component: OrderdetailmodalComponent;
  let fixture: ComponentFixture<OrderdetailmodalComponent>;

  setupTestBed({
      declarations: [ OrderdetailmodalComponent ],
      providers: [BsModalRef],
      imports: [ModalModule]
    });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderdetailmodalComponent);
    component = fixture.componentInstance;

  });

  test('should create', () => {
    expect(component).toBeTruthy();
    expect(component).toMatchSnapshot();
  });
});
