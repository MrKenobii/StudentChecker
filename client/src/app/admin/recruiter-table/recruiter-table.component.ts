import { Component } from '@angular/core';
import {CompanyService} from "../../services/company/company.service";
import {Company} from "../../interfaces/company/Company";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {lastValueFrom} from "rxjs";
import {Router} from "@angular/router";
import {DeleteResponse} from "../../interfaces/DeleteResponse";
import {MatSnackBar} from "@angular/material/snack-bar";

interface FormattedRecruiters {
  id: number;
  name: string;
  lastName: string;
  email: string;
  password: string;
  hireDate: Date;
  dateOfBirth: Date;
  address: string;
  phone: string;
  isActivated: boolean;
  image: ArrayBuffer | string | null;
  formattedDob: string;
  formattedEnrollDate: string;
}
@Component({
  selector: 'app-recruiter-table',
  templateUrl: './recruiter-table.component.html',
  styleUrls: ['./recruiter-table.component.css']
})
export class RecruiterTableComponent {
  recruiters!: RecruiterGetResponse[];
  page = 1;
  pageSize = 4;

  dumbRecruiters: FormattedRecruiters[] = [];
  collectionSize!: number;
  constructor(private router: Router,private recruiterService: RecruiterService, private snackBar: MatSnackBar) {

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


  ngOnInit(): void {
    this.fetchRecruiters().then((data: RecruiterGetResponse[]) => {
      console.log(data);
      if(data && data.length > 0){
        this.recruiters = data;
        this.recruiters.map(recruiter => {
          recruiter.image = "data:image/png;base64," + recruiter.image;
        });
        console.log("recruiter");
        this.collectionSize = this.recruiters.length;
        this.refreshRecruiters();
      }
    });
  }
  private async fetchRecruiters(){
    let recruiters1 = this.recruiterService.getRecruiters();
    console.log(recruiters1);
    return await lastValueFrom(recruiters1);
  }

  public refreshRecruiters() {
    this.recruiters.map((recruiter: RecruiterGetResponse, index: number) => {
      this.dumbRecruiters[index] = {...recruiter, formattedDob: this.format(new Date(recruiter.dateOfBirth)) , formattedEnrollDate: this.format(new Date(recruiter.hireDate)) };
    });
    console.log(this.recruiters);
    console.log(this.dumbRecruiters);

    this.dumbRecruiters = this.dumbRecruiters.map((recruiter ) => ({ ...recruiter})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }

  deleteRecruiter(id: number) {
    console.log(id);
    this.recruiterService.deleteRecruiter(id).subscribe((data: DeleteResponse) => {
      if(data){
        this.dumbRecruiters.filter(s => s.id != id);
        this.recruiters.filter(s => s.id != id);
        this.snackBar.open(data.message, "OK", {
          duration: 4000
        });
      } else {
        this.snackBar.open("Something went wrong", "OK", {
          duration: 4000
        });
      }
    });
  }

  editRecruiter(id: number) {
    this.router.navigate(['/admin/edit-recruiter/'+id]);
  }
}
