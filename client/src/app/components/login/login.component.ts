import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
class LoginPayload {
  email!: string;
  password!: string;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  createPostForm!: FormGroup;
  postPayload: LoginPayload;
  constructor() {
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
  }
}
