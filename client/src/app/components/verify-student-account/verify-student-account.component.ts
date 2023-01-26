import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {StudentService} from "../../services/student/student.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {lastValueFrom} from "rxjs";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {StudentResponse} from "../../interfaces/student/StudentResponse";

@Component({
  selector: 'app-verify-student-account',
  templateUrl: './verify-student-account.component.html',
  styleUrls: ['./verify-student-account.component.css']
})
export class VerifyStudentAccountComponent implements OnInit{
  studentId!: number;
  postPayload!: VerifyAccountRequest;
  createPostForm!: FormGroup;
  isAccountActive!: boolean;

  constructor(private router: Router, private activatedRoute: ActivatedRoute,private studentService: StudentService, private matSnackBar: MatSnackBar) {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    console.log(this.studentId);
    this.fetchStudentById(this.studentId).then((data: StudentResponse) => {
      if(data && data.name && data.lastName){
        this.isAccountActive = data.isActivated;
        if(!this.isAccountActive){
          this.postPayload = {
            verifyToken: '',
          }
        }
      } else {
        this.router.navigate(['/not-found']);
      }
    }).catch((error) => this.router.navigate(['/not-found']));

  }
  ngOnInit(): void {
    this.createPostForm = new FormGroup({
      verifyToken: new FormControl('', Validators.required),

    });
  }
  private async fetchStudentById(id: number) {
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }
  send() {
    this.postPayload.verifyToken = this.createPostForm.get('verifyToken')!.value;
    console.log(this.postPayload.verifyToken);
    console.log(this.studentId);
    this.studentService.verifyAccount(
      this.studentId, this.postPayload)
      .subscribe((data: VerifyTokenResponse) => {
          console.log(data);
          if(data.name != null){
            this.router.navigateByUrl(`login`);
            this.matSnackBar.open("Thanks for your registration. Please wait for your account to be confirmed", "Close", {
              duration: 3000
            });
          } else {
            this.matSnackBar.open(data.message, "Close", {
              duration: 5000
            });
          }
    });
  }
}
