import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {Observable} from "rxjs";
import {BaseResponse} from "../models/responses/base-response";
import {Role} from "../models/role/role";
import {RoleDetailed} from "../models/role/role-detailed";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private readonly baseurl: string = environment.apiUrl + "/Role";

  constructor(private readonly http: HttpClient) {
  }

  getRoles(): Observable<BaseResponse<Role[]>> {
    return this.http.get<BaseResponse<Role[]>>(this.baseurl + "/all");
  }

  getRole(id: number): Observable<BaseResponse<RoleDetailed>> {
    return this.http.get<BaseResponse<RoleDetailed>>(this.baseurl + "/" + id);
  }

  createRole(role: RoleDetailed): Observable<BaseResponse<boolean>> {
    return this.http.post<BaseResponse<boolean>>(this.baseurl  + '/create', role);
  }

  updateRole(role: RoleDetailed): Observable<BaseResponse<boolean>> {
    return this.http.put<BaseResponse<boolean>>(this.baseurl + '/update', role);
  }

  deleteRole(id: number): Observable<BaseResponse<boolean>> {
    return this.http.delete<BaseResponse<boolean>>(this.baseurl + "/delete" + id);
  }

}
