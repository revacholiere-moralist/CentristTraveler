import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroupDirective, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material';
import { User } from '../../../models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  username: string = '';
  password: string = '';
  isLoadingResults = false;
  isLoginIncorrect = false;
  user: User = new User();

  constructor(private router: Router,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private http: HttpClient,
  ) { }

  ngOnInit() {
    //set title
    this.titleService.setTitle('Login');

    this.loginForm = this.formBuilder.group({
      'username': [null, Validators.required],
      'password': [null, Validators.required],
    });
  }

  onFormSubmit(form: NgForm) {

    this.isLoadingResults = true;
    this.user.username = form['username'];
    this.user.password = form['password'];
    
    this.authService.authenticate(this.user)
      .subscribe(res => {
        if (res != null) {
          this.isLoginIncorrect = false;
          localStorage.setItem('token', res);
          this.isLoadingResults = false;
          this.router.navigate(['/post/posts']);
        }
        else {
          this.isLoginIncorrect = true;
        }
      }, (err) => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }
}
