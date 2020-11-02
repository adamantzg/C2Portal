import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../../c2-flex/services/header.service';
import { ProductService } from '../services/product.service';
import { Product, User, StockData, Customer } from '../domainclasses';
import { TypeaheadMatch } from 'ngx-bootstrap';
import { Observable } from 'rxjs/Observable';
import { ProductPrices } from '../modelclasses';
import { CommonService } from '../../c2-flex/services/common.service';
import { UserService } from '../services/user.service';
import * as moment from 'moment';


@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit {

  code= '';
  product: Product;
  products: Observable<Product[]>;
  autoCompleteloading = false;
  stockLoading = false;
  priceLoading = false;
  stock: number = null;
  prices: ProductPrices = null;
  errorMessage = '';
  numFormat = '1.2-2';
  customer_code = '';
  stock_level: StockLevelEnum = null;
  stock_message = '';
  showStock = false;
  user: User = this.userService.CurrentUser;
  stockLevels = Object.assign({}, StockLevelEnum);

  constructor(
    private headerService: HeaderService,
    private productService:  ProductService,
    private commonService: CommonService,
    private userService: UserService
  ) {
    headerService.title = 'Stock availability';
    this.products = Observable.create((observer: any) => {
      // Runs on every search
      observer.next(this.code);
    }).mergeMap((code: string) => this.productService.searchProduct(code));

   }

  ngOnInit() {
    // this.getProduct(this.code);
  }
 /*  getProduct(code) {
      this.productService.searchProduct(code).subscribe(

      );
  } */

  onProductSelected(e: TypeaheadMatch) {
    this.product = e.item;
    this.code = this.product.code;
    this.showStock = false;
    this.prices = null;
    this.errorMessage = '';
    this.showStockAndPrice(this.code);
  }

  showStockAndPrice(code: string) {
    if (this.product == null || this.product.code !== code) {
      this.productService.getProduct(code).subscribe(p => {
        this.product = p;
        this.loadStockAndPrice(code);
      },
      err => this.errorMessage = this.commonService.getError(err));
    } else {
      this.loadStockAndPrice(code);
    }
  }

  loadStockAndPrice(code: string) {
    this.stockLoading = true;
    this.priceLoading = true;
    this.errorMessage = '';
    this.productService.getFreeStock(code.toUpperCase()).subscribe( (res: any) => {
      const data: StockData = res.result;
      this.showStock = true;
      this.stock = data.quantity;
      this.stock_message = '';
      this.stock_level = this.getStockLevel(data.quantity);
      if (data.madeToOrder) {
        this.stock_message = 'Made to Order - please contact the office for details';
      } else {
        if (this.stock_level === StockLevelEnum.InStock || this.stock_level === StockLevelEnum.Expiring) {

          this.stock_message = data.expiresThisYear ? 'Whilst stock lasts' : 'In stock';
          if (this.user.isInternal || this.stock_level === StockLevelEnum.Expiring) {
            this.stock_message += ': ' + data.quantity.toString();
          }

        } else {
          if (data.discontinued) {
            this.stock_message = 'Product discontinued - No longer available';
          } else {
            this.stock_message = 'Out of stock.';
            if (data.ship_date != null) {
              this.stock_message += ` Next date of stock availability estimated at ${moment(data.ship_date).format('DD/MM/YYYY')}`;
            } else {
              this.stock_message += ' Please contact the office for stock availability';
            }
          }
        }
      }

      this.stockLoading = false;
    },
      err => this.errorMessage = this.commonService.getError(err));
    const customer = this.customer_code.length > 0 ? this.customer_code : this.user.customer_code;
    this.productService.getProductPrice(customer, code.toUpperCase()).subscribe( data => {
      this.prices = data.result;
      if (this.prices != null && this.prices.customerPrice == null) {
        this.prices.customerPrice = this.prices.basePrice;
      }
      this.priceLoading = false;
    },
      err => {
        this.errorMessage = this.commonService.getError(err);
        this.priceLoading = false;
      } );
  }

  changeTypeaheadLoading(e: boolean) {
    this.autoCompleteloading = e;
  }

  getStockLevel(stock: number) {
    if (stock > 10) {
      return StockLevelEnum.InStock;
    } else if (stock > 0 && stock < 10) {
      return StockLevelEnum.Expiring;
    } else {
      return StockLevelEnum.OutOfStock;
    }
  }

  customerChanged(c: Customer) {
    if (c != null) {
      this.customer_code = c.code;
    } else {
      this.customer_code = '';
    }
  }

}

export enum StockLevelEnum {
  OutOfStock = 1,
  Expiring = 2,
  InStock = 3
}
