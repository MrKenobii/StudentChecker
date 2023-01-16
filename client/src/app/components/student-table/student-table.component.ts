import {AfterViewInit, Component, ViewChild} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';

import { DecimalPipe, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {Student} from "../../interfaces/Student";
import {City} from "../../interfaces/City";
import {College} from "../../interfaces/College";


const _city: City = new class implements City {
  name = "Istanbul";
}
const _college = new class implements College {
  city= _city;
  foundationYear = new Date(2022, 2,6);
  name =  "Yeditepe University";

}
const students: Student[] = [
  {
    name: "Anıl",
    lastName: "Duyguç",
    email: "anil.duyguc@std.yeditepe.edu.tr",
    enrollDate: new Date(2018, 9, 15),
    dateOfBirth: new Date(1999, 6, 15),
    address: "Bla bla bla bla st bla bla bla apartment 1/89",
    skills: "Java Spring Boot,Python, Node.JS, C++, MySQL,MONGODB, Angular & React,",
    city: _city,
    phone: "909000909090",
    isActivated: true,
    department: "Computer Science Enginnering",
    college: _college,
    image: "https://cdn-icons-png.flaticon.com/512/149/149071.png",
  },
  {
    name: "Alex",
    lastName: "Souza",
    email: "alex.souza@std.yeditepe.edu.tr",
    enrollDate: new Date(2018, 9, 15),
    dateOfBirth: new Date(1999, 6, 15),
    address: "Bla bla bla bla st bla bla bla apartment 1/89",
    skills: "Java Spring Boot,Python, Node.JS, C++, MySQL,MONGODB, Angular & React,",
    city: _city,
    phone: "909000909090",
    isActivated: true,
    department: "Computer Science Enginnering",
    college: _college,
    image: "https://cdn-icons-png.flaticon.com/512/149/149071.png",
  },
  {
    name: "Emre",
    lastName: "Can",
    email: "emre.can@std.yeditepe.edu.tr",
    enrollDate: new Date(2018, 9, 15),
    dateOfBirth: new Date(1999, 6, 15),
    address: "Bla bla bla bla st bla bla bla apartment 1/89",
    skills: "Java Spring Boot,Python, Node.JS, C++, MySQL,MONGODB, Angular & React,",
    city: _city,
    phone: "909000909090",
    isActivated: true,
    department: "Computer Science Enginnering",
    college: _college,
    image: "https://cdn-icons-png.flaticon.com/512/149/149071.png",
  },
  {
    name: "Hans",
    lastName: "Zimmerman",
    email: "hans.zimmerman@std.yeditepe.edu.tr",
    enrollDate: new Date(2018, 9, 15),
    dateOfBirth: new Date(1999, 6, 15),
    address: "Bla bla bla bla st bla bla bla apartment 1/89",
    skills: "Java Spring Boot,Python, Node.JS, C++, MySQL,MONGODB, Angular & React,",
    city: _city,
    phone: "909000909090",
    isActivated: false,
    department: "Computer Science Enginnering",
    college: _college,
    image: "https://cdn-icons-png.flaticon.com/512/149/149071.png",
  },
]
@Component({
  selector: 'app-student-table',
  templateUrl: './student-table.component.html',
  styleUrls: ['./student-table.component.css']
})
export class StudentTableComponent implements AfterViewInit {
  // displayedColumns: string[] = ['position', 'name', 'weight', 'symbol'];
  // dataSource = new MatTableDataSource<PeriodicElement>(ELEMENT_DATA);

  // displayedColumns: string[] = ['position', 'name', 'weight', 'symbol']; before
  // dataSource = new MatTableDataSource<PeriodicElement>(ELEMENT_DATA);  before

  // @ViewChild(MatPaginator) paginator!: MatPaginator; before

  page = 1;
  pageSize = 4;
  // collectionSize = COUNTRIES.length;
  // countries!: Country[];
  collectionSize = students.length;
  students!: Student[];
  constructor() {
    this.refreshStudents();
  }
  ngAfterViewInit() {
    // this.dataSource.paginator = this.paginator; before
  }

  public refreshStudents() {
    this.students = students.map((student, i ) => ({id: i+1, ...student})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }

  goPageInfo(student: Student) {
    console.log(student);
  }
}
