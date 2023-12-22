import {Component, OnInit, ViewChild} from '@angular/core';
import {MatSidenav} from "@angular/material/sidenav";
import {BreakpointObserver} from "@angular/cdk/layout";
import {JwtHelperService} from "@auth0/angular-jwt";
import {Router} from "@angular/router";
import {TranslateService} from "@ngx-translate/core";
import {MenuService} from "../../@core/services/menu.service";
import {MenuItem} from "../../@core/models/menu-item";

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
  currentLanguage: string = '';
  isAuthenticated: boolean = false;
  menuItems: MenuItem[] = [];


  constructor(private observer: BreakpointObserver,
              public translate: TranslateService,
              private readonly menuService: MenuService,
              private readonly router: Router,
              private jwtHelper: JwtHelperService) {
  }

  ngOnInit() {
    console.log("app-shell")
    this.observer.observe(['(max-width: 800px)']).subscribe((screenSize) => {
      this.isMobile = screenSize.matches;
    });
    this.currentLanguage = localStorage.getItem('language') || 'en';

    this.isUserAuthenticated();
    this.getMenuItems();
    this.router.navigate(['/sections']);
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
  }

  getMenuItems(): void {
    this.menuService.getMenu().subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.menuItems = response.data!;
        }
      },
      error: (error) => {
        console.log(error)
      }
    })
  }

  changeLanguage = (lang: string): void => {
    console.log('change language')
    this.currentLanguage = lang;
    this.translate.use(lang);
    localStorage.setItem('language', lang);
    //window.location.reload();
    console.log(lang)
  }

  getCurrentUserName(): string {
    const token = localStorage.getItem('access_token');
    if (token) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return decodedToken['name'];
    }
    return '';
  }

  logout = (): void => {
    localStorage.removeItem('access_token');
    window.location.reload();
  }

}
