import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { faDove } from '@fortawesome/free-solid-svg-icons';
import { PostService } from 'src/app/services/post.service';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-save',
  templateUrl: './save.component.html',
  styleUrls: ['./save.component.scss']
})
export class SaveComponent {
  constructor(private postService: PostService, private dataTransferService: DataTransferService){}

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

  likePost(postId: any){
    console.log(postId);

    this.postService.likePost(postId).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })

  }

}
