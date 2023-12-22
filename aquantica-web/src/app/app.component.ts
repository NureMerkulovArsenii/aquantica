import {Component, OnInit, ViewChild,} from '@angular/core';
import {MatSidenav} from '@angular/material/sidenav';
import {JwtHelperService} from "@auth0/angular-jwt";
import {AccountService} from "./@core/services/account.service";
import {ToastrService} from "ngx-toastr";
import {TranslateService} from "@ngx-translate/core";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'aquantica';
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  protected isAuthenticated!: boolean;


  constructor(
    private translateService: TranslateService,
    private readonly toastr: ToastrService,
    private accountService: AccountService,
    private jwtHelper: JwtHelperService) {

    translateService.addLangs(['en', 'ua']);
    translateService.setDefaultLang('en');
    translateService.use('en');
  }

}
