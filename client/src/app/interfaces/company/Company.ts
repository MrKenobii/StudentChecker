import {City} from "../city/City";

export interface Company{
  id: number,
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  city: City;
  formattedDate: string;

}
