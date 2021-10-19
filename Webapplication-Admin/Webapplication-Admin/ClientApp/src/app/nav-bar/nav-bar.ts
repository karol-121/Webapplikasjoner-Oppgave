import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.html'
})

export class NavBar {
  isShowing = false;

  toggle() {
    this.isShowing = !this.isShowing;
  }
}
