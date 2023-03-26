import { CommonService } from './../../services/common.service';
import { Subscription } from 'rxjs';
import { CommonInformation } from './../../models/common-information';
import { DataTransferService } from './../../services/data-transfer.service';
import { PostService } from './../../services/post.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { faDove } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit{

  constructor(private postService: PostService,
    private dataTransferService: DataTransferService, private commonService: CommonService){}

  ngOnInit(): void {
    this.getAllPosts();


    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData )

  }

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation: CommonInformation = new CommonInformation();

  faDove = faDove;

  //postList = [1, 2, 3];
  postList = [];

  createPostForm: FormGroup = new FormGroup({
    content: new FormControl(null, Validators.required)
  });

  createPost(){
    console.log("create post");

    if(this.createPostForm.invalid){
      alert("invalid form");
      return;
    }

    let postData = {'content': this.createPostForm.controls['content'].value }

    this.postService.createPost(postData).subscribe({
      next: (response) => {
        console.log(response);
        console.log("post created");
        this.getAllPosts();
      },
      error: (error) => {
        console.log(error);
      },
    });

    this.createPostForm.reset();
  }

  getAllPosts(){
    this.postService.getAllPosts().subscribe({
      next: (response) => {
        console.log(response);
        this.postList = response;
        console.log(this.postList);
      },
      error : (error) => {
        console.log(error);
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
        this.getAllPosts();
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
        this.getAllPosts();
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }

  public processProfileDisplayName(userName:any, displayName:any): string{
    var response = this.commonService.processProfileDisplayName(userName, displayName);
    return response;
  }

  public colorHashtag(text:string){
    let createdText = "";
    let words = text.split(" ");
    for (let index = 0; index < words.length; index++) {
      let targetWord = words[index];
      if(targetWord[0] == "#"){
        targetWord = `<span class='text-primary' >${targetWord}</span>`;
      }
      createdText += targetWord + " ";
    }

    return createdText;
  }



}
