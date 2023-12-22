import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {Observable} from "rxjs";
import {BaseResponse} from "../models/responses/base-response";
import {MenuItem} from "../models/menu-item";

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  baseUrl = environment.apiUrl + "/Menu";

  constructor(
    private readonly http: HttpClient
  ) {
  }

  getMenu(): Observable<BaseResponse<MenuItem[]>> {
    return this.http.get<BaseResponse<MenuItem[]>>(this.baseUrl + "/get");
  }


}
