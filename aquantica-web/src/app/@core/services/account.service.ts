import {Injectable} from '@angular/core';
import {environment} from "../../../environments/environment";
import {LoginModel} from "../models/login-model";
import {BaseResponse} from "../models/responses/base-response";
import {AuthResponse} from "../models/responses/auth-response";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class AccountService {

  private readonly baseUrl = environment.apiUrl + "/account";

  constructor(private readonly http: HttpClient) {
  }

  login(loginRequest: LoginModel): Observable<BaseResponse<AuthResponse>> {
    return this.http.post<BaseResponse<AuthResponse>>(this.baseUrl + "/login", loginRequest);
  }

  refreshToken()
    :
    Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.post<BaseResponse<AuthResponse>>(this.baseUrl + "/refresh", {})
        .subscribe({
          next: (response) => {
            if (response.isSuccess) {
              localStorage.setItem("access_token", response.data?.accessToken ?? "");
              resolve(true);
            } else {
              reject(response.error);
            }
          },
          error: (error) => {
            reject(error);
          }
        });
    });
  }
}
