import { CommonService } from './../../services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit{


  constructor(private postService: PostService, private dataTransferService: DataTransferService,
    private commonService: CommonService){}

  ngOnInit(): void {

    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData )

    this.getPostsSelf();

  }

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation: CommonInformation = new CommonInformation();

  postList = [];

  public processProfileDisplayName(userName:any, displayName:any): string{
    var response = this.commonService.processProfileDisplayName(userName, displayName);
    return response;
  }

  public getPostsSelf(){
    this.postService.getPostsSelf().subscribe({
      next: (response) => {
        console.log(response);
        this.postList = response;
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }



  likeInteraction(postId: any, userLiked:any){
    if(userLiked == false){
      this.likePost(postId);
    }else{
      this.revokeLike(postId);
    }
  }

  revokeLike(postId: any){
    this.postService.revokeLike(postId).subscribe({
      next: (response) => {
        console.log(response);
        this.getPostsSelf();
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }

  likePost(postId: any){
    this.postService.likePost(postId).subscribe({
      next: (response) => {
        console.log(response);
        this.getPostsSelf();
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }

  public colorHashtag(text:string){
    return this.commonService.colorHashtag(text);
  }



}
