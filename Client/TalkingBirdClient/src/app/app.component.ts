import { DataTransferService } from './services/data-transfer.service';
import { CommonInformation } from './models/common-information';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'TalkingBirdClient';

  constructor(private dataTransferService: DataTransferService) { }

  ngOnInit(): void {
    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData );

  }

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();


}
