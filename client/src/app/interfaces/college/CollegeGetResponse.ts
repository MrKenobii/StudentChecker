
// GET /College
import {City} from "../city/City";
import {StudentResponse} from "../student/StudentResponse";

export interface CollegeGetResponse {
  id: number;
  name: string;
  foundationDate: Date;
  emailExtension: string;
  city: City;
  student: StudentResponse[];
}
