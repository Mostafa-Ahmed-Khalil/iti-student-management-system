import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject, isDevMode } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastService } from './toast.service';



export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (isDevMode()) {
        console.error('HTTP Error Interceptor caught error:', error);
      }

      if (error.status >= 400 && error.status < 500) {
        const apiError = error.error as any;
        if (apiError?.error?.details?.length > 0) {
          toastService.error(apiError.error.details[0]);
        } else {
          toastService.error(error.message || 'Client error occurred.');
        }
      } else if (error.status >= 500) {
        toastService.error('Something went wrong. Please try again.');
      } else {
        toastService.error('An unexpected error occurred.');
      }

      return throwError(() => error);
    })
  );
};
