import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { environment } from '../../environments/environment';
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService).snapshot;
  const api = environment.apiUrl;
  if (auth.token && req.url.startsWith(api)) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${auth.token}` } });
  }
  return next(req);
};
