import { Component, OnInit } from '@angular/core';
import { HeaderService } from '../../c2-flex/services/header.service';
import { UserService } from '../services/user.service';
import { User, Role, RoleId, Customer } from '../domainclasses';
import { NgForm, NgControl } from '@angular/forms';
import { NotificationsService } from 'angular2-notifications';
import { UsersModel } from '../modelclasses';
import { CommonService } from '../../c2-flex/services/common.service';
import { Observable } from 'rxjs/Observable';
import { TypeaheadMatch } from 'ngx-bootstrap';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { MessageBoxType, MessageBoxCommand, MessageBoxCommandValue } from '../common/ModalDialog';
import * as moment from 'moment';
import { GridDefinition, GridColumn, GridColumnType, ColumnDataType,
  ButtonGroupColumn, GridButton, GridButtonEventData, GridEditMode,
  GridEditorType, Sort, SortDirection, ColumnAutoCompleteData } from '../../common_components/components';

@Component({
  selector: 'admin-user',
  templateUrl: './admin-user.component.html',
  styleUrls: ['./admin-user.component.css']
})

export class AdminUserComponent implements OnInit {
  editedUserId = -1;
  userCopy: User = new User();

  model: UsersModel = new UsersModel();
  errorMessage = '';
  successMessage = '';
  customers: Customer[];
  currentUser = this.userService.CurrentUser;
  validation = false;

  placeHolders: any = {
    username: {ok: 'Enter Username', error: 'UserName is required'},
    firstname: {ok: 'Enter First Name', error: 'First Name is required'},
    lastname: {ok: 'Enter Last Name', error: 'Last Name is required'},
    email: {ok: 'Enter e-mail', error: 'E-mail is required'},
    customer: {ok: 'Enter customer code', error: 'Customer code is required'},
  };

  customerLoading = false;

  customerAutoComplete: ColumnAutoCompleteData;

  gridDefinition = new GridDefinition([
    new GridColumn('Username', 'username', GridColumnType.Label, 'username', null, null, null, null, true),
    new GridColumn('Email', 'email', GridColumnType.Label, 'email', null, null, null, null, true),
    new GridColumn('First Name', 'name', GridColumnType.Label, 'name', null, null, null, null, true),
    new GridColumn('Last Name', 'lastname', GridColumnType.Label, 'lastname', null, null, null, null, true),
    new GridColumn('Role Type', 'role', GridColumnType.Label, 'role', null, null, null, null, true),
    new GridColumn('Account Code', 'customer_code', GridColumnType.Label, 'customer', null, null, null, null, true, null,
    null, null, true, GridEditorType.Autocomplete, null, null, true),
    new GridColumn('Internal?', 'isInternal', GridColumnType.Checkmark, 'internal', null, null, null, null, true, null, null,
      null, null, GridEditorType.Checkbox),
    new GridColumn('Last login', 'lastLogin', GridColumnType.Label, 'lastLogin', null, null, null, null, true,
    ColumnDataType.Date, {format: 'dd/MM/yyyy HH:mm'}, null, false),
    new GridColumn('buttons', null, GridColumnType.ButtonGroup, 'buttons', null, null, null, null, false, null, null, null, false,
      null,
      [
        new GridButton('Update', 'update', 'fa fa-check', 'btn-xs', true),
        new GridButton('Cancel', 'cancel', 'fa-times-circle-o', 'btn-xs'),
        new GridButton('Edit', 'edit', 'fa-pencil-square-o', 'btn-xs'),
        new GridButton('Delete', 'delete', 'fa-remove', 'btn-danger btn-xs'),
        new GridButton('Activate', 'activate', 'fa-check', 'btn-info btn-xs'),
        new GridButton('Reset password', 'reset', 'fa-check', 'btn-warning btn-xs')
      ])
  ]);

  editMode = GridEditMode.NoEdit;
  editModes = Object.assign({}, GridEditMode);

