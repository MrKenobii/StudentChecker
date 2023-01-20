import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {CollegeService} from "../../services/college.service";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CompleteStudentProfile} from "../../interfaces/student/CompleteStudentProfile";
import {Course} from "../../interfaces/course/Course";
import {CoursesService} from "../../services/courses.service";
import {ImageUploadService} from "../../services/image-upload.service";



interface CourseControl{
  id: number;
  name: string;
}
@Component({
  selector: 'app-student-complete-profile',
  templateUrl: './student-complete-profile.component.html',
  styleUrls: ['./student-complete-profile.component.css']
})
export class StudentCompleteProfileComponent implements OnInit{
  studentId!: number;
  collegeEmailExtension!: string;
  colleges!: CollegeGetResponse[];
  college!: CollegeGetResponse;
  student!: StudentResponse;
  createPostForm!: FormGroup;
  postPayload: CompleteStudentProfile;
  _courses!: Course[];
  courses!: Course[];
  courseControl = new FormControl<Course[] | null>(null, Validators.required);
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private studentService: StudentService,
              private courseService: CoursesService,
              private collegeService: CollegeService,
              private fileService: ImageUploadService,
              private matSnackBar: MatSnackBar) {

    this.postPayload = {
      collegeName: '',
      department: '',
      address: '',
      enrollDate: new Date(),
      phone: '',
      dateOfBirth: new Date(),
      cityName: '',
      skills: '',
      languages: '',
      courses: this._courses || null,
      image: this.fileBlob


    };
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    studentService.getStudentById(this.studentId).subscribe((data:StudentResponse) => {
      this.student = data;
      this.collegeEmailExtension = this.student.email.substring(data.email.indexOf('@') + 1, data.email.length);
      this.collegeService.getColleges().subscribe((data:any) => { // any degis
        console.log(data);
        this.colleges = data;
        console.log(this.collegeEmailExtension);
        this.colleges.map((c: CollegeGetResponse) => {
          if(c.emailExtension == this.collegeEmailExtension){
            this.college = c;
          }
        });
        console.log(this.college);
      });
    })
    this.courseService.getCourses().subscribe((data: Course[]) => {
      this.courses = data;
    })
  }
  ngOnInit(): void {
    this.createPostForm = new FormGroup({
      collegeName: new FormControl('', Validators.required),
      department: new FormControl('', Validators.required),
      address: new FormControl('', Validators.required),
      enrollDate: new FormControl(new Date(), Validators.required),
      phone: new FormControl('', Validators.required),
      dateOfBirth: new FormControl(new Date(), Validators.required),
      skills: new FormControl('', Validators.required),
      languages: new FormControl('', Validators.required),
      image: new FormControl('', Validators.required),
      courses: new FormControl(this._courses, Validators.required),
      cityName: new FormControl('', Validators.required),
    });
  }

  send() {
    console.log("Inside send");
    this.onUpload();
    this.postPayload.department = this.createPostForm.get('department')!.value;
    this.postPayload.collegeName = this.college.name;
    this.postPayload.address = this.createPostForm.get('address')!.value;
    this.postPayload.enrollDate = this.createPostForm.get('enrollDate')!.value;
    this.postPayload.phone = this.createPostForm.get('phone')!.value;
    this.postPayload.dateOfBirth = this.createPostForm.get('dateOfBirth')!.value;
    this.postPayload.skills = this.createPostForm.get('skills')!.value;
    this.postPayload.languages = this.createPostForm.get('languages')!.value;
    this.postPayload.image = this.createPostForm.get('image')!.value;
    this.postPayload.courses = this.courseControl!.value;
    this.postPayload.cityName = this.createPostForm.get('cityName')!.value;

    const reader = new FileReader();
    reader.readAsDataURL(this.file); //FileStream response from .NET core backend
    reader.onload = _event => {
      // console.log(reader.result);
      const obj = {
        department: this.createPostForm.get('department')!.value,
        collegeName: this.college.name,
        address: this.createPostForm.get('address')!.value,
        enrollDate: this.createPostForm.get('enrollDate')!.value,
        phone: this.createPostForm.get('phone')!.value,
        dateOfBirth: this.createPostForm.get('dateOfBirth')!.value,
        skills: this.createPostForm.get('skills')!.value,
        languages: this.createPostForm.get('languages')!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
        courses:this.courseControl!.value,
        cityName: this.createPostForm.get('cityName')!.value
      };
      console.log(obj);
      this.studentService.updateProfile(this.studentId, obj).subscribe((data: any) => {
        console.log(data);
        this.router.navigateByUrl("login");
      });
    };
  }
  onChange(event: any) {
    // const file = new File(['hello', ' ', 'world'], 'hello_world.txt', {type: 'text/plain'});
    this.file = event.target.files[0];
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
