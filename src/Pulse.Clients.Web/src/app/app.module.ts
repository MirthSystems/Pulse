import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptorsFromDi, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// MSAL imports
import { MsalModule, MsalInterceptor, MsalGuard, MsalRedirectComponent } from '@azure/msal-angular';
import { InteractionType, PublicClientApplication } from '@azure/msal-browser';

// Material module
import { MaterialModule } from './modules/material.module';

import { AppRoutingModule } from './app-routing.module';
// Import home component and other components here

import { environment } from '../environments/environment';

@NgModule({
  declarations: [
    // Add your components here, but not AppComponent as it's standalone
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MaterialModule,
    MsalModule.forRoot(
      new PublicClientApplication({
        auth: {
          clientId: environment.auth.clientId,
          authority: environment.auth.authority,
          redirectUri: window.location.origin,
          postLogoutRedirectUri: window.location.origin,
        },
        cache: {
          cacheLocation: 'localStorage',
          storeAuthStateInCookie: false,
        }
      }),
      {
        interactionType: InteractionType.Redirect,
        authRequest: {
          scopes: environment.microsoftGraph.scopes
        }
      },
      {
        interactionType: InteractionType.Redirect,
        protectedResourceMap: new Map([
          [`${environment.microsoftGraph.domain}/${environment.microsoftGraph.version}/me`, environment.microsoftGraph.scopes]
        ])
      }
    )
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    MsalGuard
  ],
  bootstrap: [MsalRedirectComponent]
})
export class AppModule { }
