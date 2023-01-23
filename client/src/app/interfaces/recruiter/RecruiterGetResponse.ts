// GET /Recruiter
// POST, PUT
export interface RecruiterGetResponse{
  id: number;
  name: string;
  lastName: string;
  email: string;
  password: string;
  hireDate: Date;
  dateOfBirth: Date;
  address: string;
  phone: string;
  isActivated: boolean;
  image: ArrayBuffer | string | null;
}
