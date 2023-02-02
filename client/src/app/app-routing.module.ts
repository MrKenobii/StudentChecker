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
import {AdminGuard} from "./auth/admin.guard";
import {UnauthorizedComponent} from "./components/unauthorized/unauthorized.component";
import {AdminPanelComponent} from "./admin/admin-panel/admin-panel.component";
import {AdminRequestsComponent} from "./admin/admin-requests/admin-requests.component";
import {AdminComponent} from "./admin/admin/admin.component";
import {AuthGuard} from "./auth/auth.guard";
import {CompanyTableComponent} from "./admin/company-table/company-table.component";
import {RecruiterTableComponent} from "./admin/recruiter-table/recruiter-table.component";
import {AdminStudentTableComponent} from "./admin/admin-student-table/admin-student-table.component";
import {EditStudentComponent} from "./admin/edit-student/edit-student.component";
import {EditCompanyComponent} from "./admin/edit-company/edit-company.component";
import {EditRecruiterComponent} from "./admin/edit-recruiter/edit-recruiter.component";
import {AddCompanyComponent} from "./admin/add-company/add-company.component";
import {AddStudentComponent} from "./admin/add-student/add-student.component";
import {AddRecruiterComponent} from "./admin/add-recruiter/add-recruiter.component";
import {ChatRoomComponent} from "./components/chat-room/chat-room.component";
import {StudentChatRoomComponent} from "./components/student-chat-room/student-chat-room.component";
import {StudentsMessageBoxComponent} from "./components/students-message-box/students-message-box.component";
import {RecruitersMessageBoxComponent} from "./components/recruiters-message-box/recruiters-message-box.component";
import {FeedComponent} from "./components/feed/feed.component";



const routes: Route[] = [
  {
    path: '', component: FeedComponent
  },
  {
    path: 'feed', redirectTo: ''
  },
  {
    path: 'about', component: HomeComponent
  },
  {
    path: 'students', component: StudentTableComponent
  },
  {
    path: 'companies', component: CompaniesComponent
  },
  {
    path: 'sign-up', component: SignUpComponent, canActivate: [AuthGuard]
  },
  {
    path: 'sign-up/student', component: SignUpComponent, canActivate: [AuthGuard]
  },
  {
    path: 'sign-up/recruiter', component: SignUpRecruiterComponent, canActivate: [AuthGuard]
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
    path: 'login', component: LoginComponent, canActivate: [AuthGuard]
  },
  {
   path: 'chat-room/:id',component: ChatRoomComponent
  },
  {
   path: 'student-chat-room/:id',component: StudentChatRoomComponent
  },
  {
   path: 'student-message-box/:id',component: StudentsMessageBoxComponent
  },
  {
   path: 'recruiter-message-box/:id',component: RecruitersMessageBoxComponent
  },
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      {
        path: '', component: AdminDashboardComponent
      },
      {
        path:'dashboard', component: AdminPanelComponent, canActivate: [AdminGuard]
      },
      {
        path:'requests', component: AdminRequestsComponent, canActivate: [AdminGuard]
      },
      {
        path:'students', component: AdminStudentTableComponent, canActivate: [AdminGuard]
      },
      {
        path:'companies', component: CompanyTableComponent, canActivate: [AdminGuard]
      },
      {
        path:'recruiters', component: RecruiterTableComponent, canActivate: [AdminGuard]
      },
      {
        path:'edit-student/:studentId', component: EditStudentComponent, canActivate: [AdminGuard]
      },
      {
        path:'edit-company/:companyId', component: EditCompanyComponent, canActivate: [AdminGuard]
      },
      {
        path:'edit-recruiter/:recruiterId', component: EditRecruiterComponent, canActivate: [AdminGuard]
      },
      {
        path:'add-company', component: AddCompanyComponent, canActivate: [AdminGuard]
      },
      {
        path:'add-student', component: AddStudentComponent, canActivate: [AdminGuard]
      },
      {
        path:'add-recruiter', component: AddRecruiterComponent, canActivate: [AdminGuard]
      },
    ],
    canActivate: [AdminGuard],
  },
  {
    path: 'not-found', component: NotFoundComponent
  },
  {
    path: 'forbidden', component: UnauthorizedComponent
  }
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
