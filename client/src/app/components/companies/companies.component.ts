import {Component, OnInit} from '@angular/core';
import {Company} from "../../interfaces/company/Company";
import {CompanyService} from "../../services/company/company.service";
import {City} from "../../interfaces/city/City";



interface DumbCompany{
  id: number,
  name: string;
  companyType: string;
  email: string;
  foundationDate: string;
  address: string;
  phone: string;
  city: City;

}


@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css']
})
export class CompaniesComponent implements OnInit{
  constructor(private companyService: CompanyService) {

    //this.refreshCompanies();

  }
  format(inputDate: Date) {
    let date, month, year;

    date = inputDate.getDate();
    month = inputDate.getMonth() + 1;
    year = inputDate.getFullYear();

    date = date
      .toString()
      .padStart(2, '0');

    month = month
      .toString()
      .padStart(2, '0');

    return `${date}/${month}/${year}`;
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
    this.dumbCompanies.map((c, i) => {
      c.formattedDate =this.format(new Date(c.foundationDate));
    });
    this.dumbCompanies = this.dumbCompanies.map((company ) => ( { ...company})).slice(
      (this.page -1) * this.pageSize,
      (this.page -1) * this.pageSize + this.pageSize
    );
  }

}
