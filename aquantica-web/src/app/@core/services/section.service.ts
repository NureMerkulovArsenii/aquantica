import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {Section} from "../models/section/section";
import {BaseResponse} from "../models/responses/base-response";
import {Observable} from "rxjs";
import {SectionDetails} from "../models/section/section-details";
import {SectionType} from "../models/section/section-type";

@Injectable({
  providedIn: 'root'
})

export class SectionService {
  private baseUrl = environment.apiUrl + "/Section";

  constructor(private readonly http: HttpClient) {

  }

  getSections(): Observable<BaseResponse<Section[]>> {
    return this.http.get<BaseResponse<Section[]>>(this.baseUrl + "/all");
  }

  getSection(id: number): Observable<BaseResponse<SectionDetails>> {
    return this.http.get<BaseResponse<SectionDetails>>(this.baseUrl + "/" + id);
  }

  getSectionTypes(): Observable<BaseResponse<SectionType[]>> {
    return this.http.get<BaseResponse<SectionType[]>>(this.baseUrl + "/types");
  }

  createSection(section: Section): Observable<BaseResponse<boolean>> {
    return this.http.post<BaseResponse<boolean>>(this.baseUrl + "/create", section);
  }

  updateSection(section: Section): Observable<BaseResponse<boolean>> {
    return this.http.put<BaseResponse<boolean>>(this.baseUrl + "/update", section);
  }

  deleteSection(id: number): Observable<BaseResponse<boolean>> {
    return this.http.delete<BaseResponse<boolean>>(this.baseUrl + "/delete/" + id);
  }
}
