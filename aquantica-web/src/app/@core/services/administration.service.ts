import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {Observable} from "rxjs";
import {BaseResponse} from "../models/responses/base-response";

@Injectable({
  providedIn: 'root'
})
export class AdministrationService {

  private readonly baseUrl = environment.apiUrl + "/DataAdministration";

  constructor(
    private readonly http: HttpClient
  ) {
  }

  createBackup(): Observable<BaseResponse<null>> {
    return this.http.post<BaseResponse<null>>(this.baseUrl + "/backup", null);
  }
}
