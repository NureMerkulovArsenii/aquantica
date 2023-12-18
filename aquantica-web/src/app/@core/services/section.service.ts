import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {Section} from "../models/section/section";
import {BaseResponse} from "../models/responses/base-response";
import {Observable} from "rxjs";
import {SectionDetails} from "../models/section/section-details";

@Injectable({
  providedIn: 'root'
})

export class SectionService {
  private baseUrl = environment.apiUrl + "/Section";

  constructor(private readonly http: HttpClient) {

  }

  getSections(): Promise<Section[]> {
    return new Promise<Section[]>((resolve, reject) => {
      this.http.get<BaseResponse<Section[]>>(this.baseUrl + "/all")
        .subscribe({
          next: (response) => {
            if (response.isSuccess)
              resolve(response.data!);
            else
              reject(response.error);
          },
          error: (error) => {
            reject(error);
          }
        });
    });
  }

  getSection(id: number): Observable<BaseResponse<SectionDetails>> {
    return this.http.get<BaseResponse<SectionDetails>>(this.baseUrl + "/" + id);
  }

  createSection(section: Section): Promise<Section> {
    return new Promise<Section>((resolve, reject) => {
      this.http.post<BaseResponse<Section>>(this.baseUrl + '/create', section)
        .subscribe({
          next: (response) => {
            if (response.isSuccess)
              resolve(response.data!);
            else
              reject(response.error);
          },
          error: (error) => {
            reject(error);
          }
        });
    });
  }

  updateSection(section: Section): Observable<BaseResponse<boolean>> {
    return this.http.put<BaseResponse<boolean>>(this.baseUrl + "/update", section);
  }

  deleteSection(id: number): Observable<BaseResponse<boolean>> {
    return this.http.delete<BaseResponse<boolean>>(this.baseUrl + "/delete/" + id);
  }
}
