import { of } from 'rxjs';

export class HttpMock {
    url: string;
    data: any;
    params: any;
    options: any;
    postResult: any;
    getResult: any;


    post<T>(url, data, params) {
      this.data = data;
      this.url = url;
      this.params = params;
      return of(this.postResult);
    }

    get<T>(url, options) {
        this.url = url;
        this.options = options;
        return of(this.getResult);
    }

  }
