import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {lastValueFrom, Observable} from 'rxjs';
import {AdminService} from "../services/admin/admin.service";
import {AdminGetResponse} from "../interfaces/admin/AdminGetResponse";

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private router: Router, private adminService: AdminService) {
    this.adminService.getAdminByToken(localStorage.getItem("key")!).subscribe((data: AdminGetResponse) => {
      if(data.token && data.id && data.name == 'AdminUser'){
        console.log("Admin");
        this.adminService.setAdmin(true);
      } else {
        console.log("NOT ADMINNN")
        this.adminService.setAdmin(false);
      }
    });
  }
  private  getAdminByToken(token: string){
    let adminByToken = this.adminService.getAdminByToken(token);
    return  lastValueFrom(adminByToken);
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise((resolve, reject) => {
      console.log(this.adminService.isAdmin());
      resolve(this.adminService.isAdmin());
      this.adminService.isAdmin();
    });

  }

}
