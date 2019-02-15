import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ElectronService } from 'ngx-electron';
import { Router } from '@angular/router';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  userData: string;
  version: string;

  constructor(
    private location: Location,
    private router: Router,
    private electronService: ElectronService
  ) {}

  ngOnInit() {
    if (this.electronService.isElectronApp) {
      this.userData = this.electronService.remote.app.getPath('userData');
      this.version = this.electronService.remote.app.getVersion();
    }
  }

  goBack() {
    this.location.back();
  }

  externalLink(url: string) {
    if (this.electronService.isElectronApp) {
      this.electronService.shell.openExternal(url);
    } else {
      window.location.href = url;
    }
  }
}
