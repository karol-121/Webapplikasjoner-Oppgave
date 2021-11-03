import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.html'
})

export class NavBar {
  isShowing = false;

  constructor(private router: Router) { }

  toggle() {
    this.isShowing = !this.isShowing;
  }
}
