import {Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student.service";
import {Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Student} from "../../interfaces/student/Student";
import {StudentResponse} from "../../interfaces/student/StudentResponse";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  student!: StudentResponse;
  constructor(private router: Router,private studentService: StudentService, private snackBar: MatSnackBar) {
    if(localStorage.getItem("key") !== null){
      this.studentService.
      getStudentByKey(localStorage.getItem("key")!)
        .subscribe((data: StudentResponse) => {
          console.log(data);
          this.student = data;
        });
    }

  }
  ngOnInit(): void {
    if(localStorage.getItem("key") !== null){
      this.studentService.
      getStudentByKey(localStorage.getItem("key")!)
        .subscribe((data: StudentResponse) => {
          console.log(data);
          this.student = data;
      });
    }


  }

  logout() {
    localStorage.removeItem("key");
    this.router.navigateByUrl("login");
  }
}
