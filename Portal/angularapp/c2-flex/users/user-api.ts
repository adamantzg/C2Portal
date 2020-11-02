import { Observable } from 'rxjs/Observable';

export abstract class UserApi {
    signIn: (
        username: string,
        password: string,
        rememberMe: boolean
     ) => Observable<any>;
     // signOut :
     signOut: () => Observable<any>;
     logged: () => Observable<IUser>;
     sendPasswordRecoveryLink: (email: string) => Observable<any>;
    }

export interface IUser {
    name: string;
    lastname: string;
    username: string;
}
