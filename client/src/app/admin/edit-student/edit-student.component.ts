import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Course} from "../../interfaces/course/Course";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";
import {StudentService} from "../../services/student/student.service";
import {CoursesService} from "../../services/course/courses.service";
import {CollegeService} from "../../services/college/college.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";
import {lastValueFrom} from "rxjs";
import {City} from "../../interfaces/city/City";
import {CityGetResponse} from "../../interfaces/city/CityGetResponse";
import {CityService} from "../../services/city/city.service";

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
  selector: 'app-edit-student',
  templateUrl: './edit-student.component.html',
  styleUrls: ['./edit-student.component.css']
})
export class EditStudentComponent implements OnInit {
  studentId!: number;
  student!: StudentResponse;
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
        isActivated: this.createPostForm.get("isActivated")!.value,
        collegeName: this.collegeControl!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      this.updateStudent(this.studentId, obj).then((data) => {
        if(data){
          console.log(data);
          this.snackBar.open("Profile has updated successfully", "OK", {
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
  private async updateStudent(id: number, obj: any){
    let objectObservable = this.studentService.updateStudent(id, obj);
    return await lastValueFrom(objectObservable);
  }
  ngOnInit(): void {
    this.studentId = this.activatedRoute.snapshot.params['studentId'];
    this.fetchTokenByStudentId(this.studentId).then((tokenResponse: StudentGetTokenResponse) => {
        this.token = tokenResponse.key;
        console.log(this.token);
        this.isLoading = true;
        console.log("Doğruu");
        this.isOwnPage = true;
        this.fetchStudent(this.studentId).then((data) => {
          console.log("Cehckkkk");
          console.log(data);
          this.student = data;
          if(data !== null){
            this.student = data;
            this.formattedEnrolledDate = this.format(new Date(this.student!.enrollDate)); // noluyyyyoyo
            console.log("Cehckkkk");
            console.log(this.student);


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
            this.fetchCities().then((_city: CityGetResponse[]) => {
              console.log(_city);
              this.cities = _city;
              this.fetchCityByStudentId(this.studentId).then((city: any) => {
                this.selectedCity = city;
                console.log(this.cities);
                console.log(this.selectedCity.name);
                this.cityControl = new FormControl<string>(this.selectedCity.name, Validators.required);
                this.cityControl.setValue(this.selectedCity.name);
              });
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
              name: new FormControl(this.student ? this.student.name : '', Validators.required),
              lastName: new FormControl(this.student ? this.student.lastName : '', Validators.required),
              email: new FormControl(this.student ? this.student.email : '', Validators.required),
              address: new FormControl(this.student ? this.student.address : '', Validators.required),
              phone: new FormControl(this.student ? this.student.phone : '', Validators.required),
              skills: new FormControl(this.student ? this.student.skills : '', Validators.required),
              password: new FormControl(this.student ? this.student.password : '', Validators.required),
              isActivated: new FormControl(this.student ? this.student.isActivated : '', Validators.required),
              department: new FormControl(this.student ? this.student.department : '', Validators.required),
              token: new FormControl(this.student ? this.token : '', Validators.required),
              languages: new FormControl(this.student ? this.student.languages : '', Validators.required),
              enrollDate: new FormControl(this.student ? this.format(new Date(this.student.enrollDate)) : '' , Validators.required),
              dateOfBirth: new FormControl(this.student ? this.format(new Date(this.student.dateOfBirth)) : '', Validators.required),
              cityName: new FormControl(this.student ? this.student.cityName : '', Validators.required),
              image: new FormControl('', Validators.required),
            });
            this.isLoading = false;
          }
        }).catch((error) => this.router.navigate(['/not-found']));
    }).catch((error) => this.router.navigate(['/not-found']));
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
  private async fetchTokenByStudentId(id: number) {
    let observable = this.studentService.getTokenByStudentId(id);
    return await lastValueFrom(observable);
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


