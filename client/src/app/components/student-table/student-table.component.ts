import {AfterViewInit, Component, OnInit} from '@angular/core';
import {StudentService} from "../../services/student/student.service";
import {StudentResponse} from "../../interfaces/student/StudentResponse";


interface StudentDto {
  id: number,
  name: string;
  lastName: string;
  email: string;
  password: string;
  enrollDate: Date;
  dateOfBirth: Date;
  address: string;
  skills: string;
  phone: string;
  department: string;
  collegeName: string
  image: ArrayBuffer | string | null;
  cityName: string;
  isActivated: boolean;
  formattedDob: string;
  formattedEnrollDate: string;
}

interface FormattedStudents {
  id: number,
  name: string;
  lastName: string;
  email: string;
  password: string;
  enrollDate: Date;
  dateOfBirth: Date;
  address: string;
  skills: string;
  phone: string;
  department: string;
  collegeName: string
  image: ArrayBuffer | string | null;
  cityName: string;
  isActivated: boolean,
  formattedDob: string;
  formattedEnrollDate: string;
}

@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styleUrls: ['./student-table.component.css']
})
export class StudentTableComponent implements OnInit {
  page = 1;
  pageSize = 4;

  students!: StudentResponse[];
  dumbStudents: FormattedStudents[] = [];
  collectionSize!: number;

  constructor(private studentService: StudentService) {
    this.studentService.getStudents().subscribe((data: any) => {
      this.students = data;
      this.collectionSize = this.students.length;
      // console.log(data);
      this.students.map(student => {
        student.image = "data:image/png;base64," + student.image;
      });
      this.students.map((student: StudentResponse, index: number) => {
        this.dumbStudents[index] = {...student, formattedDob: this.format(new Date(student.dateOfBirth)) , formattedEnrollDate: this.format(new Date(student.enrollDate)) };
      });
      console.log(this.dumbStudents);
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
    this.students.map((student: StudentResponse, index: number) => {
      this.dumbStudents[index] = {...student, formattedDob: this.format(new Date(student.dateOfBirth)) , formattedEnrollDate: this.format(new Date(student.enrollDate)) };
    });
    // this.dumbStudents
    //   .map((student: StudentResponse) =>
    //     ({formattedDob: this.format(new Date(student.dateOfBirth)) , formattedEnrollDate: this.format(new Date(student.enrollDate)) ,...student}));

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
