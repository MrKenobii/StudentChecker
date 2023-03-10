import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {RecruiterPostSignupRequest} from "../../interfaces/recruiter/signup/RecruiterPostSignupRequest";
import {RecruiterPostSignupResponse} from "../../interfaces/recruiter/signup/RecruiterPostSignupResponse";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {RecruiterPostLoginRequest} from "../../interfaces/recruiter/login/RecruiterPostLoginRequest";
import {RecruiterPostLoginResponse} from "../../interfaces/recruiter/login/RecruiterPostLoginResponse";
import {RecruiterGetKeyResponse} from "../../interfaces/recruiter/RecruiterGetKeyResponse";
import {UpdatePassword} from "../../interfaces/UpdatePassword";
import {UpdatePasswordResponse} from "../../interfaces/UpdatePasswordResponse";
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {AdminActivateAccountResponse} from "../../interfaces/admin/AdminActivateAccountResponse";
import {DeleteResponse} from "../../interfaces/DeleteResponse";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";
import {GetRandomRecruiters} from "../../interfaces/recruiter/GetRandomRecruiters";

interface CompanyDto {
  id: number,
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  cityName: string;
  companyKey: string;
}

@Injectable({
  providedIn: 'root'
})
export class RecruiterService {

  constructor(private httpClient: HttpClient) { }

  public signUp(payload: RecruiterPostSignupRequest) : Observable<RecruiterPostSignupResponse> {
   return this.httpClient.post<RecruiterPostSignupResponse>(`http://localhost:5269/Recruiter/sign-up`, payload);
  }
  public getRecruiterById(id: number) : Observable<RecruiterGetResponse> {
    return this.httpClient.get<RecruiterGetResponse>(`http://localhost:5269/Recruiter/${id}`);
  }
  public update(id: number, obj: any) : Observable<any> {
    return this.httpClient.put<any>(`http://localhost:5269/Recruiter/${id}`, obj);
  }
  public updateProfile(id: number, obj: any) : Observable<any> {
    return this.httpClient.put<any>(`http://localhost:5269/Recruiter/${id}/update-profile`, obj);
  }
  public editProfile(id: number, obj: any) : Observable<any> {
    return this.httpClient.put<any>(`http://localhost:5269/Recruiter/${id}/edit-profile`, obj);
  }

  public login(payload: RecruiterPostLoginRequest) : Observable<RecruiterPostLoginResponse> {
    return this.httpClient.post<RecruiterPostLoginResponse>(`http://localhost:5269/Recruiter/login`, payload);

  }

  public getRecruiterByKey(key: string) : Observable<RecruiterGetResponse>{
    return this.httpClient.get<RecruiterGetResponse>(`http://localhost:5269/Recruiter/${key}/token`);
  }

  public getTokenByRecruiterId(recruiterId: number) : Observable<RecruiterGetKeyResponse> {
    return this.httpClient.get<RecruiterGetKeyResponse>(`http://localhost:5269/Recruiter/token/${recruiterId}`);
  }

  public getCompanies(recruiterId: number) : Observable<CompanyDto[]> {
    return this.httpClient.get<CompanyDto[]>(`http://localhost:5269/Recruiter/${recruiterId}/companies`);
  }

  public changePassword(recruiterId: number, obj: UpdatePassword) :Observable<UpdatePasswordResponse> {
    return this.httpClient.put<UpdatePasswordResponse>(`http://localhost:5269/Recruiter/${recruiterId}/change-password`, obj);
  }

  verifyAccount(recruiterId: number, requestPayload: VerifyAccountRequest) {
    return this.httpClient.post<VerifyTokenResponse>(`http://localhost:5269/Recruiter/${recruiterId}/verify-account`, requestPayload);
  }

  public getRecruiters() : Observable<RecruiterGetResponse[]> {
    return this.httpClient.get<RecruiterGetResponse[]>(`http://localhost:5269/Recruiter`);
  }



  confirmRecruiter(id: number) : Observable<AdminActivateAccountResponse> {
    return this.httpClient.post<AdminActivateAccountResponse>(`http://localhost:5269/Admin/activate-recruiter/${id}`, null);
  }

  deleteRecruiter(id: number) : Observable<DeleteResponse> {
    const url = `http://localhost:5269/Recruiter/${id}`;
    console.log(url)
    return this.httpClient.delete<DeleteResponse>(url);
  }

  public addRecruiter(obj: any) {
    return this.httpClient.post<AdminActivateAccountResponse>(`http://localhost:5269/Recruiter`, obj);
  }

  public getRandomRecruiters(id: number) : Observable<GetRandomRecruiters[]>{
    return this.httpClient.get<GetRandomRecruiters[]>(`http://localhost:5269/Recruiter/get-random/${id}`);
  }
}
