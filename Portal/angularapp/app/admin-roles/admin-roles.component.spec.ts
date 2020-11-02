import { UserService } from '../services/user.service';
import { HttpClient } from '@angular/common/http';
import { HttpMock } from '../testmocks/httpmock';
import { CommonAppModule } from 'common_components/common.module';
import { BlockUIService } from 'common_components/services/block-ui.service';
import { BlockUIServiceMock } from '../testmocks/blockuiservicemock';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminRolesComponent } from './admin-roles.component';
import { User, Role, Permission, PermissionId, RoleId } from '../domainclasses';
import { RolesModel } from '../modelclasses';
import { of } from 'rxjs';
import { BlockUiComponent } from '../blockui/block-ui.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpService } from '../services/http.service';
import { MessageboxService } from '../common/messagebox/messagebox.service';
import { CommonService } from '../../c2-flex/services/common.service';
import { BsModalService, ModalModule } from 'ngx-bootstrap';
import { MessageBoxCommand, MessageBoxCommandValue } from '../common/ModalDialog';
import { GridButtonEventData, GridEditMode } from 'common_components/components';



describe('admin-roles.component', () => {

    let fixture: ComponentFixture<AdminRolesComponent>;
    let component: AdminRolesComponent;
    let userService: UserService;
    let model: RolesModel;

    setupTestBed({
        declarations: [AdminRolesComponent, BlockUiComponent],
        imports: [CommonAppModule, RouterTestingModule.withRoutes([{path: 'user', component: AdminRolesComponent}]), ModalModule.forRoot()],
        providers: [UserService, HttpService, {provide: HttpClient, useClass: HttpMock},
            {provide: BlockUIService, useClass: BlockUIServiceMock}, MessageboxService, CommonService, BsModalService]
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(AdminRolesComponent);
        component = fixture.componentInstance;

        userService = TestBed.get(UserService);
        const u = new User();
        u.id = 1;
        u.isTopAdmin = true;
        model = new RolesModel();

        jest.spyOn(userService, 'CurrentUser', 'get').mockImplementation(() => u);
        jest.spyOn(userService, 'getRolesModel').mockImplementation(() => of(model));
    });

    test('should create', () => {
        expect(component).toBeTruthy();
        expect(fixture).toMatchSnapshot();
    });

    test('checkColumnButtonVisibility', () => {

        const r = new Role(3, 'admin');
        let result = component.checkColumnButtonVisibility(r, '', 'edit');
        expect(result).toBeFalsy();

        r.id = 4;
        result = component.checkColumnButtonVisibility(r, '', 'edit');
        expect(result).toBeTruthy();
    });

    test('onEditRow', () => {

        model.permissions = [new Permission(PermissionId.ViewAccountDetails), new Permission(PermissionId.ViewInvoiceHistory)] ;
        component.model = model;
        const r = new Role(4);

        r.permissions = [new Permission(PermissionId.ViewAccountDetails)];
        component.onEditRow(r);
        expect(component.roleCopy).toBeTruthy();
        expect(component.roleCopy.id).toBe(r.id);

        expect(component.roleCopy.permissions.length).toBe(2);
        expect(component.roleCopy.permissions.filter(p => p.selected).length).toBe(1);
    });

    test('onUpdateRow', () => {
        model.roles = [new Role(RoleId.User), new Role(RoleId.BranchAdmin)] ;
        component.model = model;
        const r = new Role(0);
        r.permissions = [new Permission(PermissionId.ViewAccountDetails), new Permission(PermissionId.ViewInvoiceHistory)];
        r.permissions[0].selected = true;
        component.roleCopy = r;
        const returnedRole = new Role(4);

        const fn = jest.spyOn(userService, 'createUpdateRole').mockImplementation(() => of(returnedRole));

        component.onUpdateRow();
        expect(fn.mock.calls[0][0].permissions.length).toBe(1);
        expect(model.roles.length).toBe(3);

        // Check existing
        r.id = RoleId.User;
        returnedRole.name = 'changed';
        component.onUpdateRow();
        expect(model.roles[0].name).toBe(returnedRole.name);

    });

    test('onDeleteRow', () => {
        component.model.roles = [new Role(RoleId.User), new Role(RoleId.BranchAdmin)];
        const msgBoxService = TestBed.get(MessageboxService);
        const cmd = new MessageBoxCommand(MessageBoxCommandValue.Ok);
        const fn1 = jest.spyOn(msgBoxService, 'openDialog').mockImplementation(() => of(cmd));
        const fn2 = jest.spyOn(userService, 'deleteRole').mockImplementation(() => of(null));
        component.onDeleteRow(new Role(RoleId.User));
        expect(fn1.mock.calls[0][0]).toBe('Delete role?');
        expect(fn2.mock.calls[0][0]).toBe(RoleId.User);
        expect(component.model.roles.length).toBe(1);
    });

    test('permissionsString', () => {
        const data = new Role(4);
        data.permissions = [new Permission(PermissionId.ViewInvoiceHistory, 'view invoice'),
        new Permission(PermissionId.ViewAccountDetails, 'view account')];
        const arrResult = component.permissionsString(data).split(', ');
        expect(arrResult.length).toBe(2);
        expect(arrResult[0]).toBe('view account');
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

    });
});
