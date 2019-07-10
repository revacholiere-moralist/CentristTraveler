import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AllPostsComponent } from './pages/posts/all-posts/posts.component';
import { PostAddComponent } from './pages/posts/post-add/post-add.component';
import { PostDetailComponent } from './pages/posts/post-detail/post-detail.component';
import { PostUpdateComponent } from './pages/posts/post-update/post-update.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { LogoutComponent } from './pages/auth/logout/logout.component';
import { AuthGuard } from './guards/auth.guard';


const routes: Routes = [
  {
    path: '',
    redirectTo: '/post/posts',
    pathMatch: 'full'
  },
  {
    path: 'post/posts',
    component: AllPostsComponent,
    data: {title: 'All Posts'}
  },
  {
    path: 'post/posts/category/:categoryId',
    component: AllPostsComponent,
    data: { title: 'All Posts' }
  },
  {
    path: 'post/posts/tag/:tagName',
    component: AllPostsComponent,
    data: { title: 'All Posts' }
  },
  {
    path: 'post/add',
    component: PostAddComponent,
    canActivate: [AuthGuard],
    data: { title: 'Add Post', roles: 'Writer' }
  },
  {
    path: 'post/detail/:id/:slug',
    component: PostDetailComponent,
    data: { title: 'Detail' }
  },
  {
    path: 'post/update/:id',
    component: PostUpdateComponent,
    data: { title: 'Update' }
  },
  {
    path: 'auth/register',
    component: RegisterComponent,
    data: { title: 'Register' }
  },
  {
    path: 'auth/login',
    component: LoginComponent,
    data: { title: 'Login' }
  },
  {
    path: 'auth/logout',
    component: LogoutComponent,
    data: { title: 'Logging Out...' }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    scrollPositionRestoration: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
