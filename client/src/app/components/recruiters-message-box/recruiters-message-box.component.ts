import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {MessageService} from "../../services/message/message.service";
import {lastValueFrom} from "rxjs";
import {RecruiterGetKeyResponse} from "../../interfaces/recruiter/RecruiterGetKeyResponse";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {GetMessageResponseWithDateTime} from "../../interfaces/message/GetMessageResponseWithDateTime";
import {GetMessageResponse} from "../../interfaces/message/GetMessageResponse";
import {StudentService} from "../../services/student/student.service";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import * as moment from 'moment';
interface UpdatedRecruiterDeliveredMessages {
  studentId: number;
  recruiterId: number;
  content: string;
  sendTime?: Date;
  deliveredTime?: Date
  student: StudentResponse;

}
@Component({
  selector: 'app-recruiters-message-box',
  templateUrl: './recruiters-message-box.component.html',
  styleUrls: ['./recruiters-message-box.component.css']
})
export class RecruitersMessageBoxComponent implements OnInit{
  id!: number;
  isLoading!: boolean;
  deliveredMessages!: GetMessageResponseWithDateTime[];
  updatedDeliveredMessages: any[] = [];
  recruiter!: RecruiterGetResponse;

  constructor(private activatedRoute: ActivatedRoute,
              private router: Router,
              private recruiterService: RecruiterService,
              private studentService: StudentService,
              private messageService: MessageService) {
    this.id = this.activatedRoute.snapshot.params['id'];
    this.activatedRoute.params.subscribe(() => {
      this.id = this.activatedRoute.snapshot.params['id'];
    });
  }
  ngOnInit(): void {
    if(localStorage.getItem("key")){
      this.getRecruiterByToken(localStorage.getItem("key") as string).then((d: RecruiterGetResponse) => {
        this.isLoading = true;
        if(d.id == this.id){
          this.recruiter = d;
          this.getSendDeliveredRecruiterById(this.id).then((deliveredM:GetMessageResponseWithDateTime[]) => {
            this.isLoading = true;
            this.deliveredMessages = deliveredM;
            console.log(deliveredM);
            console.log(this.deliveredMessages);
            if(this.deliveredMessages.length > 0){
              this.deliveredMessages.reverse();
              this.deliveredMessages.map(x => this.updatedDeliveredMessages.filter((a: any) => a.studentId == x.studentId).length > 0 ? null : this.updatedDeliveredMessages.push(x));
              console.log(this.updatedDeliveredMessages);
              for(let i =0; i< this.updatedDeliveredMessages.length; i++){
                this.updatedDeliveredMessages[i] = { ...this.updatedDeliveredMessages[i], deliveredTime:moment(new Date(this.updatedDeliveredMessages[i].deliveredTime)).fromNow() }
              }
              console.log(this.updatedDeliveredMessages);
              this.isLoading = false;
            }
          }).finally(() => {
            this.updatedDeliveredMessages.map((x: UpdatedRecruiterDeliveredMessages) => {
              console.log(x.content);
            });
          });
        } else {
          this.router.routeReuseStrategy.shouldReuseRoute = function () {
            return false;
          }
          this.router.onSameUrlNavigation = 'reload';
          this.router.navigate(['/forbidden']);
        }
        //this.isLoading = false;
      });
    } else {
      this.router.routeReuseStrategy.shouldReuseRoute = function () {
        return false;
      }
      this.router.onSameUrlNavigation = 'reload';
      this.router.navigate(['/forbidden']);
    }

  }
  private async getStudentById(id: number){
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }
  private async getSendDeliveredRecruiterById(id: number) {
    let sendDeliveredRecruiterById = this.messageService.getSendDeliveredRecruiterById(id);
    return await lastValueFrom(sendDeliveredRecruiterById);
  }
  private async getTokenByRecruiterId(id: number){
    let tokenByRecruiterId = this.recruiterService.getTokenByRecruiterId(id);
    return await lastValueFrom(tokenByRecruiterId);
  }
  private async getRecruiterByToken(token: string){
    let recruiterByKey = this.recruiterService.getRecruiterByKey(token);
    return await lastValueFrom(recruiterByKey);
  }

  goTo(studentId: number) {
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    }
    this.router.onSameUrlNavigation = 'reload';
    console.log("Inside recruiters");
    this.router.navigate(['/chat-room/' + studentId]);
  }
}
