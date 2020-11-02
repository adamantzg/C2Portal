import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe, DecimalPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GridComponent } from './components';
import { CustomcolumnComponent } from './components/grid/customcolumn/customcolumn.component';
import { ColumnFormatPipe } from './components/grid/formatPipe';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { TagInputModule } from 'ngx-chips';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MultipleCheckboxComponent } from './components/multipleCheckbox/multipleCheckbox';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    TypeaheadModule,
    TagInputModule
  ],
  declarations: [
    GridComponent, CustomcolumnComponent, ColumnFormatPipe, MultipleCheckboxComponent
  ],
  exports: [
    GridComponent
  ],
  providers: [

  ]
})
export class CommonAppModule { }
