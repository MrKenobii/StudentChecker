import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DeleteResponse} from "../../interfaces/DeleteResponse";
import {City} from "../../interfaces/city/City";
import {Observable} from "rxjs";
import {CityGetResponse} from "../../interfaces/city/CityGetResponse";

@Injectable({
  providedIn: 'root'
})
export class CityService {

  constructor(private http: HttpClient) { }

  public getCities() : Observable<CityGetResponse[]>{
    return this.http.get<CityGetResponse[]>(`http://localhost:5269/City`);
  }
}
