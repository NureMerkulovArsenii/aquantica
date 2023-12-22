import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CoreRoutingModule} from './core-routing.module';
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {JwtInterceptor} from "@auth0/angular-jwt";
import {SharedModule} from "../shared/shared.module";
import {LocalizationInterceptor} from "./interceptors/localization.interceptor";


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    CoreRoutingModule,
    HttpClientModule,
    SharedModule,
  ],
  exports: [],
  providers: [
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: JwtInterceptor,
    //   multi: true
    // },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LocalizationInterceptor,
      multi: true
    }
  ]
})

export class CoreModule {
}
