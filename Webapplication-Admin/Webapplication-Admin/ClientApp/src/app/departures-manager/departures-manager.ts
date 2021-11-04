import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import { Departure } from '../Departure'
import { Cruise } from '../Cruise';

@Component({
  templateUrl: "departures-manager.html"
})

export class DeparturesManager {
  departure_modifications: FormGroup;

  departures: Array<Departure>
  cruises: Array<Cruise>;

  submitButtonText: string;
  isFetchingData: boolean;
  isFetchingCruises: boolean;
  selected: number; //id til valgt element
  alertContent: string; //streng som inneholder alert tekst

  formProfile = {
    departure_id: [null],
    departure_cruise_id: ["", Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
    departure_date: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}")])]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.departure_modifications = fb.group(this.formProfile);
    this.departure_modifications.controls.departure_id.disable(); //angular foretrekker disablering input herfra og ikke direkte i dom
    this.submitButtonText = "Register";
    this.selected = -1;
    this.alertContent = null; //dersom alertcontet er null, alert vises ikke
  }

  //init funksjon
  ngOnInit() {
    this.fetchCruises();
    this.fetchDepartures();
    this.isFetchingData = true;
    this.isFetchingCruises = true;

  }

  //funksjon som lukker alert
  dissmissAlert() {
    this.alertContent = null;
  }

  //funksjon som legger valgt element inn i formen, hvorfra denne elementet kan endres
  applyForEdit(toEdit: Departure) {
    this.departure_modifications.patchValue({ departure_id: toEdit.id });
    this.departure_modifications.patchValue({ departure_cruise_id: toEdit.cruise.id });
    this.departure_modifications.patchValue({ departure_date: toEdit.date });

    this.submitButtonText = "Endre";
    this.selected = toEdit.id;

  }

  //funksjon som enten endrer eller legger til objekt basert på input verdier
  submitForm() {

    //hvis id i form er ikke null -> endre objekt med gitt id
    if (this.departure_modifications.controls.departure_id.value !== null) {

      this.editDeparture();

      //ingen id == register objekt istedenfor
    } else {

      this.addDeparture();

    }
  }

  //funksjon som tilbakestiller formen til dens opprinelig tilstand
  resetForm() {
    this.departure_modifications.reset();

    //disse her må settes dersom form.reset() fjerner default verdier for input select og den blir blank
    this.departure_modifications.patchValue({ departure_cruise_id: "" })

    this.submitButtonText = "Register";
    this.selected = -1;
  }

  //henter alle cruises fra databasen
  fetchCruises() {
    this.http.get<Cruise[]>("API/Cruise")
      .subscribe(fetchedCruises => {
        this.cruises = fetchedCruises;
        this.isFetchingCruises = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //henter alle departures fra databasen
  fetchDepartures() {
    this.http.get<Departure[]>("API/Departure")
      .subscribe(fetchedDepartures => {
        this.departures = fetchedDepartures;
        this.isFetchingData = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //funksjon som henter verdier fra form og så legge til et nytt objekt med disse verdier
  addDeparture() {

    const data = {
      cruiseId: this.departure_modifications.value.departure_cruise_id,
      dateString: this.departure_modifications.value.departure_date
    }

    this.http.post("API/Departure", data)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchDepartures(); //fetche kun utreiser på nytt dersom det er kun de som har endret
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Denne utreise kunne ikke bli lagt til";
        }

      });
  }

  //funksjon som henter verdier fra form og så endrer bestem objekt med disse verdier
  editDeparture() {

    const data = {
      Id: this.departure_modifications.controls.departure_id.value,
      cruiseId: this.departure_modifications.value.departure_cruise_id,
      dateString: this.departure_modifications.value.departure_date
    }


    this.http.put("API/Departure", data)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchDepartures(); //fetche kun utreiser på nytt dersom det er kun de som har endret
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Denne utreise kunne ikke bli endret.";
        }

      });

  }

  //funksjon som sletter element 
  deleteDeparture() {

    const id = this.departure_modifications.controls.departure_id.value;

    this.http.delete("API/Departure/" + id)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchDepartures(); //fetche kun utreiser på nytt dersom det er kun de som har endret
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Denne utreise kunne ikke bli fjernet.";
        }
      });
  }


}
