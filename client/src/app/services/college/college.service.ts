import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";

@Injectable({
  providedIn: 'root'
})
export class CollegeService {

  constructor(private httpClient: HttpClient) { }
  public getColleges() : Observable<CollegeGetResponse[]>{
    return this.httpClient.get<CollegeGetResponse[]>("http://localhost:5269/College")
  }
  public getCollegeByExtension(extension: string) : Observable<CollegeGetResponse>{
    return this.httpClient.get<CollegeGetResponse>(`http://localhost:5269/College/`); // ???
  }
}
