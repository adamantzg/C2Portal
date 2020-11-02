import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Role, Permission, RoleId } from '../domainclasses';
import { GridDefinition, GridColumn, GridButton, GridColumnType, GridEditorType,
    ColumnDataType, GridEditMode, GridButtonEventData, MultipleSelectionData } from '../../common_components/components';
import { RolesModel } from '../modelclasses';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { MessageBoxType, MessageBoxCommand, MessageBoxCommandValue } from '../common/ModalDialog';
import { CommonService } from '../../c2-flex/services/common.service';

@Component({
    selector: 'admin-roles',
    templateUrl: './admin-roles.component.html',
    styleUrls: ['./admin-roles.component.css']
  })
export class AdminRolesComponent implements OnInit {
    constructor(private userService: UserService, private messageBoxService: MessageboxService,
    private commonService: CommonService) {
        this.gridDefinition.columnButtonVisibilityCallback = this.checkColumnButtonVisibility.bind(this);
    }
    model: RolesModel = new RolesModel();
    errorMessage = '';
    successMessage = '';
    editModes = Object.assign({}, GridEditMode);
    editMode = GridEditMode.NoEdit;
    roleCopy: Role;
    editedRoleId = -1;

    gridDefinition = new GridDefinition([
        new GridColumn('Name', 'name', GridColumnType.Label, 'name', null, null, null, null, true),
        new GridColumn('Permissions', 'permissions', GridColumnType.Label, 'permissions', null, null, null, null, true, null,
        null, null, true, GridEditorType.MultipleChoiceCheckboxes, null, null, true, null,
        new MultipleSelectionData(true, null, 'id', 'name', 'selected', 'Choose permission'),
         this.permissionsString ),
        new GridColumn('buttons', null, GridColumnType.ButtonGroup, 'buttons', null, null, null, null,
        false, null, null, null, false,
          null,
          [
            new GridButton('Update', 'update', 'fa fa-check', 'btn-xs', true),
            new GridButton('Cancel', 'cancel', 'fa-times-circle-o', 'btn-xs'),
            new GridButton('Edit', 'edit', 'fa-pencil-square-o', 'btn-xs'),
            new GridButton('Delete', 'delete', 'fa-remove', 'btn-danger btn-xs')
          ])
    ]);

    ngOnInit() {
        this.userService.getRolesModel().subscribe((m: RolesModel) => {
            this.model = m;
            // this.model.roles.forEach(r => r.strPermissions = r.permissions.map(p => p.name));
        });
    }

    addNew() {
        this.roleCopy = JSON.parse(JSON.stringify(new Role(0)));
        this.roleCopy.permissions = JSON.parse(JSON.stringify(this.model.permissions));
        this.editMode = GridEditMode.AddNew;
        this.editedRoleId = 0;
    }

    gridButtonClicked(event: GridButtonEventData) {
        this.clearMessages();
        if (event.name === 'delete') {
          this.onDeleteRow(event.row);
        } else if (event.name === 'edit') {
          this.onEditRow(event.row);
          this.editMode = GridEditMode.Edit;
        } else if (event.name === 'cancel') {
          this.editMode = GridEditMode.NoEdit;
        } else if (event.name === 'update') {
          this.onUpdateRow();
        }
    }

    permissionsString(data: Role) {
        if (data.permissions != null) {
            return data.permissions.sort( (x, y) => x.id - y.id).map(p => p.name).join(', ');
        }
        return '';
    }

    clearMessages() {
        this.errorMessage = '';
        this.successMessage = '';
    }

    checkColumnButtonVisibility(r: Role, column: string, button: string) {

        if (button === 'edit' || button === 'delete') {
          return (r.id !== RoleId.Admin) &&  (this.editMode === GridEditMode.NoEdit || r.id !== this.roleCopy.id);
        } else if (button === 'update' || button === 'cancel') {
          return this.editMode !== GridEditMode.NoEdit && r.id === this.roleCopy.id;
        }
        return true;
    }

    onEditRow(role: Role) {
        this.roleCopy = JSON.parse(JSON.stringify(role));
        const selectedPermissions = {};
        this.roleCopy.permissions.forEach(p => selectedPermissions[p.id] = true);
        this.roleCopy.permissions = JSON.parse(JSON.stringify(this.model.permissions));
        this.roleCopy.permissions.forEach(p => p.selected = selectedPermissions[p.id] != null );
        this.editedRoleId = role.id;
      }

      onCancelRow() {
        this.editedRoleId = -1;
      }
      onUpdateRow() {
        /** send user to services */

        const role = JSON.parse(JSON.stringify(this.roleCopy));
        const isNew = role.id === 0;
        this.errorMessage = '';
        role.permissions = role.permissions.filter(p => p.selected);

        // tslint:disable-next-line:triple-equals

        this.userService.createUpdateRole(role).subscribe( (r: Role) => {
          this.errorMessage = '';

          if (isNew) {
            this.model.roles.push(r);
          } else {
            const existingRole = this.model.roles.find(x => x.id === role.id);
            if (existingRole != null) {
              Object.assign(existingRole, r);
            }

          }
          this.editMode = GridEditMode.NoEdit;
            },
            err => this.errorMessage = this.commonService.getError(err)
        );

      }
      onDeleteRow(role: Role) {
        this.messageBoxService.openDialog('Delete role?', MessageBoxType.Yesno).subscribe((m: MessageBoxCommand) => {
          if (m.value === MessageBoxCommandValue.Ok) {
            this.errorMessage = '';
            this.userService.deleteRole(role.id).subscribe( data => {
              const index = this.model.roles.findIndex(r => r.id === role.id);
              if (index >= 0) {
                this.model.roles.splice(index, 1);
              }
            },
            err => this.errorMessage = this.commonService.getError(err));
          }
        });

      }
}


