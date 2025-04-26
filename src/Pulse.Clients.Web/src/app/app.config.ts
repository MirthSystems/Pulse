import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import {
  MSAL_INSTANCE,
  MSAL_GUARD_CONFIG,
  MSAL_INTERCEPTOR_CONFIG,
  MsalGuardConfiguration,
  MsalInterceptorConfiguration,
  MsalService,
  MsalGuard,
  MsalInterceptor,
  MsalBroadcastService
} from '@azure/msal-angular';

import {
  InteractionType,
  IPublicClientApplication,
  PublicClientApplication
} from '@azure/msal-browser';

import { routes } from './app.routes';
import { environment } from '../environments/environment';
import { MatSnackBarModule } from '@angular/material/snack-bar';

// MSAL Instance Factory
export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication(environment.msalConfig);
}

// MSAL Guard Configuration Factory
export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: environment.apiConfig.scopes
    },
    loginFailedRoute: '/login-failed'
  };
}

// MSAL Interceptor Configuration Factory
export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap: new Map([
      [environment.apiConfig.graphEndpoint, environment.apiConfig.scopes]
    ])
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    importProvidersFrom(MatSnackBarModule),

    // MSAL providers
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    MsalService,
    MsalGuard,
    MsalInterceptor,
    MsalBroadcastService
  ]
};
