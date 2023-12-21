import {Injectable} from '@angular/core';
import {environment} from "../../../environments/environment";
import {LoginModel} from "../models/login-model";
import {BaseResponse} from "../models/responses/base-response";
import {AuthResponse} from "../models/responses/auth-response";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserUpdate} from "../models/user/user-update";

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

  refreshToken(): Observable<BaseResponse<AuthResponse>> {
    return this.http.get<BaseResponse<AuthResponse>>(this.baseUrl + "/refresh");
  }

  getAll(): Observable<BaseResponse<UserUpdate[]>> {
    return this.http.get<BaseResponse<UserUpdate[]>>(this.baseUrl + "/get-all-users");
  }

  getUser(id: number): Observable<BaseResponse<UserUpdate>> {
    return this.http.get<BaseResponse<UserUpdate>>(this.baseUrl + "/user/" + id);
  }

  updateUser(user: UserUpdate): Observable<BaseResponse<boolean>> {
    let request = {
      id: user.id,
      email: user.email,
      phoneNumber: user.phoneNumber,
      firstName: user.firstName,
      lastName: user.lastName,
      password: user.password ?? "",
      isEnabled: user.isEnabled,
      isBlocked: user.isBlocked,
      roleId: user.role?.id,
    }
    return this.http.put<BaseResponse<boolean>>(this.baseUrl + "/update-user", request);
  }

  register(user: UserUpdate): Observable<BaseResponse<boolean>> {
    return this.http.post<BaseResponse<boolean>>(this.baseUrl + "/register", user);
  }
}
