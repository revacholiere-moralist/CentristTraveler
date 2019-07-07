import { BrowserModule, Title } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { QuillModule } from 'ngx-quill';
import { MatChipsModule } from "@angular/material/chips"
import { MatFormFieldModule } from "@angular/material/form-field"
import { MatIconModule } from "@angular/material/icon"
import { JwtInterceptor} from './helpers/JwtInterceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppRoutingModule } from './app-routing.module';
import { AllPostsComponent } from './pages/posts/all-posts/posts.component';
import { AppComponent } from './app.component';
import { PostAddComponent } from './pages/posts/post-add/post-add.component';
import { PostDetailComponent } from './pages/posts/post-detail/post-detail.component';
import { PostUpdateComponent } from './pages/posts/post-update/post-update.component';
import { SidebarComponent } from './pages/shared/sidebar/sidebar.component';
import { BannerComponent } from './pages/shared/banner/banner.component';

import { RegisterComponent } from './pages/auth/register/register.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { LogoutComponent } from './pages/auth/logout/logout.component';
import { HeaderComponent } from './pages/shared/header/header.component';
import { FooterComponent } from './pages/shared/footer/footer.component';


@NgModule({
  declarations: [
    AppComponent,
    AllPostsComponent,
    PostAddComponent,
    PostDetailComponent,
    PostUpdateComponent,
    SidebarComponent,
    BannerComponent,
    RegisterComponent,
    LoginComponent,
    LogoutComponent,
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    QuillModule,
    MatChipsModule,
    MatIconModule,
    MatFormFieldModule
  ],
  exports: [
    MatChipsModule,
    MatIconModule,
    MatFormFieldModule
  ],
  providers: [Title,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }],
  bootstrap: [AppComponent, HeaderComponent, FooterComponent]
})
export class AppModule { }
//platformBrowserDynamic().bootstrapModule(AppModule);


