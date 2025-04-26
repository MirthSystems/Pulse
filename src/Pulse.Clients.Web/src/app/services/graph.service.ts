import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { Client } from '@microsoft/microsoft-graph-client';
import { User } from '@microsoft/microsoft-graph-types';
import { MsalService } from '@azure/msal-angular';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GraphService {
  private graphClient: Client;

  constructor(
    private authService: MsalService,
    private http: HttpClient
  ) {
    this.graphClient = Client.init({
      authProvider: async (done) => {
        try {
          const account = this.authService.instance.getActiveAccount() ||
                         this.authService.instance.getAllAccounts()[0];

          if (!account) {
            throw new Error('No active account! Verify a user has been signed in and setActiveAccount has been called.');
          }

          const response = await this.authService.instance.acquireTokenSilent({
            scopes: environment.microsoftGraph.scopes,
            account: account
          });

          done(null, response.accessToken);
        } catch (error) {
          done(error as Error, null);
        }
      }
    });
  }

  getUserProfile(): Observable<User> {
    return from(
      this.graphClient
        .api('/me')
        .select('displayName,givenName,surname,mail,userPrincipalName,id,jobTitle,department,officeLocation')
        .get()
    );
  }

  getUserPhoto(): Observable<Blob> {
    return from(
      this.graphClient
        .api('/me/photo/$value')
        .get()
    );
  }
}
