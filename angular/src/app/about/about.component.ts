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
  constructor(
    private location: Location,
    private router: Router,
    private electronService: ElectronService
  ) {}

  ngOnInit() {}

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
