import { CommonService } from './../../services/common.service';
import { AuthenticationService } from './../../services/authentication.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, ViewChild, ElementRef } from '@angular/core';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.scss']
})
export class LoginModalComponent {
  @ViewChild('loginModal') loginModalElementRef!: ElementRef;
  @ViewChild('overlay') overlayElementRef!: ElementRef;

  constructor(private authenticationService: AuthenticationService,
    private commonService: CommonService){}

  openModal(){
    this.loginModalElementRef.nativeElement.classList.add('active');
    this.overlayElementRef.nativeElement.classList.add('active');
  }

  closeModal(){
    this.loginModalElementRef.nativeElement.classList.remove('active');
    this.overlayElementRef.nativeElement.classList.remove('active');
  }

  loginForm: FormGroup = new FormGroup({
    email: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required)
  })

  sendLoginForm(){
    if(this.loginForm.invalid){

    }

    let newLoginForm = {
      'email': this.loginForm.controls['email'].value,
      'password': this.loginForm.controls['password'].value,
    }

    this.authenticationService.login(newLoginForm).subscribe({
      next: (response) => {
        console.log(response);
        let token = response['accessToken'];
        localStorage.setItem('token', token);
        this.closeModal();
        this.authenticationService.getUserInformation();
      },
      error: (error) => {
        console.log(error)
        this.commonService.showToast(error.error);
      }
    })

  }


}
