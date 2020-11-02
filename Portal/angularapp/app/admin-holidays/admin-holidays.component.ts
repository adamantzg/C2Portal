import { Component, OnInit, Input } from '@angular/core';
import { FormsModule, Validators } from '@angular/forms';
// import {ReactiveFormsModule} from '@angular/forms';
// import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { Holiday, HolidayExtracted } from '../domainclasses';
import { HolidayService } from '../services/holiday.service';
// import { GroupByPipe } from '../pipe/group-by.pipe';
import { getFullYear } from 'ngx-bootstrap/chronos/utils/date-getters';
// import { from } from 'rxjs/observable/from';
// import { groupBy, mergeMap, toArray } from 'rxjs/operators';
import { NotificationsService } from 'angular2-notifications';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import * as moment from 'moment';
import { HeaderService } from '../../c2-flex/services/header.service';
@Component({
  selector: 'app-admin-holidays',
  templateUrl: './admin-holidays.component.html',
  styleUrls: ['./admin-holidays.component.css']
})
export class AdminHolidaysComponent implements OnInit {
  selectedRow: Holiday;
  bsConfig: BsDatepickerConfig = new BsDatepickerConfig();
  monthsarr: any;
  title = 'app';
  // moment = require('moment');
  date: Date;
  months: any;
  year: number; // = this.moment('YYYY');
  minDate: Date;
  maxDate: Date;
  yearCounter = 0;

  /** */
  holidays: Holiday[] = [];
  holiday = new Holiday();
  holidayName: string;
  groupedHolidays = [];
  extractedHolidays = [];
  /** */
  constructor(
    private holidayService: HolidayService,
    // private groupBy: GroupByPipe,
    private toastService: NotificationsService,
    private headerService: HeaderService,
  ) {

    this.setCalendar(0);
    headerService.title = 'Holiday administration';
  }

  toastOptions = {
    timeOut: 4000,
    showProgressBar: false,
    pauseOnHover: true,
    clickToClose: true,
  };

  ngOnInit() {
    const date = new Date();
    this.bsConfig.dateInputFormat = 'DD/MM/YYYY';
  }

  getHolidays(year: number) {
    this.holidayService.getHolidays(year).subscribe(
      (h) => {
        this.holidays = h;
        this.groupedHolidays = this.sortMonths(this.groupHolidays(h));
      }
    );
  }
  groupHolidays(h: Holiday[]) {
    for (let i = 0; i < this.holidays.length; i++) {
      this.extractedHolidays.push(
        {
          name: this.holidays[i].name,
          year: moment(this.holidays[i].date).format('YYYY'),
          month: moment(this.holidays[i].date).format('MMMM'),
          date: moment(this.holidays[i].date).format('DD'),
          fullDate: this.holidays[i].date
        }
      );
    }
    // return this.groupBy.transform(this.extractedHolidays, 'month');
    return this.groupBy(this.extractedHolidays, 'month');
  }
  groupBy(collection: Array<any>, property: string): Array<any> {
    if (!collection) {
      return null;
    }

    const groupedCollection = collection.reduce((previous, current) => {
      if (!previous[current[property]]) {
        previous[current[property]] = [current];
      } else {
        previous[current[property]].push(current);
      }

      return previous;
    }, {});

    // this will return an array of objects, each object containing a group of objects
    return Object.keys(groupedCollection).map(key => ({ key, value: groupedCollection[key] }));
  }
  clearHolidaysParams() {
    this.holidays = [];
    this.groupedHolidays = [];
    this.extractedHolidays = [];
  }
  setYear(num: number) {
    this.clearHolidaysParams();
    this.setCalendar(num);
  }
  setCalendar(num: number) {
    this.yearCounter += num;
    const minDate = new Date();
    const maxDate = new Date();
    this.year = (minDate.getFullYear() + this.yearCounter);
    minDate.setFullYear(this.year); /** +1 or -1 year */

    if (this.groupedHolidays.length === 0) {
      this.getHolidays(minDate.getFullYear());
    }

    this.minDate = new Date(moment({ year: this.year }).startOf('year').format('DD/MM/YYYY'));
    this.maxDate = new Date(moment({ year: this.year }).endOf('year').format('DD/MM/YYYY'));
  }

  countDaysInMonth(year, month) {

    return moment(`${year}-${month}`, 'YYYY-MM').daysInMonth();
  }
  getDays(year, i) {
    const num = this.countDaysInMonth(year, i);
    const days = new Array(num);
    return days;
  }

  saveNewHoliday() {
    this.holiday.date = moment.utc([this.holiday.date.getFullYear(),
      this.holiday.date.getMonth(), this.holiday.date.getDate()]).toDate();
    this.holidayService.saveHoliday(this.holiday ).subscribe(
      (c) => {
        this.toastService.success(`Holiday added`, `${moment(this.holiday.date).format('DD/MM/YYYY')} ${this.holiday.name} `);
        // console.log('Praznik je spremljen u bazu');
        this.extractedHolidays.push(
          {
            name: this.holiday.name,
            year: moment(this.holiday.date).format('YYYY'),
            month: moment(this.holiday.date).format('MMMM'),
            day: moment(this.holiday.date).format('DD'),
            fullDate: this.holiday.date
          });
        // this.groupedHolidays = this.sortMonths(this.groupBy.transform(this.extractedHolidays, 'month'));
        // console.log("EXTRACTED HOLIDAYS: ", this.extractedHolidays),
        this.groupedHolidays = this.sortMonths( this.groupBy(this.extractedHolidays, 'month'));
        // for (let index = 0; index < this.groupedHolidays.length; index++) {
        //   // const element = this.groupedHolidays[index];
        //   this.groupedHolidays[index] = this.sortInMonth(this.groupedHolidays[index]);
        // }
        this.holiday = new Holiday();
      }
    );
    // console.log(`save ne holiday`);
    // this.holidays.push(initHoliday(this.holiday.name, this.holiday.date ));
  }
  deleteHoliday(holidayExt: HolidayExtracted, $groupIndex: number, $index: number) {
    const holiday = { name: holidayExt.name, date: holidayExt.fullDate };
    this.selectedRow = holiday;
    this.holidayService.deleteHoliday(holiday).subscribe(
      (h) => {
        this.groupedHolidays[$groupIndex].value.splice($index, 1);
        this.toastService.error('Holiday deleted', `${moment(holiday.date).format('MMMM-DD-YYYY')} ${holiday.name} `);
        location.reload();
      });
  }
  sortGroup(group: HolidayExtracted[]): HolidayExtracted[] {
    const ordered = group.sort(function (a: HolidayExtracted, b: HolidayExtracted) {
      return a.date - b.date;
    });
    return ordered;
  }
  sortInMonth(days: HolidayExtracted[]): HolidayExtracted[] {
    // console.log(days);
    return days.sort((da, db) => da.date - db.date);
  }
  sortMonths(months: any[]): any[] {
    // console.log('sortiraj mjesece', months);
    return months.sort((am, bm) => Number(moment().month(am.key).format('M')) - Number(moment().month(bm.key).format('M')));
  }
  sortMonthsDesc(months: any[]): any[] {
    // console.log('sortiraj mjesece', months);
    return months.sort((am, bm) => Number(moment().month(bm.key).format('M')) - Number(moment().month(am.key).format('M')));
  }

  onDatePickerValueChange(value: any) {

  }



}
// Array.prototype.groupBy = function (prop) {
//   return this.reduce(function(groups, item){
//     const val = item[prop];
//     groups[val] = groups[val]  || [];
//     groups[val].push(item);
//     return groups;
//   }, {} );
// };
