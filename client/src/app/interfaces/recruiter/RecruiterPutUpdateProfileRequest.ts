

// PUT /Recruiter/id/update-profile
export interface RecruiterPutUpdateProfileRequest {
  address: string;
  hireDate: string;
  phone: string;
  dateOfBirth: Date;
  companyName: string;
  image: string; // ?????????  ----------> Write a Response Classes for Backend and Frontend !!!!!!!!
}
