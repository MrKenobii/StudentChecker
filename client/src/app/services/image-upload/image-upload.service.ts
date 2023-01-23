import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {
  baseApiUrl = "https://file.io"
  constructor(private httpClient: HttpClient) { }

  upload(file: File) : Observable<any> {
    const formData = new FormData();
    console.log(file.name);
    // Store form name as "file" with file data
    formData.append("file", file, file.name);

    // Make http post request over api
    // with formData as req
    return this.httpClient.post(this.baseApiUrl, formData);
  }
}
