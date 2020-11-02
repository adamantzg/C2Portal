
export class Product {
    id: number;
    code: string;
    name: string;
    description: string;
    combined_name: string;
}

export class Customer {
    code: string;
    name: string;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    address5: string;
    address6: string;
    town_city: string;
    county: string;
    analysis_codes1: string;
    currency: string;
    combined_name: string;
}

export class User {
    id: number;
    name: string;
    lastname: string;
    username: string;
    password: string;
    email: string;
    customer_code: string;
    address: string;
    phone: string;
    token: string;
    customer: Customer;
    roles: Role[];
    locked: boolean;
    isTopAdmin: boolean;
    isBranchAdmin: boolean;
    activated: boolean;
    isInternal: boolean;
    lastLogin: Date;
    role: string;
    role_id: number;
    permissions: Permission[];

    constructor() {
        this.customer = new Customer();
    }
}

export class Role {
    id: number;
    name: string;
    permissions: Permission[];
    // strPermissions: string[];

    constructor(id: number, name?: string, permissions?: Permission[]) {
        this.id = id;
        this.name = name;
        this.permissions = permissions;
    }
}


export class Invoice {
    customer: string;
    dated: Date | null;
    kind: string;
    reference: string;
    item_no: string;
    due_date: Date | null;
    amount: number | null;
    open_indicator: string;
    customer_order_num: string;
    selected: boolean;
    postcode: string;
}

export enum RoleId {
    User = 1,
    BranchAdmin = 2,
    Admin = 3
}

export class Order {
    order_no: string; // Sage order number
    customer_order_no: string; // Customer Order number
    date_entered: Date; // Date order received and input to Sage
    invoice_date: Date; // Currently will be to indicate "Delivered" flag. When there is an invoice date then the order is delivered
    date_required: Date;
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    address5: string;
    address6: string;
    town_city: string;
    statusText: string;
    status: number;
    customer?: string;
    country: string;
    audits: OrderAudit[];
    productStockData: StockData[];
    details: OrderDetail[];
    planned: boolean;
}

export class OrderDetail {
    product: string;
    order_qty: number;
    val: number;
    order_line_no: number;
}

export  enum OrderStatus {
    Received = 5,
    InProgress = 7,
    Delivered = 8,
    Cancelled = 9
    // Processed = 'Processed',
    // Confirmed = 'Confirmed',
    // Delivered = 'Delivered',
    // Cancelled = 'Cancelled',
}
export class OrderAudit {
    audit_key: string; // need to join to the Order_no field of opheadm
    character_value: string; // limited to 6 to infer Despatch Note printed
    audit_date: Date; // is the date used for Order Processed date.
}

export class StockData {
    quantity: number;
    product: Product;
    ship_date: Date | null;
    discontinued: boolean;
    madeToOrder: boolean;
    expiresThisYear: boolean;
}

export class Holiday {
    name: string;
    date: Date;
}
export class HolidayExtracted {
    name: string;
    year: number;
    month: number;
    date: number;
    fullDate: Date;
}
export class Permission {
    id: number;
    name: string;
    selected: boolean;

    constructor(id?: number, name?: string, selected?: boolean) {
        this.id = id;
        this.name = name;
        this.selected = selected;
    }
}

export enum PermissionId {
    ViewAccountDetails = 1,
    ViewHolidayAdministration = 2,
    ViewInvoiceHistory = 3,
    ViewOrderHistory = 4,
    ViewStockSearch = 5,
    ViewUserAdministration = 6,
    ViewRoleAdministration = 7
}

export class ClearanceStock {
    product_code: string;
    long_description: string;
    range: string;
    exvat_price: number | null;
    price: number | null;
    freestock: number | null;
    discount: number;
}
