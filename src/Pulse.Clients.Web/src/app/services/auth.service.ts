import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { AccountInfo } from '@azure/msal-browser';
import { Observable, from } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private msalService: MsalService) { }

  public isLoggedIn(): boolean {
    return this.msalService.instance.getAllAccounts().length > 0;
  }

  public login(): Observable<void> {
    return from(this.msalService.loginRedirect());
  }

  public logout(): Observable<void> {
    return from(this.msalService.logout());
  }

  public acquireToken(scopes: string[]): Observable<any> {
    const account = this.msalService.instance.getAllAccounts()[0];
    if (!account) {
      return from(Promise.reject('User not logged in'));
    }

    return from(this.msalService.instance.acquireTokenSilent({
      scopes: scopes,
      account: account
    }));
  }
}
