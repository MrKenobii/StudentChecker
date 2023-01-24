import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Company} from "../../interfaces/company/Company";

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  constructor(private httpClient: HttpClient) { }
  public getCompanies() : Observable<Company[]>{
    return this.httpClient.get<Company[]>("http://localhost:5269/Company")
  }
}
