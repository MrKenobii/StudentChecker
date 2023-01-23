export interface StudentResponse{
  id: number,
  name: string;
  lastName: string;
  email: string;
  enrollDate: Date;
  dateOfBirth: Date;
  address: string;
  skills: string;
  cityName: string;
  phone: string;
  isActivated: boolean;
  department: string;
  collegeName: string
  image: string;
  formattedEnrollDate: string;
  formattedDob: string;
}
