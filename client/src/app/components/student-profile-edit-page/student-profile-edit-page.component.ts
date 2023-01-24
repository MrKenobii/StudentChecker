import {Component, OnInit} from '@angular/core';
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {Form, FormControl, FormGroup, Validators} from "@angular/forms";
import {Company} from "../../interfaces/company/Company";
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {CompanyService} from "../../services/company/company.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {lastValueFrom} from "rxjs";
import {StudentService} from "../../services/student/student.service";
import {CoursesService} from "../../services/course/courses.service";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {Course} from "../../interfaces/course/Course";
import {CollegeService} from "../../services/college/college.service";
import {City} from "../../interfaces/city/City";
import {Student} from "../../interfaces/student/Student";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";

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
}

interface UpdateProfile {
  name: string;
  lastName: string;
  email: string;
  address: string;
  password: string;
  languages: string;
  skills: string;
  enrollDate: Date;
  phone: string;
  dateOfBirth: Date;
  collegeName: string;
  cityName: string;
  image: ArrayBuffer | string | null
}

interface CollegeI {
  city: City;
  emailExtension: string;
  foundationDate: Date;
  id: number;
  name: string;
  students: Student[]
}
@Component({
  selector: 'app-student-profile-edit-page',
  templateUrl: './student-profile-edit-page.component.html',
  styleUrls: ['./student-profile-edit-page.component.css']
})
export class StudentProfileEditPageComponent implements OnInit {
  studentId!: number;
  student!: StudentResponse;
  courseControl!: FormControl;
  collegeControl!: FormControl;
  courses!: Course[];
  createPostForm!: FormGroup;
  postPayload!: UpdateProfile;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  colleges!: CollegeGetResponse[];
  selectedCollege!: CollegeGetResponse;
  formattedEnrolledDate!: string;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private studentService: StudentService,
              private courseService: CoursesService,
              private collegeService: CollegeService,
              private fileService: ImageUploadService) {

  }
  send() {

    this.postPayload.address = this.createPostForm.get('address')!.value;
    this.postPayload.enrollDate = this.createPostForm.get('enrollDate')!.value;
    this.postPayload.phone = this.createPostForm.get('phone')!.value;
    this.postPayload.dateOfBirth = this.createPostForm.get('dateOfBirth')!.value;
    this.postPayload.collegeName = this.courseControl.value!.at(0)!.name; // college yao
    this.postPayload.image = this.createPostForm.get('image')!.value;

    //console.log(this.companyControl.value);
    const reader = new FileReader();
    reader.readAsDataURL(this.file); //FileStream response from .NET core backend
    reader.onload = _event => {
      // console.log(reader.result);

      const obj = { // değişçek
        name: this.createPostForm.get('name')!.value,
        lastName: this.createPostForm.get('lastName')!.value,
        address: this.createPostForm.get('address')!.value,
        email: this.createPostForm.get('email')!.value,
        password: this.createPostForm.get('newPassword')!.value,
        languages: this.createPostForm.get('languages')!.value,
        skills: this.createPostForm.get('skills')!.value,
        enrollDate: this.createPostForm.get('enrollDate')!.value,
        phone: this.createPostForm.get('phone')!.value,
        dateOfBirth: this.createPostForm.get('dateOfBirth')!.value,
        cityName: this.createPostForm.get('cityName')!.value,
        courses: this.courseControl!.value,
        // collegeName: this.createPostForm.get('collegeName')!.value,
        collegeName: this.collegeControl!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      // this.studentService.editProfile(this.studentId,obj).subscribe(data => {
      //   console.log(data);
      //   this.router.navigate(['/profile/student/'+this.studentId]);
      // });
    }
  }

  ngOnInit(): void {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    this.fetchStudent(this.studentId).then((data) => {
      console.log("Cehckkkk");
      console.log(data);
      this.student = data;
      if(data !== null){
        this.student = data;
        this.formattedEnrolledDate = this.format(new Date(this.student!.enrollDate)); // noluyyyyoyo
        console.log("Cehckkkk");
        console.log(this.student);
        // console.log("HireDate");


        console.log("Checkpoint");
        var courses: string[] = []
        this.fetchCourses().then((_courses: Course[]) => {
          this.courses = _courses;
          this.fetchCoursesByStudent(this.studentId).then((sCourses: Course[]) => {
            sCourses.map((sCourse: Course) => {
              courses.push(sCourse.name);
            });
            console.log(this.courses);
            this.courseControl =  new FormControl<string[]>(courses, Validators.required);
            this.courseControl.setValue(courses);
          });
        });
        // this.courseService.getCourses().subscribe((data: Course[]) => {
        //   //console.log(data);
        //   this.courses = data;
        //   console.log(this.courses);
        //   courses.push(this.courses.at(0)!.name);
        //   courses.push(this.courses.at(1)!.name); // fetch them
        //   this.courseControl =  new FormControl<string[]>(courses, Validators.required);
        //   this.courseControl.setValue(courses);
        // });

        this.fetchColleges().then((data: any) => {
          this.colleges = data;
            this.fetchCollegeByStudentId(this.studentId).then((college: any) => {
              this.selectedCollege = college;
              console.log(this.colleges);
              console.log(this.selectedCollege.name);
              this.collegeControl = new FormControl<string>(this.selectedCollege.name, Validators.required);
              this.collegeControl.setValue(this.selectedCollege.name);
            });
        });



        this.postPayload = {
          name: '',
          lastName: '',
          email: '',
          address: '',
          password: '',
          languages: '',
          skills: '',
          enrollDate: new Date(),
          phone: '',
          dateOfBirth: new Date(),
          collegeName: '',
          image: this.fileBlob,
          cityName: '',
        };

        this.createPostForm = new FormGroup({
          name: new FormControl(this.student ? this.student.name : '', Validators.required),
          lastName: new FormControl(this.student ? this.student.lastName : '', Validators.required),
          email: new FormControl(this.student ? this.student.email : '', Validators.required),
          address: new FormControl(this.student ? this.student.address : '', Validators.required),
          phone: new FormControl(this.student ? this.student.phone : '', Validators.required),
          skills: new FormControl(this.student ? this.student.skills : '', Validators.required),
          languages: new FormControl(this.student ? this.student.languages : '', Validators.required),
          enrollDate: new FormControl(this.student ? this.format(new Date(this.student.enrollDate)) : '' , Validators.required),
          dateOfBirth: new FormControl(this.student ? this.format(new Date(this.student.dateOfBirth)) : '', Validators.required),
          cityName: new FormControl(this.student ? this.student.cityName : '', Validators.required),
          image: new FormControl('', Validators.required),
          prevPassword: new FormControl('', Validators.required),
          newPassword: new FormControl('', Validators.required),
        });
      }
    });
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

    return `${year}-${month}-${date}`;
  }
  onChange(event: any) {
    this.file = event.target.files[0];
  }
  private async fetchStudent(id: number){
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }
  private async fetchColleges(){
    let observable = this.collegeService.getColleges();
    return await lastValueFrom(observable);
  }
  private async fetchCollegeByStudentId(id: number) {
    let collegeByStudent = this.studentService.getCollegeByStudent(id);
    return await lastValueFrom(collegeByStudent);
  }

  private async fetchCourses(){
    let observable = this.courseService.getCourses();
    return await lastValueFrom(observable);
  }
  private async fetchCoursesByStudent(id: number){
    let observable = this.studentService.getCoursesByStudent(id);
    return await lastValueFrom(observable);
  }
  onUpload() {
    this.loading = !this.loading;
    // console.log(this.file);
    this.fileService.upload(this.file).subscribe(
      (event: any) => {
        if (typeof (event) === 'object') {

          // Short link via api response
          this.shortLink = event.link;

          this.loading = false; // Flag variable
        }
      }
    );
  }

}
