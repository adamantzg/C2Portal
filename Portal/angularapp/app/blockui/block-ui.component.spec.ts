import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BlockUiComponent } from './block-ui.component';
import { BlockUIService } from '../../common_components/services/block-ui.service';

describe('BlockUiComponent', () => {
  let component: BlockUiComponent;
  let fixture: ComponentFixture<BlockUiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BlockUiComponent ],
      providers: [BlockUIService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BlockUiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
