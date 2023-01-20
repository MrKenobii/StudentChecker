import {City} from "../city/City";
import {CollegeResponse} from "../college/CollegeResponse";

export interface CompanyPostResponse{
  id: number;
  name: string;
  companyType: string;
  email: string;
  foundationDate: Date;
  address: string;
  phone: string;
  city: City;
  colleges: CollegeResponse[]; // student devam
  companyKey: string;
}
