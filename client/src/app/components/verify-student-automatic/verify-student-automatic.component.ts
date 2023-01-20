import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";

@Component({
  selector: 'app-verify-student-automatic',
  templateUrl: './verify-student-automatic.component.html',
  styleUrls: ['./verify-student-automatic.component.css']
})
export class VerifyStudentAutomaticComponent implements OnInit{
  studentId!: number;
  verifyToken!: string;
  requestPayload: VerifyAccountRequest
  constructor(private router: Router, private activatedRoute: ActivatedRoute,private studentService: StudentService, private matSnackBar: MatSnackBar) {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    this.verifyToken = this.activatedRoute.snapshot.queryParams['token'];
    console.log(this.studentId);
    console.log(this.verifyToken);
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
  ngOnInit(): void {

  }

  send() {
    this.router.navigateByUrl("login");
  }
}
