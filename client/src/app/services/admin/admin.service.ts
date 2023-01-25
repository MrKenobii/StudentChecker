import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AdminLoginRequest} from "../../interfaces/admin/AdminLoginRequest";
import {Observable} from "rxjs";
import {AdminLoginResponse} from "../../interfaces/admin/AdminLoginResponse";
import {AdminGetResponse} from "../../interfaces/admin/AdminGetResponse";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isAdmin: boolean = false;

  // admin_user@admin.com -- 12345678
  constructor(private httpClient: HttpClient) {}
  public loginAsAdmin(adminLoginRequest: AdminLoginRequest): Observable<AdminLoginResponse>{
    let adminLoginResponseObservable = this.httpClient.post<AdminLoginResponse>(`http://localhost:5269/Admin/login`, adminLoginRequest);
    adminLoginResponseObservable.subscribe((data: AdminLoginResponse) => {
      if(data.key){
        this.setAdmin(true);
      }
    });
    return adminLoginResponseObservable;
  }
  public getAdmin(id: number) : Observable<AdminGetResponse>{
    return this.httpClient.get<AdminGetResponse>(`http://localhost:5269/Admin/${id}`);
  }
  isAdmin(){
    return this._isAdmin;
  }
  public setAdmin(flag: boolean){
    this._isAdmin = flag;
  }

  public getAdminByToken(token: string) : Observable<AdminGetResponse> {
    return this.httpClient.get<AdminGetResponse>(`http://localhost:5269/Admin/token/${token}`);
  }
}
