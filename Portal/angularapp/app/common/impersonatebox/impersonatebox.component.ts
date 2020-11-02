import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UserService } from '../../services/user.service';
import { Customer, RoleId } from '../../domainclasses';
import { Observable, Subject, Subscriber } from 'rxjs';

@Component({
  selector: 'app-impersonatebox',
  templateUrl: './impersonatebox.component.html',
  styleUrls: ['./impersonatebox.component.css']
})
export class ImpersonateboxComponent implements OnInit {

  constructor(private userService: UserService) { }

  customer_code: string;
  customer: Customer;

  /*@Input()
  public get Code() {
    return this.customer_code;
  }
  public set Code(value: string) {
    this.customer_code = value;
  }

  @Input()
  public get Customer() {
    return this.customerObj;
  }
  public set Customer(value: any)  {
    this.customerObj = value;
  }*/

  @Input()
  showLabel = false;

  @Input()
  showButton = true;

  /*@Output()
  buttonClicked = new EventEmitter();*/
  /*@Output()
  CodeChange = new EventEmitter<string>();*/
  @Output()
  CustomerChange = new EventEmitter<Customer>();
  showWarning = false;

  customers: Customer[] = [];
  autoCompleteLoading = false;

  ngOnInit() {
    const storage = localStorage.getItem('impersonated_customer');
    if (storage != null) {
      this.customer = JSON.parse(storage);
      this.CustomerChange.emit(this.customer);
      // this.CodeChange.emit(this.customerObj.code);
      this.customer_code = this.customer.code;
    } else {
      this.CustomerChange.emit(null);
    }
    this.customers = Observable.create((observer: any) => {
      // Runs on every search
      observer.next(this.customer_code);
    }).mergeMap((code: string) => this.userService.customerSearch(code, RoleId.User));

  }

  clicked() {
    // extract code
    const code = this.customer_code.split(' ')[0];
    this.userService.getCustomer(code).subscribe(c => {
      this.customer = c;
      this.CustomerChange.emit(this.customer);
      // this.CodeChange.emit(this.customer_code);
      if (c != null) {
        localStorage.setItem('impersonated_customer', JSON.stringify(c));
      } else {
        this.customer_code = '';
        this.showWarning = true;
        setTimeout(() => {
          this.showWarning = false;
        }, 1000 );
        localStorage.removeItem('impersonated_customer');
      }
    });
    // this.buttonClicked.emit();
  }

  clear() {
    this.customer_code = '';
    this.customer = null;
    localStorage.removeItem('impersonated_customer');
    this.CustomerChange.emit(null);
  }

  changeTypeaheadLoading(e: boolean) {
    this.autoCompleteLoading = e;
  }


}
