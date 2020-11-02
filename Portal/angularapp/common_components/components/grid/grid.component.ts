import { Component, OnInit, Input, Output, EventEmitter, ComponentFactoryResolver, ViewContainerRef, Type } from '@angular/core';
import { DatePipe, DecimalPipe, CurrencyPipe, PercentPipe } from '@angular/common';
import { FormGroup, FormControl } from '@angular/forms';
import { TypeaheadMatch } from 'ngx-bootstrap';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css'],
  providers: [DatePipe, DecimalPipe, CurrencyPipe, PercentPipe]
})
export class GridComponent implements OnInit {

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
    private viewContainerRef: ViewContainerRef) { }

  private _definition: GridDefinition;

  @Input()
  data: any[];
  @Input()
  get definition(): GridDefinition {
    return this._definition;
  }
  set definition(value: GridDefinition) {
    this._definition = value;
    this.onDefinitionSet(value);
  }
  @Output()
  ButtonClicked = new EventEmitter();
  columnTypes = Object.assign({}, GridColumnType);
  dataTypes = Object.assign({}, ColumnDataType);
  fixedHeaderStyle: any;
  @Input()
  style = {};
  @Input()
  filter: string;
  @Output()
  SortChange = new EventEmitter();
  @Input()
  css = 'table';
  @Input()
  keyField = 'id';
  @Input()
  editMode = GridEditMode.NoEdit;
  @Input()
  editedRow: any;
  editModes = Object.assign({}, GridEditMode);
  editorTypes = Object.assign({}, GridEditorType);

  editValidation = false;

  form: FormGroup;

  ngOnInit() {
    this.fixedHeaderStyle = { height: this.calculateScrollableHeight(this.style['height']), 'overflow-y': 'auto'};
  }

  buttonClicked(d: any, c: GridColumn, button?: GridButton) {
    if (button != null) {
      if (!button.requiresValidation || this.form.valid) {
        this.ButtonClicked.emit(new GridButtonEventData(c.name, d, button.name));
      }
      if (button.requiresValidation && this.form.invalid) {
        this.editValidation = true;
      }
    } else {
      this.ButtonClicked.emit(new GridButtonEventData(c.name, d));
    }

  }

  calculateScrollableHeight(styleHeight: string) {
    if ( styleHeight != null) {
      const num = +styleHeight.replace('px', '');
      if (num > 0) {
        return (num - 40).toString() + 'px';
      }
    }
    return '';
  }

  getValue(row: any, c: GridColumn) {

    if (c.fieldValueCallback == null) {
      let obj = row;
      const parts = c.field.split('.');
      for (let i = 0; i < parts.length - 1; i++) {
        obj = obj[parts[i]];
      }
      return obj[parts[parts.length - 1]];
    }
    return c.fieldValueCallback(row);
  }

  checkSort(c: GridColumn) {
    return this.definition.sort != null && this.definition.sort.column.field === c.field;
  }

  toggleSort(c: GridColumn) {
    if (this.definition.sort == null) {
      this.definition.sort = {column : c, direction: SortDirection.Asc};
    } else {
      let direction = SortDirection.Asc;
      if (this.definition.sort.column.field === c.field && this.definition.sort.direction === SortDirection.Asc) {
        direction = SortDirection.Desc;
      }
      this.definition.sort.column = c;
      this.definition.sort.direction = direction;

    }
    this.SortChange.emit(this.definition.sort);
  }

  getSortIconClass(c: GridColumn) {
    return `glyphicon glyphicon-arrow-${this.definition.sort.direction === SortDirection.Asc ? 'up' : 'down' } sort-icon`;
  }

  checkColumnVisibility(c: GridColumn) {
    if (this.definition.columnVisibilityCallback == null) {
      return true;
    }
    return this.definition.columnVisibilityCallback(c);
  }

  checkColumnButtonVisibility(row: any, c: GridColumn, button: string) {
    if (this.definition.columnButtonVisibilityCallback == null) {
      return true;
    }
    return this.definition.columnButtonVisibilityCallback(row, c.name, button);
  }

  checkColumnGroupDisabledStatus(row: any, c: GridColumn, button: string) {
    if (this.definition.columnButtonDisabledStatusCallback == null) {
      return false;
    }
    return this.definition.columnButtonDisabledStatusCallback(row, c.name, button);
  }

  checkColumnEditorVisibilityStatus(row: any, c: GridColumn) {
    if (this.definition.columnEditorVisibilityStatusCallback == null) {
      return true;
    }
    return this.definition.columnEditorVisibilityStatusCallback(row, c);
  }

  toggleValue(c: GridColumn) {
    this.data.forEach(d => d[c.field] = c.selected);
  }

  checkMode(c: GridColumn) {
    return c.editModes.find(m => m === this.editMode) != null;
  }

  onDefinitionSet(def: GridDefinition) {
    const controls = {};
    def.columns.forEach(c => {
      if (c.editable) {
        controls[c.name] = new FormControl(c.name);
      }
    });
    this.form = new FormGroup(controls);
  }

  onAutoCompleteSelected(c: GridColumn, $event: TypeaheadMatch ) {
    if (c.autoCompleteData.onSelectedCallback != null) {
      c.autoCompleteData.onSelectedCallback($event.item);
    }
  }

  onAutoCompleteLoading(c: GridColumn, $event: boolean) {
    if (c.autoCompleteData.loadingCallback != null) {
      c.autoCompleteData.loadingCallback($event);
    }
  }

}

