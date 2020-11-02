import { MenuItem } from '../../c2-flex/services/menu.service';
import { RoleId, PermissionId } from '../domainclasses';

export class SecuredMenuItem extends MenuItem {
    roles: RoleId[];
    permissions: PermissionId[];
}
