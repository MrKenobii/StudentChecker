import {Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student/student.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";


@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent implements OnInit{
  studentId: number;
  student!: StudentResponse;
  courses: any;
  isOwnProfilePage: boolean =false;
  constructor(private activatedRoute: ActivatedRoute,private studentService: StudentService, private router: Router, private snackBar: MatSnackBar) {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    this.activatedRoute.params.subscribe(val => {
      this.studentId = this.activatedRoute.snapshot.params['studentId'];
    });

  }
  ngOnInit(): void {
    this.studentService.getStudentById(this.studentId).subscribe((data: StudentResponse) => {
        this.student = data;
        console.log(data);
        this.student.image = "data:image/png;base64," + this.student.image;
        console.log(this.student);
        this.studentService.getCoursesByStudent(this.studentId).subscribe((_courses: any) => {
          this.courses =_courses;
          console.log(_courses);
        });
    });

    this.studentService.getTokenByStudentId(this.studentId).subscribe((data: StudentGetTokenResponse) => {
      if(localStorage.getItem("key") !== null && localStorage.getItem("key") == data.key){
        this.isOwnProfilePage = true;
        this.snackBar.open(data.message, "Ok", {
          duration: 3000
        });
      }
    });
  }

  editProfile() {
    this.router.navigate(['student/edit-profile/'+this.studentId]);
    console.log("Inside edit Profile");
  }
}
