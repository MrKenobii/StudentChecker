import {Component, OnInit} from '@angular/core';
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {GetMessageResponse} from "../../interfaces/message/GetMessageResponse";
import {ActivatedRoute, Router} from "@angular/router";
import {StudentService} from "../../services/student/student.service";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {MessageService} from "../../services/message/message.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {lastValueFrom} from "rxjs";
import {GetMessageResponseWithDateTime} from "../../interfaces/message/GetMessageResponseWithDateTime";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {PostMessageRequest} from "../../interfaces/message/PostMessageRequest";
import {RecruiterStudentMessageResponse} from "../../interfaces/message/RecruiterStudentMessageResponse";

@Component({
  selector: 'app-student-chat-room',
  templateUrl: './student-chat-room.component.html',
  styleUrls: ['./student-chat-room.component.css']
})
export class StudentChatRoomComponent implements OnInit{
  fromStudent!: StudentResponse;

  id!: number;
  toRecruiter!: RecruiterGetResponse;
  isLoading!: boolean;
  studentGetMessages!: GetMessageResponse[];
  studentDeliveredMessages!: GetMessageResponse[];
  displayArr!: GetMessageResponse[];
  createPostForm!: FormGroup;

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
    this.getMessages();
  }
  getMessages(){
    if(localStorage.getItem("key")){
      const key:string = (localStorage.getItem("key") as string);
      this.getStudentByKey(key).then((data: StudentResponse)=>{
        this.isLoading = true;
        console.log("Checkpint")
        if(data && data.id > 0 && data.name != null && data.lastName != null){
          this.fromStudent = data;
          console.log(this.fromStudent);
          this.fromStudent.image = "data:image/png;base64," + this.fromStudent.image;
          this.getRecruiterById(this.id).then((toRec: RecruiterGetResponse) => {
            this.isLoading = true;
            if(toRec && toRec.id > 0 && toRec.name != null && toRec.lastName != null){
              this.toRecruiter = toRec;
              // console.log(this.toRecruiter);

              this.toRecruiter.image = "data:image/png;base64," + this.toRecruiter.image;
              this.getSendMessageStudentById(this.fromStudent.id).then((messageGet: GetMessageResponseWithDateTime[]) => {
                this.isLoading = true;
                // console.log(messageGet);
                this.studentGetMessages = messageGet;
                this.studentGetMessages = this.studentGetMessages.filter(s => s.studentId == this.fromStudent.id);
                this.getSendDeliveredStudentById(this.fromStudent.id).then((_d: GetMessageResponseWithDateTime[]) => {
                  this.isLoading = true;
                  // console.log(_d);
                  this.studentDeliveredMessages = _d;
                  this.studentDeliveredMessages=this.studentDeliveredMessages.filter(s => s.recruiterId == this.id);
                  this.displayArr = this.studentDeliveredMessages.concat(this.studentGetMessages);
                  // console.log(this.displayArr);
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
                  // console.log(this.displayArr);
                  // console.log("-------------------------");
                  // console.log(this.fromStudent);
                  // console.log(this.toRecruiter);
                  this.isLoading = false;
                });
                //this.isLoading = false;
              });
               //this.isLoading = false;
            } else {
              this.snackBar.open("Nothing found with recruiter ID: " + this.id, "OK", {
                duration: 4000
              });
            }
          });
        }
        else {
          this.snackBar.open("NOT FOUND", "OK", {
            duration: 4000
          });
          this.router.onSameUrlNavigation = 'reload';
          console.log("Inside recruiters");
          this.router.navigate(['/forbidden']);
        }
        //this.isLoading = false;
      });
    }
    else {
      this.snackBar.open("NOT FOUND", "OK", {
        duration: 4000
      });
      this.router.onSameUrlNavigation = 'reload';
      console.log("Inside recruiters");
      this.router.navigate(['/not-found']);
    }
  }
  ngOnInit(): void {
  }
  private async getSendDeliveredStudentById(id: number){
      let sendDeliveredStudentById = this.messageService.getSendDeliveredStudentById(id);
      return await lastValueFrom(sendDeliveredStudentById);
  }
  private async getSendMessageStudentById(id: number){
      let sendDeliveredRecruiterById = this.messageService.getSendMessageStudentById(id);
      return await lastValueFrom(sendDeliveredRecruiterById);
  }
  private async getStudentByKey(key: string){
      let studentByKey = this.studentService.getStudentByKey(key);
      return await lastValueFrom(studentByKey);
  }
  private async getRecruiterById(id: number){
      let recruiterById = this.recruiterService.getRecruiterById(id);
      return await lastValueFrom(recruiterById);
  }

  private getTime(date: Date) {
      return date != null ? date.getTime() : 0;
    }
  private async postStudentToRecruiterMessage(payload: PostMessageRequest){
    let recruiterStudentMessageResponseObservable = this.messageService.postStudentToRecruiterMessage(payload);
    return await lastValueFrom(recruiterStudentMessageResponseObservable);
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
          studentId: this.fromStudent.id,
          recruiterId: this.toRecruiter.id,
          content: elementById.value.trim(),
          sendTime: new Date(Date.now()),
          deliveredTime: new Date(Date.now())
        };
        let root = document.getElementById("chat-box") as HTMLInputElement | null;
        if(root != null){
          this.postStudentToRecruiterMessage({
            fromId: this.fromStudent.id,
            toId: this.toRecruiter.id,
            content: elementById.value.trim()
          }).then((res: any) => {
            console.log(res);
            if(res.fromStudentId > 0){
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


              img.src = (this.fromStudent.image as string);
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
}
