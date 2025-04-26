import { Injectable, ErrorHandler, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandlerService implements ErrorHandler {

  constructor(private injector: Injector) { }

  handleError(error: any): void {
    const router = this.injector.get(Router);

    console.error('Error caught by Global Error Handler:', error);

    // Generate a unique request ID
    const requestId = this.generateRequestId();

    // Determine error code and message
    let statusCode = '500';
    let errorMessage = 'An unexpected error occurred';

    if (error instanceof HttpErrorResponse) {
      statusCode = error.status.toString();
      errorMessage = error.message;
    } else if (error instanceof Error) {
      errorMessage = error.message;
    }

    // Navigate to error page with details
    router.navigate(['/error', statusCode], {
      queryParams: {
        message: errorMessage,
        requestId: requestId
      }
    });
  }

  private generateRequestId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substring(2);
  }
}
