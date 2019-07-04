import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { PostService } from '../../../services/posts/post.service';
import { Post } from '../../../models/post';
import { PostSearchParam } from '../../../models/SearchParam/postSearchParam';
import { ActivatedRoute } from '@angular/router';
import { SidebarComponent } from '../../shared/sidebar/sidebar.component';
@Component({
  selector: 'all-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class AllPostsComponent implements OnInit {

  paramFromSidebar: PostSearchParam = { title: '', body: '', category_id: 0, tag: '' };;

  displayedColumns: string[] = ['title', 'body'];
  data: Post[] = [];
  postSearchParam: PostSearchParam = { title: '', body: '', category_id: 0, tag: ''};
  isLoadingResults = true;

  constructor(private postService: PostService,
    private titleService: Title,
    private route: ActivatedRoute) { }

  ngOnInit() {
    //set title
    this.titleService.setTitle('Joseph And Mary');
    if (this.route.snapshot.params['categoryId'] != null) {
      this.postSearchParam.category_id = this.route.snapshot.params['categoryId'];
    }
    if (this.route.snapshot.params['tagName'] != null) {
      this.postSearchParam.tag = this.route.snapshot.params['tagName'];
    }

    //get all posts
    this.postService.getPosts(this.postSearchParam)
      .subscribe(res => {
        this.data = res;
        console.log(this.data);
        this.isLoadingResults = false;
      }, err => {
        this.isLoadingResults = false;
      });

  }

  receiveMessage($event) {
    this.paramFromSidebar = $event;
    if (this.route.snapshot.params['categoryId'] != null) {
      this.postSearchParam.category_id = this.route.snapshot.params['categoryId'];
    }
    this.postService.getPosts(this.paramFromSidebar)
      .subscribe(res => {
        this.data = res;
        this.isLoadingResults = false;
      }, err => {
        this.isLoadingResults = false;
      });
  }
}
