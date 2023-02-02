import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {GetPostResponse} from "../../interfaces/post/GetPostResponse";
import {Observable} from "rxjs";
import {CreatePostRequest} from "../../interfaces/post/CreatePostRequest";

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) { }

  public getPosts() : Observable<GetPostResponse[]>{
    return this.http.get<GetPostResponse[]>(`http://localhost:5269/Post`);
  }

  createPostForRecruiter(payload: CreatePostRequest) : Observable<GetPostResponse> {
    return this.http.post<GetPostResponse>(`http://localhost:5269/Post/recruiter`, payload);
  }

  createPostForStudent(payload: CreatePostRequest) : Observable<GetPostResponse> {
    return this.http.post<GetPostResponse>(`http://localhost:5269/Post/student`, payload);
  }
}