export class GridDefinition {
  columns: GridColumn[];
  fixedHeaders = false;
  multiSelect = false;
  scrollHeight: string;
  sort: Sort;
  columnVisibilityCallback: (c: GridColumn) => boolean;
  columnButtonVisibilityCallback: (row: any, name: string, button: string) => boolean;
  columnButtonDisabledStatusCallback: (row: any, name: string, button: string) => boolean;
  columnEditorVisibilityStatusCallback: (row: any, c: GridColumn) => boolean;

  constructor(columns: GridColumn[], fixedHeaders?: boolean, multiSelect?: boolean, scrollHeight?: string) {
    this.columns = columns;
    if (fixedHeaders != null) {
      this.fixedHeaders = fixedHeaders;
    }
    if (multiSelect != null) {
      this.multiSelect = multiSelect;
    }
    if (scrollHeight != null) {
      this.scrollHeight = scrollHeight;
    }
  }

  getColumn(name: string) {
    return this.columns.find(c => c.name === name);
  }
}

export class GridColumn {
  title: string;
  field: string;
  name: string;
  style: any;
  buttonCss: string;
  customComponentType: Type<any>;
  type: GridColumnType;
  data: any;
  sortable: boolean;
  dataType: ColumnDataType;
  format: any;
  selected: boolean; // used for checbox all selection
  buttonIcon: string;
  editorType: GridEditorType;
  editable = true;
  buttons: GridButton[] = [];
  editModes: GridEditMode[] = [GridEditMode.NoEdit];
  required: boolean;
  valueField: string; // dropdown data id
  displayField: string; // dropdown data title
  selectedValueField: string; // field in data to be updated by valueField
  dropdownData: any[];
  autoCompleteData: ColumnAutoCompleteData;
  multipleSelectionData: MultipleSelectionData;
  fieldValueCallback: any;

