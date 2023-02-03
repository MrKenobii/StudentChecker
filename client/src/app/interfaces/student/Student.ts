import {College} from "../college/College";
import {City} from "../city/City";

export interface Student{
  id: number;
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
