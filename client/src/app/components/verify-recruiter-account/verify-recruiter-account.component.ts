import { Component } from '@angular/core';
import {VerifyAccountRequest} from "../../interfaces/student/VerifyAccountRequest";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {VerifyTokenResponse} from "../../interfaces/student/VerifyTokenResponse";
import {lastValueFrom} from "rxjs";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";

@Component({
  selector: 'app-verify-recruiter-account',
  templateUrl: './verify-recruiter-account.component.html',
  styleUrls: ['./verify-recruiter-account.component.css']
})
export class VerifyRecruiterAccountComponent {
  recruiterId!: number;
  postPayload!: VerifyAccountRequest;
  createPostForm!: FormGroup;
  isAccountActive!: boolean;

  constructor(private router: Router, private activatedRoute: ActivatedRoute,private recruiterService: RecruiterService, private matSnackBar: MatSnackBar) {
    this.recruiterId = this.activatedRoute.snapshot.params['recruiterId'];
    console.log(this.recruiterId);
    this.fetchRecruiterById(this.recruiterId).then((data: RecruiterGetResponse) => {
      if(data && data.name && data.lastName){
        this.isAccountActive = data.isActivated;
        if(!this.isAccountActive){
          this.postPayload = {
            verifyToken: '',
          }
        }
      } else {
        this.router.navigate(['/not-found']);
      }
    }).catch((error) => this.router.navigate(['/not-found']));
  }
  ngOnInit(): void {
    this.createPostForm = new FormGroup({
      verifyToken: new FormControl('', Validators.required),

    });
  }
  private async fetchRecruiterById(id: number) {
    let recruiterById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(recruiterById);
  }
  send() {
    this.postPayload.verifyToken = this.createPostForm.get('verifyToken')!.value;
    console.log(this.postPayload.verifyToken);
    console.log(this.recruiterId);
    this.recruiterService.verifyAccount(
      this.recruiterId, this.postPayload)
      .subscribe((data: VerifyTokenResponse) => {
        console.log(data);
        if(data.name != null){
          this.router.navigateByUrl(`login`);
          this.matSnackBar.open("Thanks for your registration. Please wait for your account to be confirmed", "Close", {
            duration: 3000
          });
        } else {
          this.matSnackBar.open(data.message, "Close", {
            duration: 5000
          });
        }
      });
  }

}
