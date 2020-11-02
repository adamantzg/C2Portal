import { Tracking, AccountHistory } from './tracking';

export const TRACKING_ORDER: Tracking = {
    order_no: 123456,
    customer_order_no: '12345',

    /** 1 ORDER RECEIVED*/
    // date_entered:  null,
    date_entered:  new Date(),

    /** 2 ORDER PROCESSED*/
    // audit_date: null,
    audit_date:   new Date(new Date().getTime() + 24 * 60 * 60 * 1000),

    /** 3 DELIVERD*/
    // invoice_date: null,
    invoice_date: new Date(),

    address1: 'Jon Doe',
    address2: 'Kutnjacki put 10',
    address3: '10000',
    address4: 'Zagreb',
    address5: '',
    address6: '',
    town_city: 'Zagreb',
    country: 'Hrvatska',
    audit_key: '333',
    character_value: 'cbcb'
};

export const ACCOUNT_HISTORY: AccountHistory[] = [
    // tslint:disable-next-line:max-line-length
    { dated: new Date('06/11/2017'), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: -74.35 },
    { dated: new Date('11/20/2017'), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31006', item_no: 'OP/C187286', due_date: new Date(), amount: -126.77 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date('02/11/2017'), kind: 'CRN', reference: 'CNRN147198', cust_num: '1017001982', item_no: 'OP/C187286', due_date: new Date(), amount: 510.00 },
    { dated: new Date('02/11/2017'), kind: 'INV', reference: '205237', cust_num: '1017000861', item_no: 'OP/C187286', due_date: new Date(), amount: 144.88 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date('02/11/2017'), kind: 'INV', reference: '219075', cust_num: '1017002428', item_no: 'OP/I172032', due_date: new Date(), amount: 0.00 },
    { dated: new Date('02/11/2017'), kind: 'INV', reference: '219075', cust_num: '1017002428', item_no: 'OP/I172032', due_date: new Date(), amount: 0.00 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date('03/11/2017'), kind: 'INV', reference: '221260', cust_num: 'SHOWROOMSAMPLE2/11', item_no: 'OP/I173934', due_date: new Date(), amount: 375.0 },
    { dated: new Date('06/11/2017'), kind: 'INV', reference: '974841/2', cust_num: '1017002110', item_no: 'OP/I175391', due_date: new Date(), amount: 300.00 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date('06/11/2017'), kind: 'INV', reference: '974841/1', cust_num: '1017002110', item_no: 'OP/I175391', due_date: new Date(), amount: 51.77 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 66.12 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 206.63 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    // tslint:disable-next-line:max-line-length
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
    { dated: new Date(), kind: 'CRN', reference: 'CNRN147198', cust_num: '0917L31005', item_no: 'OP/C187286', due_date: new Date(), amount: 74.35 },
];
