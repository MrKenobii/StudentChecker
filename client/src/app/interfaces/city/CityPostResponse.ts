// POST /City
// PUT /City
import {CompanyResponse} from "../company/CompanyResponse";
import {CollegeResponse} from "../college/CollegeResponse";
import {StudentResponse} from "../student/StudentResponse";

export interface CityPostResponse{
  id: number;
  name: string;
  companies: CompanyResponse[],
  colleges: CollegeResponse[],
  students: StudentResponse[],
}
