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

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // console.log("canActivate")
    // const token = localStorage.getItem("access_token");
    // if (token && !this.jwtHelper.isTokenExpired(token)) {
    //   return true;
    // } else {
    //   let isRefreshed = await this.accountService.refreshToken();
    //   if (isRefreshed) {
    //     return true;
    //   }
    // }
    // await this.router.navigate(["login"]);
    // return false;

    return true;
  }
}
