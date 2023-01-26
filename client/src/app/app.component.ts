import { Component } from '@angular/core';
import {AdminService} from "./services/admin/admin.service";
import {lastValueFrom, Observable} from "rxjs";
import {AdminGetResponse} from "./interfaces/admin/AdminGetResponse";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isAdmin!: boolean;
  constructor(private adminService: AdminService) {

    this.fetchAdminByToken().then((data: AdminGetResponse) => {
      if(data){
        if(data?.id && data?.token && data.name == "A_Anil"){
          this.isAdmin = true;
        }
      } else this.isAdmin = false;
    }).then(() => console.log(this.isAdmin));
  }
  title = 'Student Checker';

  private async fetchAdminByToken(){
    let adminByToken = this.adminService.getAdminByToken(localStorage.getItem("key")!);
      return await lastValueFrom(adminByToken);
  }
}
