import { Component, OnInit, AfterViewInit } from '@angular/core';

import { Settings } from '../settings';
import { HeaderService } from '../../c2-flex/services/header.service';
import { HttpClient } from '@angular/common/http';
import { Order, OrderStatus, User, RoleId, Customer, Holiday } from '../domainclasses';
import { OrderService } from '../services/order.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { UserService } from '../services/user.service';
import * as moment from 'moment';
import { ActivatedRoute, Router } from '@angular/router';

// import { HttpClient } from 'selenium-webdriver/http';
import { HolidayService } from '../services/holiday.service';

@Component({
  selector: 'order-tracking',
  templateUrl: './order-tracking.component.html',
  styleUrls: ['./order-tracking.component.css']
})
export class OrderTrackingComponent implements OnInit {
  holidays: Holiday[];
  order: Order;
  customer: Customer;
  orderStatuses = Object.assign({}, OrderStatus);
  //  order = TRACKING_ORDER;
  //  order2: {};
  datePattern = Settings.datePattern;
  order_no = '';
  errorMessage = '';
  customer_code = '';
  return_to = 'invoices';
  expectedDeliveryDate: moment.Moment;
  paramsChecked = false;

  param = '';
  constructor(
    private orderService: OrderService,
    private commonService: CommonService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private holidayService: HolidayService
  ) {

  }

  user = this.userService.CurrentUser;

  ngOnInit() {
    if (!this.user.isTopAdmin && !this.user.isBranchAdmin) {
      this.checkParams();
    }
  }

  checkParams() {
    this.paramsChecked = true;
    this.route.queryParams.subscribe(params => {
      this.return_to = params['return_to'] || 'invoices';
      this.param = params['order_no'] || null;
      if (this.param) {
        this.order_no = this.param;
        this.searchOrder();
      }
    });
  }

  searchOrder() {
    this.orderService.getOrder(this.order_no.trim(), this.customer_code).subscribe(data => {
      this.order = data.result;
      if (this.order == null) {
        this.errorMessage = 'Order not found or you don\'t have access.';
      } else {
        this.errorMessage = '';
        this.getCustomer(this.order.customer); // (this.order.customer); //('P00101');
      }


    },
      err => this.errorMessage = this.commonService.getError(err));
  }
  getCustomer(cust: string) {
    this.orderService.getCustomer(cust).subscribe(data => {
      this.customer = data;
      if (this.order.status !== OrderStatus.Delivered) {
        this.getExpectedDelivery();
      }

      if (this.customer === null) {
        this.errorMessage = 'Customer is empty';
      }
    });
  }
  getExpectedDelivery(): void {
    // Check for out of stock products
    let res: moment.Moment = null;
    if (this.order.productStockData.length > 0) {
      if (this.order.productStockData.find(d => d.ship_date != null)) {
        res = moment.max(this.order.productStockData.filter(d => d.ship_date != null).map(d => moment(d.ship_date)));
      }

    } else if (this.customer != null) {
      const auditsDespatch = this.order.audits.filter(d => +d.character_value === 6);
      if (auditsDespatch.length > 0) {
        res = moment
          .max(auditsDespatch
            .map(d => moment(d.audit_date)
              .add(
                this.customer.analysis_codes1 === 'GMCCROSSAN' ||
                  this.customer.analysis_codes1 === 'CWALSH' ||
                  this.customer.analysis_codes1 === 'SANDERSON' ? 2 : 1, 'days'
              )));

        // this.checkDays(res );

      } else {
        this.expectedDeliveryDate = null;
      }
    }
    if (res != null) {
      this.holidayService.getHolidays(res.year()).subscribe(
        (holidays: Holiday[]) => {
          const holidaysMoment = holidays.map(h => moment(h.date));

          // let nextHoliday = true;
          let mDate = res;
          while (this.isHoliday(mDate, holidaysMoment) || this.isWeekend(mDate)) {
              mDate = mDate.add(1, 'day');
          }

          this.expectedDeliveryDate = mDate;
        }
      );
    }
  }

  isHoliday(m: moment.Moment, holidays: moment.Moment[]) {
    return holidays.find(h => h.isSame(m, 'days')) != null;
  }

  isWeekend(m: moment.Moment) {
    return m.day() === 0 || m.day() === 6;
  }

  getStatus() {
    let result = '';

    if (+this.order.status === OrderStatus.Cancelled) {
      // r = this.RoleId.User
      result = 'Cancelled';
    } else if (+this.order.status === OrderStatus.Delivered) { // before were are hade this.order.invoice_date != null
      result = 'Delivered';
    } else if (+this.order.status > 5 && +this.order.status < OrderStatus.Delivered) { // this.getAuditDate(this.order) != null
      result = 'Processed';
    } else if (+this.order.status <= 5) {
      result = 'Received';
    }
    return result;
  }

  getAuditDate(o: Order) {
    if (o != null && o.audits != null && o.audits.length > 0) {
      return o.audits[0].audit_date;
    }
    return null;
  }

  getProcessedDate(o: Order) {
    if (o != null && o.audits != null && o.audits.length > 0) {
      const processedAudit = o.audits.find(a => +a.character_value === 6);
      if (processedAudit != null) {
        return processedAudit.audit_date;
      }
    }
    return null;
  }

  navigateToCaller() {
    if (this.return_to === 'invoices') {
      this.router.navigate(['./c2/invoice-history']);
    } else if (this.return_to === 'orders') {
      this.router.navigate(['./c2/order-history']);
    }

  }

  onOrderInput($event: KeyboardEvent) {
    if ($event.key === 'Enter') {
      this.searchOrder();
    }
  }

  showForCustomer(c: Customer) {
    if (c != null) {
      this.customer_code = c.code;
    } else {
      this.customer_code = '';
    }
    if (!this.paramsChecked) {
      this.checkParams();
    } else {
      this.searchOrder();
    }
  }

}
