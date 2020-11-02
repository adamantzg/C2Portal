import { Component, OnInit } from '@angular/core';
import { Order } from '../../domainclasses';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-orderdetailmodal',
  templateUrl: './orderdetailmodal.component.html',
  styleUrls: ['./orderdetailmodal.component.css']
})
export class OrderdetailmodalComponent implements OnInit {

  constructor(private bsModalRef: BsModalRef) { }

  order: Order = new Order();
  total = 0;
  errorMessage = '';
  numberFormat = '1.2-2';

  ngOnInit() {
  }

  close() {
    this.bsModalRef.hide();
  }

}
