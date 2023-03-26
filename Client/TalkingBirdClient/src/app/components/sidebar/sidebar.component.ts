import { CommonService } from './../../services/common.service';
import { DataTransferService } from 'src/app/services/data-transfer.service';
import { AuthenticationService } from './../../services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { faDove } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { CommonInformation } from 'src/app/models/common-information';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit{

  constructor(private authenticationService: AuthenticationService,
    private dataTransferService: DataTransferService, private commonService: CommonService) { }

  ngOnInit(): void {
    this.commonInformationSubscription = this.dataTransferService.currentCommonInformation
      .subscribe((commonData) => this.commonInformation = commonData );
  }

  faDove = faDove;

  commonInformationSubscription: Subscription = new Subscription();
  commonInformation:CommonInformation = new CommonInformation();



  logout(){
    this.authenticationService.logout();
  }

  public processProfileDisplayName(userName:any, displayName:any): string{
    var response = this.commonService.processProfileDisplayName(userName, displayName);
    return response;
  }




}
