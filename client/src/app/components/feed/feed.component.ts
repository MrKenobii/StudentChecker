import {Component, OnInit} from '@angular/core';
import {PostService} from "../../services/post/post.service";
import {Router} from "@angular/router";
import {lastValueFrom} from "rxjs";
import {GetPostResponse} from "../../interfaces/post/GetPostResponse";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {MatSnackBar} from "@angular/material/snack-bar";

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

  constructor(private router: Router, private postService: PostService, private snackBar: MatSnackBar) {
    this.postPayload = {
      title: '',
      content: '',
    }
    this.getPosts().then((posts: GetPostResponse[]) => {
      this.isLoading = true;
      if(posts.length > 0){
        this.posts = posts;
        console.log(this.posts);
      }
      this.isLoading = false;
    });
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

  send() {
    this.postPayload.title = this.createPostForm.get('title')!.value;
    this.postPayload.content = this.createPostForm.get('content')!.value;
    const obj = {
      title: this.postPayload.title,
      content: this.postPayload.content
    };
    console.log(obj);
    if((obj.title == null || obj.title.trim == "") || (obj.content == null || obj.content.trim == "")){
      this.snackBar.open("You need to enter a title and content", "OK", {
        duration: 4000
      });
    } else {
      this.snackBar.open("Success", "OK", {
        duration: 4000
      });
    }
  }
}
