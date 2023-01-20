import {AfterViewInit, Component } from '@angular/core';
import {StudentService} from "../../services/student.service";
import {StudentResponse} from "../../interfaces/student/StudentResponse";





@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styleUrls: ['./student-table.component.css']
})
export class StudentTableComponent implements AfterViewInit {page = 1;
  pageSize = 4;

  students!: StudentResponse[];
  dumbStudents!: StudentResponse[];
  collectionSize!: number;
  constructor(private studentService: StudentService) {
    this.studentService.getStudents().subscribe((data: any) => {
      this.students = data;
      this.collectionSize = this.students.length;
      console.log(data);
      this.refreshStudents();
    })

  }
  ngAfterViewInit() {
    // this.dataSource.paginator = this.paginator; before
  }

  public refreshStudents() {
    this.dumbStudents = this.students;
    this.dumbStudents = this.dumbStudents.map((student ) => ({ ...student})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }

  goPageInfo(student: StudentResponse) {
    console.log(student);
  }
}
