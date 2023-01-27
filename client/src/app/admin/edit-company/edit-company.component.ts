import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {ImageUploadService} from "../../services/image-upload/image-upload.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {lastValueFrom} from "rxjs";
import {City} from "../../interfaces/city/City";
import {CityService} from "../../services/city.service";
import {CompanyService} from "../../services/company/company.service";
import {Company} from "../../interfaces/company/Company";
import {CityGetResponse} from "../../interfaces/city/CityGetResponse";

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
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styleUrls: ['./edit-company.component.css']
})
export class EditCompanyComponent implements OnInit {
  companyId!: number;
  company!: Company;
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
  formattedFoundationDAte!: string;
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
        companyKey: "KEY"
        // image: reader.result!.toString().slice(23, reader.result!.toString().length),
      };
      console.log(obj);
      this.updateCompany(this.companyId, obj).then((data) => {
        if(data){
          console.log(data);
          this.snackBar.open("Company has updated successfully", "OK", {
            duration: 4000
          });
          this.router.navigate(['/admin/companies/']);
        } else {
          this.snackBar.open("Error!! Bad Request", "OK", {
            duration: 4000
          });
        }
      })

  }
  private async updateCompany(id: number, obj: any){
    let objectObservable = this.companyService.updateCompany(id, obj);
    return await lastValueFrom(objectObservable);
  }
  ngOnInit(): void {
    this.companyId = this.activatedRoute.snapshot.params['companyId'];


      this.fetchCompany(this.companyId).then((data: Company) => {
        this.isLoading = true;
        console.log(data);
        this.company = data;
        if(data !== null){
          this.company = data;
          this.formattedFoundationDAte = this.format(new Date(this.company!.foundationDate)); // noluyyyyoyo
          console.log("Checkkkk");
          console.log(this.company);


          console.log("Checkpoint");
          var courses: string[] = []
          this.fetchCities().then((_city: CityGetResponse[]) => {
            console.log(_city);
            this.cities = _city;
            this.fetchCityByCompanyId(this.companyId).then((city: any) => {
              this.selectedCity = city;
              console.log(this.cities);
              console.log(this.selectedCity.name);
              this.cityControl = new FormControl<string>(this.selectedCity.name, Validators.required);
              this.cityControl.setValue(this.selectedCity.name);
            });
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
            name: new FormControl(this.company ? this.company.name : '', Validators.required),
            email: new FormControl(this.company ? this.company.email : '', Validators.required),
            address: new FormControl(this.company ? this.company.address : '', Validators.required),
            phone: new FormControl(this.company ? this.company.phone : '', Validators.required),
            companyType: new FormControl(this.company ? this.company.companyType : '', Validators.required),
            foundationDate: new FormControl(this.company ? this.format(new Date(this.company.foundationDate)) : '', Validators.required),
            cityName: new FormControl(this.company ? this.company.city.name : '', Validators.required),
            // image: new FormControl('', Validators.required),
          });
          this.isLoading = false;
        }
        //this.router.navigate(['/not-found'])
      }).catch((error) => console.log(error));
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

  private async fetchCityByCompanyId(companyId: number) {
    let cityByCompanyId = this.companyService.getCityByCompanyId(companyId);
    return await lastValueFrom(cityByCompanyId);
  }
}
