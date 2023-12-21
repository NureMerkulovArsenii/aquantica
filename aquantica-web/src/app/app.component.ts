import {Component, OnInit, ViewChild,} from '@angular/core';
import {MatSidenav} from '@angular/material/sidenav';
import {JwtHelperService} from "@auth0/angular-jwt";
import {AccountService} from "./@core/services/account.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'aquantica';
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  currentLanguage: string = 'en';
  protected isAuthenticated!: boolean;


  constructor(
    private readonly toastr: ToastrService,
    private accountService: AccountService,
    private jwtHelper: JwtHelperService) {
  }


  ngOnInit() {
    //this.isAuthenticated = true;
    //this.isUserAuthenticated();
  }

  isUserAuthenticated(): boolean {
    const token = localStorage.getItem('access_token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.isAuthenticated = true;
      return true;
    } else {
      this.accountService.refreshToken().subscribe({
        next: (response) => {
          if (response.isSuccess) {
            localStorage.setItem('access_token', response.data!.accessToken);
            this.isAuthenticated = true;
            return true;
          }
          this.isAuthenticated = false;
          return false;
        },
        error: (error) => {
          this.toastr.error(error.error.error);
          this.isAuthenticated = false;
          return false;
        }
      })
    }
    this.isAuthenticated = false;
    return false;

  }

  // isUserAuthenticated(): void {
  //   const token = localStorage.getItem('access_token');
  //   if (token && !this.jwtHelper.isTokenExpired(token)) {
  //     this.isAuthenticated = true;
  //     return;
  //   } else {
  //     this.accountService.refreshToken().subscribe({
  //       next: (response) => {
  //         if (response.isSuccess) {
  //           localStorage.setItem('access_token', response.data!.accessToken);
  //           this.isAuthenticated = true;
  //           return;
  //         }
  //         this.isAuthenticated = false;
  //         return;
  //       },
  //       error: (error) => {
  //         this.toastr.error(error.error.error);
  //         this.isAuthenticated = false;
  //         return;
  //       }
  //     })
  //   }
  //   this.isAuthenticated = false;
  // }

  changeLanguage = (lang: string): void => {
    this.currentLanguage = lang;
    //todo: implement

  }

}
