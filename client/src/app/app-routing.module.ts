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
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {
  StudentPrivacySettingsComponent
} from "./components/student-privacy-settings/student-privacy-settings.component";
import {
  RecruiterPrivacySettingsComponent
} from "./components/recruiter-privacy-settings/recruiter-privacy-settings.component";
import {
  VerifyRecruiterAutomaticComponent
} from "./components/verify-recruiter-automatic/verify-recruiter-automatic.component";
import {
  VerifyRecruiterAccountComponent
} from "./components/verify-recruiter-account/verify-recruiter-account.component";
import {AdminDashboardComponent} from "./admin/admin-dashboard/admin-dashboard.component";
import {AuthGuard} from "./auth/auth.guard";
import {UnauthorizedComponent} from "./components/unauthorized/unauthorized.component";


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
    path: 'sign-up/student/activate/:studentId', component: VerifyStudentAccountComponent
  },
  {
    path: 'activate/student/:studentId', component: VerifyStudentAutomaticComponent,
  },
  {
    path: 'sign-up/recruiter/activate/:recruiterId', component: VerifyRecruiterAccountComponent
  },
  {
    path: 'activate/recruiter/:recruiterId', component: VerifyRecruiterAutomaticComponent,
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
    path: 'student/privacy/:studentId', component: StudentPrivacySettingsComponent,
  },
  {
    path: 'profile/recruiter/:recruiterId', component: RecruiterProfilePageComponent,
  },
  {
    path: 'recruiter/edit-profile/:recruiterId', component: RecruiterProfileEditPageComponent,
  },
  {
    path: 'recruiter/privacy/:recruiterId', component: RecruiterPrivacySettingsComponent,
  },
  {
    path: 'login', component: LoginComponent
  },
  {
    path: 'admin', component: AdminDashboardComponent,
    // canActivate: [AuthGuard]
  },
  {
    path: 'not-found', component: NotFoundComponent
  },
  {
    path: 'forbidden', component: UnauthorizedComponent
  },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
