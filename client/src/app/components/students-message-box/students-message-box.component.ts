import {Component, OnInit} from '@angular/core';
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {GetMessageResponseWithDateTime} from "../../interfaces/message/GetMessageResponseWithDateTime";
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {StudentService} from "../../services/student/student.service";
import {MessageService} from "../../services/message/message.service";
import {GetMessageResponse} from "../../interfaces/message/GetMessageResponse";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import * as moment from 'moment';
import {lastValueFrom} from "rxjs";
interface UpdatedRecruiterDeliveredMessages {
  studentId: number;
  recruiterId: number;
  content: string;
  sendTime?: Date;
  deliveredTime?: Date
  recruiter: RecruiterGetResponse;
}
@Component({
  selector: 'app-students-message-box',
  templateUrl: './students-message-box.component.html',
  styleUrls: ['./students-message-box.component.css']
})
export class StudentsMessageBoxComponent implements OnInit{
  id!: number;
  isLoading!: boolean;
  deliveredMessages!: GetMessageResponseWithDateTime[];
  updatedDeliveredMessages: any[] = [];
  student!: StudentResponse;

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
  private timeSince(date: any) {

    var seconds = Math.floor(((new Date().getTime()) - date) / 1000);

    var interval = seconds / 31536000;

    if (interval > 1) {
      return Math.floor(interval) + " years";
    }
    interval = seconds / 2592000;
    if (interval > 1) {
      return Math.floor(interval) + " months";
    }
    interval = seconds / 86400;
    if (interval > 1) {
      return Math.floor(interval) + " days";
    }
    interval = seconds / 3600;
    if (interval > 1) {
      return Math.floor(interval) + " hours";
    }
    interval = seconds / 60;
    if (interval > 1) {
      return Math.floor(interval) + " minutes";
    }
    return Math.floor(seconds) + " seconds";
  }
  ngOnInit(): void {
    if(localStorage.getItem("key")){
      this.getStudentByToken(localStorage.getItem("key") as string).then((d: StudentResponse) => {
        this.isLoading = true;
        if(d.id == this.id){
          this.student = d;
          this.getSendDeliveredStudentById(this.id).then((deliveredM:GetMessageResponseWithDateTime[]) => {
            this.isLoading = true;
            this.deliveredMessages = deliveredM;
            this.isLoading = false;
            console.log(this.deliveredMessages);
            if(this.deliveredMessages.length > 0){
              this.deliveredMessages.reverse();
              this.deliveredMessages.map(x => this.updatedDeliveredMessages.filter((a: any) => a.recruiterId == x.recruiterId).length > 0 ? null : this.updatedDeliveredMessages.push(x));
              for(let i = 0; i<this.updatedDeliveredMessages.length; i++){
                this.updatedDeliveredMessages[i] = { ...this.updatedDeliveredMessages[i], deliveredTime:moment(new Date(this.updatedDeliveredMessages[i].deliveredTime)).fromNow() }
              }
              console.log(this.updatedDeliveredMessages);
              this.isLoading = false;
            }
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
  private async getRecruiterById(id: number){
    let studentById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(studentById);
  }
  private async getSendDeliveredStudentById(id: number) {
    let sendDeliveredRecruiterById = this.messageService.getSendDeliveredStudentById(id);
    return await lastValueFrom(sendDeliveredRecruiterById);
  }
  private async getTokenByRecruiterId(id: number){
    let tokenByRecruiterId = this.recruiterService.getTokenByRecruiterId(id);
    return await lastValueFrom(tokenByRecruiterId);
  }
  private async getStudentByToken(token: string){
    let recruiterByKey = this.studentService.getStudentByKey(token);
    return await lastValueFrom(recruiterByKey);
  }

  goTo(recruiterId: number) {
    this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
    }
    this.router.onSameUrlNavigation = 'reload';
    console.log("Inside recruiters");
    this.router.navigate(['/student-chat-room/' + recruiterId]);
  }
}
