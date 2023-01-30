import {Component, OnInit} from '@angular/core';
import {Company} from "../../interfaces/company/Company";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {CityGetResponse} from "../../interfaces/city/CityGetResponse";
import {City} from "../../interfaces/city/City";
import {ActivatedRoute, Router} from "@angular/router";
import {CityService} from "../../services/city/city.service";
import {CompanyService} from "../../services/company/company.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {lastValueFrom} from "rxjs";


interface UpdateProfile {
  name: string;
  email: string;
  companyType: string;
  address: string;
  foundationDate: Date;
  phone: string,
  cityName: string,
  // image: ArrayBuffer | string | null
}
@Component({
  selector: 'app-add-company',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.css']
})
export class AddCompanyComponent  implements OnInit {
  cityControl!: FormControl;
  cities!: CityGetResponse[];
  createPostForm!: FormGroup;
  postPayload!: UpdateProfile;
  shortLink: string = "";
  loading: boolean = false;
  file!: File; //
  fileBlob! :ArrayBuffer;
  // colleges!: City[];
  selectedCity!: City;
  isLoading: boolean = true;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private cityService: CityService,
              private companyService: CompanyService,
              private fileService: ImageUploadService,
              private snackBar: MatSnackBar) {

  }
  send() {

    this.postPayload.address = this.createPostForm.get('address')!.value;
    this.postPayload.foundationDate = this.createPostForm.get('foundationDate')!.value;
    this.postPayload.phone = this.createPostForm.get('phone')!.value;
    this.postPayload.cityName = this.cityControl!.value; // college yao
    // this.postPayload.image = this.createPostForm.get('image')!.value;

    //console.log(this.companyControl.value);
    // const reader = new FileReader();
    // reader.readAsDataURL(this.file); //FileStream response from .NET core backend
    // reader.onload = _event => {
    // console.log(reader.result);
    const obj = {
      name: this.createPostForm.get('name')!.value,
      address: this.createPostForm.get('address')!.value,
      email: this.createPostForm.get('email')!.value,
      foundationDate: this.createPostForm.get('foundationDate')!.value,
      phone: this.createPostForm.get('phone')!.value,
      companyType: this.createPostForm.get('companyType')!.value,
      cityName:  this.cityControl!.value,
      companyKey: this.createPostForm.get('companyKey')!.value,
      // image: reader.result!.toString().slice(23, reader.result!.toString().length),
    };
    console.log(obj);
    this.addCompany(obj).then((data) => {
      if(data){
        console.log(data);
        this.snackBar.open("Company has been added successfully", "OK", {
          duration: 4000
        });
        this.router.navigate(['/admin/companies/']);
      } else {
        this.snackBar.open("Error!! Bad Request", "OK", {
          duration: 4000
        });
      }
    }).catch((error) => {
      console.log(error);
      this.router.navigate(['/admin/companies/']);
    })

  }
  private async addCompany( obj: any){
    let objectObservable = this.companyService.addCompany( obj);
    return await lastValueFrom(objectObservable);
  }
  ngOnInit(): void {


        this.fetchCities().then((_city: CityGetResponse[]) => {
          console.log(_city);
          this.cities = _city;
          this.cityControl = new FormControl<string>(this.cities.at(0)!.name, Validators.required);
          this.cityControl.setValue(this.cities.at(0)!.name);
        });



        this.postPayload = {
          name: '',
          email: '',
          companyType: '',
          address: '',
          foundationDate: new Date(),
          phone: '',
          cityName: '',
          // image: this.fileBlob,
        };

        this.createPostForm = new FormGroup({
          name: new FormControl( '', Validators.required),
          email: new FormControl( '', Validators.required),
          address: new FormControl( '', Validators.required),
          phone: new FormControl( '', Validators.required),
          companyType: new FormControl( '', Validators.required),
          companyKey: new FormControl('', Validators.required),
          foundationDate: new FormControl( '', Validators.required),
          cityName: new FormControl( '', Validators.required),
          // image: new FormControl('', Validators.required),
        });
        this.isLoading = false;
      }
      //this.router.navigate(['/not-found'])
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
  private async fetchCompany(id: number){
    let companyById = this.companyService.getCompanyById(id);
    return await lastValueFrom(companyById);
  }
  private async fetchCities(){
    let observable = this.cityService.getCities();
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
