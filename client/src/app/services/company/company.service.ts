import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Company} from "../../interfaces/company/Company";
import {DeleteResponse} from "../../interfaces/DeleteResponse";

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  constructor(private httpClient: HttpClient) { }
  public getCompanies() : Observable<Company[]>{
    return this.httpClient.get<Company[]>("http://localhost:5269/Company")
  }

  deleteCompanyById(id: number) : Observable<DeleteResponse> {
    return this.httpClient.delete<DeleteResponse>(`http://localhost:5269/Company/${id}`);
  }

  updateCompany(id: number, obj: any) {
    return this.httpClient.put(`http://localhost:5269/Company/${id}`, obj);
  }

  getCompanyById(id: number) : Observable<Company> {
    return this.httpClient.get<Company>(`http://localhost:5269/Company/${id}`);
  }

  getCityByCompanyId(companyId: number) {
    return this.httpClient.get(`http://localhost:5269/Company/${companyId}/city`);
  }

  public addCompany(obj: any) {
    return this.httpClient.post(`http://localhost:5269/Company`, obj);
  }
}
