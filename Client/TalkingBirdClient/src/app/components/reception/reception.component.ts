import { SignUpModalComponent } from './../sign-up-modal/sign-up-modal.component';
import { LoginModalComponent } from './../login-modal/login-modal.component';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { faDove } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-reception',
  templateUrl: './reception.component.html',
  styleUrls: ['./reception.component.scss']
})
export class ReceptionComponent {
  @ViewChild('signUpModal') signUpModalElementRef!: ElementRef;
  @ViewChild('overlay') overlayElementRef!: ElementRef;
  @ViewChild('loginModal') loginModal!: LoginModalComponent;
  @ViewChild('signUpModal') signUpModal!: SignUpModalComponent;


  // openModal(){
  //   this.signUpModalElementRef.nativeElement.classList.add('active');
  //   this.overlayElementRef.nativeElement.classList.add('active');
  // }

  // closeModal(){
  //   this.signUpModalElementRef.nativeElement.classList.remove('active');
  //   this.overlayElementRef.nativeElement.classList.remove('active');
  // }

  faDove = faDove;

  openSignUpModal(){
    this.signUpModal.openModal();
  }

  openLoginModal(){
    this.loginModal.openModal();
  }




}
