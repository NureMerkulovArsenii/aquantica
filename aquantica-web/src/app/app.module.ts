import {NgModule} from '@angular/core';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatListModule} from "@angular/material/list";
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {ToastrModule} from "ngx-toastr";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {JwtInterceptor, JwtModule} from "@auth0/angular-jwt";
import {MatButtonToggleModule} from "@angular/material/button-toggle";
import {AuthGuard} from "./@core/guards/auth.guard";
import { RulesetListComponent } from './modules/ruleset/ruleset-list/ruleset-list.component';
import { RulesetDetailsComponent } from './modules/ruleset/ruleset-details/ruleset-details.component';

export function tokenGetter() {
  return localStorage.getItem("access_token");
}
@NgModule({
  declarations: [
    AppComponent,
    RulesetListComponent,
    RulesetDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatButtonModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7240"],
        disallowedRoutes: []
      }
    }),
    ToastrModule.forRoot(
      {
        timeOut: 4000,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
      }
    ),
    MatButtonToggleModule
  ],
  providers: [AuthGuard, /* {
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptor,
    multi: true
  }*/],
  bootstrap: [AppComponent]
})
export class AppModule {
}
