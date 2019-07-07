import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { PostService } from '../../../services/posts/post.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Post } from '../../../models/post';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {

  jwtHelperService = new JwtHelperService();
  isWriter: boolean = false;
  isAdmin: boolean = false;
  post: Post = { id: this.route.snapshot.params['id'], title: '', body: '', thumbnail_path: null, tags: null, preview_text: null, banner_path: '', banner_text: '', category_id: null, views: 0, slug: '', author_display_name:'', author_username: '', created_date: '' };
  isLoadingResults = true;
  style: any = {};
  constructor(private postService: PostService,
    private titleService: Title,
    private route: ActivatedRoute,
    private router: Router) {
      this.router.events.subscribe((e) => {
        if (e instanceof NavigationEnd) {
          let token = localStorage.getItem('token');
          if (token != null) {
            
            let tokenDecode = this.jwtHelperService.decodeToken(token);
            
            if (tokenDecode['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].indexOf('Admin') != -1) {
              // role not authorised so redirect to home page
              this.isAdmin = true;
            }

            if (tokenDecode['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].indexOf('Writer') != -1) {
              this.isWriter = true;
            }
          }
          else {
            
            this.isWriter = false;
            this.isAdmin = false;
          }
        }
      });
  }

  ngOnInit() {
    this.route.params.subscribe(
      params => {
        const id = +params['id'];
        this.getPostDetail(id);
      }
    );
    
    
  }

  getPostDetail(id) {
    this.postService.getDetail(id)
      .subscribe(data => {
        this.post = data;
        this.style = {'background-image': 'url("' + this.post.banner_path + '")'};
        this.titleService.setTitle(this.post.title);
        this.isLoadingResults = false;
      });
  }

  deletePost(id) {
    this.isLoadingResults = true;
    debugger;
    this.postService.deletePost(id)
      .subscribe(res => {
        this.isLoadingResults = false;
        this.router.navigate(['/post/posts']);
      }, (err) => {
        console.log(err);
        this.isLoadingResults = false;
      }
      );
  }
}
