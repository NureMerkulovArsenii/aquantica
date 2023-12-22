import {Component, OnInit, ViewChild} from '@angular/core';
import {MatSidenav} from "@angular/material/sidenav";
import {BreakpointObserver} from "@angular/cdk/layout";
import {JwtHelperService} from "@auth0/angular-jwt";
import {AccountService} from "../../@core/services/account.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-app-shell',
  templateUrl: './app-shell.component.html',
  styleUrls: ['./app-shell.component.scss']
})
export class AppShellComponent implements OnInit {
  title = 'aquantica';
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  isMobile = true;
  isCollapsed = false;
  currentLanguage: string = 'en';
  isAuthenticated: boolean = false;


  constructor(private observer: BreakpointObserver,
              private readonly accountService: AccountService,
              private readonly router: Router,
              private jwtHelper: JwtHelperService) {
  }

  ngOnInit() {
    console.log("app-shell")
    this.observer.observe(['(max-width: 800px)']).subscribe((screenSize) => {
      this.isMobile = screenSize.matches;
    });


    ///this.isUserAuthenticated();

    this.isUserAuthenticated();
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
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.isAuthenticated = true;
      return true;
    }

    this.isAuthenticated = false;
    return false;

    //this.router.navigate(['/login']);

  }

  // isUserAuthenticated = (): void => {
  //
  // }

  // isUserAuthenticated = (): boolean => {
  //   try {
  //     const token = localStorage.getItem('access_token');
  //     if (token && !this.jwtHelper.isTokenExpired(token)) {
  //       return true;
  //     }
  //
  //     this.accountService.refreshToken().subscribe({
  //       next: (response) => {
  //         if (response.isSuccess) {
  //           localStorage.setItem('access_token', response.data!.accessToken);
  //           return true;
  //         }
  //         this.router.navigate(['/login']);
  //         localStorage.removeItem('access_token');
  //       },
  //       error: (error) => {
  //         console.log(error)
  //         this.router.navigate(['/login']);
  //         localStorage.removeItem('access_token');
  //       }
  //     });
  //
  //     localStorage.removeItem('access_token');
  //     return false;
  //   } catch (error) {
  //     console.log(error)
  //     return false;
  //   }
  // }


  changeLanguage = (lang: string): void => {
    this.currentLanguage = lang;
    //todo: implement

  }

  logout = (): void => {
    localStorage.removeItem('access_token');
  }

}
