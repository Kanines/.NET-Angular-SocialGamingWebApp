import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Image } from 'src/app/_models/image';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-image-editor',
  templateUrl: './image-editor.component.html',
  styleUrls: ['./image-editor.component.css']
})
export class ImageEditorComponent implements OnInit {
  @Input() images: Image[];
  @Output() getMemberImageChange = new EventEmitter<string>();
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Image;

  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/images',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024  //10MB
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Image = JSON.parse(response);
        const image = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.images.push(image);
        if (image.isMain) {
          this.authService.changeMemberImage(image.url);
          this.authService.currentUser.imageUrl = image.url;
          localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
        }
      }
    }
  }

  setMainImage(image: Image) {
    this.userService.setMainImage(this.authService.decodedToken.nameid, image.id).subscribe(() => {
      this.currentMain = this.images.filter(i => i.isMain === true)[0];
      this.currentMain.isMain = false;
      image.isMain = true;
      this.authService.changeMemberImage(image.url);
      this.authService.currentUser.imageUrl = image.url;
      localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    }, error => {
      this.alertify.error(error);
    });
  }


  deleteImage(id: number) {
    this.alertify.confirm('Are you sure you want to delete this image?', () => {
      this.userService.deleteImage(this.authService.decodedToken.nameid, id).subscribe(() => {
        this.images.splice(this.images.findIndex(i => i.id === id), 1);
        this.alertify.success('Image has been deleted');
      }, error => {
        this.alertify.error('Failed to delete the image');
      });
    }, 'Confirm deletion');
  }
}
