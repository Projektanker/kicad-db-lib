import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
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
  ) {}

  ngOnInit() {   
  }

  goBack() {
    this.location.back();
  }

  externalLink(url: string) {    
      window.location.href = url;
  }
}
