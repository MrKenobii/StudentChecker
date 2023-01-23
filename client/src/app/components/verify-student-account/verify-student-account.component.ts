import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {StudentService} from "../../services/student/student.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";

class SignupPayload {
  verifyToken!: string;
}
@Component({
  selector: 'app-verify-student-account',
  templateUrl: './verify-student-account.component.html',
  styleUrls: ['./verify-student-account.component.css']
})
export class VerifyStudentAccountComponent implements OnInit{
  studentId!: number;
  postPayload: VerifyAccountRequest;
  createPostForm!: FormGroup;

  constructor(private router: Router, private activatedRoute: ActivatedRoute,private studentService: StudentService, private matSnackBar: MatSnackBar) {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    console.log(this.studentId);
    this.postPayload = {
      verifyToken: '',
    }
  }
  ngOnInit(): void {
    this.createPostForm = new FormGroup({
      verifyToken: new FormControl('', Validators.required),

    });
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
              this.router.navigateByUrl(`complete-profile/${this.studentId}`);
          } else {
            this.matSnackBar.open(data.message, "Close", {
              duration: 5000
            });
          }
    });
  }
}
