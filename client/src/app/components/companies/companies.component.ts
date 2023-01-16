import { Component } from '@angular/core';
import {City} from "../../interfaces/City";
import {Company} from "../../interfaces/Company";



const _city: City = new class implements City {
  name = "Istanbul";
}

const companies: Company[] = [
  {
    name: "Metal YAPI",
    companyType: "Constructor",
    email: "metalyapi@gmail.com",
    foundationDate: new Date(1960, 2, 25),
    address: "Kavacık",
    phone: "909090909",
    city: _city,
  },
  {
    name: "Turk Telekom",
    companyType: "Communcation",
    email: "turktelekom@gmail.com",
    foundationDate: new Date(1960, 2, 25),
    address: "Kavacık",
    phone: "909090909",
    city: _city,
  },
  {
    name: "Is Bankasi",
    companyType: "Finance",
    email: "isbankasi@gmail.com",
    foundationDate: new Date(1960, 2, 25),
    address: "Kavacık",
    phone: "909090909",
    city: _city,
  },
  {
    name: "E Vision",
    companyType: "Mobile Application",
    email: "evision@gmail.com",
    foundationDate: new Date(1960, 2, 25),
    address: "Kavacık",
    phone: "909090909",
    city: _city,
  },
]
@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent {
  constructor() {
    this.refreshCompanies();
  }
  page = 1;
  pageSize = 4;
  // collectionSize = COUNTRIES.length;
  // countries!: Country[];
  collectionSize = companies.length;
  companies!: Company[];
  public refreshCompanies() {
    this.companies = companies.map((company, i ) => ({id: i+1, ...company})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }
}
