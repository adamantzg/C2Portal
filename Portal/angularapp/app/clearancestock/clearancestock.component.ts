import { Component, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { UserService } from '../services/user.service';
import { User, Customer, ClearanceStock } from '../domainclasses';
import { GridDefinition, GridColumn, GridColumnType, ColumnDataType, Sort, SortDirection } from '../../common_components/components';
import { HeaderService } from '../../c2-flex/services/header.service';

@Component({
    selector: 'app-clearance-stock',
    templateUrl: './clearancestock.component.html',
    styleUrls: ['./clearancestock.component.css']
  })
  export class ClearanceStockComponent implements OnInit {

    constructor(
        private headerService: HeaderService,
        private productService:  ProductService,
        private commonService: CommonService,
        private userService: UserService
      ) {
        this.headerService.title = 'Clearance stock';
    }

    user: User = this.userService.CurrentUser;
    customer_code = '';
    customer: Customer;
    errorMessage = '';
    loading = false;
    pageSize = 50;
    page = 1;
    lastpage = 1;
    code = '';
    description = '';
    range = '';
    numOfRecords = 0;
    currency = 'GBP';
    data: ClearanceStock[] = [];
    numFormat = '1.2-2';
    dataRetrieved = false;

    gridDefinition = new GridDefinition([
        new GridColumn('Product Code', 'product_code', GridColumnType.Label, 'code', null, null, null, null, true ),
        new GridColumn('Description', 'long_description', GridColumnType.Label, 'description', null, null, null, null, true ),
        new GridColumn('Range', 'range', GridColumnType.Label, 'range', null, null, null, null, true ),
        new GridColumn('RRP £ (Ex VAT)', 'exvat_price', GridColumnType.Label, 'exvat_price', null, null, null, null, true,
         ColumnDataType.Currency, {currencyCode: this.currency, display: 'symbol', format: this.numFormat  } ),
        new GridColumn('Clearance price £ (Ex VAT)', 'price', GridColumnType.Label, 'price', null, null, null, null, true,
        ColumnDataType.Currency, {currencyCode: this.currency, display: 'symbol', format: this.numFormat  } ),
        new GridColumn('Discount', 'discount', GridColumnType.Label, 'discount', null, null, null, null, true,
        ColumnDataType.Percent, {format: this.numFormat} ),
        new GridColumn('Stock remaining', 'freestock', GridColumnType.Label, 'freestock', null, null, null, null, true )
      ]);


    ngOnInit(): void {
        this.getData();
        this.gridDefinition.sort = new Sort(this.gridDefinition.columns.find(c => c.field === 'product_code'), SortDirection.Desc);
        setInterval(this.getData.bind(this), 60000);
    }

    pageChanged(e: any) {
        this.page = e.page;
        if (this.page !== this.lastpage) {
          this.getData();
          this.lastpage = this.page;
        }
    }

    getData() {
        const limits = this.getPageLimits();
        this.loading = true;
        this.productService.getClearanceStockCount(this.code, this.description, this.range).subscribe(
            (num) => {
                this.numOfRecords = num.result;
                this.productService.getClearanceStock(this.code, this.description, this.range, limits.from, limits.to,
                    this.gridDefinition.sort.column.field, this.gridDefinition.sort.direction).subscribe( (data) => {
                        this.data = data.result.filter(d => d.product_code !== 'L');
                        this.loading = false;
                        this.dataRetrieved = true;
                    },
                    err => {
                        this.errorMessage = this.commonService.getError(err);
                        this.loading = false;
                      }
                    );
            },
            err => {
                this.errorMessage = this.commonService.getError(err);
                this.loading = false;
              }
        );
    }

    getPageLimits() {
        const result: any = {};
        result.from = (this.page - 1) * this.pageSize;
        result.to = result.from + this.pageSize - 1;
        return result;
    }

    onGridSortChange($event) {
        this.getData();
    }

    resetFilter() {
        this.code = '';
        this.description = '';
        this.range = '';
        this.getData();
    }

    export() {
        const headers = ['Product Code', 'Description', 'Range', 'RRP £ (Ex VAT)', 'Clearance price £ (Ex VAT)',
        'Discount', 'Stock remaining'];
        let text = headers.join(',');
        text += '\n';
        for (let i = 0; i < this.data.length; i++) {
            const r = this.data[i];
            const strData = [r.product_code, r.long_description, r.range, r.exvat_price, r.price, r.discount, r.freestock];
            text += strData.join(',') + '\n';
        }
        this.createLink('Clearance stock.csv', text);
    }

    createLink(filename: string, data: string) {
        const blob = this.createBlob(data);
        if (navigator.msSaveOrOpenBlob) {
          // IE
          const retVal = navigator.msSaveOrOpenBlob(blob, filename);
        } else {
          const link = document.createElement('a');
          link.href = (window.URL).createObjectURL(blob);
          link.download = (filename);
          link.target = '_blank';
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }

      }

      createBlob(data: string) {
        /*const bData = atob(data);
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
          }*/
        return new Blob([data], { type: 'text/csv' });
      }
  }
