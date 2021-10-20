import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';

import { Route } from '../Route';

@Component({
  templateUrl: "routes-manager.html"
})

export class RoutesManager {
  routes: Array<Route>;
  a: HttpErrorResponse;


  constructor(private http: HttpClient, private router: Router) {

  }

  ngOnInit() {
    this.fetchRoutes();
  }

  fetchRoutes() {
    this.http.get<Route[]>("API/Route")
      .subscribe(fetchedRoutes => {
        this.routes = fetchedRoutes;

        console.log(this.routes);
      }, error => {
        this.a = error;

        console.log(this.a.status);
      });
  }

}
