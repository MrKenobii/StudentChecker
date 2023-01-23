import {AfterViewInit, Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student/student.service";
import {StudentResponse} from "../../interfaces/student/StudentResponse";





@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styleUrls: ['./student-table.component.css']
})
export class StudentTableComponent implements OnInit {
  page = 1;
  pageSize = 4;

  students!: StudentResponse[];
  dumbStudents!: StudentResponse[];
  collectionSize!: number;
  constructor(private studentService: StudentService) {
    this.studentService.getStudents().subscribe((data: any) => {
      this.students = data;
      this.collectionSize = this.students.length;
      // console.log(data);
      this.students.map(student => {
        student.image = "data:image/png;base64," + student.image;
      });
      console.log(this.students);
      this.refreshStudents();

    })

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

    return `${date}/${month}/${year}`;
  }
  public refreshStudents() {
    this.dumbStudents = this.students;
    this.dumbStudents.map((s, i) => {
      s.formattedDob =this.format(new Date(s.dateOfBirth));
      s.formattedEnrollDate =this.format(new Date(s.enrollDate));
    });
    this.dumbStudents = this.dumbStudents.map((student ) => ({ ...student})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }

  goPageInfo(student: StudentResponse) {
    console.log(student);
  }

  ngOnInit(): void {
  }
}
