import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { UserInfo } from '../UserInfo';


@Component({
  templateUrl: "logg-inn.html"
})

export class LoggInn {
  logg_inn_form: FormGroup;

  formProfile = {
    username: [null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9\-_]{3,15}")])],
    password: [null, Validators.compose([Validators.required, Validators.pattern("[0-9A-Za-z]{8,64}")])]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.logg_inn_form = fb.group(this.formProfile);
  }

  //funksjon som kjøres ved submit av form, den lager et user info objekt som inneholder form data og kaller logg inn funksjon på server
  authenticate() {
    const userInfo = new UserInfo();
    userInfo.username = this.logg_inn_form.value.username;
    userInfo.password = this.logg_inn_form.value.password;

    //her skal det redirectes tilbake der hvorfra logg inn request ble laget,
    //f.eks dersom noen prøvde å åpne route admin side, og var ikke logget inn, så skal den bli redirected her og så etter innlogging skal den gå tilbake til routes
    this.http.post("API/EstabilishAdministratorToken", userInfo)
      .subscribe(body => {}, response => {

        //hvis det har blitt vellykket 
        if (response.status === 200) {
          //gå til hoved siden
          this.router.navigate(['/Dashboard']);
        }

      });
  }


}



