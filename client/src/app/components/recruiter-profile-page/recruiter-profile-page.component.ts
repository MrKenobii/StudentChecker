import { Component } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterGetKeyResponse} from "../../interfaces/recruiter/RecruiterGetKeyResponse";
import {last, lastValueFrom} from "rxjs";
interface CompanyDto {
  id: number,
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  cityName: string;
  companyKey: string;
}
@Component({
  selector: 'app-recruiter-profile-page',
  templateUrl: './recruiter-profile-page.component.html',
  styleUrls: ['./recruiter-profile-page.component.css']
})
export class RecruiterProfilePageComponent {
  recruiterId: number;
  recruiter!: RecruiterGetResponse;
  isOwnProfilePage: boolean =false;
  companies!: CompanyDto[];
  activeCompany!: CompanyDto;
  constructor(private activatedRoute: ActivatedRoute,
              private recruiterService: RecruiterService,
              private router: Router,
              private snackBar: MatSnackBar) {
    this.recruiterId = this.activatedRoute.snapshot.params['recruiterId'];
    this.activatedRoute.params.subscribe(val => {
      this.recruiter = this.activatedRoute.snapshot.params['recruiterId'];
    });
    this.fetchRecruiterById(this.recruiterId).then((data: RecruiterGetResponse) => {
      if(data !== null){
        this.recruiter = data;
        console.log(data);
        this.recruiter.image = this.recruiter.image = "data:image/png;base64," + this.recruiter.image;
        console.log(this.recruiter);
        this.getCompaniesByRecruiterId(this.recruiterId).then((data: CompanyDto[]) => {
          if(data.length > 0){
            this.companies = data;
            console.log("This.companies");
            console.log(this.companies);
            this.activeCompany= this.companies.at(0)!;
            console.log(this.activeCompany);
          } else this.snackBar.open("No Companies retrieved", "OK", {
            duration: 4000
          });
        });
      } else this.router.navigate(['/not-found']);
    }).catch((error) => {
      this.snackBar.open("Error!!: " + error, "OK", {
        duration: 3000
      });
      this.router.navigate(['/not-found']);
    });
    this.fetchTokenByRecruiterId(this.recruiterId).then((data: RecruiterGetKeyResponse) => {
      this.isOwnProfilePage = localStorage.getItem("key") !== null && localStorage.getItem("key") == data.key;
    }).catch((error) => {
      this.snackBar.open("Error!! " + error, "OK", {
        duration: 3000
      });
      this.router.navigate(['/not-found']);
    });
  }
  private async fetchRecruiterById(id: number) {
    let recruiterById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(recruiterById);
  }
  private async fetchTokenByRecruiterId(id: number) {
    let tokenByRecruiterId = this.recruiterService.getTokenByRecruiterId(id);
    return await lastValueFrom(tokenByRecruiterId);
  }
  private async getCompaniesByRecruiterId(id: number){
   let _companies = this.recruiterService.getCompanies(id);
   return await lastValueFrom(_companies);
  }
  ngOnInit(): void {
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

    return `${date}/${month}/${year}`;
  }
  editProfile() {
    this.router.navigate(['recruiter/edit-profile/'+this.recruiterId]);
    console.log("Inside edit Profile");
  }

  privacySettings() {
    this.router.navigate(["/recruiter/privacy/" + this.recruiterId]);
  }

  chatRoom() {
    this.router.navigate(["/chat-room/" + this.recruiterId]);
  }
}
