export class OrderFilter {

    constructor(from?: Date, to?: Date, searchText: string = '') {
        this.from = from;
        this.to = to;
        this.searchText = searchText;
    }

    from: Date;
    to: Date;
    searchText = '';
}
