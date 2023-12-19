import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Ruleset} from "../models/ruleset/ruleset";
import {BaseResponse} from "../models/responses/base-response";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class RulesetService {
  private readonly baseUrl = environment.apiUrl + "/Ruleset";

  constructor(
    private readonly http: HttpClient
  ) {
  }

  getAll(): Observable<BaseResponse<Ruleset[]>> {
    return this.http.get<BaseResponse<Ruleset[]>>(this.baseUrl + "/all");
  }

  get(id: number): Observable<BaseResponse<Ruleset>> {
    return this.http.get<BaseResponse<Ruleset>>(this.baseUrl + "/" + id);
  }

  create(ruleset: Ruleset): Observable<BaseResponse<boolean>> {
    return this.http.post<BaseResponse<boolean>>(this.baseUrl + "/create", ruleset);
  }

  update(ruleset: Ruleset): Observable<BaseResponse<boolean>> {
    return this.http.put<BaseResponse<boolean>>(this.baseUrl + "/update", ruleset);
  }

  delete(id: number): Observable<BaseResponse<boolean>> {
    return this.http.delete<BaseResponse<boolean>>(this.baseUrl + "/delete/" + id);
  }
}

