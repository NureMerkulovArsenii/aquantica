import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class LocalizationInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let language = localStorage.getItem("language") ?? "en";
    if(language === "en") language = "en-US";
    if(language === "ua") language = "uk-UA";

    request = request.clone({
      setHeaders: {
        "Accept-Language": language
      }
    });
    return next.handle(request);
  }
}
