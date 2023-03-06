import { CommonService } from './../../services/common.service';
import { AuthenticationService } from './../../services/authentication.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-sign-up-modal',
  templateUrl: './sign-up-modal.component.html',
  styleUrls: ['./sign-up-modal.component.scss']
})
export class SignUpModalComponent {
  @ViewChild('signUpModal') signUpModalElementRef!: ElementRef;
  @ViewChild('overlay') overlayElementRef!: ElementRef;

  constructor(private authenticationService: AuthenticationService,
    private commonService: CommonService){}

  openModal(){
    this.signUpModalElementRef.nativeElement.classList.add('active');
    this.overlayElementRef.nativeElement.classList.add('active');
  }

  closeModal(){
    this.signUpModalElementRef.nativeElement.classList.remove('active');
    this.overlayElementRef.nativeElement.classList.remove('active');
  }

  signUpForm: FormGroup = new FormGroup({
    email: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required),
    userName: new FormControl(null, Validators.required)
  });

  register(){
    if(this.signUpForm.invalid){

    }

    let newSignUpForm = {
      'email': this.signUpForm.controls['email'].value,
      'password': this.signUpForm.controls['password'].value,
      'userName': this.signUpForm.controls['userName'].value
    }

    this.authenticationService.register(newSignUpForm).subscribe({
      next: (response) => {
        console.log(response);
        this.closeModal();
      },
      error : (error) => {
        console.log(error);
        if( typeof error.error === 'string' ){
          this.commonService.showToast(error.error);
        }else{
          this.commonService.showToast(error.error);
          console.log(error.error);
        }
        //this.commonService.showToast(error.error);
      }
    })

  }



}