  constructor(
    private userService: UserService,
    private toastService: NotificationsService,
    private commonService: CommonService,
    private messageBoxService: MessageboxService
  ) {
    this.customers = Observable.create((observer: any) => {
      // Runs on every search
      observer.next(this.userCopy.customer_code);
    }).mergeMap((code: string) => this.userService.customerSearch(code, this.userCopy.roles[0].id));

    this.gridDefinition.getColumn('customer').autoCompleteData = new ColumnAutoCompleteData(this.customers, 20, 3, 'code',
    this.onCustomerSelected.bind(this), this.changeTypeaheadLoading.bind(this));

    this.gridDefinition.sort = {column: this.gridDefinition.columns[3], direction: SortDirection.Asc};
    this.gridDefinition.columnVisibilityCallback = this.checkColumnVisibility.bind(this);
    this.gridDefinition.columnButtonVisibilityCallback = this.checkColumnButtonVisibility.bind(this);
    this.gridDefinition.columnButtonDisabledStatusCallback = this.checkColumnButtonDisabledStatus.bind(this);
    this.gridDefinition.columnEditorVisibilityStatusCallback = this.checkColumnEditorVisibilityStatus.bind(this);
    const requiredFields = ['username', 'email', 'name', 'lastname', 'customer'];
    this.gridDefinition.columns.forEach(c => {
      if (requiredFields.indexOf(c.name) >= 0) {
        c.required = true;
      }

    });



  }


  toastOptions= {
    timeOut: 3000,
    showProgressBar: false,
    pauseOnHover: true,
    clickToClose: true,
  };


  ngOnInit() {

    this.userService.getUsersModel().subscribe (
      (m: UsersModel) => {
        this.model = m;
        m.users.forEach(u => {
          if (u.roles.length > 0) {
            u.role = u.roles[0].name;
            u.role_id = u.roles[0].id;
          }
        });
        const roleColumn = this.gridDefinition.getColumn('role');
        if (roleColumn != null && (this.currentUser.isTopAdmin || this.currentUser.isBranchAdmin)) {
          roleColumn.editorType = GridEditorType.Dropdown;
          roleColumn.dropdownData = m.roles;
          roleColumn.valueField = 'id';
          roleColumn.displayField = 'name';
          roleColumn.selectedValueField = 'role_id';
        }
        this.onSortChanged(this.gridDefinition.sort);

        // this.role = this.model.roles[0];
      },
      (err) => this.errorMessage = this.commonService.getError(err)
    );
  }

  trackByIndex(index, value) {
    return index;
  }
  addNew() {
    this.userCopy = JSON.parse(JSON.stringify(new User()));
    this.userCopy.roles = [this.model.roles.find(r => r.id === RoleId.User)];
    this.userCopy.role = this.userCopy.roles[0].name;
    this.userCopy.role_id = this.userCopy.roles[0].id;
    this.userCopy.id = 0;
    this.editMode = GridEditMode.AddNew;
    this.editedUserId = 0;
  }
  /* addToUser($event) {
    this.userCopy.roles = [];
    this.userCopy.roles.unshift({ id: this.role.id, name: this.role.name });
  } */

  getPlaceholder(name: string, control: NgControl) {
    if (control.invalid && this.validation) {
      return this.placeHolders[name]['error'];
    }
    return this.placeHolders[name]['ok'];
  }

  onSubmit(userForm: NgForm) {

    // console.log(`DALI JE FORMA VALIDNA: ${userForm.valid}, ${userForm.value}, ${userForm.valueChanges} ` );


  }
  onEditRow(user: User) {
    this.userCopy = JSON.parse(JSON.stringify(user));
    const role = this.model.roles.find(r => r.id === user.roles[0].id);
    if (role != null) {
      this.userCopy.roles = [];
      this.userCopy.roles.push(role);
      this.userCopy.role = this.userCopy.roles[0].name;
    }

    // this.role = this.userCopy.roles[0];
    this.editedUserId = user.id;
  }

  onCancelRow() {
    this.editedUserId = -1;
  }
  onUpdateRow() {
    /** send user to services */

    const user = this.userCopy;
    user.customer = null;
    const isNew = user.id === 0;
    this.errorMessage = '';

    // tslint:disable-next-line:triple-equals
    const role = this.model.roles.find(r => r.id == user.role_id);
    if (role != null) {
      user.roles = [role];
    }


    this.userService.createUpdateUser(user).subscribe( (u: User) => {
      this.errorMessage = '';
      u.role = u.roles[0].name;
      u.role_id = u.roles[0].id;
      if (isNew) {
        this.model.users.unshift(u);
      } else {
        const existingUser = this.model.users.find(us => us.id === user.id);
        if (existingUser != null) {
          Object.assign(existingUser, u);
        }

      }
      this.editMode = GridEditMode.NoEdit;
      this.toastService.success(isNew ? 'User added' : 'User updated', `User: ${u.username} `);
    },
    err => this.errorMessage = this.commonService.getError(err)
  );

  }
  onDeleteRow(user: User) {
    this.messageBoxService.openDialog('Delete user?', MessageBoxType.Yesno).subscribe((m: MessageBoxCommand) => {
      if (m.value === MessageBoxCommandValue.Ok) {
        this.errorMessage = '';
        this.userService.deleteUser(user.id).subscribe( data => {
          const index = this.model.users.findIndex(u => u.id === user.id);
          if (index >= 0) {
            this.model.users.splice(index, 1);
          }

          this.toastService.error('Deleted', `User ${user.username} is deleted!`);
        },
        err => this.errorMessage = this.commonService.getError(err));

      }
    });

  }

