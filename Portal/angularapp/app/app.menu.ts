import { SecuredMenuItem } from './c2/securedmenuitem';
import { RoleId, PermissionId } from './domainclasses';

export let initialMenuItems: SecuredMenuItem[] = [
    { text: 'Account Details', icon: 'glyphicon glyphicon-user', route: 'account', submenu: null, roles: null,
     permissions: [PermissionId.ViewAccountDetails]},
    { text: 'Stock availability', icon: 'glyphicon glyphicon-tasks', route: 'stock', submenu: null, roles: null,
    permissions: [PermissionId.ViewStockSearch]},
    { text: 'Clearance stock', icon: 'glyphicon glyphicon-tasks', route: 'clearance', submenu: null, roles: null,
    permissions: [PermissionId.ViewStockSearch]},
    { text: 'Invoice History', icon: 'glyphicon glyphicon-dashboard', route: 'invoice-history', submenu: null, roles: null,
    permissions: [PermissionId.ViewInvoiceHistory]},
    { text: 'Order History', icon: 'glyphicon glyphicon-list-alt', route: 'order-history', submenu: null, roles: null,
    permissions: [PermissionId.ViewOrderHistory]},
    // { text: 'Order tracking', icon: 'glyphicon glyphicon-flag', route: 'order-tracking', submenu: null, roles: null},
    { text: 'User Administration', icon: 'glyphicon glyphicon-list', route: 'user', submenu: null,
    roles: null,
    permissions: [PermissionId.ViewUserAdministration]},
    { text: 'Holiday Administration', icon: 'glyphicon glyphicon-calendar', route: 'admin-holidays', submenu: null,
    roles: null,
    permissions: [PermissionId.ViewHolidayAdministration]},
    { text: 'Role Administration', icon: 'glyphicon glyphicon-calendar', route: 'role', submenu: null,
    roles: null,
    permissions: [PermissionId.ViewRoleAdministration]}
    // { text: 'Logout', icon: 'glyphicon glyphicon-log-in', route: '/login', submenu: null},
    // { text: 'Order history', icon: 'glyphicon glyphicon-wrench', route: 'order-history', submenu: null},
    // { text: 'Lock Screen', icon: 'fa fa-lock', route: 'lock-screen', submenu: null}
];
