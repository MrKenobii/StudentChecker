import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {AdminGetResponse} from "../../interfaces/admin/AdminGetResponse";
import {lastValueFrom} from "rxjs";
import {AdminService} from "../../services/admin/admin.service";
import {StudentService} from "../../services/student/student.service";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {MatSnackBar} from "@angular/material/snack-bar";
import {DeleteResponse} from "../../interfaces/DeleteResponse";
import {AdminActivateAccountResponse} from "../../interfaces/admin/AdminActivateAccountResponse";

@Component({
  selector: 'app-admin-requests',
  templateUrl: './admin-requests.component.html',
  styleUrls: ['./admin-requests.component.css']
})
export class AdminRequestsComponent implements OnInit{
  isAdmin!: boolean;
  token!: string;
  status: boolean = false;
  admin!: AdminGetResponse;
  isLoading: boolean = true;
  requests!: string[];
  studentRequests!: any[];
  recruiterRequests!: RecruiterGetResponse[];
  constructor(private router: Router, private adminService: AdminService, private studentService: StudentService, private recruiterService: RecruiterService, private snackBar: MatSnackBar) {

    if (localStorage.getItem("key")){
      this.token = (localStorage.getItem("key") as string)
      console.log(this.token);
      this.fetchAdminByToken(this.token).then((response: AdminGetResponse) => {
        this.isLoading = true;
        if((response.id && response.name && response.email) && (response.token === this.token)) {
          this.admin =response;
          this.fetchStudents().then((data: any) => {
            console.log(data);
            this.studentRequests = data.filter((s: any) => !s.isActivated);
          }).then(() => {
            this.fetchRecruiters().then((recruiters: RecruiterGetResponse[]) => {
              console.log(recruiters);
              this.recruiterRequests = recruiters.filter(r => !r.isActivated);
            }).catch((error) => this.snackBar.open("Error!!!", "OK", {
              duration: 5000
            }));
          }).catch((error) => this.snackBar.open("Error!!!", "OK", {
            duration: 5000
          }));
          console.log(this.admin);
          this.isAdmin = true;
        } else {
          this.isAdmin = false;
          this.router.navigate(['/forbidden']);
        }
        this.isLoading = false;
      });
    } else this.router.navigate(['/forbidden']);

  }
  private async fetchStudents(){
    let students = this.studentService.getStudents();
    return await lastValueFrom(students);
  }
  private async fetchRecruiters(){
    let recruiters = this.recruiterService.getRecruiters();
    return await lastValueFrom(recruiters);
  }
  ngOnInit() {
  }
  private async fetchAdminByToken(token: string) {
    let adminByToken = this.adminService.getAdminByToken(token);
    return await lastValueFrom(adminByToken);
  }

  deleteRecruiter(id: number) {
    console.log(id);
    this.recruiterService.deleteRecruiter(id).subscribe((data: DeleteResponse) => {
      console.log(data);
      this.recruiterRequests = this.recruiterRequests.filter(s => s.id != id);
      this.snackBar.open(data.message, "OK", {
        duration: 5000
      });
    });
  }

  confirmRecruiter(id: number) {
    this.recruiterService.confirmRecruiter(id).subscribe((data: AdminActivateAccountResponse) => {
      this.recruiterRequests = this.recruiterRequests.filter(s => s.id != id);
      this.snackBar.open(data.message, "OK", {
        duration: 5000
      });
    });
  }

  confirmStudent(id: number) {
    this.studentService.confirmStudent(id).subscribe((data: AdminActivateAccountResponse) => {
      console.log(data);
      this.studentRequests = this.studentRequests.filter(s => s.id != id);
      this.snackBar.open(data.message, "OK", {
        duration: 5000
      });
    });
  }

  deleteStudent(id: number) {
    this.studentService.deleteStudent(id).subscribe((data: DeleteResponse) => {
      console.log(data);
      this.studentRequests = this.studentRequests.filter(s => s.id != id);
      this.snackBar.open(data.message, "OK", {
        duration: 5000
      });
    });
  }
}
