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

  public processProfileDisplayName(userName:any, displayName:any){
    if(displayName == null || displayName == "" ){
      return '@'+userName;
    }else{
      return displayName;
    }
  }

  public colorHashtag(text:string){
    let createdText = "";
    let words = text.split(" ");
    for (let index = 0; index < words.length; index++) {
      let targetWord = words[index];
      if(targetWord[0] == "#"){
        targetWord = `<span class='text-primary' >${targetWord}</span>`;
      }
      createdText += targetWord + " ";
    }

    return createdText;
  }



}
