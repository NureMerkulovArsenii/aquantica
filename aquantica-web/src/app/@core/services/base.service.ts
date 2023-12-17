import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {Observable} from "rxjs";
import {ApiResponse} from "../contracts/api-response";

@Injectable({
  providedIn: 'root'
})

export class BaseService {
  constructor(protected http: HttpClient) { }

  public getAll<T>(url: string, queryParam?: any): Observable<T[]> {
    let params = new HttpParams();
    if (queryParam) {
      Object.keys(queryParam).forEach((key) => {
        params = params.append(key, queryParam[key]);
      });
    }
    return this.http.get<T[]>(`${url}`, { params });
  }

  public getById<T>(url: string, id: string): Observable<ApiResponse<T>> {
    return this.http.get<ApiResponse<T>>(`${url}/${id}`);
  }

  public create<T>(url: string, data: any = null): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(`${url}`, data);
  }

  public update<T>(url: string, id: string, item: T): Observable<T> {
    return this.http.put<T>(`${url}/${id}`, item);
  }

  public delete<T>(url: string, id: string): Observable<any> {
    return this.http.delete(`${url}/${id}`);
  }
}
