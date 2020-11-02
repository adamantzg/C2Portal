import { Component, OnInit, AfterViewInit } from '@angular/core';
import { HeaderService } from '../../c2-flex/services/header.service';
import { User, Invoice, Customer } from '../domainclasses';
import { UserService } from '../services/user.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { InvoiceService } from '../services/invoice.service';
import { CustomerTotals } from '../modelclasses';
import * as moment from 'moment';
import { BsDatepickerConfig, BsLocaleService, BsModalService } from 'ngx-bootstrap';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { enGbLocale } from 'ngx-bootstrap/locale';
import { Router, ActivatedRoute } from '@angular/router';
import { GridDefinition, GridColumn, GridColumnType, GridButtonEventData, ColumnDataType,
  SortDirection, Sort, GridButton } from '../../common_components/components';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { MessageBoxCommand } from '../common/ModalDialog';
import { OrderFilter } from '../common/orderfilter/orderfilter';

@Component({
  selector: 'invoice-history',
  templateUrl: './invoice-history.component.html',
  styleUrls: ['./invoice-history.component.css']
})

export class InvoiceHistoryComponent implements OnInit {


  constructor(
    private userService: UserService,
    private commonService: CommonService,
    private invoiceService: InvoiceService,
    private _localeService: BsLocaleService,
    private router: Router,
    private messageBoxService: MessageboxService,
    private headerService: HeaderService
    ) {

    this._localeService.use('en-gb');
    defineLocale('en-gb', enGbLocale);
    this.headerService.title = 'Invoice history';
   }

   user: User = this.userService.CurrentUser;
   customer_code = '';
   customer: Customer;
  // impersonated: Customer;
   totals: CustomerTotals = new CustomerTotals();
   invoices: Invoice[] = [];
   filteredInvoices: Invoice[] = [];
   errorMessage = '';
   page = 1;
   lastpage = 1;
   pageSize = 50;
   numFormat = '1.2-2';
   loadingTotals = true;
   loadingInvoices = true;
   currency = 'GBP';
   impersonation = false;
   sortBy = 'DESC';
   showDesc = true;
   showAsc= false;
   dateFormat = 'dd/MM/yyyy';
   filter: OrderFilter = new OrderFilter();
   bsConfig: BsDatepickerConfig;
   fromStr: string;
   toStr: string;
   gridDefinition = new GridDefinition([
     new GridColumn('', 'selected', GridColumnType.Checkbox),
     new GridColumn('Item Date', 'dated', GridColumnType.Label, 'dated', null, null, null, null, true,
      ColumnDataType.Date, {format: this.dateFormat}),
     new GridColumn('Item Type', 'kind'),
     new GridColumn('C2 Order No', 'reference', GridColumnType.Label, 'c2order', null, null, null, null, true ),
     new GridColumn('Customer Order Number', 'customer_order_num', GridColumnType.Label, 'customerorder', null, null, null, null, true ),
     new GridColumn('Invoice Number', 'item_no', GridColumnType.Label, 'invoiceNumber', null, null, null, null, true ),
     new GridColumn('Due Date', 'due_date', GridColumnType.Label, 'duedate', null, null, null, null, true, ColumnDataType.Date,
     {format: this.dateFormat} ),
     new GridColumn('Amount', 'amount', GridColumnType.Label, 'amount', null, null, null, null, true, ColumnDataType.Currency,
      {currencyCode: this.currency, display: 'symbol', format: this.numFormat  } ),
      new GridColumn('buttons', '', GridColumnType.ButtonGroup, 'buttons', null, null, null, null, false, null, null, null, false,
      null,
      [
        new GridButton('View', 'view', '', 'btn'),
        new GridButton('POD', 'pod', '', 'btn')
      ])

   ]);



  ngOnInit() {
    this.bsConfig = new BsDatepickerConfig();
    this.bsConfig.dateInputFormat = 'DD/MM/YYYY';
    this.initFilter();
    /*this.filter.from = moment().subtract(6, 'month').toDate();
    this.filter.to = new Date();*/
    this.gridDefinition.sort = new Sort(this.gridDefinition.columns.find(c => c.field === 'dated'), SortDirection.Desc);
    this.gridDefinition.columnButtonVisibilityCallback = this.checkInvoiceButtonVisibility;
    if (!this.user.isTopAdmin && !this.user.isBranchAdmin) {
      this.getData(this.user.customer_code);
    }
  }


  getPageLimits() {
    const result: any = {};
    result.from = (this.page - 1) * this.pageSize;
    result.to = result.from + this.pageSize - 1;
    return result;
  }

  showForCustomer(c: Customer) {
    this.page = 1;
    if (c != null) {
      this.customer_code = c.code;
      this.impersonation = true;
      this.getData(this.customer_code);
    } else {
      this.impersonation = false;
      this.getData(this.user.customer_code);
    }

  }

  getData(customer: string) {

    this.loadingTotals = true;

    this.invoiceService.getCustomer(customer).subscribe(data => {
      if (data != null) {
        this.customer = data;
        if (this.customer.currency != null && this.customer.currency.trim().length > 0) {
          const curr = this.customer.currency;
          if (curr === 'EU1') {
            this.currency = 'EUR';
          } else if (curr === 'USD') {
            this.currency = 'USD';
          }
          if (this.gridDefinition != null) {
            this.gridDefinition.columns.filter(c => c.dataType === ColumnDataType.Currency)
              .forEach(c => c.format.currencyCode = this.currency);
          }
        }
      }

    },
    err =>  {
        this.errorMessage = this.commonService.getError(err);
      }
    );

    this.getCustomerTotals(customer);
    this.getInvoices(customer);
  }

