import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {CollegeService} from "../../services/college/college.service";
import {CollegeGetResponse} from "../../interfaces/college/CollegeGetResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CompleteStudentProfile} from "../../interfaces/student/CompleteStudentProfile";
import {Course} from "../../interfaces/course/Course";
import {CoursesService} from "../../services/course/courses.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {lastValueFrom} from "rxjs";
import {StudentGetTokenResponse} from "../../interfaces/student/StudentGetTokenResponse";
import {CityGetResponse} from "../../interfaces/city/CityGetResponse";
import {CityService} from "../../services/city/city.service";


@Component({
  selector: 'app-student-complete-profile',
  templateUrl: './student-complete-profile.component.html',
  styleUrls: ['./student-complete-profile.component.css']
})
export class StudentCompleteProfileComponent implements OnInit{
  studentId!: number;
  collegeEmailExtension!: string;
  colleges!: CollegeGetResponse[];
  cities!: CityGetResponse[];
  college!: CollegeGetResponse;
  student!: StudentResponse;
  createPostForm!: FormGroup;
  postPayload: CompleteStudentProfile;
  _courses!: Course[];
  courses!: Course[];
  courseControl = new FormControl<Course[] | null>(null, Validators.required);
  cityControl!: FormControl;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  isAccountActive!: boolean;
  isLoading!: boolean;
  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private studentService: StudentService,
              private courseService: CoursesService,
              private cityService: CityService,
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
    this.fetchStudentById(this.studentId).then((data: StudentResponse) => {
      if(data && data.name && data.lastName && !data.collegeName && !data.address){
        this.isLoading = true;
        this.isAccountActive = data.isActivated;
        console.log(data);
        if(this.isAccountActive){
          this.student = data;
          this.collegeEmailExtension = this.student.email.substring(data.email.indexOf('@') + 1, data.email.length);
          this.fetchColleges().then((data: any) =>{
            this.isLoading = true;
            console.log(data);
            this.colleges = data;
            console.log(this.collegeEmailExtension);
            this.colleges.map((c: CollegeGetResponse) => {
              if(c.emailExtension == this.collegeEmailExtension){
                this.college = c;
              }
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
            console.log(this.college);
            this.isLoading = false;
          });


        } else {
          this.router.navigate(['/not-found'])
        }
      }
    }).catch((error) => this.router.navigate(['/not-found']));
    this.courseService.getCourses().subscribe((data: Course[]) => {
      this.courses = data;
    })
  }
  private async fetchStudentById(id: number) {
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }
  private async fetchColleges() {
    let observable = this.collegeService.getColleges();
    return await lastValueFrom(observable);
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
        cityName: this.cityControl!.value,
      };
      console.log(obj);
      this.updateProfile(this.studentId, obj).then((data: any) => {
        console.log(data);
        if(data && data.id > 0){
          this.fetchTokenByStudentId(data.id!).then((token: StudentGetTokenResponse) => {
            if(token && token.key){
              localStorage.setItem("key", token.key);
              this.router.routeReuseStrategy.shouldReuseRoute = function () {
                return false;
              }
              this.router.onSameUrlNavigation = 'reload';
              this.router.navigate(['/']).then(() => {
                window.location.reload();
              });
            } else {
              this.matSnackBar.open("Someth went wrong", "OK", {
                duration: 5000
              });
            }
          });
        } else {
          this.matSnackBar.open("Someth went wrong -2", "OK", {
            duration: 5000
          });
          this.router.navigate(['not-found']);
        }
      });

    };
  }
  private async updateProfile(id: number, obj: any){
    let observable = this.studentService.updateProfile(id, obj);
    return await lastValueFrom(observable);
  }
  private async fetchTokenByStudentId(id: number){
    let tokenByStudentId = this.studentService.getTokenByStudentId(id);
    return await lastValueFrom(tokenByStudentId);
  }
  private async fetchCities(){
    let observable = this.cityService.getCities();
    return await lastValueFrom(observable);
  }
  private async fetchCityByStudentId(studentId: number) {
    let observable = this.studentService.getCityByStudent(studentId);
    return await lastValueFrom(observable);
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
