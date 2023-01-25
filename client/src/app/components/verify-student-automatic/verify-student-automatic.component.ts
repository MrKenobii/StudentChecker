import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {lastValueFrom} from "rxjs";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {Student} from "../../interfaces/student/Student";
import {StudentResponse} from "../../interfaces/student/StudentResponse";

@Component({
  selector: 'app-verify-student-automatic',
  templateUrl: './verify-student-automatic.component.html',
  styleUrls: ['./verify-student-automatic.component.css']
})
export class VerifyStudentAutomaticComponent implements OnInit{
  studentId!: number;
  verifyToken!: string;
  requestPayload!: VerifyAccountRequest
  isAccountActive!: boolean;
  constructor(private router: Router, private activatedRoute: ActivatedRoute,private studentService: StudentService, private matSnackBar: MatSnackBar) {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    this.fetchStudentById(this.studentId).then((data: StudentResponse) => {
      this.verifyToken = this.activatedRoute.snapshot.queryParams['token'];
      console.log(this.studentId);
      console.log(this.verifyToken);
      if(data && data.name && data.lastName){
        this.isAccountActive = data.isActivated;
        if(!this.isAccountActive){
          this.requestPayload = {
            verifyToken: '',
          }
          this.requestPayload.verifyToken = this.verifyToken;
          this.studentService.verifyAccount(this.studentId, this.requestPayload)
            .subscribe((data: VerifyTokenResponse) => {
              if(data.name != null) {
                this.router.navigateByUrl(`complete-profile/${this.studentId}`);
              } else {
                this.matSnackBar.open(data.message, "Close", {
                  duration: 3000
                });
              }
            });
        }

      } else {
        this.router.navigate(['/not-found']);
      }


    });
  }
  ngOnInit(): void {

  }
  private async fetchStudentById(id: number) {
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }
  send() {
    this.router.navigateByUrl("login");
  }
}
