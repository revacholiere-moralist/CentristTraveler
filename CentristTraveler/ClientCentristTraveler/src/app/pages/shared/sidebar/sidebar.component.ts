import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroupDirective, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { PostSearchParam } from '../../../models/SearchParam/postSearchParam';
import { PostService } from '../../../services/posts/post.service';
import { Tag } from '../../../models/tag';
import { Post } from '../../../models/post';
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  tags: Tag[] = [];
  posts: Post[] = [];
  searchTagForm: FormGroup;
  searchPostForm: FormGroup;
  postSearchParam: PostSearchParam = { title: '', body: '', category_id: 0, tag: '' };
  constructor(private formBuilder: FormBuilder,
    private postService: PostService) { }

  ngOnInit() {
    this.searchTagForm = this.formBuilder.group({
      'tag_name': [null],
    });
    this.searchPostForm = this.formBuilder.group({
      'post_search': [null],
    })
    //get all posts
    this.postService.getPopularTags()
      .subscribe(res => {
        this.tags = res;
      }, err => {
      });

    this.postService.getLatestPosts()
      .subscribe(res => {
        this.posts = res;
      }, err => {
      });
  }

  @Output() messageEvent = new EventEmitter<PostSearchParam>();
  onFormSubmit(form: NgForm) {
    this.postSearchParam.tag = form['tag_name'];
    this.messageEvent.emit(this.postSearchParam);
  }

  onPostFormSubmit(form: NgForm) {
    this.postSearchParam.body = form['post_search'];
    this.postSearchParam.title = form['post_search'];
    this.messageEvent.emit(this.postSearchParam);
  }
}
