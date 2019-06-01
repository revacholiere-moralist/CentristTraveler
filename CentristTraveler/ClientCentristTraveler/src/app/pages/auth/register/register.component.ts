import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroupDirective, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material';
import { Role } from '../../../models/role';
import { User } from '../../../models/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  username: string = '';
  password: string = '';
  email: string = '';
  isLoadingResults = false;

  user: User = new User();
  roles: Role[] = [];
  
  constructor(private router: Router,
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private http: HttpClient,
  ) { }

  ngOnInit() {
    //set title
    this.titleService.setTitle('Add Post');

    this.registerForm = this.formBuilder.group({
      'username': [null, Validators.required],
      'password': [null, Validators.required],
      'email': [null, Validators.required]
    });

    // add role (hardcoded for now)
    this.roles.push({
      id: 2,
      name: 'writer'
    });
  }

  onFormSubmit(form: NgForm) {

    this.isLoadingResults = true;
    this.user.username = form['username'];
    this.user.password = form['password'];
    this.user.email = form['email'];
    this.user.roles = this.roles;
    this.authService.register(this.user)
      .subscribe(res => {
        this.isLoadingResults = false;
        this.router.navigate(['/post/posts']);
      }, (err) => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }
}
