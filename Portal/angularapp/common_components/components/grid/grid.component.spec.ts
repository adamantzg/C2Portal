import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GridComponent, GridDefinition, GridColumn, GridEditMode, GridColumnType, GridButton, GridButtonEventData } from './grid.component';
import { FormsModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { DatePipe, DecimalPipe, CurrencyPipe } from '@angular/common';
import { ColumnFormatPipe } from './formatPipe';
import { CustomcolumnComponent } from './customcolumn/customcolumn.component';
import { TypeaheadModule } from 'ngx-bootstrap';
import * as $ from 'jquery';
import { CommonAppModule } from '../../common.module';
import { TagInputModule } from 'ngx-chips';

describe('GridComponent', () => {
  let component: GridComponent;
  let fixture: ComponentFixture<GridComponent>;

  setupTestBed({
      providers: [DatePipe, DecimalPipe, CurrencyPipe],
      declarations: [],
      imports: [FormsModule, ReactiveFormsModule, TypeaheadModule, CommonAppModule, TagInputModule]
    });

  beforeEach(() => {
    fixture = TestBed.createComponent(GridComponent);
    component = fixture.componentInstance;

  });

  test('should create', () => {
    expect(component).toBeTruthy();
    expect(fixture).toMatchSnapshot();
  });

  test('hidden editors', () => {
    const def = new GridDefinition([]);
    let c = new GridColumn('a', 'a', GridColumnType.Label, 'a');
    c.editable = true;
    def.columns.push(c);
    c = new GridColumn('b', 'b', GridColumnType.Label, 'b');
    c.editable = true;
    def.columns.push(c);
    const data = [{id: 1, a: 1, b: 2}, {id: 2, a: 3, b: 4}];
    component.data = data;
    component.editMode = GridEditMode.Edit;

    component.definition = def;

    component.editedRow = JSON.parse(JSON.stringify(data[0]));
    component.definition.columnEditorVisibilityStatusCallback = (row, col) =>  row.id === 1 && col.name === 'a' ? false : true;

    fixture.detectChanges();

    expect($('input').length).toBe(1);
    expect($('input[name=a]').length).toBe(0);

    component.editedRow = JSON.parse(JSON.stringify(data[1]));
    fixture.detectChanges();
    expect($('input').length).toBe(2);

  });

  test('buttonClicked', () => {
    component.form = new FormGroup({});
    const b = new GridButton('a', 'a');
    const c = new GridColumn('c', 'c', null, 'c');
    let event: GridButtonEventData;
    component.ButtonClicked.subscribe((e) => event = e );
    component.buttonClicked({}, c, b );
    expect(event.column).toBe(c.name);
    expect(event.name).toBe(b.name);
    expect(event.row).toBeTruthy();

    b.requiresValidation = true;
    jest.spyOn(component.form, 'valid', 'get').mockImplementationOnce(() => true);
    event.name = '';
    component.buttonClicked({}, c, b );
    expect(event.name).toBe(b.name);

    jest.spyOn(component.form, 'valid', 'get').mockImplementationOnce(() => false);
    jest.spyOn(component.form, 'invalid', 'get').mockImplementationOnce(() => true);
    component.buttonClicked({}, c, b );
    expect(component.editValidation).toBeTruthy();

  });

  test('calculateScrollableHeight', () => {
    let sh = null;
    expect(component.calculateScrollableHeight(sh)).toEqual('');

    sh = '100px';
    expect(component.calculateScrollableHeight(sh)).toEqual('60px');

    sh = 'a';
    expect(component.calculateScrollableHeight(sh)).toEqual('');

    sh = '20px';
    expect(component.calculateScrollableHeight(sh)).toEqual('-20px');
  });

  test('getValue', () => {

  });
});
