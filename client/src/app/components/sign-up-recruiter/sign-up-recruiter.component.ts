import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MatSnackBar} from "@angular/material/snack-bar";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterPostSignupRequest} from "../../interfaces/recruiter/signup/RecruiterPostSignupRequest";
import {RecruiterPostSignupResponse} from "../../interfaces/recruiter/signup/RecruiterPostSignupResponse";
import {Router} from "@angular/router";

class SignupPayload {
  name!: string;
  lastName!: string;
  email!: string;
  password!: string;
  confirmPassword!: string;
}
@Component({
  selector: 'app-sign-up-recruiter',
  templateUrl: './sign-up-recruiter.component.html',
  styleUrls: ['./sign-up-recruiter.component.css']
})
export class SignUpRecruiterComponent implements OnInit{
  createPostForm!: FormGroup;
  postPayload: SignupPayload;
  requestPayload!: RecruiterPostSignupRequest;
  constructor(private router: Router, private recruiterService: RecruiterService ,private snackBar: MatSnackBar) {
    this.postPayload = {
      name: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: ''
    }
  }
  ngOnInit(): void {
    this.createPostForm = new FormGroup({
      name: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', Validators.required),
    });
  }
  signup(){
    this.postPayload.name = this.createPostForm.get('name')!.value;
    this.postPayload.lastName = this.createPostForm.get('lastName')!.value;
    this.postPayload.email = this.createPostForm.get('email')!.value;
    this.postPayload.confirmPassword = this.createPostForm.get('confirmPassword')!.value;
    this.postPayload.password = this.createPostForm.get('password')!.value;
    if(this.postPayload.password === this.postPayload.confirmPassword){
      console.log(this.postPayload);

      this.requestPayload = {
        name: this.postPayload.name,
        email: this.postPayload.email,
        lastName: this.postPayload.lastName,
        password: this.postPayload.password
      };

      console.log(this.requestPayload);
      this.recruiterService.signUp(this.requestPayload).subscribe((data: RecruiterPostSignupResponse) => {
        this.snackBar.open(data.message, "Ok", {
          duration: 3000
        });
        if(data.id > 0){
          this.router.navigateByUrl('sign-up/recruiter/activate/'+ data.id);
        }
      });
    }
    else
      this.snackBar.open("Password are not matching", "Ok", {
        duration: 3000
      })
  }

}
