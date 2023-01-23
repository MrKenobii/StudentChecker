import {Component, OnInit} from '@angular/core';
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Company} from "../../interfaces/company/Company";
import {RecruiterPutUpdateProfileRequest} from "../../interfaces/recruiter/RecruiterPutUpdateProfileRequest";
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {CompanyService} from "../../services/company/company.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";

interface CompanyDto {
  id: number,
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  cityName: string;
  formattedDate: string;
}
interface UpdateProfile {
  name: string;
  lastName: string;
  email: string;
  address: string;
  password: string;
  hireDate: Date;
  phone: string;
  dateOfBirth: Date;
  companyName: string;
  image: ArrayBuffer | string | null
}
@Component({
  selector: 'app-recruiter-profile-edit-page',
  templateUrl: './recruiter-profile-edit-page.component.html',
  styleUrls: ['./recruiter-profile-edit-page.component.css']
})
export class RecruiterProfileEditPageComponent implements OnInit{
  recruiterId!: number;
  recruiter!: RecruiterGetResponse;
  companyControl = new FormControl<Company[] | null>(null, Validators.required);
  _companies!: Company[];
  companies!: CompanyDto[];
  createPostForm!: FormGroup;
  postPayload!: UpdateProfile;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  formattedHireDate!: string;
  deneme: string = "2010-06-13";
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private recruiterService: RecruiterService,
              private companyService: CompanyService,
              private fileService: ImageUploadService) {

  }
  send() {

    this.postPayload.address = this.createPostForm.get('address')!.value;
    this.postPayload.hireDate = this.createPostForm.get('hireDate')!.value;
    this.postPayload.phone = this.createPostForm.get('phone')!.value;
    this.postPayload.dateOfBirth = this.createPostForm.get('dateOfBirth')!.value;
    this.postPayload.companyName = this.companyControl.value!.at(0)!.name;
    this.postPayload.image = this.createPostForm.get('image')!.value;
    //console.log(this.companyControl.value);
    const reader = new FileReader();
    reader.readAsDataURL(this.file); //FileStream response from .NET core backend
    reader.onload = _event => {
      // console.log(reader.result);
      const obj = {
        address: this.createPostForm.get('address')!.value,
        name: this.createPostForm.get('name')!.value,
        lastName: this.createPostForm.get('lastName')!.value,
        email: this.createPostForm.get('email')!.value,
        password: this.createPostForm.get('newPassword')!.value,
        hireDate: this.createPostForm.get('hireDate')!.value,
        phone: this.createPostForm.get('phone')!.value,
        dateOfBirth: this.createPostForm.get('dateOfBirth')!.value,
        companyName: this.companyControl!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      // this.recruiterService.updateProfile(this.recruiterId,obj).subscribe(data => {
      //   console.log(data);
      //   this.router.navigate(['/login?role=recruiter']);
      // });
    }
  }

  ngOnInit(): void {
    this.recruiterId = this.activatedRoute.snapshot.params['recruiterId'];
    this.recruiterService.getRecruiterById(this.recruiterId).subscribe((data: RecruiterGetResponse) => {
      this.recruiter = data;
      this.formattedHireDate = this.format(new Date(this.recruiter.hireDate)) || this.deneme; // noluyyyyoyo
      console.log("HireDate");
      console.log(this.recruiter.hireDate);
      console.log("Formatted Date");
      console.log(this.formattedHireDate);
      console.log(this.deneme);

    });

    console.log("Checkpoint")
    this.recruiterService.getCompanies(this.recruiterId).subscribe((data: CompanyDto[]) => {
      console.log(data);
      this.companies = data;
    })
    this.postPayload = {
      name: '',
      lastName: '',
      email: '',
      address: '',
      password: '',
      phone: '',
      hireDate: new Date(),
      dateOfBirth: new Date(),
      companyName: '',
      image: this.fileBlob,
    };
    this.createPostForm = new FormGroup({
      name: new FormControl(this.recruiter.name, Validators.required),
      lastName: new FormControl(this.recruiter.lastName, Validators.required),
      prevPassword: new FormControl('', Validators.required),
      newPassword: new FormControl('', Validators.required),
      email: new FormControl(this.recruiter.email, Validators.required),
      address: new FormControl('', Validators.required),
      hireDate: new FormControl('', Validators.required),
      phone: new FormControl('', Validators.required),
      dateOfBirth: new FormControl('', Validators.required),
      image: new FormControl('', Validators.required),
      companyName: new FormControl(this.companies.at(0)!, Validators.required),
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