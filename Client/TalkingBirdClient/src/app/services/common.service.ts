import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor() { }

  showToast(message:string, type:string="normal"){
    if(type == "error"){
      document.getElementById("appToast")?.classList.remove("bg-primary");
      document.getElementById("appToast")?.classList.add("bg-danger");
    }

    document.getElementById("appToast")?.classList.add("show");
    let toastTextElement = document.getElementById("appToastText") as HTMLInputElement;
    toastTextElement.innerHTML = message;
  }

}
