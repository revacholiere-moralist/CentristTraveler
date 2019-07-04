import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { Post } from '../../models/post';
import { Category } from '../../models/category';
import { Tag } from '../../models/tag';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const postUrl = "/api/Post";

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private http: HttpClient) { }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  getPopularPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(`${postUrl}/GetPopularPosts`);
  }
  getPosts(postSearchParam): Observable<Post[]> {
    return this.http.post<Post[]>(`${postUrl}/SearchPosts`, postSearchParam, httpOptions);
  }

  addPost(post): Observable<Post> {
    return this.http.post<Post>(`${postUrl}/AddPost`, post, httpOptions);
  }

  updatePost(post): Observable<Post> {
    return this.http.post<Post>(`${postUrl}/Update`, post, httpOptions);
  }

  getDetail(id): Observable<Post> {
    return this.http.get<Post>(`${postUrl}/Detail/${id}`);
  }

  deletePost(id): Observable<boolean> {
    return this.http.get<boolean>(`${postUrl}/Delete/${id}`);
  }

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${postUrl}/GetAllCategories/`);
  }

  getPopularTags(): Observable<Tag[]> {
    return this.http.get<Tag[]>(`${postUrl}/GetPopularTags/`);
  }
}
