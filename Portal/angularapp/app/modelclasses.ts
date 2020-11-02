import { User, Role, Product, Permission } from './domainclasses';

export class ProductPrices {
    basePrice: number;
    customerPrice: number;
}

export class CustomerTotals  {
    orderTotal: number | null;
    balance: number | null;
    creditLimit: number | null;
    numOfInvoices: number | null;
    numOfOrders: number | null;
}

export class UsersModel {
    users: User[];
    roles: Role[];
}

export class ChangePass {
    code: string;
    username: string;
    password1: string;
    password2: string;
    ruleText: string;
}

export class OrderTotals {
    orderCount: number;
}

export class RolesModel {
    roles: Role[];
    permissions: Permission[];
}


