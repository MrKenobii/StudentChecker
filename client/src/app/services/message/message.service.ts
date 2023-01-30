import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {GetMessageResponse} from "../../interfaces/message/GetMessageResponse";
import {PostMessageRequest} from "../../interfaces/message/PostMessageRequest";
import {RecruiterStudentMessageResponse} from "../../interfaces/message/RecruiterStudentMessageResponse";
import {GetMessageResponseWithDateTime} from "../../interfaces/message/GetMessageResponseWithDateTime";

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private httpClient: HttpClient) { }

  public getSendMessages(id: number) : Observable<GetMessageResponse[]> {
    return this.httpClient.get<GetMessageResponse[]>(`http://localhost:5269/Message/send`);
  }
  public getDeliveredMessages(id: number) : Observable<GetMessageResponse[]> {
    return this.httpClient.get<GetMessageResponse[]>(`http://localhost:5269/Message/delivered`);
  }
  public getSendMessageStudentById(id: number) : Observable<GetMessageResponseWithDateTime[]> {
    return this.httpClient.get<GetMessageResponseWithDateTime[]>(`http://localhost:5269/Message/send/student/${id}`);
  }
  public getSendDeliveredStudentById(id: number) : Observable<GetMessageResponseWithDateTime[]> {
    return this.httpClient.get<GetMessageResponseWithDateTime[]>(`http://localhost:5269/Message/delivered/student/${id}`);
  }
  public getSendMessageRecruiterById(id: number) : Observable<GetMessageResponseWithDateTime[]> {
    return this.httpClient.get<GetMessageResponseWithDateTime[]>(`http://localhost:5269/Message/send/recruiter/${id}`);
  }
  public getSendDeliveredRecruiterById(id: number) : Observable<GetMessageResponseWithDateTime[]> {
    return this.httpClient.get<GetMessageResponseWithDateTime[]>(`http://localhost:5269/Message/delivered/recruiter/${id}`);
  }
  public postRecruiterToStudentMessage(payload: PostMessageRequest) : Observable<RecruiterStudentMessageResponse> {
    return this.httpClient.post<RecruiterStudentMessageResponse>(`http://localhost:5269/Message/recruiter-student`, payload);
  }
  public postStudentToRecruiterMessage(payload: PostMessageRequest) : Observable<RecruiterStudentMessageResponse> {
    return this.httpClient.post<RecruiterStudentMessageResponse>(`http://localhost:5269/Message/student-recruiter`, payload);
  }
}
