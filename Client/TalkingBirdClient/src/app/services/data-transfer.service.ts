import { CommonInformation } from './../models/common-information';
import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject, Subscription } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataTransferService {

  constructor() { }

  private commonInformationSource = new BehaviorSubject(new CommonInformation());
  currentCommonInformation = this.commonInformationSource.asObservable();

  changeCommonInformation(newInformation: CommonInformation){
    this.commonInformationSource.next(newInformation);
  }



}
