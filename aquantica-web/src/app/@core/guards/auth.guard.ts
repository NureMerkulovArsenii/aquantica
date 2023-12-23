import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt';
import {AccountService} from "../services/account.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router,
              private jwtHelper: JwtHelperService,
              private accountService: AccountService
  ) {
  }

  //method is used to determine if a user can activate a route
  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    console.log("canActivate")
    const token = localStorage.getItem("access_token");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    } else {
      //check if we can recieve new token with refresh token
      this.accountService.refreshToken().subscribe({
        next: (response) => {
          if (response.isSuccess) {
            localStorage.setItem("access_token", response.data!.accessToken);
            return true;
          }
          this.router.navigate(["login"]);
          localStorage.removeItem("access_token");
          return false;
        },
        error: (error) => {
          console.log(error);
          return false;
        }
      });
    }
    return false;
  }

}
