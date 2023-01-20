import {CollegeResponse} from "../college/CollegeResponse";

interface C {
  id: number;
  name: string;
  foundationDate: Date;
  emailExtension: string;
}
export interface CityGetCollegesResponse{
  colleges: C[]
}
