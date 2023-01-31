import {Component, OnInit} from '@angular/core';
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";
import {lastValueFrom} from "rxjs";
import {UpdatePassword} from "../../interfaces/UpdatePassword";
import {UpdatePasswordResponse} from "../../interfaces/UpdatePasswordResponse";

@Component({
  selector: 'app-student-privacy-settings',
  templateUrl: './student-privacy-settings.component.html',
  styleUrls: ['./student-privacy-settings.component.css']
})
export class StudentPrivacySettingsComponent implements OnInit{
  studentId!: number;
  student!: StudentResponse;

  createPostForm!: FormGroup;
  postPayload!: UpdatePassword;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //

  isOwnPage!: boolean;
  isLoading: boolean = true;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private studentService: StudentService,
              private snackBar: MatSnackBar) {

  }
  send() {

    this.postPayload.prevPassword = this.createPostForm.get('prevPassword')!.value;
    this.postPayload.newPassword = this.createPostForm.get('newPassword')!.value;
    this.postPayload.newPasswordCopy = this.createPostForm.get('newPasswordCopy')!.value;
    const obj: UpdatePassword = {
      prevPassword: this.createPostForm.get('prevPassword')!.value,
      newPassword: this.createPostForm.get('newPassword')!.value,
      newPasswordCopy: this.createPostForm.get('newPasswordCopy')!.value,
    };
    if((obj.prevPassword  == null || obj.prevPassword.trim() == "")
      ||(obj.newPassword  == null || obj.newPassword.trim() == "")
      ||(obj.newPasswordCopy  == null || obj.newPasswordCopy.trim() == "")){
      this.snackBar.open("You must fill all the fields", "OK", {
        duration: 4000
      });
    } else{
      if(obj.newPassword != obj.newPasswordCopy){
        this.snackBar.open("Passwords are not matching", "OK", {
          duration: 4000
        });
      } else {
        this.studentService.changePassword(this.studentId,obj).subscribe((passwordResponse:UpdatePasswordResponse) => { //
          console.log(passwordResponse);
          if(passwordResponse && passwordResponse.status){
            this.snackBar.open(passwordResponse.message, "OK", {
              duration: 5000
            });
            this.router.navigate(['/profile/student/'+this.studentId]);
          } else {
            this.snackBar.open(passwordResponse.message, "OK", {
              duration: 5000
            });
          }
        });
      }
    }
  }


  ngOnInit(): void {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    this.fetchTokenByStudentId(this.studentId).then((tokenResponse: StudentGetTokenResponse) => {
      this.isLoading = true;
      if(tokenResponse !== null && tokenResponse.key !== null && tokenResponse.key === localStorage.getItem("key")){
        console.log("Inside if token");
        console.log(tokenResponse);
        this.isOwnPage = true;
        this.fetchStudent(this.studentId).then((data) => {
          console.log(data);
          this.student = data;
          if(data !== null){
            this.student = data;

            this.postPayload = {
              prevPassword: '',
              newPassword: '',
              newPasswordCopy: ''
            };

            this.createPostForm = new FormGroup({
              prevPassword: new FormControl('', Validators.required),
              newPassword: new FormControl('', Validators.required),
              newPasswordCopy: new FormControl('', Validators.required),
            });
            this.isLoading = false;
          } else {
            console.log("Inside else 1.");
            console.log(tokenResponse);
            this.snackBar.open("404 Not Found", "OK", {
              duration: 10000
            });
            this.isOwnPage = false;
            this.router.navigate(['/not-found']);
          }
        });
      } else {
        console.log("Inside else");
        console.log(tokenResponse);
        this.snackBar.open("You are unauthorized", "OK", {
          duration: 10000
        });
        this.isOwnPage = false;
        this.router.navigate(['/not-found']);
      }

    });
  }
  format(inputDate: Date) {
    let date, month, year;

    date = inputDate.getDate();
    month = inputDate.getMonth() + 1;
    year = inputDate.getFullYear();

    date = date
      .toString()
      .padStart(2, '0');

    month = month
      .toString()
      .padStart(2, '0');

    return `${year}-${month}-${date}`;
  }

  private async fetchStudent(id: number){
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }

  private async fetchTokenByStudentId(id: number) {
    let observable = this.studentService.getTokenByStudentId(id);
    return await lastValueFrom(observable);
  }

}
