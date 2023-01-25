import {Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student/student.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";
import {lastValueFrom} from "rxjs";


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
    this.activatedRoute.params.subscribe(() => {
      this.studentId = this.activatedRoute.snapshot.params['studentId'];
    });

  }
  ngOnInit(): void {
    this.fetchStudentById(this.studentId).then((data: StudentResponse) => {
      this.student = data;
      console.log(data);
      this.student.image = "data:image/png;base64," + this.student.image;
      console.log(this.student);
      this.fetchCoursesByStudentId(this.studentId).then((_courses: any) => {
        console.log(_courses);
        if(_courses.length > 0) this.courses =_courses;
        else this.snackBar.open("No Courses retrieved", "OK", {
          duration: 4000
        });
      }).catch((error) => this.router.navigate(['/not-found']));
    }).catch((error) => {
      this.snackBar.open("Error!!!" +error, "Ok", {
        duration: 3000
      })
      this.router.navigate(['/not-found']);
    });


    this.fetchTokenByStudentId(this.studentId).then((data: StudentGetTokenResponse) => {
      this.isOwnProfilePage = localStorage.getItem("key") !== null && localStorage.getItem("key") == data.key;
    }).catch((error) => {
      this.snackBar.open("Error!!!" +error, "Ok", {
        duration: 3000
      })
      this.router.navigate(['/not-found']);
    });
  }
  private async fetchStudentById(id: number){
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }
  private async fetchTokenByStudentId(id: number){
    let tokenByStudentId = this.studentService.getTokenByStudentId(id);
    return await lastValueFrom(tokenByStudentId);
  }
  private async fetchCoursesByStudentId(id: number) {
    let coursesByStudent = this.studentService.getCoursesByStudent(this.studentId);
    return await lastValueFrom(coursesByStudent);
  }

  editProfile() {
    this.router.navigate(['student/edit-profile/'+this.studentId]);
    console.log("Inside edit Profile");
  }

  chat() {

  }
}
