import {BreakpointObserver} from '@angular/cdk/layout';
import {Component, OnInit, ViewChild,} from '@angular/core';
import {MatSidenav} from '@angular/material/sidenav';
import {JwtHelperService} from "@auth0/angular-jwt";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent  {
  title = 'aquantica';
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  isMobile = true;
  currentLanguage: string = 'en';


  constructor(
    private observer: BreakpointObserver,
    private jwtHelper: JwtHelperService) {
  }

  ngOnInit() {
    // this.observer.observe(['(max-width: 800px)']).subscribe((screenSize) => {
    //   this.isMobile = screenSize.matches;
    // });
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem('access_token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

  changeLanguage = (lang: string): void => {
    this.currentLanguage = lang;
    //todo: implement

  }

}
