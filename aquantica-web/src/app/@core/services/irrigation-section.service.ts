import {Injectable} from '@angular/core';
import {BaseService} from "./base.service";
import {HttpClient} from "@angular/common/http";
import {IrrigationSection} from "../models/irrigation-section";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class IrrigationSectionService extends BaseService {

  private readonly baseUrl = environment.apiUrl + '/Section';

  constructor(http: HttpClient) {
    super(http);
  }

  public getAllIrrigationSections(): Observable<IrrigationSection[]> {
    return this.getAll<IrrigationSection>(this.baseUrl + '/all');
  }

  public getIrrigationSectionById(id: number): any {
    return this.getById<IrrigationSection>(this.baseUrl, id.toString());
  }

}

