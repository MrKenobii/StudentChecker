import {City} from "../city/City";


//  GET /Company
export interface CompanyGetResponse {
  id: number,
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  city: City;
  companyKey: string;
}
