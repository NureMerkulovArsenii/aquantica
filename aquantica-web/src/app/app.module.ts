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
import {FormsModule, NgModel, ReactiveFormsModule} from '@angular/forms';
import {ToastrModule} from "ngx-toastr";
import {HttpClientModule} from "@angular/common/http";
import {JwtModule} from "@auth0/angular-jwt";
import {MatButtonToggleModule} from "@angular/material/button-toggle";
import {AuthGuard} from "./@core/guards/auth.guard";
import {RulesetListComponent} from './modules/ruleset/ruleset-list/ruleset-list.component';
import {RulesetDetailsComponent} from './modules/ruleset/ruleset-details/ruleset-details.component';
import {MatCardModule} from "@angular/material/card";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatOptionModule} from "@angular/material/core";
import {MatSelectModule} from "@angular/material/select";
import {MatSlideToggleModule} from "@angular/material/slide-toggle";
import {MatTableModule} from "@angular/material/table";
import {MAT_DIALOG_DEFAULT_OPTIONS, MatDialogModule} from "@angular/material/dialog";
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import {AccountModule} from "./modules/account/account.module";
import {SharedModule} from "./shared/shared.module";
import { UserListComponent } from './modules/user/user-list/user-list.component';
import { UserDetailedComponent } from './modules/user/user-detailed/user-detailed.component';

export function tokenGetter() {
  return localStorage.getItem("access_token");
}

@NgModule({
  declarations: [
    AppComponent,
    RulesetListComponent,
    RulesetDetailsComponent,
    ConfirmDialogComponent,
    UserListComponent,
    UserDetailedComponent
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
    MatButtonToggleModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatOptionModule,
    MatSlideToggleModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatDialogModule,
    AccountModule,
    SharedModule,
    MatSelectModule
  ],
  providers: [AuthGuard,
    {provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: {hasBackdrop: false, /*width: '80vw', height: '80vh'*/}}
    /* {
    provide: HTTP_INTERCEPTORS,
    useClass: JwtInterceptor,
    multi: true
  }*/],

  bootstrap: [AppComponent]
})
export class AppModule {
}
