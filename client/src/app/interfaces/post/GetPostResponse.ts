import {RecruiterGetResponse} from "../recruiter/RecruiterGetResponse";
import {Student} from "../student/Student";

export interface GetPostResponse{
  id: number;
  content: string;
  title: string;
  recruiter?: RecruiterGetResponse;
  student?: Student
  createdTime: Date
}
