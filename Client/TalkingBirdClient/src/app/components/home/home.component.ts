import { CommonInformation } from './../../models/common-information';
import { Subscription } from 'rxjs';
import { DataTransferService } from './../../services/data-transfer.service';
import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit{

  constructor(private dataTransferService: DataTransferService) {}


  ngOnInit(): void {

    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => { this.commonInformation = commonData } )

  }

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation: CommonInformation = new CommonInformation();


}
