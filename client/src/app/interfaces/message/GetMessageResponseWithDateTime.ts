export interface GetMessageResponseWithDateTime{
  studentId: number;
  recruiterId: number;
  content: string;
  sendTime?: Date;
  deliveredTime?: Date
}
