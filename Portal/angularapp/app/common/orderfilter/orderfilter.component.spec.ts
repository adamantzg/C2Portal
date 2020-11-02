import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderfilterComponent } from './orderfilter.component';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule, BsDatepickerConfig, ComponentLoaderFactory, PositioningService, BsLocaleService } from 'ngx-bootstrap';

describe('OrderfilterComponent', () => {
  let component: OrderfilterComponent;
  let fixture: ComponentFixture<OrderfilterComponent>;

  setupTestBed({
    declarations: [ OrderfilterComponent ],
    imports: [FormsModule, BsDatepickerModule ],
    providers: [BsDatepickerConfig, ComponentLoaderFactory, PositioningService, BsLocaleService ]
  });


  beforeEach(() => {
    fixture = TestBed.createComponent(OrderfilterComponent);
    component = fixture.componentInstance;
  });

  test('should create', () => {
    expect(component).toBeTruthy();
    expect(fixture).toMatchSnapshot();
  });

  test('events', () => {
    let applied = false, cleared = false;
    component.filterApplied.subscribe(() => applied = true );
    component.filterCleared.subscribe(() => cleared = true );
    component.applyFilter();
    expect(applied).toBeTruthy();
    component.clearFilter();
    expect(cleared).toBeTruthy();
    expect(component.filter.searchText).toBe('');


  });
});
