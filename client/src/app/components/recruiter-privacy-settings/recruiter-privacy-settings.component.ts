import {Component, OnInit} from '@angular/core';
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Company} from "../../interfaces/company/Company";
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {RecruiterGetKeyResponse} from "../../interfaces/recruiter/RecruiterGetKeyResponse";
import {lastValueFrom} from "rxjs";
import {UpdatePassword} from "../../interfaces/UpdatePassword";
import {UpdatePasswordResponse} from "../../interfaces/UpdatePasswordResponse";

@Component({
  selector: 'app-recruiter-privacy-settings',
  templateUrl: './recruiter-privacy-settings.component.html',
  styleUrls: ['./recruiter-privacy-settings.component.css']
})
export class RecruiterPrivacySettingsComponent implements OnInit{
  recruiterId!: number;
  recruiter!: RecruiterGetResponse;
  companies!: Company[];
  createPostForm!: FormGroup;
  postPayload!: UpdatePassword;
  loading: boolean = false;

  isOwnPage!: boolean;
  isLoading: boolean = true;
  constructor(private router: Router,
              private activatedRoute: ActivatedRoute,
              private recruiterService: RecruiterService,
              private snackBar: MatSnackBar) {

  }
  send() {

    this.postPayload.prevPassword = this.createPostForm.get('prevPassword')!.value;
    this.postPayload.newPassword = this.createPostForm.get('newPassword')!.value;
    this.postPayload.newPasswordCopy = this.createPostForm.get('newPasswordCopy')!.value;

      const obj: UpdatePassword = {
        prevPassword: this.createPostForm.get('prevPassword')!.value,
        newPassword: this.createPostForm.get('newPassword')!.value,
        newPasswordCopy: this.createPostForm.get('newPasswordCopy')!.value,
      };
      if((obj.prevPassword  == null || obj.prevPassword.trim() == "")
        ||(obj.newPassword  == null || obj.newPassword.trim() == "")
        ||(obj.newPasswordCopy  == null || obj.newPasswordCopy.trim() == "")) {
        this.snackBar.open("You must fill all the fields", "OK", {
          duration: 4000
        });
      } else {
        if(obj.newPassword != obj.newPasswordCopy){
          this.snackBar.open("Passwords are not matching", "OK", {
            duration: 4000
          });
        } else {
          console.log(obj);
          this.recruiterService.changePassword(this.recruiterId,obj).subscribe((passwordResponse: UpdatePasswordResponse) => {
            console.log(passwordResponse);
            if(passwordResponse && passwordResponse.status){
              this.snackBar.open(passwordResponse.message, "OK", {
                duration: 5000
              });
              this.router.navigate(['/profile/recruiter/'+this.recruiterId]);
            } else {
              this.snackBar.open(passwordResponse.message, "OK", {
                duration: 5000
              });
            }
          });

        }
      }
  }

  ngOnInit(): void {
    this.recruiterId = this.activatedRoute.snapshot.params['recruiterId'];
    this.fetchTokenByRecruiterId(this.recruiterId).then((tokenResponse: RecruiterGetKeyResponse) => {
      this.isLoading = true;
      if(tokenResponse !== null && tokenResponse.key !== null && tokenResponse.key === localStorage.getItem("key")) {
        console.log("Inside if token");
        console.log(tokenResponse);
        this.isOwnPage = true;
        this.fetchRecruiter(this.recruiterId).then((data) => {
          this.recruiter = data;
          if(data !== null){
            this.recruiter = data;
            this.postPayload = {
              prevPassword: '',
              newPassword: '',
              newPasswordCopy: ''
            };

            this.createPostForm = new FormGroup({
              prevPassword: new FormControl('', Validators.required),
              newPassword: new FormControl('', Validators.required),
              newPasswordCopy: new FormControl('', Validators.required),
            });
          } else {
            console.log("Inside else 1.");
            console.log(tokenResponse);
            this.snackBar.open("404 Not Found", "OK", {
              duration: 10000
            });
            this.isOwnPage = false;
            this.router.navigate(['/not-found']);
          }
        });
        this.isLoading = false;
      } else {
        console.log("Inside else");
        this.snackBar.open("You are unauthorized", "OK", {
          duration: 10000
        });
        this.isOwnPage = false;
        this.router.navigate(['/not-found']);
      }
    });
  }

  private async fetchRecruiter(id: number){
    let recruiterById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(recruiterById);
  }
  private async fetchTokenByRecruiterId(id: number){
    let observable = this.recruiterService.getTokenByRecruiterId(id);
    return await lastValueFrom(observable);
  }


}
