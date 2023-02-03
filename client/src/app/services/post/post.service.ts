import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {GetPostResponse} from "../../interfaces/post/GetPostResponse";
import {Observable} from "rxjs";
import {CreatePostRequest} from "../../interfaces/post/CreatePostRequest";
import {GetPostByIdResponse} from "../../interfaces/post/GetPostByIdResponse";

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

  public getPostsByStudentId(id: number) : Observable<GetPostByIdResponse[]> {
    return this.http.get<GetPostByIdResponse[]>(`http://localhost:5269/Post/student/${id}`);
  }
  public getPostsByRecruiterId(id: number) : Observable<GetPostByIdResponse[]> {
    return this.http.get<GetPostByIdResponse[]>(`http://localhost:5269/Post/recruiter/${id}`);
  }
}
