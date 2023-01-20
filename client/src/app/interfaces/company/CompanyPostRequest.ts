// POST /Company
// PUT /Company
export interface CompanyPostRequest {
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  cityName: string;
  companyKey: string;
}