  constructor(title: string, field: string, type?: GridColumnType, name?: string, style?: any,
    buttonCss?: string, customComponentType?: Type<any>, data?: any, sortable?: boolean,
    dataType?: ColumnDataType, format?: any, buttonIcon?: string, editable?: boolean,
    editorType?: GridEditorType, buttons?: GridButton[], editModes?: GridEditMode[],
    required?: boolean, autoCompleteData?: ColumnAutoCompleteData, multipleSelectionData?: MultipleSelectionData,
     fieldValueCallback?: any) {
    this.title = title;
    this.field = field;
    this.name = name;
    if (type != null) {
      this.type = type;
    } else {
      this.type = GridColumnType.Label;
    }
    this.style = style;
    this.buttonCss = buttonCss;
    if (name == null) {
      this.name = this.title;
    }
    this.customComponentType = customComponentType;
    this.data = data;
    this.sortable = sortable;
    this.dataType = dataType;
    if (this.dataType == null) {
      this.dataType = ColumnDataType.Text;
    }
    this.format = format;
    this.buttonIcon = buttonIcon;
    this.editorType = editorType;
    if (editable != null) {
      this.editable = editable;
    }

    if (this.editorType == null) {
      this.editorType = GridEditorType.Textbox;
    }
    this.buttons = buttons;
    if (editModes != null) {
      this.editModes = editModes;
    }
    this.required = required;
    this.autoCompleteData = autoCompleteData;
    this.multipleSelectionData = multipleSelectionData;
    this.fieldValueCallback = fieldValueCallback;
  }

}

export class GridButton {
  css: string;
  icon: string;
  text: string;
  name: string;
  requiresValidation = false;

  constructor(text: string, name: string, icon?: string, css?: string, requiresValidation?: boolean) {
    this.text = text;
    this.name = name;
    this.css = css;
    this.icon = icon;
    if (requiresValidation) {
      this.requiresValidation = requiresValidation;
    }
  }
}

export class ButtonGroupColumn extends GridColumn {
  buttons: GridButton[] = [];
  editModes: GridEditMode[] = [GridEditMode.NoEdit];

  constructor(title: string, buttons: GridButton[], editModes?: GridEditMode[]) {
    super(title, '', GridColumnType.ButtonGroup);
    this.buttons = buttons;
    if (editModes != null) {
      this.editModes = editModes;
    }
  }
}

export class GridButtonEventData {
  column: string;
  row: any;
  name: string;

  constructor(column: string, row: any, name?: string) {
    this.column = column;
    this.row = row;
    this.name = name;
  }
}

export class CustomColumnEventData {
  name: string;
  row: any;
}

export enum GridColumnType {
  Label = 0,
  Checkbox = 1,
  Button = 2,
  Custom = 3,
  Checkmark = 4,
  ButtonGroup = 5
}

export enum ColumnDataType {
  Text,
  Numeric,
  Date,
  Currency,
  Percent
}

export enum SortDirection {
  Asc = 0,
  Desc = 1
}

export class Sort {
  column: GridColumn;
  direction: SortDirection;

  constructor(column: GridColumn, direction: SortDirection) {
    this.column = column;
    this.direction = direction;
  }
}

export enum GridEditMode {
  NoEdit,
  AddNew,
  Edit
}

export enum GridEditorType {
  Textbox,
  Checkbox,
  Dropdown,
  Autocomplete,
  MultipleChoiceTagInput,
  MultipleChoiceCheckboxes
}

export class ColumnAutoCompleteData {
  data: any[];
  onSelectedCallback: (obj: any) => any;
  optionsLimit = 20;
  loadingCallback: (isLoading: boolean) => any;
  minLength = 3;
  optionField: string;

  constructor(data: any[], optionsLimit: number, minLength: number, optionField: string, onSelectedCallback: any, loadingCallback: any) {
    this.data = data;
    this.optionsLimit = optionsLimit;
    this.minLength = minLength;
    this.optionField = optionField;
    this.onSelectedCallback = onSelectedCallback;
    this.loadingCallback = loadingCallback;
  }
}

export class MultipleSelectionData {
  autocompleteItems: any[];
  autocompleteOnly = false;
  idField = 'id';
  displayField = 'name';
  placeHolder = 'Choose...';
  selectedField = 'selected';

  constructor(autocompleteOnly?: boolean, autocompleteItems?: any[], idField: string = 'id',
    displayField: string = 'name', selectedField: string = 'selected', placeHolder?: string ) {
    this.autocompleteItems = autocompleteItems;
    this.autocompleteOnly = autocompleteOnly;
    this.idField = idField;
    this.displayField = displayField;
    this.selectedField = selectedField;
    this.placeHolder = placeHolder;
  }
}




