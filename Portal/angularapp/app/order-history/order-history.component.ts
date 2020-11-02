import { Component, OnInit, AfterViewInit } from '@angular/core';
import { HeaderService } from '../../c2-flex/services/header.service';
import { UserService } from '../services/user.service';
import { User, Customer, Order } from '../domainclasses';
import { Sort, SortDirection, GridDefinition, GridColumn, ColumnDataType,
  GridColumnType, GridButtonEventData, GridButton } from '../../common_components/components';
import { OrderFilter } from '../common/orderfilter/orderfilter';
import * as moment from 'moment';
import { CustomerTotals, OrderTotals } from '../modelclasses';
import { OrderService } from '../services/order.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { BsLocaleService, enGbLocale, BsModalService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { Router } from '@angular/router';
import { OrderdetailmodalComponent } from './orderdetailmodal/orderdetailmodal.component';
import { InvoiceService } from '../services/invoice.service';
import { MessageboxService } from '../common/messagebox/messagebox.service';

@Component({
  selector: 'order-history',
  templateUrl: './order-history.component.html',
  styleUrls: ['./order-history.component.css']
})
export class OrderHistoryComponent implements OnInit {

  constructor(
    private userService: UserService,
    private orderService: OrderService,
    private commonService: CommonService,
    private _localeService: BsLocaleService,
    private router: Router,
    private bsModalService: BsModalService,
    private invoiceService: InvoiceService,
    private messageBoxService: MessageboxService,
    private headerService: HeaderService
  ) {


    this._localeService.use('en-gb');
    defineLocale('en-gb', enGbLocale);
    this.headerService.title = 'Order history';

   }

   user: User = this.userService.CurrentUser;
   errorMessage = '';
   customer_code = '';
   customer: Customer;
   impersonated: Customer;
   loading = false;
   dateFormat = 'dd/MM/yyyy';
   numFormat = '1.2-2';
   filter: OrderFilter = new OrderFilter();
   totals: OrderTotals = new OrderTotals();
   pageSize = 50;
   page = 1;
   lastpage = 1;
   impersonation = false;
   filteredOrders: Order[] = [];
   sortBy = 'DESC';
   showDesc = true;
   showAsc= false;


   gridDefinition = new GridDefinition([
    new GridColumn('C2 Order No', 'order_no', GridColumnType.Label, 'c2order', null, null, null, null, true ),
    new GridColumn('Customer Order Number', 'customer_order_no', GridColumnType.Label, 'customerorder', null, null, null, null, true ),
    new GridColumn('Status', 'statusText', GridColumnType.Label, 'status', null, null, null, null, true),
    new GridColumn('Order Date', 'date_entered', GridColumnType.Label, 'date_entered', null, null, null, null, true,
     ColumnDataType.Date, {format: this.dateFormat}),
    new GridColumn('Required Date', 'date_required', GridColumnType.Label, 'date_required', null, null, null, null, true,
    ColumnDataType.Date, {format: this.dateFormat} ),
    new GridColumn('buttons', '', GridColumnType.ButtonGroup, 'buttons', null, null, null, null, false, null, null, null, false,
    null,
    [
      new GridButton('Details', 'view', '', 'btn'),
      new GridButton('Status', 'order', '', 'btn'),
      new GridButton('POD', 'pod', '', 'btn')
    ])
  ]);

  showImpersonationBox() {
    return this.user.isTopAdmin || this.user.isBranchAdmin;
  }

  ngOnInit() {
    this.initFilter();
    this.gridDefinition.sort = new Sort(this.gridDefinition.columns.find(c => c.field === 'date_entered'), SortDirection.Desc);
    this.getOrderTotals(this.user.customer_code);
    this.gridDefinition.columnButtonVisibilityCallback = this.checkOrderButtonVisibility;
    if (!this.showImpersonationBox()) {
      this.getOrders(this.user.customer_code);
    }

  }




  showForCustomer(c: Customer) {
    this.page = 1;
    if (c != null) {
      this.customer_code = c.code;
      this.impersonation = true;
    } else {
      this.customer_code = '';
      this.impersonation = false;
    }
    this.applyFilter();
  }