  changeTypeaheadLoading(data) {
    this.customerLoading = data;
  }

  onCustomerSelected(c: Customer) {
    this.userCopy.customer_code = c.code;
  }

  onActivate(user: User) {
    this.errorMessage = '';
    this.userService.activateUser(user.id).subscribe( data => this.successMessage = `Activation email sent to ${user.email}` ,
    err => this.errorMessage = this.commonService.getError(err));
  }

  resetPass(user: User) {
    this.errorMessage = '';
    this.userService.resetPassword(user.id).subscribe( data => this.successMessage = `Reset password email sent to ${user.email}` ,
    err => this.errorMessage = this.commonService.getError(err));
  }

  /*showButton(user: User, name: string) {
    if (name === 'delete') {
      return this.currentUser.isTopAdmin || (this.currentUser.isBranchAdmin && user.roles[0].id === RoleId.User) ;
    }
  }*/

  checkColumnVisibility(c: GridColumn) {

    if (c.name === 'internal') {
      return this.currentUser.isTopAdmin;
    } else if (c.name === 'role') {
      return this.currentUser.isTopAdmin || this.currentUser.isBranchAdmin;
    }
    return true;
  }

  checkColumnButtonVisibility(u: User, column: string, button: string) {

    if (button === 'activate') {
      return (this.editMode === GridEditMode.NoEdit || u.id !== this.userCopy.id) && !u.activated;
    } else if (button === 'reset') {
      return (this.editMode === GridEditMode.NoEdit || u.id !== this.userCopy.id) && u.activated;
    } else if (button === 'edit') {
      return (this.editMode === GridEditMode.NoEdit || u.id !== this.userCopy.id) && (
        this.currentUser.isTopAdmin || (this.currentUser.isBranchAdmin &&
          (u.roles[0].id !== RoleId.BranchAdmin || u.id === this.currentUser.id) )
      ) ;
    } else if (button === 'delete') {
      return (this.editMode === GridEditMode.NoEdit || u.id !== this.userCopy.id) && (
      this.currentUser.isTopAdmin || (this.currentUser.isBranchAdmin && u.roles[0].id !== RoleId.BranchAdmin)) ;
    } else if (button === 'update' || button === 'cancel') {
      return this.editMode !== GridEditMode.NoEdit && u.id === this.userCopy.id;
    }
    return true;
  }

  checkColumnButtonDisabledStatus(u: User, column: GridColumn, button: string) {
    if (button === 'activate' || button === 'reset' ||  button === 'delete' || button === 'edit') {
      return this.editMode !== GridEditMode.NoEdit;
    }
    return false;
  }

  checkColumnEditorVisibilityStatus(u: User, column: GridColumn) {
    if ( this.currentUser.isBranchAdmin && !this.currentUser.isTopAdmin && u.isBranchAdmin &&
      (column.name === 'role' || column.name === 'customer')) {
      return false;
    }
    return true;
  }

  gridButtonClicked($event: GridButtonEventData) {
    this.clearMessages();
    if ($event.name === 'delete') {
      this.onDeleteRow($event.row);
    } else if ($event.name === 'activate' || $event.name === 'reset') {
      if ($event.name === 'activate') {
        this.onActivate($event.row);
      } else {
        this.resetPass($event.row);
      }
    } else if ($event.name === 'edit') {
      this.onEditRow($event.row);
      this.editMode = GridEditMode.Edit;

    } else if ($event.name === 'cancel') {
      this.editMode = GridEditMode.NoEdit;
    } else if ($event.name === 'update') {
      this.onUpdateRow();
    }
  }

  onSortChanged($event: Sort) {
    this.model.users.sort((a, b) => (this.userCompare(a, b, $event.column) ? -1 : 1) *
    ($event.direction === SortDirection.Asc ? 1 : -1));
  }

  userCompare(a: User, b: User, col: GridColumn) {
    if (col.dataType !== ColumnDataType.Date) {
      return a[col.field] < b[col.field];
    }
    return a[col.field] == null ||  moment(a[col.field]).isBefore(moment(b[col.field]));
  }

  clearMessages() {
    this.successMessage = '';
    this.errorMessage = '';
  }

}
