import {Component, OnInit} from '@angular/core';
import {PostService} from "../../services/post/post.service";
import {Router} from "@angular/router";
import {lastValueFrom} from "rxjs";
import {GetPostResponse} from "../../interfaces/post/GetPostResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MatSnackBar} from "@angular/material/snack-bar";
import {StudentService} from "../../services/student/student.service";
import {RecruiterService} from "../../services/recruiter/recruiter-service.service";
import {StudentResponse} from "../../interfaces/student/StudentResponse";
import {RecruiterGetResponse} from "../../interfaces/recruiter/RecruiterGetResponse";
import {CreatePostRequest} from "../../interfaces/post/CreatePostRequest";
import * as moment from 'moment';
import {Student} from "../../interfaces/student/Student";

interface UpdatedPosts{
  id: number;
  content: string;
  title: string;
  recruiter?: RecruiterGetResponse;
  student?: Student
  createdTime: Date,
  formattedDate: string
}
@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit{
  posts!: GetPostResponse[];
  isLoading!: boolean;
  createPostForm!: FormGroup;
  postPayload!: any;
  student!: StudentResponse;
  recruiter!: RecruiterGetResponse;
  updatedPosts: UpdatedPosts[] = [];

  constructor(private router: Router, private studentService: StudentService, private recruiterService: RecruiterService, private postService: PostService, private snackBar: MatSnackBar) {
    this.postPayload = {
      title: '',
      content: '',
    }
    this.getPosts().then((posts: GetPostResponse[]) => {
      this.isLoading = true;
      if(posts.length > 0){
        this.posts = posts;

        this.posts.sort((a: GetPostResponse, b: GetPostResponse) => {
          if(a.createdTime != null && b.createdTime != null){
            return  new Date(a.createdTime!).getTime() - new Date(b.createdTime!).getTime();
          }
          return 0;
        });
        posts.reverse();
        for(let i =0; i< this.posts.length; i++){
          this.updatedPosts[i] = { ...this.posts[i], formattedDate :moment(new Date(this.posts[i].createdTime)).fromNow() }
        }
        //this.updatedPosts.reverse();
        console.log(this.updatedPosts);
        console.log(this.posts);
      }
      this.isLoading = false;
    });
    if(localStorage.getItem("key")){
      const token: string = (localStorage.getItem("key") as string);
      this.getStudentByToken(token).then((studentResponse: StudentResponse) => {
        this.isLoading = true;
        if(studentResponse.id > 0 && studentResponse.name !== null && studentResponse.lastName !== null && studentResponse.email !== null && studentResponse.cityName !== null){
          console.log(studentResponse);
          this.student = studentResponse;
        } else {
          this.getRecruiterByKey(token).then((recResponse: RecruiterGetResponse) => {
            if(recResponse.id > 0 && recResponse.name !== null && recResponse.lastName !== null && recResponse.email !== null && recResponse.dateOfBirth !== null){
              console.log(recResponse);
              this.recruiter = recResponse;
            } else {
              // NO ONE IS LOGGED IN
              console.log("NO ONE IS LOGGED IN");
              this.snackBar.open("NO ONE IS LOGGED IN", "OK", {
                duration: 4000
              });
            }
            this.isLoading = false;
          });
        }
      })
    } else {
      this.snackBar.open("NO ONE IS LOGGED IN", "OK", {
        duration: 4000
      });
    }

  }
  private async getPosts(){
    let posts = this.postService.getPosts();
    return await lastValueFrom(posts)
  }
  ngOnInit(): void {
    this.createPostForm = new FormGroup({
      title: new FormControl('', Validators.required),
      content: new FormControl('', Validators.required),
    });

  }
  private async getStudentByToken(token: string){
    let studentByKey = this.studentService.getStudentByKey(token);
    return await lastValueFrom(studentByKey);
  }
  private async getRecruiterByKey(token: string){
    let recruiterByKey = this.recruiterService.getRecruiterByKey(token);
    return await lastValueFrom(recruiterByKey);
  }
  private async createPostForStudent(payload: CreatePostRequest) {
    let getPostResponseObservable = this.postService.createPostForStudent(payload);
    return await lastValueFrom(getPostResponseObservable);
  }
  private async createPostForRecruiter(payload: CreatePostRequest) {
    let getPostResponseObservable = this.postService.createPostForRecruiter(payload);
    return await lastValueFrom(getPostResponseObservable);
  }
  send() {
    this.postPayload.title = this.createPostForm.get('title')!.value;
    this.postPayload.content = this.createPostForm.get('content')!.value;
    const obj = {
      title: this.postPayload.title,
      content: this.postPayload.content
    };
    console.log(obj);

      if((obj.title === null || obj.title.trim === "" || obj.title === "" ) || (obj.content === null || obj.content.trim === "" || obj.content === "")){
        this.snackBar.open("You need to enter a title and content", "OK", {
          duration: 4000
        });
      } else {
        if(this.student != null && this.recruiter == null) {
          const payload: CreatePostRequest = {
            ...obj,
            recruiterId: null,
            studentId: this.student.id
          }
          console.log(payload);
          this.createPostForStudent(payload).then((res: GetPostResponse) => {
            console.log(res);
            if(res && res.id > 0){
              this.snackBar.open("Success STUDENT", "OK", {
                duration: 4000
              });
            } else {
              this.snackBar.open("FAILURE STUDENT", "OK", {
                duration: 4000
              });
            }
          })
        } else if(this.recruiter != null && this.student == null){
          const payload: CreatePostRequest = {
            ...obj,
            recruiterId: this.recruiter.id,
            studentId: null
          }
          this.createPostForRecruiter(payload).then((res: GetPostResponse) => {
            if(res && res.id > 0){
              console.log(res)
              this.snackBar.open("Success RECRUITER", "OK", {
                duration: 4000
              });
            } else {
              this.snackBar.open("FAILURE RECRUITER", "OK", {
                duration: 4000
              });
            }
          });
          this.snackBar.open("Success RECRUITER", "OK", {
            duration: 4000
          });
        }

      }
    }
}
