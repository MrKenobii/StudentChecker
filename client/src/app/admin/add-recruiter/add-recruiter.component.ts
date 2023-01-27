import { Component } from '@angular/core';
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Company} from "../../interfaces/company/Company";
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {CompanyService} from "../../services/company/company.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {lastValueFrom} from "rxjs";
import {RecruiterGetKeyResponse} from "../../interfaces/recruiter/RecruiterGetKeyResponse";

interface UpdateProfile {
  name: string;
  lastName: string;
  email: string;
  password: string;
  address: string;
  token: string;
  isActivated: boolean;
  hireDate: Date;
  phone: string;
  dateOfBirth: Date;
  companyName: string;
  image: ArrayBuffer | string | null
}
@Component({
  selector: 'app-add-recruiter',
  templateUrl: './add-recruiter.component.html',
  styleUrls: ['./add-recruiter.component.css']
})
export class AddRecruiterComponent {
  companyControl = new FormControl<Company[] | null>(null, Validators.required);
  companies!: Company[];
  createPostForm!: FormGroup;
  postPayload!: UpdateProfile;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  formattedHireDate!: string;
  isOwnPage!: boolean;
  isLoading: boolean = true;
  token!: string;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private recruiterService: RecruiterService,
              private companyService: CompanyService,
              private fileService: ImageUploadService,
              private snackBar: MatSnackBar) {

  }
  send() {

    this.postPayload.address = this.createPostForm.get('address')!.value;
    this.postPayload.hireDate = this.createPostForm.get('hireDate')!.value;
    this.postPayload.phone = this.createPostForm.get('phone')!.value;
    this.postPayload.dateOfBirth = this.createPostForm.get('dateOfBirth')!.value;
    this.postPayload.companyName = this.companyControl.value!.at(0)!.name;
    this.postPayload.image = this.createPostForm.get('image')!.value;
    const reader = new FileReader();
    reader.readAsDataURL(this.file); //FileStream response from .NET core backend
    reader.onload = _event => {
      // console.log(reader.result);
      const obj = {
        address: this.createPostForm.get('address')!.value,
        name: this.createPostForm.get('name')!.value,
        lastName: this.createPostForm.get('lastName')!.value,
        email: this.createPostForm.get('email')!.value,
        password: this.createPostForm.get('password')!.value,
        hireDate: this.createPostForm.get('hireDate')!.value,
        phone: this.createPostForm.get('phone')!.value,
        token: this.createPostForm.get('token') ?  this.createPostForm.get('token')!.value :  "ABC",
        isActivated: true,
        dateOfBirth: this.createPostForm.get('dateOfBirth')!.value,
        companyName: this.companyControl!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      this.addRecruiter(obj).then((data) => {
        if(data){
          this.snackBar.open("Recruiter has been added", "OK", {
            duration: 4000
          });
          this.router.navigate(['/admin/recruiters/']);
        } else {
          this.snackBar.open("Error!! Bad Request", "OK", {
            duration: 4000
          });
        }
      });
    }
  }
  ngOnInit(): void {
    this.companyService.getCompanies().subscribe((data: Company[]) => {
      this.isLoading = true;
      console.log(data);
      this.companies = data;
      this.isLoading = false;
    });
    this.postPayload = {
      name: '',
      lastName: '',
      email: '',
      address: '',
      password: '',
      token: '',
      isActivated: false,
      phone: '',
      hireDate: new Date(),
      dateOfBirth: new Date(),
      companyName: '',
      image: this.fileBlob,
    };

    this.createPostForm = new FormGroup({
      name: new FormControl('', Validators.required),
      lastName: new FormControl( '', Validators.required),
      email: new FormControl(  '', Validators.required),
      address: new FormControl( '', Validators.required),
      phone: new FormControl(  '', Validators.required),
      token: new FormControl( '', Validators.required),
      password: new FormControl( '', Validators.required),
      isActivated: new FormControl( '', Validators.required),
      hireDate: new FormControl('' , Validators.required),
      dateOfBirth: new FormControl('', Validators.required),
      companyName: new FormControl(this.companies ? this.companies.at(0) : '', Validators.required),
      image: new FormControl('', Validators.required),
    });
    this.isLoading = true;
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

  private async addRecruiter(obj: any) {
    let addRecruiter = this.recruiterService.addRecruiter(obj);
    return await lastValueFrom(addRecruiter);
  }
}
