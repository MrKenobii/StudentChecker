import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {StudentLoginRequest} from "../../interfaces/student/login/StudentLoginRequest";
import {StudentLoginResponse} from "../../interfaces/student/login/StudentLoginResponse";
import {MatSnackBar} from "@angular/material/snack-bar";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterPostLoginResponse} from "../../interfaces/recruiter/login/RecruiterPostLoginResponse";
import {AdminService} from "../../services/admin/admin.service";
import {AdminLoginResponse} from "../../interfaces/admin/AdminLoginResponse";
import {AdminLoginRequest} from "../../interfaces/admin/AdminLoginRequest";
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  createPostForm!: FormGroup;
  postPayload: StudentLoginRequest;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private studentService: StudentService,
              private adminService: AdminService,
              private snackBar: MatSnackBar,
              private recruiterService: RecruiterService) {
    this.postPayload = {
      email: '',
      password: ''
    };
  }
  ngOnInit(){
    this.createPostForm = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }
  login() {
    this.postPayload.email = this.createPostForm.get('email')!.value;
    this.postPayload.password = this.createPostForm.get('password')!.value;
    const obj: AdminLoginRequest = {
      email: this.createPostForm.get('email')!.value,
      password: this.createPostForm.get('password')!.value
    }
    if((obj.email == null || obj.email == "") || (obj.password == null || obj.password == "")){
      this.snackBar.open("Please fill all the fields", "OK", {
        duration: 4000
      });
    } else {
      this.adminService.loginAsAdmin(obj).subscribe((data: AdminLoginResponse) => {
        console.log(data);
        if(data.key){
          localStorage.clear();
          console.log("In here");
          localStorage.setItem("key", data.key);
          this.adminService.setAdmin(true);
          this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
          }
          this.router.onSameUrlNavigation = 'reload';
          this.router.navigate(['/admin']).then(() => {
            window.location.reload();
          });
        } else {
          this.studentService.login(this.postPayload).subscribe((data: StudentLoginResponse) => {
            console.log(data);
            if (data.key !== null) {
              this.snackBar.open(data.message, "Dismiss", {
                duration: 3000
              });
              localStorage.clear();
              localStorage.setItem("key", (data.key));
              console.log(data.key);
              this.router.routeReuseStrategy.shouldReuseRoute = function () {
                return false;
              }
              this.router.onSameUrlNavigation = 'reload';
              this.router.navigate(['/']).then(() => {
                window.location.reload();
              });
            } else {
              this.recruiterService.login(this.postPayload).subscribe((_data: RecruiterPostLoginResponse) => {
                console.log(_data);
                if (_data.key !== null) {
                  console.log(_data);
                  this.snackBar.open(_data.message, "Dismiss", {
                    duration: 3000
                  });
                  localStorage.setItem("key", _data.key);
                  console.log(_data.key);
                  this.router.routeReuseStrategy.shouldReuseRoute = function () {
                    return false;
                  }
                  this.router.onSameUrlNavigation = 'reload';
                  this.router.navigate(['/companies']).then(() => {
                    window.location.reload();
                  });
                } else {
                  this.snackBar.open(`Invalid credentials`, "Ok", {
                    duration: 5000
                  })
                }

              });
            }

          });
        }
      });
    }





  }
}
