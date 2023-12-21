import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {AccessAction} from "../models/access-action/access-action";
import {BaseResponse} from "../models/responses/base-response";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AccessActionService {

  private readonly baseUrl = environment.apiUrl + "/AccessActions";

  constructor(
    private readonly http: HttpClient
  ) {
  }

  getAccessActions(): Observable<BaseResponse<AccessAction[]>> {
    return this.http.get<BaseResponse<AccessAction[]>>(this.baseUrl + '/all');
  }

  getAccessAction(id: number): Observable<BaseResponse<AccessAction>> {
    return this.http.get<BaseResponse<AccessAction>>(this.baseUrl + '/' + id);
  }

  createAccessAction(accessAction: AccessAction): Observable<BaseResponse<boolean>> {
    return this.http.post<BaseResponse<boolean>>(this.baseUrl + '/create', accessAction);
  }

  updateAccessAction(accessAction: AccessAction): Observable<BaseResponse<boolean>> {
    return this.http.put<BaseResponse<boolean>>(this.baseUrl + '/update', accessAction);
  }

  deleteAccessAction(id: number): Observable<BaseResponse<boolean>> {
    return this.http.delete<BaseResponse<boolean>>(this.baseUrl + '/delete/' + id);
  }


}
