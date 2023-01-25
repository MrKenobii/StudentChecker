import { Component } from '@angular/core';
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {lastValueFrom} from "rxjs";

@Component({
  selector: 'app-verify-recruiter-automatic',
  templateUrl: './verify-recruiter-automatic.component.html',
  styleUrls: ['./verify-recruiter-automatic.component.css']
})
export class VerifyRecruiterAutomaticComponent {
  recruiterId!: number;
  verifyToken!: string;
  requestPayload!: VerifyAccountRequest
  isAccountActive!: boolean;
  constructor(private router: Router, private activatedRoute: ActivatedRoute,private recruiterService: RecruiterService, private matSnackBar: MatSnackBar) {
    this.recruiterId = this.activatedRoute.snapshot.params['recruiterId'];
    this.fetchRecruiterById(this.recruiterId).then((data: RecruiterGetResponse) => {
      this.verifyToken = this.activatedRoute.snapshot.queryParams['token'];
      console.log(this.recruiterId);
      console.log(this.verifyToken);
      if(data && data.name && data.lastName){
        this.isAccountActive = data.isActivated;
        if(!this.isAccountActive){
          this.requestPayload = {
            verifyToken: '',
          }
          this.requestPayload.verifyToken = this.verifyToken;
          this.recruiterService.verifyAccount(this.recruiterId, this.requestPayload)
            .subscribe((data: VerifyTokenResponse) => {
              if(data.name != null) {
                this.router.navigateByUrl(`recruiter/complete-profile/${this.recruiterId}`);
              } else {
                this.matSnackBar.open(data.message, "Close", {
                  duration: 3000
                });
              }
            });
        }
      } else {
        this.router.navigate(['/not-found']);
      }
    }).catch((error) => this.router.navigate(['/not-found']));
  }
  send() {
    this.router.navigateByUrl("login");
  }
  private async fetchRecruiterById(id: number) {
    let recruiterById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(recruiterById);
  }
}
