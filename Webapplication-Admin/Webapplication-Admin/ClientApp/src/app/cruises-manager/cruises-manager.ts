import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import { Route } from '../Route';
import { CruiseDetails } from '../CruiseDetails';
import { Cruise } from '../Cruise';

@Component({
  templateUrl: "cruises-manager.html"
})

export class CruisesManager {
  cruise_modifications: FormGroup;

  cruises: Array<Cruise>;
  routes: Array<Route>;
  cruisesdetails: Array<CruiseDetails>;

  submitButtonText: string;
  isFetchingData: boolean;
  isFetchingRoutes: boolean;
  isFetchingDetails: boolean;
  selected: number; //id til valgt element
  alertContent: string; //streng som inneholder alert tekst

  formProfile = {
    cruise_id: [null],
    cruise_route_id: ["", Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
    cruise_details_id: ["", Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.cruise_modifications = fb.group(this.formProfile);
    this.cruise_modifications.controls.cruise_id.disable(); //angular foretrekker disablering input herfra og ikke direkte i dom
    this.submitButtonText = "Register";
    this.selected = -1;
    this.alertContent = null; //dersom alertcontet er null, alert vises ikke
  }

  //init funksjon
  ngOnInit() {
    this.fetchCruises();
    this.fetchRoutes();
    this.fetchCruisedetails();
    this.isFetchingData = true;
    this.isFetchingRoutes = true;
    this.isFetchingDetails = true;
  }

  //funksjon som lukker alert
  dissmissAlert() {
    this.alertContent = null;
  }

  //funksjon som legger valgt element inn i formen, hvorfra denne elementet kan endres
  applyForEdit(toEdit: Cruise) {

    this.cruise_modifications.patchValue({ cruise_id: toEdit.id });
    this.cruise_modifications.patchValue({ cruise_route_id: toEdit.route.id });
    this.cruise_modifications.patchValue({ cruise_details_id: toEdit.cruiseDetails.id });

    this.submitButtonText = "Endre";
    this.selected = toEdit.id;

  }

  //funksjon som enten endrer eller legger til objekt basert på input verdier
  submitForm() {

    //hvis id i form er ikke null -> endre objekt med gitt id
    if (this.cruise_modifications.controls.cruise_id.value !== null) {

      this.editCruise();

      //ingen id == register objekt istedenfor
    } else {

      this.addCruise();

    }
  }

  //funksjon som tilbakestiller formen til dens opprinelig tilstand
  resetForm() {

    this.cruise_modifications.reset();

    //disse her må settes dersom form.reset() fjerner default verdier for input select og den blir blank
    this.cruise_modifications.patchValue({ cruise_route_id: "" })
    this.cruise_modifications.patchValue({ cruise_details_id: "" })

    this.submitButtonText = "Register";
    this.selected = -1;
  }

  //henter alle ruter fra databasen
  fetchRoutes() {
    this.http.get<Route[]>("API/Route")
      .subscribe(fetchedRoutes => {
        this.routes = fetchedRoutes;
        this.isFetchingRoutes = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //henter alle cruise details fra databasen
  fetchCruisedetails() {
    this.http.get<CruiseDetails[]>("API/CruiseDetails")
      .subscribe(fetchedCruisedetails => {
        this.cruisesdetails = fetchedCruisedetails;
        this.isFetchingDetails = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //henter alle cruises fra databasen
  fetchCruises() {
    this.http.get<Cruise[]>("API/Cruise")
      .subscribe(fetchedCruises => {
        this.cruises = fetchedCruises;
        this.isFetchingData = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //funksjon som henter verdier fra form og så legge til et nytt objekt med disse verdier
  addCruise() {
    
    const data = {
      routeId: this.cruise_modifications.value.cruise_route_id,
      detailsId:this.cruise_modifications.value.cruise_details_id
    }

    this.http.post("API/Cruise", data)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchCruises(); //fetche kun cruises på nytt dersom det er kun de som har endret
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Denne cruise kunne ikke bli lagt til";
        }

      });
  }

  //funksjon som henter verdier fra form og så endrer bestem objekt med disse verdier
  editCruise() {

    const data = {
      Id: this.cruise_modifications.controls.cruise_id.value,
      routeId: this.cruise_modifications.value.cruise_route_id,
      detailsId: this.cruise_modifications.value.cruise_details_id
    }

    this.http.put("API/Cruise", data)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchCruises(); //fetche kun cruises på nytt dersom det er kun de som har endret
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Denne cruise kunne ikke bli endret.";
        }

      });

  }

  //funksjon som sletter element 
  deleteCruise() {

    const id = this.cruise_modifications.controls.cruise_id.value;

    this.http.delete("API/Cruise/" + id)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchCruises(); //fetche kun cruises på nytt dersom det er kun de som har endret
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Denne cruise kunne ikke bli fjernet. Er du sikkert på at den referers ikke til andre objekter?";
        }
      });
  }

}
