import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {StudentSignupRequest} from "../../interfaces/student/signup/StudentSignupRequest";
import {StudentSignupResponse} from "../../interfaces/student/signup/StudentSignupResponse";
import {Observable} from "rxjs";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {StudentLoginRequest} from "../../interfaces/student/login/StudentLoginRequest";
import {StudentLoginResponse} from "../../interfaces/student/login/StudentLoginResponse";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  constructor(private http: HttpClient) {

  }
  public getStudents(){
    console.log("Inside getStudents()");
    return this.http.get("http://localhost:5269/Student");
  }
  public getStudentById(id: number) : Observable<StudentResponse>{
    return this.http.get<StudentResponse>(`http://localhost:5269/Student/${id}`);
  }
  public getCityByStudent(id: number){
    return this.http.get(`http://localhost:5269/Student/${id}/city`);
  }
  public getCollegeByStudent(id: number){
    return this.http.get(`http://localhost:5269/Student/${id}/college`);
  }
  public getCoursesByStudent(id: number){
    return this.http.get(`http://localhost:5269/Student/${id}/courses`);
  }
  public addCourse(id: number, courses: any){
    return this.http.put(`http://localhost:5269/Student/${id}/add-course`, courses);
  }
  public updateProfile(id: number, payload: any){
    return this.http.put(`http://localhost:5269/Student/${id}/update-profile`, payload);
  }
  public login(payload: StudentLoginRequest) : Observable<StudentLoginResponse>{
    return this.http.post<StudentLoginResponse>(`http://localhost:5269/Student/login`, payload);
  }
  public signUp(payload: StudentSignupRequest) : Observable<StudentSignupResponse>{
     console.log(payload);
     return this.http.post<StudentSignupResponse>(`http://localhost:5269/Student/sign-up`, payload);
  }
  public deleteStudent(id: number){
    return this.http.delete(`http://localhost:5269/Student/${id}`);
  }
  public getStudentByKey(key: string) : Observable<StudentResponse>{
    return this.http.get<StudentResponse>(`http://localhost:5269/Student/key/${key}`)
  }
  public getTokenByStudentId(id: number) : Observable<StudentGetTokenResponse> {
    return this.http.get<StudentGetTokenResponse>(`http://localhost:5269/Student/${id}/token`);
  }
  public updateStudent(id: number, payload: any){
    return this.http.put(`http://localhost:5269/Student/${id}`, payload);
  }
  public createStudent( payload: any){
    return this.http.post(`http://localhost:5269/Student`, payload);
  }
  public verifyAccount(studentId: number, payload: VerifyAccountRequest) : Observable<VerifyTokenResponse> {
    return this.http.post<VerifyTokenResponse>(`http://localhost:5269/Student/${studentId}/verify-account`, payload);
  }
}
