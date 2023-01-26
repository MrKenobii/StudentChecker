import { Injectable } from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import { Observable } from 'rxjs';
import {AdminService} from "../services/admin/admin.service";
import {StudentService} from "../services/student/student.service";
import {RecruiterService} from "../services/recruiter/recruiter-service.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if(localStorage.getItem("key")){
      this.router.navigate(['/forbidden']);
      return false;
    }
    else {
      return true;
    }

  }

}
