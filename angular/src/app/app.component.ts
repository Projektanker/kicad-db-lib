import { Component } from '@angular/core';
import { ElectronService } from 'ngx-electron';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'KiCad Database Library';
  constructor(private electronService: ElectronService) {
    console.log('Hello from AppComponent!');
    console.log(`isElectronApp: ${this.electronService.isElectronApp}`);
  }
}
