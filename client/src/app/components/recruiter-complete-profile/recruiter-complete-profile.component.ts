import {Component, OnInit} from '@angular/core';
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {ActivatedRoute, Router} from "@angular/router";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {RecruiterPutUpdateProfileRequest} from "../../interfaces/recruiter/RecruiterPutUpdateProfileRequest";
import {Company} from "../../interfaces/company/Company";
import {CompanyService} from "../../services/company/company.service";
import {lastValueFrom} from "rxjs";


interface CompanyControlName {
  name: string;
}
@Component({
  selector: 'app-recruiter-complete-profile',
  templateUrl: './recruiter-complete-profile.component.html',
  styleUrls: ['./recruiter-complete-profile.component.css']
})
export class RecruiterCompleteProfileComponent implements OnInit{
  recruiterId!: number;
  recruiter!: RecruiterGetResponse;
  companyControl = new FormControl<Company[] | null>(null, Validators.required);
  _companies!: Company[];
  companies!: Company[];
  createPostForm!: FormGroup;
  postPayload!: RecruiterPutUpdateProfileRequest;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  isAccountActive!: boolean;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private recruiterService: RecruiterService,
              private companyService: CompanyService,
              private fileService: ImageUploadService) {
    this.postPayload = {
      address: '',
      hireDate: new Date(),
      phone: '',
      dateOfBirth: new Date(),
      companyName: "",
      image: this.fileBlob,
    };
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
        hireDate: this.createPostForm.get('hireDate')!.value,
        phone: this.createPostForm.get('phone')!.value,
        dateOfBirth: this.createPostForm.get('dateOfBirth')!.value,
        companyName: this.companyControl!.value,
        image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      this.recruiterService.updateProfile(this.recruiterId,obj).subscribe(data => {
        console.log(data);
        this.router.navigate(['/login']);
      });
    }
  }
  private async fetchRecruiterById(id: number) {
    let recruiterById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(recruiterById);
  }

  ngOnInit(): void {
    this.recruiterId = this.activatedRoute.snapshot.params['recruiterId'];
    this.fetchRecruiterById(this.recruiterId).then((data: RecruiterGetResponse) => {
      if(data && data.name && data.lastName && !data.hireDate && !data.dateOfBirth){
        this.isAccountActive = data.isActivated;
        if(this.isAccountActive){
          this.recruiter = data;
        } else {
          this.router.navigate(['/not-found'])
        }
      }

    }).catch((error) =>  this.router.navigate(['/not-found']));
    this.companyService.getCompanies().subscribe((data: Company[]) => {
      this.companies = data;
    })
    this.createPostForm = new FormGroup({
      address: new FormControl('', Validators.required),
      hireDate: new FormControl('', Validators.required),
      phone: new FormControl('', Validators.required),
      dateOfBirth: new FormControl(new Date(), Validators.required),
      image: new FormControl('', Validators.required),
      companyName: new FormControl('', Validators.required),
    });
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
