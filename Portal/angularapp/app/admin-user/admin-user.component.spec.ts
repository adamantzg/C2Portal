import { AdminUserComponent } from './admin-user.component';
import { UserService } from '../services/user.service';
import { UserServiceMock } from '../testmocks/userserviceMock';
import { HttpClient } from '@angular/common/http';
import { HttpMock } from '../testmocks/httpmock';
import { NotificationsService, SimpleNotificationsComponent, SimpleNotificationsModule } from 'angular2-notifications';
import { CommonService } from '../../c2-flex/services/common.service';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { TestBed, ComponentFixture } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GridComponent, GridEditMode, GridColumn, GridColumnType, GridEditorType,
    GridButtonEventData, ColumnDataType } from 'common_components/components';
import { CommonAppModule } from 'common_components/common.module';
import { BlockUiComponent } from '../blockui/block-ui.component';
import { BlockUIService } from 'common_components/services/block-ui.service';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';
import { ModalModule, BsModalService } from 'ngx-bootstrap';
import { User, RoleId, Role } from '../domainclasses';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpService } from '../services/http.service';
import { UsersModel } from '../modelclasses';
import { of, throwError } from 'rxjs';
import { MessageBoxCommand, MessageBoxCommandValue } from '../common/ModalDialog';

describe('admin-user-component', () => {

    let userService: UserService, nsService: NotificationsService;
    let component: AdminUserComponent, fixture: ComponentFixture<AdminUserComponent>;
    let model: UsersModel;

    setupTestBed({
        declarations: [AdminUserComponent, BlockUiComponent],
        providers: [UserService, HttpService, {provide: HttpClient, useClass: HttpMock},
        NotificationsService, CommonService, MessageboxService, {provide: BlockUIService, useClass: BlockUIServiceMock}, BsModalService],
        imports: [FormsModule, ReactiveFormsModule, CommonAppModule, SimpleNotificationsModule.forRoot(), ModalModule.forRoot(),
        RouterTestingModule.withRoutes([{path: 'user', component: AdminUserComponent}])]
    });

    beforeEach(() => {
        userService = TestBed.get(UserService);
        nsService = TestBed.get(NotificationsService);
        const u = new User();
        u.id = 1;
        u.isTopAdmin = true;
        model = new UsersModel();

        jest.spyOn(userService, 'CurrentUser', 'get').mockImplementation(() => u);
        jest.spyOn(userService, 'getUsersModel').mockImplementation(() => of(model));

        fixture = TestBed.createComponent(AdminUserComponent);
        component = fixture.componentInstance;
    });

    test('checkColumnButtonVisibility', () => {
        // activate
        component.userCopy = new User();
        component.userCopy.id = 4;


        const u = new User();
        u.id = 2;
        u.activated = false;
        u.roles = [new Role(RoleId.User, '')];

        const u2 = new User();
        u2.id = 3;
        u2.roles = [new Role(RoleId.BranchAdmin)];

        expect(component.checkColumnButtonVisibility(u, '', 'activate')).toBeTruthy();
        u.activated = true;
        expect(component.checkColumnButtonVisibility(u, '', 'activate')).toBeFalsy();
        expect(component.checkColumnButtonVisibility(u, '', 'reset')).toBeTruthy();
        expect(component.checkColumnButtonVisibility(u, '', 'edit')).toBeTruthy();

        expect(component.checkColumnButtonVisibility(u, '', 'delete')).toBeTruthy();
        component.currentUser.isTopAdmin = false;
        component.currentUser.isBranchAdmin = true;
        expect(component.checkColumnButtonVisibility(u, '', 'delete')).toBeTruthy();

        // branch admin - can't delete, can't edit others, can edit himself
        u.roles[0].id = RoleId.BranchAdmin;
        expect(component.checkColumnButtonVisibility(u, '', 'delete')).toBeFalsy();
        expect(component.checkColumnButtonVisibility(u, '', 'edit')).toBeFalsy();
        u.id = component.currentUser.id;
        expect(component.checkColumnButtonVisibility(u, '', 'edit')).toBeTruthy();

        u.id = 2;
        u.roles[0].id = 4;  // some other user role other than branch admin
        expect(component.checkColumnButtonVisibility(u, '', 'delete')).toBeTruthy();
        expect(component.checkColumnButtonVisibility(u, '', 'edit')).toBeTruthy();

        expect(component.checkColumnButtonVisibility(u, '', 'update')).toBeFalsy();
        expect(component.checkColumnButtonVisibility(u, '', 'cancel')).toBeFalsy();

        component.editMode = GridEditMode.Edit;
        expect(component.checkColumnButtonVisibility(u, '', 'edit')).toBeTruthy();
        u.id = component.userCopy.id;
        expect(component.checkColumnButtonVisibility(u, '', 'edit')).toBeFalsy();
        expect(component.checkColumnButtonVisibility(u, '', 'delete')).toBeFalsy();
        expect(component.checkColumnButtonVisibility(u, '', 'update')).toBeTruthy();
        expect(component.checkColumnButtonVisibility(u, '', 'cancel')).toBeTruthy();

    });

    test('checkColumnVisibility', () => {
        const c = new GridColumn('role', 'role_id');
        component.currentUser.isTopAdmin = true;
        expect(component.checkColumnVisibility(c)).toBeTruthy();
        component.currentUser.isTopAdmin = false;
        component.currentUser.isBranchAdmin = true;
        expect(component.checkColumnVisibility(c)).toBeTruthy();
        component.currentUser.isBranchAdmin = false;
        component.currentUser.isTopAdmin = false;
        expect(component.checkColumnVisibility(c)).toBeFalsy();

        c.name = 'internal';
        component.currentUser.isBranchAdmin = true;
        expect(component.checkColumnVisibility(c)).toBeFalsy();
        component.currentUser.isTopAdmin = true;
        expect(component.checkColumnVisibility(c)).toBeTruthy();
    });

    test('checkColumnEditorVisibilityStatus', () => {
        component.currentUser.isBranchAdmin = true;
        component.currentUser.isTopAdmin = false;

        const u = new User();
        u.roles = [new Role(RoleId.User)];
        const column = new GridColumn('role', 'role', GridColumnType.Label, 'role');
        expect(component.checkColumnEditorVisibilityStatus(u, column)).toBeTruthy();
        u.isBranchAdmin = true;
        expect(component.checkColumnEditorVisibilityStatus(u, column)).toBeFalsy();

        u.roles = [new Role(RoleId.User)];
        u.isBranchAdmin = false;
        column.name = 'customer';
        expect(component.checkColumnEditorVisibilityStatus(u, column)).toBeTruthy();
        u.isBranchAdmin = true;
        expect(component.checkColumnEditorVisibilityStatus(u, column)).toBeFalsy();

    });

    test('addNew', () => {
        component.model.roles = [new Role(RoleId.User, 'user'), new Role(RoleId.BranchAdmin)];
        component.userCopy = null;
        component.addNew();
        expect(component.userCopy).toBeTruthy();
        expect(component.userCopy.roles.length).toBe(1);
        expect(component.userCopy.role).toBe('user');
        expect(component.userCopy.role_id).toBe(RoleId.User);
        expect(component.userCopy.id).toBe(0);
        expect(component.editMode).toBe(GridEditMode.AddNew);
        expect(component.editedUserId).toBe(0);
    });

    test('onEditRow', () => {
        // For branch admin, no role branch admin in role dropdown but role needs to be displayed read-only
        model.roles = [new Role(RoleId.User), new Role(4)] ;
        component.model = model;
        const u = new User();
        u.id = 2;
        u.roles = [new Role(RoleId.BranchAdmin)];
        u.role = 'Branch admin';
        component.onEditRow(u);
        expect(component.userCopy).toBeTruthy();
        expect(component.userCopy.id).toBe(u.id);
        expect(component.userCopy.role).toBe(u.role);
        expect(component.userCopy.roles.length).toBe(1);

        // for admin when role exists
        model.roles = [new Role(RoleId.User), new Role(RoleId.BranchAdmin)];
        u.roles = [new Role(RoleId.User)];
        component.onEditRow(u);
        expect(component.userCopy).toBeTruthy();
        expect(component.userCopy.id).toBe(u.id);
        expect(component.userCopy.roles.length).toBe(1);
        expect(component.userCopy.roles[0].id).toBe(RoleId.User);
    });

    test('onUpdateRow', () => {
        model.roles = [new Role(RoleId.User), new Role(4)] ;
        component.model = model;
        const u = new User();
        u.id = 1;
        u.roles = [new Role(RoleId.User)];
        u.role_id = RoleId.User;
        component.userCopy = u;
        model.users = [u];

        const returnedUser = new User();
        returnedUser.roles = [new Role(RoleId.User)];
        const fn = jest.spyOn(userService, 'createUpdateUser').mockImplementation(() => of(returnedUser));
        const ns_fn = jest.spyOn(nsService, 'success').mockImplementation(() => {});
        component.onUpdateRow();
        expect(fn.mock.calls[0][0].roles[0].id).toBe(RoleId.User);
        expect(ns_fn.mock.calls[0][0]).toBe('User updated');

        u.role_id = null;
        u.roles = [new Role(RoleId.BranchAdmin)];
        returnedUser.roles = [new Role(RoleId.BranchAdmin)];
        component.onUpdateRow();
        expect(fn.mock.calls[1][0].roles[0].id).toBe(RoleId.BranchAdmin);

        // Check new
        u.id = 0;
        returnedUser.id = 2;
        component.onUpdateRow();
        expect(ns_fn.mock.calls[2][0]).toBe('User added');
        expect(model.users.length).toBe(2);
        expect(model.users[0].id).toEqual(returnedUser.id);
        expect(component.editMode).toBe(GridEditMode.NoEdit);


    });

    test('onDeleteRow', () => {
        const u1 = new User();
        u1.id = 1;
        const u2 = new User();
        u2.id = 2;
        component.model.users = [u1, u2];
        const msgBoxService = TestBed.get(MessageboxService);
        const cmd = new MessageBoxCommand(MessageBoxCommandValue.Ok);
        const fn1 = jest.spyOn(msgBoxService, 'openDialog').mockImplementation(() => of(cmd));
        const fn2 = jest.spyOn(userService, 'deleteUser').mockImplementation(() => of(null));
        component.onDeleteRow(u1);
        expect(fn1.mock.calls[0][0]).toBe('Delete user?');
        expect(fn2.mock.calls[0][0]).toBe(1);
        expect(component.model.users.length).toBe(1);
    });

    test('initialization', () => {
        expect(component.gridDefinition.columns.length).toBe(9);
        expect(component.gridDefinition.sort.column.name).toBe('lastname');
        ['username', 'email', 'name', 'lastname', 'customer'].forEach(s => {
            expect(component.gridDefinition.columns.find(c => c.name === s).required).toBeTruthy();
        });
        model.roles = [new Role(RoleId.User), new Role(4)] ;
        model.users = [];
        const u = new User();
        u.roles = [new Role(RoleId.User, 'user')];
        model.users.push(u);
        component.currentUser.isBranchAdmin = true;
        component.currentUser.isTopAdmin = false;
        component.ngOnInit();
        expect(u.role_id).toBeTruthy();
        expect(u.role).toBe('user');

        expect(component.gridDefinition.columns.find(c => c.name === 'role').editorType).toBe(GridEditorType.Dropdown);

    });

    test('onActivate', () => {
        const fn = jest.spyOn(userService, 'activateUser').mockImplementationOnce(() => of({}));
        const u = new User();
        u.email = 'email';
        component.onActivate(u);
        expect(component.successMessage).toBeTruthy();
        expect(component.successMessage).toBe(`Activation email sent to ${u.email}`);

        // error
        jest.spyOn(userService, 'activateUser').mockImplementationOnce(() => throwError({error: 'error'}));
        component.onActivate(u);
        expect(component.errorMessage).toBeTruthy();
    });

    test('resetPass', () => {
        const fn = jest.spyOn(userService, 'resetPassword').mockImplementationOnce(() => of({}));
        const u = new User();
        u.email = 'email';
        component.resetPass(u);
        expect(component.successMessage).toBeTruthy();
        expect(component.successMessage).toBe(`Reset password email sent to ${u.email}`);

        // error
        jest.spyOn(userService, 'resetPassword').mockImplementationOnce(() => throwError({error: 'error'}));
        component.resetPass(u);
        expect(component.errorMessage).toBeTruthy();
    });

    test('checkColumnButtonDisabledStatus', () => {
        const buttons = ['activate', 'update', 'cancel', 'edit', 'delete', 'reset'] ;
        component.editMode = GridEditMode.NoEdit;
        buttons.forEach(b => expect(component.checkColumnButtonDisabledStatus(null, null, b)).toBeFalsy() );
        component.editMode = GridEditMode.Edit;
        buttons.forEach(b => ['update', 'cancel'].indexOf(b) >= 0 ?
         expect(component.checkColumnButtonDisabledStatus(null, null, b)).toBeFalsy() :
         expect(component.checkColumnButtonDisabledStatus(null, null, b)).toBeTruthy());

    });

    test('gridButtonClicked', () => {
        const data = new GridButtonEventData('c', {id: 1});
        const fn1 = jest.spyOn(component, 'onDeleteRow').mockImplementation(() => {});
        let fn2 = jest.spyOn(component, 'onEditRow').mockImplementation(() => {});
        data.name = 'delete';
        component.gridButtonClicked(data);
        expect(fn1.mock.calls.length).toBe(1);
        expect(fn1.mock.calls[0][0].id).toBe(data.row.id);
        data.name = 'edit';
        component.gridButtonClicked(data);
        expect(fn2.mock.calls.length).toBe(1);
        expect(fn2.mock.calls[0][0].id).toBe(data.row.id);
        expect(component.editMode).toBe(GridEditMode.Edit);
        data.name = 'cancel';
        component.gridButtonClicked(data);
        expect(component.editMode).toBe(GridEditMode.NoEdit);
        data.name = 'update';
        fn2 = jest.spyOn(component, 'onUpdateRow').mockImplementation(() => {});
        component.gridButtonClicked(data);
        expect(fn2.mock.calls.length).toBe(1);

        data.name = 'activate';
        fn2 = jest.spyOn(component, 'onActivate').mockImplementation(() => {});
        component.gridButtonClicked(data);
        expect(fn2.mock.calls.length).toBe(1);

        data.name = 'reset';
        fn2 = jest.spyOn(component, 'resetPass').mockImplementation(() => {});
        component.gridButtonClicked(data);
        expect(fn2.mock.calls.length).toBe(1);

    });

    test('userCompare', () => {
        const u1 = new User();
        u1.lastname = 'b';
        u1.lastLogin = new Date();
        u1.lastLogin.setDate(1);
        const u2 = new User();
        u2.lastname = 'a';
        u2.lastLogin = new Date();
        u2.lastLogin.setDate(2);

        const col = new GridColumn('', '');
        col.dataType = ColumnDataType.Text;
        col.field = 'lastname';
        let result = component.userCompare(u1, u2, col);
        expect(result).toBeFalsy();

        col.field = 'lastLogin';
        col.dataType = ColumnDataType.Date;
        result = component.userCompare(u1, u2, col);
        expect(result).toBeTruthy();

    });
});