  applyFilter() {
    const customer = this.impersonation ? this.customer_code : this.user.customer_code;
    this.getOrderTotals(customer);
    this.getOrders(customer);
  }

  pageChanged(e: any) {
    this.page = e.page;
    if (this.page !== this.lastpage) {
      this.getOrders(this.impersonation ? this.customer_code : this.user.customer_code);
      this.lastpage = this.page;
    }
  }

  gridButtonClicked($event: GridButtonEventData) {
    if ($event.name === 'view') {
      this.viewOrder($event.row);
    }else if ($event.name === 'order') {
      this.goToTracking($event.row);
    } else if ($event.name === 'pod') {
      this.showPod($event.row);
    }
  }

  onGridSortChange($event) {
    this.getOrders(this.impersonation ? this.customer_code : this.user.customer_code);
  }

  getOrders(customer: string) {
    this.loading = true;
    const limits = this.getPageLimits();
    const sortBy = this.sortBy;
    this.orderService.getOrders(customer, this.filter.from, this.filter.to, limits.from, limits.to,
      this.gridDefinition.sort.column.field, this.gridDefinition.sort.direction, this.filter.searchText).subscribe( (data) => {

      const orders: Order[] = data.result;
      orders.forEach(o => {
        if (o.date_entered != null) {
          o.date_entered = new Date(o.date_entered);
        }
        if (o.date_required != null) {
          o.date_required = new Date(o.date_required);
        }

      });
      this.filteredOrders = orders;
      this.loading = false;
    },
    err => {
      this.errorMessage = this.commonService.getError(err);
      this.loading = false;
    });
  }

  getPageLimits() {
    const result: any = {};
    result.from = (this.page - 1) * this.pageSize;
    result.to = result.from + this.pageSize - 1;
    return result;
  }

  viewOrder(o: Order) {
    this.orderService.getDetails(o.order_no).subscribe(data => {
      if (data != null) {
        o.details = data.result;
        let total = 0;
        o.details.forEach(d => {
          total += d.val;
        });
        const modal = this.bsModalService.show(OrderdetailmodalComponent);
        modal.content.order = o;
        modal.content.total = total;
      }
    });
  }

  goToTracking(o: Order) {
    this.router.navigate(['./c2/order-tracking'], {queryParams: {'order_no': o.order_no, 'return_to': 'orders'}});
  }

  getOrderTotals(customer: string) {
    this.loading = true;
    this.orderService.getOrderTotals(customer, this.filter.from, this.filter.to, this.filter.searchText).subscribe( data =>  {
      this.totals = data.result;
      this.loading = false;
    } ,
      err =>  {
        this.errorMessage = this.commonService.getError(err);
        this.loading = false;
      });
  }

  clearFilter() {
    this.initFilter();
    this.applyFilter();
  }

  initFilter() {
    this.filter.from = moment().subtract(6, 'month').toDate();
    this.filter.to = moment().toDate();
    this.filter.searchText = '';
  }

  checkOrderButtonVisibility(o: Order, name: string, buttonName: string) {
    if (buttonName === 'pod') {
      // tslint:disable-next-line:triple-equals
      return o.statusText === 'Invoiced' && o.planned != true;
    }
    return true;
  }

  showPod(o: Order) {
    /*const modal = this.bsModalService.show(PodModalComponent);
    modal.content.Invoice = i;*/

      const customer = this.impersonation ? this.customer_code : this.user.customer_code;
      this.invoiceService.checkInvoicePod(o.order_no, customer, o.customer_order_no, o.address6).subscribe((data: any) => {
        if (!data) {
          this.messageBoxService.
          openDialog('Request for POD sent, you will receive a copy within 2 working days from Customer Services');
        } else {
          let url = `/ShowOrderStatus.html`;
          //url += `?mod=POR&src=LOTS&sub=tracking&act=find&ref=${o.order_no.trim()}&pc=${o.address6.replace(' ', '')}`;
          window.open(url);
        }
      },
      err => {
        this.errorMessage = this.commonService.getError(err);
      });


  }

}
