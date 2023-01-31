import {Component, OnInit} from '@angular/core';
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {GetMessageResponseWithDateTime} from "../../interfaces/message/GetMessageResponseWithDateTime";
import {ActivatedRoute, Router} from "@angular/router";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {StudentService} from "../../services/student/student.service";
import {MessageService} from "../../services/message/message.service";
import {GetMessageResponse} from "../../interfaces/message/GetMessageResponse";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
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
  updatedDeliveredMessages: UpdatedRecruiterDeliveredMessages[] = [];
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
              this.deliveredMessages.map(x => unique2.filter((a: any) => a.recruiterId == x.recruiterId).length > 0 ? null : unique2.push(x));
              console.log(unique2);
              for(let i = 0; i<unique2.length; i++){
                this.getRecruiterById(unique2[i].recruiterId).then((_rec: RecruiterGetResponse) => {
                  this.isLoading = true;
                  this.updatedDeliveredMessages.push({ ...unique2[i], recruiter:  _rec});
                  this.isLoading = false;
                });
              }
              console.log(this.updatedDeliveredMessages);
              this.isLoading = false;
            }
            // for(let i = 0; i<this.deliveredMessages.length; i++){
            //   this.getRecruiterById(this.deliveredMessages[i].recruiterId).then((_rec: RecruiterGetResponse) => {
            //     this.isLoading = true;
            //     this.updatedDeliveredMessages.push({ ...this.deliveredMessages[i], recruiter:  _rec});
            //     this.isLoading = false;
            //   });
            // }
            //this.isLoading = false;
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
