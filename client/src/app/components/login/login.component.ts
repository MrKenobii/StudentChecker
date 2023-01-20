import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {StudentService} from "../../services/student.service";
import {StudentLoginRequest} from "../../interfaces/student/login/StudentLoginRequest";
import {StudentLoginResponse} from "../../interfaces/student/login/StudentLoginResponse";
import {MatSnackBar} from "@angular/material/snack-bar";
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
  postPayload: StudentLoginRequest;
  constructor(private router: Router, private studentService: StudentService, private snackBar: MatSnackBar) {
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
    this.studentService.login(this.postPayload).subscribe((data: StudentLoginResponse) => {
        console.log(data);
        this.snackBar.open(data.message, "Dismiss", {
          duration: 3000
        });
        localStorage.setItem("key", data.key);
        this.router.navigateByUrl("companies");
    });
  }
}
