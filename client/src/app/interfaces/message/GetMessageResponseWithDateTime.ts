import {Student} from "../student/Student";
import {RecruiterGetResponse} from "../recruiter/RecruiterGetResponse";

export interface GetMessageResponseWithDateTime{
  studentId: number;
  recruiterId: number;
  content: string;
  sendTime?: Date;
  deliveredTime?: Date
  student: Student
  recruiter: RecruiterGetResponse
}