  getCustomerTotals(customer: string) {
    this.invoiceService.getCustomerTotals(customer, this.filter.from, this.filter.to, this.filter.searchText).subscribe( data =>  {
      this.totals = data.result;
      this.loadingTotals = false;
    } ,
      err =>  {
        this.errorMessage = this.commonService.getError(err);
        this.loadingTotals = false;
      });
  }

  getInvoices(customer: string) {
    this.loadingInvoices = true;
    const limits = this.getPageLimits();
    const sortBy = this.sortBy;
    this.invoiceService.getInvoices(customer, this.filter.from, this.filter.to, limits.from, limits.to,
      this.gridDefinition.sort.column.field, this.gridDefinition.sort.direction, this.filter.searchText).subscribe( (data) => {

      const invoices: Invoice[] = data.result;
      invoices.forEach(i => {
        i.dated = new Date(i.dated);
        i.due_date = new Date(i.due_date);
        if (i.kind === 'CSH') {
          i.customer_order_num = 'Payment on Account';
        }
      });
      this.invoices = invoices;
      this.filteredInvoices = invoices;
      this.loadingInvoices = false;
    },
    err => {
      this.errorMessage = this.commonService.getError(err);
      this.loadingInvoices = false;
    });
  }

  pageChanged(e: any) {
    this.page = e.page;
    if (this.page !== this.lastpage) {
      this.getInvoices(this.impersonation ? this.customer_code : this.user.customer_code);
      this.lastpage = this.page;
    }
  }

  viewInvoice(order_no: string) {
  this.invoiceService.getInvoicePdf(order_no).subscribe((data: string) => {
      this.createPdfLink(order_no, data);
    });
  }

  createPdfLink(title: string, data: string) {
    const blob = this.createBlob(data);
    if (navigator.msSaveOrOpenBlob) {
      // IE
      const retVal = navigator.msSaveOrOpenBlob(blob, title + '.pdf');
    } else {
      const link = document.createElement('a');
      link.href = (window.URL).createObjectURL(blob);

      link.download = (title + '.pdf');
      link.target = '_blank';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }

  }

  createBlob(data: string) {
    const bData = atob(data);
    const byteArrays = [];
    const sliceSize = 512;

      for (let offset = 0; offset < bData.length; offset += sliceSize) {
          const slice = bData.slice(offset, offset + sliceSize);

          const byteNumbers = new Array(slice.length);
          for (let i = 0; i < slice.length; i++) {
              byteNumbers[i] = slice.charCodeAt(i);
          }

          const byteArray = new Uint8Array(byteNumbers);

          byteArrays.push(byteArray);
      }
    return new Blob(byteArrays, { type: 'application/pdf' });
  }



  applyFilter() {
    const customer = this.impersonation ? this.customer_code : this.user.customer_code;
    this.getCustomerTotals(customer);
    this.getInvoices(customer);
  }

  clearFilter() {
    this.initFilter();
    this.applyFilter();
  }

  orderTracking(ref) {
    console.log('Button je stisnut ref je: ', ref);
    this.router.navigate(['./c2/order-tracking'], {queryParams: {'order_no': ref}});
  }

  gridButtonClicked($event: GridButtonEventData) {
    if ($event.name === 'view') {
      this.viewInvoice($event.row.reference);
    }else if ($event.name === 'pod') {
      this.showPod($event.row);
    }
  }

  onGridSortChange($event) {
    this.getInvoices(this.impersonation ? this.customer_code : this.user.customer_code);
  }

  selectedOrders() {
    return this.invoices.filter(i => i.selected);
  }

  viewSelectedOrders() {
    const order_nos = this.selectedOrders().map(o => o.reference).join(',');
    this.invoiceService.getInvoicePdfs(order_nos).subscribe((data: string) => {
      this.createPdfLink(order_nos, data);
    });
  }

  showPod(i: Invoice) {
    /*const modal = this.bsModalService.show(PodModalComponent);
    modal.content.Invoice = i;*/

      const customer = this.impersonation ? this.customer_code : this.user.customer_code;
      this.invoiceService.checkInvoicePod(i.reference, customer, i.customer_order_num, i.postcode).subscribe((data: any) => {
        if (!data) {
          this.messageBoxService.
          openDialog('Request for POD sent, you will receive a copy within 2 working days from Customer Services');
        } else {
          let url = `http://crosswater.calidus-portal.com:45554/CROLIVE/Gateway`;
          url += `?mod=POR&src=LOTS&sub=tracking&act=find&ref=${i.reference.trim()}&pc=${i.postcode.replace(' ', '')}`;
          window.open(url);
        }
      },
      err => {
        this.errorMessage = this.commonService.getError(err);
      });


  }

  checkInvoiceButtonVisibility(i: Invoice, name: string, buttonName: string) {
    if (buttonName === 'pod') {
      return i.kind !== 'CRN' && i.kind !== 'CSH';
    } else if (buttonName === 'view') {
      return i.kind !== 'CSH';
    }
    return true;
  }



  initFilter() {
    this.filter.from = moment().subtract(6, 'month').toDate();
    this.filter.to = moment().toDate();
    this.filter.searchText = '';
  }

}
