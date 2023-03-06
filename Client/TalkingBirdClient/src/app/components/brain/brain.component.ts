import { AuthenticationService } from './../../services/authentication.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-brain',
  templateUrl: './brain.component.html',
  styleUrls: ['./brain.component.scss']
})
export class BrainComponent  implements OnInit {

  constructor(private authenticationService: AuthenticationService){}

  ngOnInit(): void {
    if(localStorage.getItem('token') != null){
      this.authenticationService.getUserInformation();
    }
  }

}
