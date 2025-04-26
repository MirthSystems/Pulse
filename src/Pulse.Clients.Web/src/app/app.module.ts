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
          clientId: '{{REPLACE_WITH_CLIENT_ID_FROM_ENV}}',
          authority: '{{REPLACE_WITH_AUTHORITY_FROM_ENV}}',
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
          scopes: ['user.read']
        }
      },
      {
        interactionType: InteractionType.Redirect,
        protectedResourceMap: new Map([
          ['https://graph.microsoft.com/v1.0/me', ['user.read']]
          // Add your API endpoints and required scopes here
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
