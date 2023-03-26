import { TrendService } from './../../services/trend.service';
import { SharedFunctionService } from './../../services/shared-function.service';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';
import { DataTransferService } from 'src/app/services/data-transfer.service';

@Component({
  selector: 'app-sidebar-trend',
  templateUrl: './sidebar-trend.component.html',
  styleUrls: ['./sidebar-trend.component.scss']
})
export class SidebarTrendComponent implements OnInit {

  constructor(private dataTransferService: DataTransferService,
    private sharedFunctionService: SharedFunctionService, private trendService: TrendService) { }

  ngOnInit(): void {
    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData );

    this.getTrends();
  }

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();


  trendList = [];

  getTrends(){
    this.trendService.getTrends().subscribe({
      next: (response) => {
        console.log(response);
        this.trendList = response;
      },
      error: (errorResponse) => {
        console.log(errorResponse);
      }
    })
  }

  openSignUpModal(){
    this.sharedFunctionService.openSignUpModal();
  }

  openLoginModal(){
    this.sharedFunctionService.openLoginModal();
  }


}
