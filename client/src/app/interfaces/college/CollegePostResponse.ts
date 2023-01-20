import {City} from "../city/City";
import {StudentResponse} from "../student/StudentResponse";

// POST PUT
export interface CollegePostResponse { // change Backend
  id: number;
  name: string;
  foundationDate: Date;
  emailExtension: string;
  city: City;
  student: StudentResponse[];
}
