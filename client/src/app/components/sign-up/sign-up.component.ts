import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CollegeService} from "../../services/college.service";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";
import {StudentService} from "../../services/student.service";
import {StudentSignupRequest} from "../../interfaces/student/signup/StudentSignupRequest";
import {StudentSignupResponse} from "../../interfaces/student/signup/StudentSignupResponse";
import {Router} from "@angular/router";

interface EmailExtension{
  emailExtension: string;
}

class SignupPayload {
  name!: string;
  lastName!: string;
  email!: string;
  password!: string;
  confirmPassword!: string;
}
@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit{
  emailExtensionControl = new FormControl<EmailExtension | null>(null, Validators.required);
  createPostForm!: FormGroup;
  postPayload: SignupPayload;
  colleges!: CollegeGetResponse[];
  student: StudentSignupRequest = {
    name: '',
    lastName: '',
    email: '',
    password: ''
  };
  constructor(private router: Router,private collegeService: CollegeService, private studentService: StudentService) {
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
    this.collegeService.getColleges().subscribe((data: any) => {
      this.colleges = data;
      console.log(this.colleges);
    });
  }
  signup(){
    this.postPayload.name = this.createPostForm.get('name')!.value;
    this.postPayload.lastName = this.createPostForm.get('lastName')!.value;
    this.postPayload.email = this.createPostForm.get('email')!.value.concat("@"+this.emailExtensionControl?.value?.emailExtension);
    console.log(this.postPayload.email);
    this.postPayload.confirmPassword = this.createPostForm.get('confirmPassword')!.value;
    this.postPayload.password = this.createPostForm.get('password')!.value;

    if(this.postPayload.password === this.postPayload.confirmPassword){
      console.log("Passwords matching");
      this.student.name = this.postPayload.name = this.createPostForm.get('name')!.value;
      this.student.email = this.postPayload.email = this.createPostForm.get('email')!.value.concat("@"+this.emailExtensionControl?.value?.emailExtension);
      this.student.lastName = this.postPayload.lastName = this.createPostForm.get('lastName')!.value;
      this.student.password = this.postPayload.password = this.createPostForm.get('password')!.value;

      // this.router.navigateByUrl('sign-up/activate/'+ 2);
      this.studentService.signUp(this.student).subscribe((data: StudentSignupResponse) => {
        console.log("After sign up operation");
        console.log(data.id);
        this.router.navigateByUrl('sign-up/activate/'+ data.id);
      });
    }
  }
}
