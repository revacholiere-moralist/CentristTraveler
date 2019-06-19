import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormControl, FormGroupDirective, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { PostService } from '../../../services/posts/post.service';

import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material';
import { Tag } from '../../../models/tag';
import { Post } from '../../../models/post';
import { Category } from '../../../models/category';
import Quill from 'quill'
import { ImageUpload } from 'quill-image-upload';

import { QuillEditorComponent } from 'ngx-quill'
import { JwtHelperService } from '@auth0/angular-jwt';


@Component({
  selector: 'app-post-add',
  templateUrl: './post-add.component.html',
  styleUrls: ['./post-add.component.css']
})
export class PostAddComponent implements OnInit {
  
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
  title: string = '';
  body: string = '';
 
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

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private postService: PostService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private http: HttpClient,
    ) { }
  jwtHelperService = new JwtHelperService();
  ngOnInit() {
    Quill.register('modules/imageUpload', ImageUpload);
    //set title
    this.titleService.setTitle('Add Post');
    this.postService.getAllCategories()
      .subscribe(data => {
        console.log(data);
        this.categories = data
      });
    
    this.postForm = this.formBuilder.group({
      'title': [null, Validators.required],
      'body': [null, Validators.required],
      'thumbnail_path': [{ value: null, disabled: true }, Validators.required],
      'banner_path': [{ value: null, disabled: true }, Validators.required],
      'banner_text': [null, Validators.required],
      'preview_text': [null, Validators.required],
      'category_id': [null, Validators.required]
    });
  }

  onFormSubmit(form: NgForm) {

    let value = this.postForm.getRawValue();
    this.isLoadingResults = true;   
    this.post.title = form['title'];
    this.post.body = form['body'];
    this.post.thumbnail_path = value['thumbnail_path'];
    this.post.banner_path = value['banner_path'];
    this.post.preview_text =form['preview_text'];
    this.post.banner_text = form['banner_text'];
    this.post.category_id = form['category_id'];

    let token = this.jwtHelperService.decodeToken(localStorage.getItem('token'));
    this.post.author_username = (token['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']);

    this.post.tags = this.tags;
    this.postService.addPost(this.post)
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
          debugger;
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
          debugger;
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
        name: value
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
