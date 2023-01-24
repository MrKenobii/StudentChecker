import {Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student/student.service";
import {Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  student!: StudentResponse;
  recruiter!: RecruiterGetResponse;
  public isCollapsed = false;

  constructor(private router: Router,private studentService: StudentService,
              private snackBar: MatSnackBar,
              private recruiterService: RecruiterService) {
  }
  ngOnInit(): void {
    if(localStorage.getItem("key") !== null){
      this.studentService.
      getStudentByKey(localStorage.getItem("key")!).subscribe((data: StudentResponse) => {
          if(data !== null && data?.id !== null && data?.name !== null){
            console.log(data);
            this.student = data;
            this.student.image = "data:image/png;base64," + this.student?.image
          } else {
            this.recruiterService.getRecruiterByKey(localStorage.getItem("key")!)
              .subscribe((_recruiter: RecruiterGetResponse) => {
              if(_recruiter.id !== null && _recruiter.name !== null){
                console.log(_recruiter)
                this.recruiter = _recruiter;
                this.recruiter.image = "data:image/png;base64," + this.recruiter?.image
              } else {
                this.snackBar.open("Something went wrong. Refresh the page", "Ok", {
                  duration: 3000
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
}
