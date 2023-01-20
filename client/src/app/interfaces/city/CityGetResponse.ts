import {StudentResponse} from "../student/StudentResponse";
import {CollegeResponse} from "../college/CollegeResponse";
import {CompanyResponse} from "../company/CompanyResponse";


// GET /City
export interface CityGetResponse {
  id: number;
  name: string;
  colleges: CollegeResponse[],
  students: StudentResponse[],
  companies: CompanyResponse[],
}
