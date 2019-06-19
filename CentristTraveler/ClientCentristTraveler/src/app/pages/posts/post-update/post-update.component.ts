import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, Validators, FormGroup, NgForm } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { PostService } from '../../../services/posts/post.service';

import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material';
import { Tag } from '../../../models/tag';
import { Post } from '../../../models/post';
import { HttpRequest, HttpEventType, HttpClient } from '@angular/common/http';
import { Category } from '../../../models/category';
import Quill from 'quill'
import { ImageUpload } from 'quill-image-upload';

import { QuillEditorComponent } from 'ngx-quill'
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-post-update',
  templateUrl: './post-update.component.html',
  styleUrls: ['./post-update.component.css']
})
export class PostUpdateComponent implements OnInit {

  config = {
    toolbar: [
      ['bold', 'italic', 'underline', 'strike'],
      ['blockquote', 'code-block'],
      ['image']
    ],
    imageUpload: {
      url: '/api/Upload',// server url. If the url is empty then the base64 returns
      method: 'POST', // change query method, default 'POST'
      name: 'image', // custom form name
      withCredentials: false, // withCredentials
      // personalize successful callback and call next function to insert new url to the editor
      callbackOK: (serverResponse, next) => {
        debugger;
        var result = serverResponse.toString();
        next(result.substring(result.indexOf(".") + 1));
      },
      // personalize failed callback
      callbackKO: serverError => {
        alert(serverError);
      },
      // optional
      // add callback when a image have been chosen
      checkBeforeSend: (file, next) => {
        console.log(file);
        next(file); // go back to component and send to the server
      }
    }
  }

  postForm: FormGroup;
  id: number = null;
  title: string = '';
  body: string = '';
  thumbnail_path: string = '';
  isLoadingResults = false;

  categories: Category[] = [];
  post: Post = new Post();

  public progress: number;
  public message: string;


  public progressBanner: number;
  public messageBanner: string;

  //chips and add tags
  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  tags: Tag[] = [];

  jwtHelperService = new JwtHelperService();

  constructor(private router: Router,
    private route: ActivatedRoute,
    private postService: PostService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private http: HttpClient,) { }
 
  ngOnInit() {
    Quill.register('modules/imageUpload', ImageUpload);
    //set title
    this.titleService.setTitle('Edit Post');
    this.getPostDetail(this.route.snapshot.params['id']);
    this.postService.getAllCategories()
      .subscribe(data => {
        console.log(data);
        this.categories = data
      });

    this.postForm = this.formBuilder.group({
      'id': [null, Validators.required],
      'title': [null, Validators.required],
      'body': [null, Validators.required],
      'thumbnail_path': [{ value: null, disabled: true }, Validators.required],
      'banner_path': [{ value: null, disabled: true }, Validators.required],
      'banner_text': [null, Validators.required],
      'preview_text': [null, Validators.required],
      'category_id': [null, Validators.required]
    });
  }

  getPostDetail(id) {
    this.postService.getDetail(id)
      .subscribe(data => {
        this.id = data.id;
        this.postForm.setValue({
          id: data.id,
          title: data.title,
          body: data.body,
          thumbnail_path: data.thumbnail_path,
          banner_path: data.banner_path,
          banner_text: data.banner_text,
          preview_text: data.preview_text,
          category_id: data.category_id
        });
        this.tags = data.tags;
        this.isLoadingResults = false;
      });
  }

  onFormSubmit(form: NgForm) {
    debugger;
    let value = this.postForm.getRawValue();
    this.isLoadingResults = true;
    this.post.id = form['id'];
    this.post.title = form['title'];
    this.post.body = form['body'];
    this.post.thumbnail_path = value['thumbnail_path'];
    this.post.banner_path = value['banner_path'];
    this.post.banner_text = form['banner_text'];
    this.post.preview_text = form['preview_text'];
    this.post.category_id = form['category_id'];

    let token = this.jwtHelperService.decodeToken(localStorage.getItem('token'));
    this.post.author_username = (token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']);

    this.post.tags = this.tags;
    this.postService.updatePost(this.post)
      .subscribe(res => {
        this.isLoadingResults = false;
        this.router.navigate(['/post/posts']);
      }, (err) => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }

  //upload thumbnail
  upload(files, uploadType) {
    debugger;
    if (files.length === 0)
      return;

    const formData = new FormData();

    for (let file of files)
      formData.append(file.name, file);

    const uploadReq = new HttpRequest('POST', `api/upload`, formData, {
      reportProgress: true,
    });

    if (uploadType == 'thumbnail_path') {
      this.http.request(uploadReq).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round(100 * event.loaded / event.total);
        }
        else if (event.type === HttpEventType.Response) {
          var result = event.body.toString();
          this.message = result.substring(0, result.indexOf(".") + 1);
          this.postForm.controls['thumbnail_path'].setValue(result.substring(result.indexOf(".") + 1));
        }
      });
    }
    else if (uploadType == 'banner_path') {
      this.http.request(uploadReq).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progressBanner = Math.round(100 * event.loaded / event.total);
        }
        else if (event.type === HttpEventType.Response) {
          var result = event.body.toString();
          this.messageBanner = result.substring(0, result.indexOf(".") + 1);
          this.postForm.controls['banner_path'].setValue(result.substring(result.indexOf(".") + 1));
        }
      });
    }
  }

  //tags
  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add our fruit
    if ((value || '').trim()) {
      this.tags.push({
        id: 0,
        name: value.trim()
      });
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  remove(tag: Tag): void {
    const index = this.tags.indexOf(tag);

    if (index >= 0) {
      this.tags.splice(index, 1);
    }
  }

}
