import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class MessageService {
  messages: any[] = [];

  constructor() {}

  add(message: any) {
    console.log(message);
    this.messages.push(message);
  }

  clear() {
    this.messages = [];
  }
}
