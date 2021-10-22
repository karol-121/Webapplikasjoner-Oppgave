import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import { Route } from '../Route';

@Component({
  templateUrl: "routes-manager.html"
})

export class RoutesManager {
  route_modifications: FormGroup;
  routes: Array<Route>;
  submitButtonText: string;
  isFetchingData: boolean;
  selected: number;

  //forsk på hvorfor ikke latinske bokstaver inn i validtors pattern knekker hele siden, angualr har noe problem med http
  formProfile = {
    route_id: [null],
    route_orgin: [null, Validators.compose([Validators.required, Validators.pattern("[A-Za-z\- .]{2,30}")])],
    route_destination: [null, Validators.compose([Validators.required, Validators.pattern("^[A-Za-z\- .]{2,30}$")])]
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.route_modifications = fb.group(this.formProfile);
    this.route_modifications.controls.route_id.disable(); //angular foretrekker disablering input herfra og ikke direkte i dom
    this.submitButtonText = "Register";
    this.selected = -1;
  }

  ngOnInit() {
    this.fetchRoutes();
    this.isFetchingData = true;
  }

  //funksjon som legger valgt element inn i formen, hvorfra denne elementet kan endres
  applyForEdit(toEdit: Route) {

    this.route_modifications.patchValue({ route_id: toEdit.id }); 
    this.route_modifications.patchValue({ route_orgin: toEdit.origin });
    this.route_modifications.patchValue({ route_destination: toEdit.destination });

    this.submitButtonText = "Endre";
    this.selected = toEdit.id;

  }

  //funksjon som enten endrer eller legger til objekt basert på input verdier
  submitForm() {

    //hvis id i form er ikke null -> endre objekt med gitt id
    if (this.route_modifications.controls.route_id.value !== null) {

      this.editRoute();

      //ingen id == register objekt istedenfor
    } else { 
      
      this.addRoute();

    }
  }

  //funksjon som tilbakestiller formen til dens opprinelig tilstand
  resetForm() {

    this.route_modifications.reset();
    this.submitButtonText = "Register";
    this.selected = -1;
  }

  //henter alle ruter fra databasen
  fetchRoutes() {
    this.http.get<Route[]>("API/Route")
      .subscribe(fetchedRoutes => {
        this.routes = fetchedRoutes;
        this.isFetchingData = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //funksjon som henter verdier fra form og så legge til et nytt objekt med disse verdier
  addRoute() {
    const newRoute = new Route();
    //Id blir null dersom denne krever ikke route add metode på serveren
    newRoute.origin = this.route_modifications.value.route_orgin;
    newRoute.destination = this.route_modifications.value.route_destination;
    //Return_id blir null dersom denne krever ikke route add metode på serveren

    this.http.post("API/Route", newRoute)
      .subscribe(body => {}, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchRoutes();
        }

        //do fail here

      });
  }

  //funksjon som henter verdier fra form og så endrer bestem objekt med disse verdier
  editRoute() {
    const modifiedRoute = new Route();
    modifiedRoute.id = this.route_modifications.controls.route_id.value; //for å hente verdi fra et disabled input
    modifiedRoute.origin = this.route_modifications.value.route_orgin;
    modifiedRoute.destination = this.route_modifications.value.route_destination;
    //Return_id blir null dersom denne krever ikke route put metode på serveren

    this.http.put("API/Route", modifiedRoute)
      .subscribe(body => {}, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchRoutes();
        }

        //do fail here

      });

  }

  //funksjon som sletter element 
  deleteRoute() {

    const id = this.route_modifications.controls.route_id.value;

    this.http.delete("API/Route/" + id)
      .subscribe(body => {}, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchRoutes();
        }

        //do fail here

      });
  }

}
