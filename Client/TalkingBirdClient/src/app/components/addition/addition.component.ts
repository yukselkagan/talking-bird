import { SharedFunctionService } from './../../services/shared-function.service';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { LoginModalComponent } from '../login-modal/login-modal.component';
import { SignUpModalComponent } from '../sign-up-modal/sign-up-modal.component';

@Component({
  selector: 'app-addition',
  templateUrl: './addition.component.html',
  styleUrls: ['./addition.component.scss']
})
export class AdditionComponent implements OnInit{
  @ViewChild('signUpModal') signUpModalElementRef!: ElementRef;
  @ViewChild('overlay') overlayElementRef!: ElementRef;
  @ViewChild('loginModal') loginModal!: LoginModalComponent;
  @ViewChild('signUpModal') signUpModal!: SignUpModalComponent;

  constructor(private dataTransferService: DataTransferService,
    private sharedFunctionService: SharedFunctionService) {}

  ngOnInit() {

    this.assignFunction();

  }

  assignFunction(){

    this.sharedFunctionService.OpenSignUpModalEmitter.subscribe((someValue) => {
      this.openSignUpModal();
    });

    this.sharedFunctionService.OpenLoginModalEmitter.subscribe(() => {
      this.openLoginModal();
    });

  }

  openSignUpModal(){
    this.signUpModal.openModal();
  }

  openLoginModal(){
    this.loginModal.openModal();
  }

}
