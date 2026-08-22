import {
  HttpInterceptorFn
} from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn =
  (req, next) => {

    const token =
      localStorage.getItem('token');

    if (!token) {
      return next(req);
    }

    // Public APIs
    if (
      req.url.includes('/api/auth') ||
      req.url.includes('/api/products')
    ) {
      return next(req);
    }

    const authRequest =
      req.clone({
        setHeaders: {
          Authorization:
            `Bearer ${token}`
        }
      });

    return next(authRequest);
  };