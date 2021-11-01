import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  templateUrl: "dashboard.html"
})

export class Dashboard {

  constructor(private http: HttpClient, private router: Router) {
    
  }

  //funksjon som logger ut
  unauthenticate() {
    this.http.get("API/DemolishAdministratorToken")
      .subscribe(body => {}, response => {

        //hvis det har blitt vellykket utlogging
        if (response.status === 200) {
          //gå til innlogging side
          this.router.navigate(['/Logg-Inn']);
        }
        
      });
  }
}
