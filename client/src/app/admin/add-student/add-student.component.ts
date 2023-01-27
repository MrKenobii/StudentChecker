import {Component, OnInit} from '@angular/core';
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Course} from "../../interfaces/course/Course";
import {CityGetResponse} from "../../interfaces/city/CityGetResponse";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";
import {City} from "../../interfaces/city/City";
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {CoursesService} from "../../services/course/courses.service";
import {CityService} from "../../services/city.service";
import {CollegeService} from "../../services/college/college.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {lastValueFrom} from "rxjs";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";

interface UpdateProfile {
  name: string;
  lastName: string;
  email: string;
  address: string;
  password: string;
  languages: string;
  skills: string;
  enrollDate: Date;
  department: string;
  token: string;
  isActivated: boolean;
  phone: string;
  dateOfBirth: Date;
  collegeName: string;
  cityName: string;
  image: ArrayBuffer | string | null
}
@Component({
  selector: 'app-add-student',
  templateUrl: './add-student.component.html',
  styleUrls: ['./add-student.component.css']
})
export class AddStudentComponent implements OnInit {
  courseControl!: FormControl;
  collegeControl!: FormControl;
  cityControl!: FormControl;
  courses!: Course[];
  createPostForm!: FormGroup;
  cities!: CityGetResponse[];
  postPayload!: UpdateProfile;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  colleges!: CollegeGetResponse[];
  selectedCollege!: CollegeGetResponse;
  selectedCity!: City;
  formattedEnrolledDate!: string;
  isOwnPage!: boolean;
  isLoading: boolean = true;
  token!: string;
  constructor(private router: Router,
    private activatedRoute: ActivatedRoute,
    private studentService: StudentService,
    private courseService: CoursesService,
    private cityService: CityService,
    private collegeService: CollegeService,
    private fileService: ImageUploadService,
    private snackBar: MatSnackBar) {

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
      var thatCourses: any[] = [];
      this.courseControl!.value.map((val:any) => {
        thatCourses.push({ name: val });
      });

      const obj = { // değişçek
        name: this.createPostForm.get('name')!.value,
        lastName: this.createPostForm.get('lastName')!.value,
        address: this.createPostForm.get('address')!.value,
        email: this.createPostForm.get('email')!.value,
        languages: this.createPostForm.get('languages')!.value,
        skills: this.createPostForm.get('skills')!.value,
        enrollDate: this.createPostForm.get('enrollDate')!.value,
        phone: this.createPostForm.get('phone')!.value,
        dateOfBirth: this.createPostForm.get('dateOfBirth')!.value,
        cityName:  this.cityControl!.value,
        password: this.createPostForm.get("password")!.value,
        token: this.createPostForm.get("token")!.value == null ? 'ABCDEF' :  this.createPostForm.get("token")!.value  ,
        courses: thatCourses,
        department: this.createPostForm.get("department")!.value,
        // collegeName: this.createPostForm.get('collegeName')!.value,
        isActivated: true,
        collegeName: this.collegeControl!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      this.addStudent(obj).then((data) => {
        if(data){
          console.log(data);
          this.snackBar.open("Student has been added successfully", "OK", {
            duration: 4000
          });
          this.router.navigate(['/admin/students/']);
        } else {
          this.snackBar.open("Error!! Bad Request", "OK", {
            duration: 4000
          });
        }
      }).catch((error) =>{
        this.snackBar.open("Something went wrong", "OK", {
          duration: 4000
        });
        this.router.navigate(['/admin/students/']);
      });
    }
  }
private async addStudent(obj: any){
    let objectObservable = this.studentService.addStudent(obj);
    return await lastValueFrom(objectObservable);
  }
  ngOnInit(): void {

    console.log("Checkpoint");
    var courses: string[] = []
    this.fetchCourses().then((_courses: Course[]) => {
      this.isLoading = true;
      this.courses = _courses;
      console.log(this.courses);
      this.courseControl = new FormControl<string[] | null>(null, Validators.required);
      this.courseControl.setValue(null);
      this.isLoading = false;
    });

    this.fetchColleges().then((data: any) => {
      this.isLoading = true;
        this.colleges = data;
        console.log(this.colleges);
        this.collegeControl = new FormControl<string>(this.colleges.at(0)!.name, Validators.required);
        this.collegeControl.setValue(this.colleges.at(0)!.name);
      this.isLoading = false;
    });

    this.fetchCities().then((_city: CityGetResponse[]) => {
      this.isLoading = true;
      console.log(_city);
      this.cities = _city;
      console.log(this.cities);
        this.cityControl = new FormControl<string>(this.cities.at(0)!.name, Validators.required);
        this.cityControl.setValue(this.cities.at(0)!.name);
      this.isLoading = false;
      });


      this.postPayload = {
        name: '',
        lastName: '',
        email: '',
        address: '',
        department: '',
        token: '',
        password: '',
        isActivated: true,
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
        name: new FormControl( '', Validators.required),
        lastName: new FormControl( '', Validators.required),
        email: new FormControl( '', Validators.required),
        address: new FormControl( '', Validators.required),
        phone: new FormControl( '', Validators.required),
        skills: new FormControl( '', Validators.required),
        password: new FormControl( '', Validators.required),
        isActivated: new FormControl('', Validators.required),
        department: new FormControl('', Validators.required),
        token: new FormControl('', Validators.required),
        languages: new FormControl('', Validators.required),
        enrollDate: new FormControl('' , Validators.required),
        dateOfBirth: new FormControl( '', Validators.required),
        cityName: new FormControl( '', Validators.required),
        image: new FormControl('', Validators.required),
      });
      this.isLoading = false;
  }
private async fetchCities(){
    let observable = this.cityService.getCities();
    return await lastValueFrom(observable);
  }
private async fetchCityByStudentId(studentId: number) {
    let observable = this.studentService.getCityByStudent(studentId);
    return await lastValueFrom(observable);
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
