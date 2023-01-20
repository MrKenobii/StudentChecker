import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Course} from "../interfaces/course/Course";

@Injectable({
  providedIn: 'root'
})
export class CoursesService {

  constructor(private httpClient: HttpClient) { }
  public getCourses(): Observable<Course[]>{
    return this.httpClient.get<Course[]>("http://localhost:5269/Course");
  }
}
