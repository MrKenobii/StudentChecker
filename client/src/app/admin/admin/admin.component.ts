import {Component, OnInit} from '@angular/core';
import {AdminGetResponse} from "../../interfaces/admin/AdminGetResponse";
import {Router} from "@angular/router";
import {AuthService} from "../../services/admin/admin.service";
import {lastValueFrom} from "rxjs";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit{
  ngOnInit() {
  }
}
