import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TrendService {

  constructor(private httpClient: HttpClient) { }

  baseUrl = "https://localhost:44351/api";

  public getTrends(){
    return this.httpClient.get<any>(this.baseUrl+'/trends')
  }

}
