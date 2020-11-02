import { Component, OnInit, Input, Output, EventEmitter} from '@angular/core';
import { OrderFilter } from './orderfilter';
import { BsDatepickerConfig } from 'ngx-bootstrap';


@Component({
  selector: 'app-orderfilter',
  templateUrl: './orderfilter.component.html',
  styleUrls: ['./orderfilter.component.css']
})
export class OrderfilterComponent implements OnInit {

  constructor() { }

  @Input()
  filter = new OrderFilter();
  bsConfig: BsDatepickerConfig = new BsDatepickerConfig();
  @Input()
  searchLabel = 'Containing text:';

  @Output()
  filterApplied = new EventEmitter();
  @Output()
  filterCleared = new EventEmitter();

  ngOnInit() {
    this.bsConfig.dateInputFormat = 'DD/MM/YYYY';
  }

  applyFilter() {
    this.filterApplied.emit();
  }

  clearFilter() {
    this.filter.searchText = '';
    this.filterCleared.emit();
  }

}
