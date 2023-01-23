import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class CollegeService {

  constructor(private httpClient: HttpClient) { }
  public getColleges(){
    return this.httpClient.get("http://localhost:5269/College")
  }
  public getCollegeByExtension(extension: string){
    return this.httpClient.get(`http://localhost:5269/College/`); // ???
  }
}
