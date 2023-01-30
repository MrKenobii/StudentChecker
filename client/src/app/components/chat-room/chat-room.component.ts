import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {MessageService} from "../../services/message/message.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {lastValueFrom} from "rxjs";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {GetMessageResponse} from "../../interfaces/message/GetMessageResponse";
import {GetMessageResponseWithDateTime} from "../../interfaces/message/GetMessageResponseWithDateTime";

@Component({
  selector: 'app-chat-room',
  templateUrl: './chat-room.component.html',
  styleUrls: ['./chat-room.component.css']
})
export class ChatRoomComponent implements OnInit{
  fromStudent!: StudentResponse;
  toStudent!: StudentResponse;
  id!: number;
  fromRecruiter!: RecruiterGetResponse;
  toRecruiter!: RecruiterGetResponse;
  isLoading!: boolean;
  recruiterSendMessages!: GetMessageResponse[];
  recruiterDeliveredMessages!: GetMessageResponse[];
  displayArr!: GetMessageResponse[];
  ngOnInit(): void {
  }
  constructor(private activatedRoute: ActivatedRoute,
              private router: Router,
              private studentService: StudentService,
              private recruiterService: RecruiterService,
              private messageService: MessageService,
              private snackBar: MatSnackBar) {
    this.id = this.activatedRoute.snapshot.params['id'];
    this.activatedRoute.params.subscribe(() => {
      this.id = this.activatedRoute.snapshot.params['id'];
    });
    if(localStorage.getItem("key")){
      const key:string = (localStorage.getItem("key") as string);
      this.getStudentByKey(key).then((data: StudentResponse)=>{
        this.isLoading = true;
        if(data && data.id > 0 && data.name != null && data.lastName != null){
          this.fromStudent = data;
          console.log(this.fromStudent);
          this.getRecruiterById(this.id).then((toRec: RecruiterGetResponse) => {
            if(toRec && toRec.id > 0 && toRec.name != null && toRec.lastName != null){
              this.toRecruiter = toRec;
              console.log(this.toRecruiter);
            } else {
              this.snackBar.open("Nothing found with recruiter ID: " + this.id, "OK", {
                duration: 4000
              });
            }
          });
        } else {
          this.getRecruiterByKey(key).then((_recruiter: RecruiterGetResponse) => {
            if(_recruiter && _recruiter.id > 0 && _recruiter.name != null && _recruiter.lastName != null){
              this.fromRecruiter = _recruiter;
              this.fromRecruiter.image = "data:image/png;base64," + this.fromRecruiter.image;
              console.log(this.fromRecruiter);
              this.getStudentById(this.id).then((toStudent: StudentResponse) => {
                if(toStudent && toStudent.id > 0 && toStudent.name != null && toStudent.lastName != null){
                  this.toStudent = toStudent;
                  console.log(this.toStudent);
                  this.toStudent.image = "data:image/png;base64," + this.toStudent.image;
                  this.getSendMessageRecruiterById(this.fromRecruiter.id).then((messageGet: GetMessageResponseWithDateTime[]) => {
                    console.log(messageGet);
                    this.recruiterSendMessages = messageGet;
                    this.recruiterSendMessages = this.recruiterSendMessages.filter(s => s.studentId == this.id);
                    this.getSendDeliveredRecruiterById(this.fromRecruiter.id).then((_d: GetMessageResponseWithDateTime[]) => {
                      console.log(_d);
                      this.recruiterDeliveredMessages = _d;
                      this.recruiterDeliveredMessages=this.recruiterDeliveredMessages.filter(s => s.studentId == this.id);
                      this.displayArr = this.recruiterDeliveredMessages.concat(this.recruiterSendMessages);
                      console.log(this.displayArr);
                      this.displayArr.sort((a: GetMessageResponse, b: GetMessageResponse) => {
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
                      console.log(this.displayArr);
                      console.log("-------------------------");
                      // this.displayArr.map((a) => {
                      //   if(a.deliveredTime != null && a.sendTime == null){
                      //     console.log(a.deliveredTime);
                      //     console.log(new Date(a.deliveredTime));
                      //   }
                      //   else if(a.sendTime != null && a.deliveredTime == null){
                      //     console.log(a.sendTime);
                      //     console.log(new Date(a.sendTime));
                      //   }
                      // })
                      // console.log("-------------------------");
                      // console.log(this.displayArr);
                    });
                  });

                  // this.getSendDeliveredStudentById(this.id).then((deliverGet: GetMessageResponse[]) => {
                  //   console.log(deliverGet);
                  // })
                } else {
                  this.snackBar.open("Nothing found with student ID: " + this.id, "OK", {
                    duration: 4000
                  });
                }

              })
            } else {
              this.snackBar.open("Nothing found", "OK", {
                duration: 4000
              });
            }
          });
        }
        this.isLoading = false;
      });
    }
  }
  private async getSendDeliveredStudentById(id: number){
    let sendDeliveredStudentById = this.messageService.getSendDeliveredStudentById(id);
    return await lastValueFrom(sendDeliveredStudentById);
  }
  private async getSendMessageRecruiterById(id: number){
    let sendMessageRecruiterById = this.messageService.getSendMessageRecruiterById(id);
    return await lastValueFrom(sendMessageRecruiterById);
  }
  private async getSendDeliveredRecruiterById(id: number){
    let sendDeliveredRecruiterById = this.messageService.getSendDeliveredRecruiterById(id);
    return await lastValueFrom(sendDeliveredRecruiterById);
  }
  private async getSendMessageStudentById(id: number){
    let sendDeliveredRecruiterById = this.messageService.getSendMessageStudentById(id);
    return await lastValueFrom(sendDeliveredRecruiterById);
  }
  private async getStudentByKey(key: string){
    let studentByKey = this.studentService.getStudentByKey(key);
    return await lastValueFrom(studentByKey);
  }
  private async getRecruiterByKey(key: string){
    let recruiterByKey = this.recruiterService.getRecruiterByKey(key);
    return await lastValueFrom(recruiterByKey)
  }
  private async getRecruiterById(id: number){
    let recruiterById = this.recruiterService.getRecruiterById(id);
    return await lastValueFrom(recruiterById);
  }
  private async getStudentById(id: number){
    let studentById = this.studentService.getStudentById(id);
    return await lastValueFrom(studentById);
  }

  private getTime(date: Date) {
    return date != null ? date.getTime() : 0;
  }
}
