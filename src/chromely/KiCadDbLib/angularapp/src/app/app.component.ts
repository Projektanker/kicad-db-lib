import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'KiCad Database Library';
  constructor() {
    console.log('Hello from AppComponent!');
  }
}
