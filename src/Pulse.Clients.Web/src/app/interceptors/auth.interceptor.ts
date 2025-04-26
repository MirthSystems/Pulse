import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, from, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { environment } from '../../environments/environment';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!request.url.startsWith(environment.apiUrl)) {
      return next.handle(request);
    }

    return this.addToken(request).pipe(
      switchMap(requestWithToken => next.handle(requestWithToken)),
      catchError(error => {
        if (error.status === 401) {
          this.authService.login();
        }
        return throwError(() => error);
      })
    );
  }

  private addToken(request: HttpRequest<any>): Observable<HttpRequest<any>> {
    return from(this.authService.acquireToken(environment.microsoftGraph.scopes)
      .pipe(
        switchMap(token => {
          return from([request.clone({
            setHeaders: {
              Authorization: `Bearer ${token.accessToken}`
            }
          })]);
        }),
        catchError(() => {
          return from([request]);
        })
      ));
  }
}
