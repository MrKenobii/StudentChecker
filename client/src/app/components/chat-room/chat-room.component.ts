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
  toStudent!: StudentResponse;
  id!: number;
  fromRecruiter!: RecruiterGetResponse;
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
          this.snackBar.open("NOT FOUND", "OK", {
            duration: 4000
          });
          this.router.onSameUrlNavigation = 'reload';
          console.log("Inside recruiters");
          this.router.navigate(['/not-found']);
        } else {
          this.getRecruiterByKey(key).then((_recruiter: RecruiterGetResponse) => {
            this.isLoading = true;
            if(_recruiter && _recruiter.id > 0 && _recruiter.name != null && _recruiter.lastName != null){
              this.fromRecruiter = _recruiter;
              this.fromRecruiter.image = "data:image/png;base64," + this.fromRecruiter.image;
              console.log(this.fromRecruiter);
              this.getStudentById(this.id).then((toStudent: StudentResponse) => {
                this.isLoading = true;
                if(toStudent && toStudent.id > 0 && toStudent.name != null && toStudent.lastName != null){
                  this.toStudent = toStudent;
                  console.log(this.toStudent);
                  this.toStudent.image = "data:image/png;base64," + this.toStudent.image;
                  this.getSendMessageRecruiterById(this.fromRecruiter.id).then((messageGet: GetMessageResponseWithDateTime[]) => {
                    this.isLoading = true;
                    console.log(messageGet);
                    this.recruiterSendMessages = messageGet;
                    this.recruiterSendMessages = this.recruiterSendMessages.filter(s => s.studentId == this.id);
                    this.getSendDeliveredRecruiterById(this.fromRecruiter.id).then((_d: GetMessageResponseWithDateTime[]) => {
                      this.isLoading = true;
                      console.log(_d);
                      this.recruiterDeliveredMessages = _d;
                      this.recruiterDeliveredMessages = this.recruiterDeliveredMessages.filter(s => s.studentId == this.id);
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
                      this.isLoading = false;
                    });
                    //this.isLoading = false;
                  });

                  // this.getSendDeliveredStudentById(this.id).then((deliverGet: GetMessageResponse[]) => {
                  //   console.log(deliverGet);
                  // })
                  //this.isLoading = false;
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
    } else {
      this.router.routeReuseStrategy.shouldReuseRoute = function () {
        return false;
      }
      this.router.onSameUrlNavigation = 'reload';
      this.router.navigate(['/forbidden']);
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

  sendMessage() {
    let elementById = document.getElementById('exampleFormControlInput1') as HTMLInputElement | null;
    if(elementById != null){
      if(elementById.value.trim() == "" || elementById.value.trim() == " " || elementById.value == null){
        this.snackBar.open("You need to enter a message", "OK", {
          duration: 4000
        });
      } else {
        console.log(elementById.value);
        const e: GetMessageResponse = {
          studentId: this.toStudent.id,
          recruiterId: this.fromRecruiter.id,
          content: elementById.value.trim(),
          sendTime: new Date(Date.now()),
          deliveredTime: new Date(Date.now())
        };
        let root = document.getElementById("chat-box") as HTMLInputElement | null;
        if(root != null){
          this.postRecrutiterToStudentMessage({
            fromId: this.fromRecruiter.id,
            toId: this.toStudent.id,
            content: elementById.value.trim()
          }).then((res: any) => {
            console.log(res);
            if(res.fromRecruiterId > 0){
              console.log(this.displayArr);
              this.displayArr.push(e);
              console.log(this.displayArr);
              var div = document.createElement("div");
              var div2 = document.createElement("div");
              var img = document.createElement("img");
              var p = document.createElement("p");


              div.className = "d-flex flex-row justify-content-end mb-4 pt-1"
              p.className = "small p-2 me-3 mb-1 text-white rounded-3 bg-primary";

              p.innerText = elementById!.value.trim();
              div2.appendChild(p);


              img.src = (this.fromRecruiter.image as string);
              img.style.width = "45px";
              img.style.height = "100%";
              div.appendChild(div2);
              div.appendChild(img)

              root!.appendChild(div);
              elementById!.value = "";
            } else {
              this.snackBar.open("Message could not be sent", "OK", {
                duration: 4000
              });
            }
          });
        }

      }

    }
  }

  private async postRecrutiterToStudentMessage(payload: any) {
    let recruiterStudentMessageResponseObservable = this.messageService.postRecruiterToStudentMessage(payload);
    return await lastValueFrom(recruiterStudentMessageResponseObservable);
  }
}
