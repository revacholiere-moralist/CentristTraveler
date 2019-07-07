import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router, NavigationEnd } from '@angular/router';
import { Category } from '../../../models/category';
import { PostService } from '../../../services/posts/post.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  jwtHelperService = new JwtHelperService();
  isLogin: boolean = false;
  isWriter: boolean = false;
  isAdmin: boolean = false;
  categories: Category[] = [];
  categoryId: number = 0;
  constructor(private router: Router,
              private postService: PostService) {
    this.router.events.subscribe((e) => {
      if (e instanceof NavigationEnd) {

        let token = localStorage.getItem('token');
        if (token != null) {
          this.isLogin = true;
          try {
            let tokenDecode = this.jwtHelperService.decodeToken(token);
            var exp = tokenDecode['exp'];
            if (Math.floor(new Date().getTime() / 1000.0) > exp) {
              this.isLogin = false;
              localStorage.removeItem('token');
            }
            if (tokenDecode['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].indexOf('Admin') != -1) {
              // role not authorised so redirect to home page
              this.isAdmin = true;
              this.isLogin = true;
            }

            if (tokenDecode['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].indexOf('Writer') != -1) {
              this.isWriter = true;
              this.isLogin = true;
            }
          }
          catch{
            this.isLogin = false;
          }
        }
        else {
          this.isLogin = false;
          this.isWriter = false;
          this.isAdmin = false;
        }
      }
    });
  }

  ngOnInit() {
    this.postService.getAllCategories()
      .subscribe(data => {
        this.categories = data
      });
  }

}
