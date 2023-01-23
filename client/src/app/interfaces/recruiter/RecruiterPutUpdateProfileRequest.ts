

// PUT /Recruiter/id/update-profile
export interface RecruiterPutUpdateProfileRequest {
  address: string;
  hireDate: Date;
  phone: string;
  dateOfBirth: Date;
  companyName: string;
  image: ArrayBuffer | string | null
}
