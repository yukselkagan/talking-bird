import { EventEmitter, Injectable } from '@angular/core';
import { Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedFunctionService {

  constructor() { }

  OpenSignUpModalEmitter = new EventEmitter();
  OpenLoginModalEmitter = new EventEmitter();

  openSignUpModal(){
    this.OpenSignUpModalEmitter.emit();
  }

  openLoginModal(){
    this.OpenLoginModalEmitter.emit();
  }

}
