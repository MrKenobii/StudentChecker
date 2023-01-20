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
    ImageUploadComponent
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
        MatNativeDateModule
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
