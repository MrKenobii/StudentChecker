import { NgModule } from '@angular/core';
import {Route, RouterModule, Routes} from '@angular/router';
import {StudentTableComponent} from "./components/student-table/student-table.component";
import {HomeComponent} from "./components/home/home.component";
import {CompaniesComponent} from "./components/companies/companies.component";
import {SignUpComponent} from "./components/sign-up/sign-up.component";
import {LoginComponent} from "./components/login/login.component";
import {SignUpRecruiterComponent} from "./components/sign-up-recruiter/sign-up-recruiter.component";
import {VerifyStudentAccountComponent} from "./components/verify-student-account/verify-student-account.component";
import {
  VerifyStudentAutomaticComponent
} from "./components/verify-student-automatic/verify-student-automatic.component";
import {
  StudentCompleteProfileComponent
} from "./components/student-complete-profile/student-complete-profile.component";
import {ProfilePageComponent} from "./components/profile-page/profile-page.component";
import {
  RecruiterCompleteProfileComponent
} from "./components/recruiter-complete-profile/recruiter-complete-profile.component";
import {RecruiterProfilePageComponent} from "./components/recruiter-profile-page/recruiter-profile-page.component";
import {
  StudentProfileEditPageComponent
} from "./components/student-profile-edit-page/student-profile-edit-page.component";
import {
  RecruiterProfileEditPageComponent
} from "./components/recruiter-profile-edit-page/recruiter-profile-edit-page.component";


const routes: Route[] = [
  {
    path: '', component: HomeComponent
  },
  {
    path: 'students', component: StudentTableComponent
  },
  {
    path: 'companies', component: CompaniesComponent
  },
  {
    path: 'sign-up', component: SignUpComponent
  },
  {
    path: 'sign-up/student', component: SignUpComponent
  },
  {
    path: 'sign-up/recruiter', component: SignUpRecruiterComponent
  },
  {
    path: 'sign-up/activate/:studentId', component: VerifyStudentAccountComponent
  },
  {
    path: 'activate/:studentId', component: VerifyStudentAutomaticComponent,
  },
  {
    path: 'complete-profile/:studentId', component: StudentCompleteProfileComponent,
  },
  {
    path: 'recruiter/complete-profile/:recruiterId', component: RecruiterCompleteProfileComponent,
  },
  {
    path: 'profile/student/:studentId', component: ProfilePageComponent,
  },
  {
    path: 'student/edit-profile/:studentId', component: StudentProfileEditPageComponent,
  },
  {
    path: 'profile/recruiter/:recruiterId', component: RecruiterProfilePageComponent,
  },
  {
    path: 'recruiter/edit-profile/:recruiterId', component: RecruiterProfileEditPageComponent,
  },
  {
    path: 'login', component: LoginComponent
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
