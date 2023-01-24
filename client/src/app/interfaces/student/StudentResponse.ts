export interface StudentResponse{
  id: number,
  name: string;
  lastName: string;
  email: string;
  password: string;
  enrollDate: Date;
  dateOfBirth: Date;
  address: string;
  languages: string;
  skills: string;
  phone: string;
  department: string;
  collegeName: string
  image: ArrayBuffer | string | null;
  cityName: string;
  isActivated: boolean;
}
