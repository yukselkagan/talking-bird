import { DataTransferService } from './data-transfer.service';
import { CommonInformation } from './../models/common-information';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private httpClient: HttpClient,
    private dataTransferService: DataTransferService, private router: Router) { }

  baseUrl = "https://localhost:44398/api";

  public register(model:any){
    return this.httpClient.post<any>(this.baseUrl+'/authentication/user', model);
  }

  public login(model:any){
    return this.httpClient.post<any>(this.baseUrl+'/authentication/token', model);
  }

  public logout(){
    this.removeOldToken();
    this.clearCommonInformation();
    this.router.navigateByUrl("/");
  }

  public getToken(){
    return localStorage.getItem('token');
  }

  private removeOldToken(){
    localStorage.removeItem('token');
  }

  private clearCommonInformation(){
    let newInformation = new CommonInformation();
    this.dataTransferService.changeCommonInformation(newInformation);
  }

  public getSelf(){
    return this.httpClient.get<any>(this.baseUrl+'/authentication/user/self');
  }


  public getUserInformation() {
    this.getSelf().subscribe({
      next: (response) => {
        console.log(response);

        let newCommonInformation = new CommonInformation();
        newCommonInformation.userId = response['userId'];
        newCommonInformation.email = response['email'];
        newCommonInformation.userName = response['userName'];
        newCommonInformation.displayName = response['displayName'];
        newCommonInformation.isAuthenticated = true;

        this.dataTransferService.changeCommonInformation(newCommonInformation);
      },
      error: (errorResponse) => {
        console.log(errorResponse);
        this.logout();
      }
    })
  }





}
