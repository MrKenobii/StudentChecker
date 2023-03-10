import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatToolbarModule} from '@angular/material/toolbar'
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import {MatIconModule} from "@angular/material/icon";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatButtonModule} from "@angular/material/button";
import { FooterComponent } from './components/footer/footer.component';
import { StudentTableComponent } from './components/student-table/student-table.component';
import {MatTableModule} from "@angular/material/table";
import {MatPaginatorModule} from "@angular/material/paginator";
import {RouterLink, RouterOutlet} from "@angular/router";
import {AppRoutingModule} from "./app-routing.module";
import { CompaniesComponent } from './components/companies/companies.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SignUpRecruiterComponent } from './components/sign-up-recruiter/sign-up-recruiter.component';
import {HttpClientModule} from "@angular/common/http";
import {MatInputModule} from "@angular/material/input";
import {MatSelectModule} from "@angular/material/select";
import { VerifyStudentAccountComponent } from './components/verify-student-account/verify-student-account.component';
import {MatSnackBarModule} from "@angular/material/snack-bar";
import { VerifyStudentAutomaticComponent } from './components/verify-student-automatic/verify-student-automatic.component';
import { StudentCompleteProfileComponent } from './components/student-complete-profile/student-complete-profile.component';
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from '@angular/material/core';
import { ImageUploadComponent } from './components/image-upload/image-upload.component';
import { ProfilePageComponent } from './components/profile-page/profile-page.component';
import { RecruiterCompleteProfileComponent } from './components/recruiter-complete-profile/recruiter-complete-profile.component';
import { RecruiterProfilePageComponent } from './components/recruiter-profile-page/recruiter-profile-page.component';
import { RecruiterProfileEditPageComponent } from './components/recruiter-profile-edit-page/recruiter-profile-edit-page.component';
import { StudentProfileEditPageComponent } from './components/student-profile-edit-page/student-profile-edit-page.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { LoaderComponent } from './components/loader/loader.component';
import { RecruiterPrivacySettingsComponent } from './components/recruiter-privacy-settings/recruiter-privacy-settings.component';
import { StudentPrivacySettingsComponent } from './components/student-privacy-settings/student-privacy-settings.component';
import { VerifyRecruiterAutomaticComponent } from './components/verify-recruiter-automatic/verify-recruiter-automatic.component';
import { VerifyRecruiterAccountComponent } from './components/verify-recruiter-account/verify-recruiter-account.component';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { UnauthorizedComponent } from './components/unauthorized/unauthorized.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { AdminRequestsComponent } from './admin/admin-requests/admin-requests.component';
import { AdminComponent } from './admin/admin/admin.component';
import { RecruiterTableComponent } from './admin/recruiter-table/recruiter-table.component';
import { CompanyTableComponent } from './admin/company-table/company-table.component';
import { AdminStudentTableComponent } from './admin/admin-student-table/admin-student-table.component';
import { EditCompanyComponent } from './admin/edit-company/edit-company.component';
import { EditStudentComponent } from './admin/edit-student/edit-student.component';
import { EditRecruiterComponent } from './admin/edit-recruiter/edit-recruiter.component';
import { AddRecruiterComponent } from './admin/add-recruiter/add-recruiter.component';
import { AddCompanyComponent } from './admin/add-company/add-company.component';
import { AddStudentComponent } from './admin/add-student/add-student.component';
import { MessageComponent } from './components/message/message.component';
import { ChatRoomComponent } from './components/chat-room/chat-room.component';
import { StudentsMessageBoxComponent } from './components/students-message-box/students-message-box.component';
import { RecruitersMessageBoxComponent } from './components/recruiters-message-box/recruiters-message-box.component';
import { StudentChatRoomComponent } from './components/student-chat-room/student-chat-room.component';
import {MatCardModule} from "@angular/material/card";
import {MatListModule} from "@angular/material/list";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import { FeedComponent } from './components/feed/feed.component';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { PipeComponent } from './components/pipe/pipe.component';



@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    StudentTableComponent,
    CompaniesComponent,
    HomeComponent,
    LoginComponent,
    SignUpComponent,
    SignUpRecruiterComponent,
    VerifyStudentAccountComponent,
    VerifyStudentAutomaticComponent,
    StudentCompleteProfileComponent,
    ImageUploadComponent,
    ProfilePageComponent,
    RecruiterCompleteProfileComponent,
    RecruiterProfilePageComponent,
    RecruiterProfileEditPageComponent,
    StudentProfileEditPageComponent,
    NotFoundComponent,
    LoaderComponent,
    RecruiterPrivacySettingsComponent,
    StudentPrivacySettingsComponent,
    VerifyRecruiterAutomaticComponent,
    VerifyRecruiterAccountComponent,
    AdminDashboardComponent,
    UnauthorizedComponent,
    AdminPanelComponent,
    AdminRequestsComponent,
    AdminComponent,
    RecruiterTableComponent,
    CompanyTableComponent,
    AdminStudentTableComponent,
    EditCompanyComponent,
    EditStudentComponent,
    EditRecruiterComponent,
    AddRecruiterComponent,
    AddCompanyComponent,
    AddStudentComponent,
    MessageComponent,
    ChatRoomComponent,
    StudentsMessageBoxComponent,
    RecruitersMessageBoxComponent,
    StudentChatRoomComponent,
    FeedComponent,
    PipeComponent,
  ],
  imports: [
    BrowserModule,
    MatToolbarModule,
    MatIconModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    RouterOutlet,
    RouterLink,
    AppRoutingModule,
    FormsModule,
    NgbModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatListModule,
    MatProgressBarModule,
    Ng2SearchPipeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
