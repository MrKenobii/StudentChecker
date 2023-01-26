import {Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student/student.service";
import {Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {AdminService } from "../../services/admin/admin.service";
import {lastValueFrom} from "rxjs";
import {AdminGetResponse} from "../../interfaces/admin/AdminGetResponse";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  student!: StudentResponse;
  recruiter!: RecruiterGetResponse;
  public isCollapsed = false;
  admin!: AdminGetResponse;

  constructor(private router: Router,private studentService: StudentService, private adminService: AdminService,
              private snackBar: MatSnackBar,
              private recruiterService: RecruiterService) {
  }
  ngOnInit(): void {
    if(localStorage.getItem("key") !== null){
      this.fetchAdminByToken(localStorage.getItem("key")!).then((adminRes: AdminGetResponse) => {
        if(adminRes){
          if(adminRes.token && adminRes.id && adminRes.name) {
            console.log(adminRes);
            this.admin = adminRes;
          }
        } else {
          this.fetchStudentByToken(localStorage.getItem("key")!).then((studentRes: StudentResponse) => {
            if(studentRes !== null && studentRes?.id !== null && studentRes?.name !== null){
              console.log(studentRes);
              this.student = studentRes;
              this.student.image = "data:image/png;base64," + this.student?.image
            } else {
              this.fetchRecruiterByToken(localStorage.getItem("key")!).then((_recruiter: RecruiterGetResponse) => {
                if(_recruiter.id !== null && _recruiter.name !== null){
                  console.log(_recruiter)
                  this.recruiter = _recruiter;
                  this.recruiter.image = "data:image/png;base64," + this.recruiter?.image
                }
              }).catch((error) => {
                console.log("Nothing found");
                this.router.navigate(['/not-found']);
              });
            }
          });
        }
      });
    }



  }

  logout() {
    localStorage.removeItem("key");
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    }
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate(['/login']).then(() => {
      window.location.reload();
    });
  }

  private async fetchAdminByToken(token: string){
    let adminByToken = this.adminService.getAdminByToken(token);
    return await lastValueFrom(adminByToken);
  }
  private async fetchStudentByToken(token: string){
    let studentByKey = this.studentService.getStudentByKey(token);
    return await lastValueFrom(studentByKey);
  }
  private async fetchRecruiterByToken(token: string){
    let recruiterByKey = this.recruiterService.getRecruiterByKey(token);
    return await lastValueFrom(recruiterByKey);
  }


  goToStudent(id: number) {
    // console.log(this.recruiter);
    // console.log(this.student);
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    }
    this.router.onSameUrlNavigation = 'reload';
    if(this.student.id !== null && this.student.name !== null){
      console.log("Inside students");
      this.router.navigate(['/profile/student/'+id]);
    }
  }
  goToRecruiter(id: number){
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    }
    this.router.onSameUrlNavigation = 'reload';
    if(this.recruiter.id !== null && this.recruiter.name !== null){
      console.log("Inside recruiters");
      this.router.navigate(['/profile/recruiter/'+id]);
    }
  }

  goToAdmin(id: number) {

  }
}
