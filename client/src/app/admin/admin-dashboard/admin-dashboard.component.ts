import {Component, OnInit} from '@angular/core';
import {AuthService} from "../../services/admin/admin.service";
import {Router} from "@angular/router";
import {lastValueFrom} from "rxjs";
import {AdminGetResponse} from "../../interfaces/admin/AdminGetResponse";
import {AdminLoginResponse} from "../../interfaces/admin/AdminLoginResponse";

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  isAdmin!: boolean;
  token!: string;
  status: boolean = false;
  admin!: AdminGetResponse;
  isLoading: boolean = true;
  constructor(private router: Router, private adminService: AuthService) {
    if (localStorage.getItem("key") ){
      this.token = (localStorage.getItem("key") as string)
      console.log(this.token);
      this.fetchAdminByToken(this.token).then((response: AdminGetResponse) => {
        this.isLoading = true;
        if((response.id && response.name && response.email) && (response.token === this.token)) {
          this.admin =response;
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
  clickEvent(){
    this.status = !this.status;
  }

  ngOnInit(): void {
  }

  private async fetchAdminByToken(token: string) {
    let adminByToken = this.adminService.getAdminByToken(token);
    return await lastValueFrom(adminByToken);
  }
}
