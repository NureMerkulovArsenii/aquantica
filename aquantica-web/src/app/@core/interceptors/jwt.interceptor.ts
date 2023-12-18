import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // console.log('intercepted request ... ');
    // const token = localStorage.getItem('access_token');
    //
    // if (token) {
    //   request = request.clone({
    //     setHeaders: {
    //       'Authorization': `Bearer ${token}`,
    //     },
    //   });
    // }
    //
    // console.log('Sending request with new header now ...')

    return next.handle(request);
  }
}
