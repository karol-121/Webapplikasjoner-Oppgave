import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { UserInfo } from '../UserInfo';


@Component({
  templateUrl: "logg-inn.html"
})

export class LoggInn {
  logg_inn_form: FormGroup;

  form_validation = {
    username: [
      null, Validators.compose([Validators.required, Validators.pattern("[a-zA-Z0-9\-_]{3,15}")])
    ],
    password: [
      null, Validators.compose([Validators.required, Validators.pattern("[0-9A-Za-z]{8,64}")])
    ]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.logg_inn_form = fb.group(this.form_validation);
  }

  //funksjon som kjøres ved submit av form, den lager et user info objekt som inneholder form data og kaller logg inn funksjon på server
  authenticate() {
    const userInfo = new UserInfo();
    userInfo.Username = this.logg_inn_form.value.username;
    userInfo.Password = this.logg_inn_form.value.password;

    this.http.post("API/EstabilishAdministratorToken", userInfo)
      .subscribe(success => { console.log("logg inn success;"); }, error => console.log(error));
  }


}



