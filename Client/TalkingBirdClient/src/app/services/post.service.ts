import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(private httpClient: HttpClient) { }

  baseUrl = "https://localhost:44312/api";

  public createPost(model:any){
    return this.httpClient.post<any>(this.baseUrl+'/posts', model);
  }

  public getAllPosts(){
    return this.httpClient.get<any>(this.baseUrl+'/posts');
  }

  public likePost(postId: any){
    return this.httpClient.post<any>(this.baseUrl+'/posts/like/'+postId, null);
  }


}
