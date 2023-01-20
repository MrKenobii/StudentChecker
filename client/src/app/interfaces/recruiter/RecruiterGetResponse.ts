// GET /Recruiter
// POST, PUT
export interface RecruiterGetResponse{
  id: number;
  name: string;
  email: string;
  password: string;
  hireDate: Date;
  dateOfBirth: Date;
  address: string;
  phone: string;
  token: string;
  image: string; // ??????
  isActivated: boolean;
  //recruiterCompanies fixxxxx !!!
}
