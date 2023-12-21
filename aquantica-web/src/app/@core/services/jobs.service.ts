import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {Observable} from "rxjs";
import {BaseResponse} from "../models/responses/base-response";
import {Job} from "../models/job-control/job";
import {JobDetailed} from "../models/job-control/job-detailed";

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  private readonly baseUrl = environment.apiUrl + "/JobControls";

  constructor(
    private readonly http: HttpClient
  ) {
  }

  getAllJobs(): Observable<BaseResponse<Job[]>> {
    return this.http.get<BaseResponse<Job[]>>(this.baseUrl + "/all");
  }

  getJob(id: number): Observable<BaseResponse<JobDetailed>> {
    return this.http.get<BaseResponse<JobDetailed>>(this.baseUrl + "/" + id);
  }

  createJob(job: Job): Observable<BaseResponse<boolean>> {
    return this.http.post<BaseResponse<boolean>>(this.baseUrl + "/create", job);
  }

  updateJob(job: Job): Observable<BaseResponse<boolean>> {
    return this.http.put<BaseResponse<boolean>>(this.baseUrl + "/update", job);
  }

  deleteJob(id: number): Observable<BaseResponse<boolean>> {
    return this.http.delete<BaseResponse<boolean>>(this.baseUrl + "/delete/" + id);
  }


  startJob(id: number): Observable<BaseResponse<boolean>> {
    return this.http.get<BaseResponse<boolean>>(this.baseUrl + "/start/" + id);
  }

  stopJob(id: number): Observable<BaseResponse<boolean>> {
    return this.http.get<BaseResponse<boolean>>(this.baseUrl + "/stop/" + id);
  }

  startAllJobs(): Observable<BaseResponse<boolean>> {
    return this.http.get<BaseResponse<boolean>>(this.baseUrl + "/start-all");
  }

  stopAllJobs(): Observable<BaseResponse<boolean>> {
    return this.http.get<BaseResponse<boolean>>(this.baseUrl + "/stop-all");
  }

  fireJobAsMethod(id: number): Observable<BaseResponse<boolean>> {
    return this.http.get<BaseResponse<boolean>>(this.baseUrl + "/fire-method/" + id);
  }

  triggerJob(id: number): Observable<BaseResponse<boolean>> {
    return this.http.get<BaseResponse<boolean>>(this.baseUrl + "/trigger/" + id);
  }


}
