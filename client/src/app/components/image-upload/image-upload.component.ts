import { Component } from '@angular/core';
import {ImageUploadService} from "../../services/image-upload/image-upload.service";

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css']
})
export class ImageUploadComponent {

  // Inject service
  constructor(private fileUploadService: ImageUploadService) { }

  ngOnInit(): void {
  }

  // On file Select

}
