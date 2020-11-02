import { TestBed, inject } from '@angular/core/testing';
import { OrderService } from './order.service';
import {RouterTestingModule} from '@angular/router/testing';
import {Router} from '@angular/router';
import {routes} from '../routestest';
import { HttpClientModule } from '@angular/common/http';
import { HttpService } from './http.service';
import { BlockUIService } from '../../common_components/services/block-ui.service';
import { CommonService } from '../../c2-flex/services/common.service';

describe('OrderService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrderService, HttpService, BlockUIService, CommonService ],
      imports: [RouterTestingModule.withRoutes(routes), HttpClientModule]
    });
  });

  it('should be created', inject([OrderService], (service: OrderService) => {
    expect(service).toBeTruthy();
  }));
});
