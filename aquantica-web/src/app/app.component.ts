import {BreakpointObserver} from '@angular/cdk/layout';
import {Component, ViewChild,} from '@angular/core';
import {MatSidenav} from '@angular/material/sidenav';
import {AccountService} from "./@core/services/account.service";
import {JwtHelperService} from "@auth0/angular-jwt";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'aquantica';
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  isMobile = true;
  isCollapsed = false;
  currentLanguage: string = 'en';


  constructor(private observer: BreakpointObserver,
              private jwtHelper: JwtHelperService)
  {
  }

  ngOnInit() {
    this.observer.observe(['(max-width: 800px)']).subscribe((screenSize) => {
      this.isMobile = screenSize.matches;
    });
  }

  toggleMenu() {
    if (this.isMobile) {
      this.sidenav.toggle();
      this.isCollapsed = false; // On mobile, the menu can never be collapsed
    } else {
      this.sidenav.open(); // On desktop/tablet, the menu can never be fully closed
      this.isCollapsed = !this.isCollapsed;
    }
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem('access_token');
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  changeLanguage = (lang: string): void => {
    this.currentLanguage = lang;
    //todo: implement

  }


  logout = (): void => {
    localStorage.removeItem('access_token');
  }
}
