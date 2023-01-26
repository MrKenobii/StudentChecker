import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {AdminLoginRequest} from "../../interfaces/admin/AdminLoginRequest";
import {lastValueFrom, Observable} from "rxjs";
import {AdminLoginResponse} from "../../interfaces/admin/AdminLoginResponse";
import {AdminGetResponse} from "../../interfaces/admin/AdminGetResponse";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isAdmin!: boolean;

  // admin_user@admin.com -- 12345678
  constructor(private httpClient: HttpClient) {}
  public loginAsAdmin(adminLoginRequest: AdminLoginRequest): Observable<AdminLoginResponse>{
    return this.httpClient.post<AdminLoginResponse>(`http://localhost:5269/Admin/login`, adminLoginRequest);
  }
  public getAdmin(id: number) : Observable<AdminGetResponse>{
    return this.httpClient.get<AdminGetResponse>(`http://localhost:5269/Admin/${id}`);
  }
  private async fetchAdminByToken(){
    let adminByToken = this.getAdminByToken(localStorage.getItem("key")!);
    return await lastValueFrom(adminByToken);
  }
  public isLoggedIn(){
    if(localStorage.getItem("key")){
      return true;
    } else return false;
  }
  public async isAdmin(): Promise<boolean> {
    var isAdmin!: boolean;
    this.fetchAdminByToken().then((data: AdminGetResponse) => {
      if(data.id && data.token && data.name == "AdminUser"){
        console.log("ASDASDASDASDSASDASDASAS");
        isAdmin = true;
      } else isAdmin = false;
    }).then(() => console.log(isAdmin));
    return isAdmin;

  }
  public setAdmin(flag: boolean){
    this._isAdmin = flag;
  }

  public getAdminByToken(token: string) : Observable<AdminGetResponse> {
    return this.httpClient.get<AdminGetResponse>(`http://localhost:5269/Admin/token/${token}`);
  }
}
