import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  constructor(private httpClient: HttpClient) { }
  public getCompanies(){
    return this.httpClient.get("http://localhost:5269/Company")
  }
}
