import {Course} from "../course/Course";

export interface CompleteStudentProfile {
  collegeName: string;
  department: string;
  address: string;
  enrollDate: Date;
  phone: string;
  dateOfBirth: Date;
  skills: string;
  cityName: string;
  languages: string;
  courses: Course[] | null;
  image: ArrayBuffer | string | null // ??
}

