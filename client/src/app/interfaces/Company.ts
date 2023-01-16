import {City} from "./City";

export interface Company{
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  city: City;

}
