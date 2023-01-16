import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";

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
  constructor() {
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
    console.log(`${this.postPayload}`);
  }

}
