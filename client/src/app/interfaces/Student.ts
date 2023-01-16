import {College} from "./College";
import {City} from "./City";

export interface Student{
  name: string;
  lastName: string;
  email: string;
  enrollDate: Date;
  dateOfBirth: Date;
  address: string;
  skills: string;
  city: City;
  phone: string;
  isActivated: boolean;
  department: string;
  college: College
  image: string;
}
