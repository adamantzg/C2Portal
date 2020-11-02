import { CommonService } from './common.service';
import { HttpErrorResponse } from '@angular/common/http';

describe('commonService', () => {
    let service;

    beforeEach(() => {
        service = new CommonService();
    });

    test('getError', () => {
        const error = new Error('message');
        let err = new HttpErrorResponse({error: error });
        let msg = service.getError(err);

        expect(msg).toBe('message');

        err = new HttpErrorResponse({error: 'message2'});
        msg = service.getError(err);
        expect(msg).toBe('message2');

        err = new HttpErrorResponse({error: null, statusText: 'status'});
        msg = service.getError(err);
        expect(msg).toBe('status');


    });
});
