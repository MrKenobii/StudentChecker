import {Component, OnInit} from '@angular/core';
import {CompanyService} from "../../services/company/company.service";
import {Company} from "../../interfaces/company/Company";
import {DeleteResponse} from "../../interfaces/DeleteResponse";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-company-table',
  templateUrl: './company-table.component.html',
  styleUrls: ['./company-table.component.css']
})
export class CompanyTableComponent  implements OnInit{
  constructor(private companyService: CompanyService, private snackBar: MatSnackBar) {

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
    this.companyService.getCompanies().subscribe((data: Company[]) =>{
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

  deleteCompany(id: number) {
    this.companyService.deleteCompanyById(id).subscribe((res: DeleteResponse) => {
      if(res){
        this.dumbCompanies.filter(s => s.id != id);
        this.companies.filter(s => s.id != id);
        this.snackBar.open(res.message, "OK", {
          duration: 4000
        });
      } else {
        this.snackBar.open("Delete failure", "OK", {
          duration: 4000
        });
      }
    })
  }

  addCompany() {

  }
}

