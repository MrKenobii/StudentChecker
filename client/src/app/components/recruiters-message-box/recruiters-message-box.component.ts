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
  updatedDeliveredMessages: UpdatedRecruiterDeliveredMessages[] = [];
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
            this.deliveredMessages.sort((a: GetMessageResponse, b: GetMessageResponse) => {
              if(a.sendTime != null && b.deliveredTime != null){
                return  new Date(a.sendTime!).getTime() - new Date(b.deliveredTime!).getTime();
              } else if(a.deliveredTime != null && b.sendTime != null){
                return  new Date(a.deliveredTime!).getTime() - new Date(b.sendTime!).getTime();
              } else if(a.sendTime != null && b.sendTime != null) {
                return new Date(a.sendTime!).getTime() - new Date(b.sendTime!).getTime();
              } else if(a.deliveredTime != null && b.deliveredTime != null){
                return new Date(a.deliveredTime!).getTime() - new Date(b.deliveredTime!).getTime();
              }
              return 0;
            });
            console.log(this.deliveredMessages);
            if(this.deliveredMessages.length > 0){
              const unique2: any[] = [];
              this.deliveredMessages.map(x => unique2.filter((a: any) => a.studentId == x.studentId).length > 0 ? null : unique2.push(x));
              console.log(unique2);
              for(let i = 0; i<unique2.length; i++){
                this.getStudentById(unique2[i].studentId).then((stu: StudentResponse) => {
                  this.isLoading = true;
                  this.updatedDeliveredMessages.push({ ...unique2[i], student:  stu});
                  this.isLoading = false;
                });
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
