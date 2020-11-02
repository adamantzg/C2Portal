import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SetPasswordComponent } from './set-password.component';
import { FormsModule } from '@angular/forms';
import { BlockUiComponent } from '../../blockui/block-ui.component';
import { routes } from '../../routestest';
import { RouterTestingModule } from '@angular/router/testing';
import { MockActivatedRoute } from '../../mocks/mocked';
import { UserService } from '../../services/user.service';
import { HttpService } from '../../services/http.service';
import { BlockUIService } from '../../../common_components/services/block-ui.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonService } from '../../../c2-flex/services/common.service';

describe('SetPasswordComponent', () => {
  let component: SetPasswordComponent;
  let fixture: ComponentFixture<SetPasswordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SetPasswordComponent, BlockUiComponent ],
      providers: [MockActivatedRoute, UserService, HttpService, BlockUIService, CommonService],
      imports: [RouterTestingModule.withRoutes(routes), FormsModule, HttpClientModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SetPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
