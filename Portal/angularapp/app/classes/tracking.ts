export class Tracking {
    // opheadm
    order_no: number; // Sage order number
    customer_order_no: string; // Customer Order number
    date_entered: Date; // Date order received and input to Sage
    invoice_date: Date; // Currently will be to indicate "Delivered" flag. When there is an invoice date then the order is delivered
    address1: string;
    address2: string;
    address3: string;
    address4: string;
    address5: string;
    address6: string;
    town_city: string;
    country: string;
    // opaudm joined table
    audit_key: string; // need to join to the Order_no field of opheadm
    character_value: string; // limited to 6 to infer Despatch Note printed
    audit_date: Date; // is the date used for Order Processed date.
}
export class AccountHistory {
    dated: Date;
    kind: string;
    reference: string;
    cust_num: string;
    item_no: string;
    due_date?: Date;
    amount: number;
    // open_indicator: string;
}
export class User {
    constructor(
        public id: number,
        public username: string,
        public user_welcome: string,
        public customer_code: string,
        public business_name: string,
        public business_address: string,
        public phone: string,
    ) { }
}
