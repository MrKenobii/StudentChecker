import {Component, OnInit} from '@angular/core';
import {Company} from "../../interfaces/company/Company";
import {CompanyService} from "../../services/company.service";






@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent implements OnInit{
  constructor(private companyService: CompanyService) {

    //this.refreshCompanies();

  }
  ngOnInit(): void {
    this.companyService.getCompanies().subscribe((data: any) =>{
      this.companies = data;
      console.log(data);
      this.collectionSize = this.companies.length;
      this.refreshCompanies();
    });
  }
  page = 1;
  pageSize = 4;
  companies!: Company[];
  dumbCompanies!: Company[];
  collectionSize!: number;
  public refreshCompanies() {
    this.dumbCompanies = this.companies;
    this.dumbCompanies = this.dumbCompanies.map((company ) => ( {...company})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }

}
