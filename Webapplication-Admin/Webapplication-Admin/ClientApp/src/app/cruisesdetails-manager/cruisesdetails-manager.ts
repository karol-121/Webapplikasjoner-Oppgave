import { Component, OnInit } from "@angular/core";
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

import { CruiseDetails } from '../CruiseDetails';

@Component({
  templateUrl: "cruisesdetails-manager.html"
})

export class CruisesdetailsManager {
  cruisedetails_modifications: FormGroup;
  cruisesdetails: Array<CruiseDetails>;
  submitButtonText: string;
  isFetchingData: boolean;
  selected: number; //id til det valgte element
  alertContent: string; //streng som inneholder alert tekst

  formProfile = {
    cruisedetails_id: [null],
    cruisedetails_max_passengers: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
    cruisedetails_passeger_price: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
    cruisedetails_passenger_underage_price: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
    cruisedetails_pet_price: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
    cruisedetails_vehicle_price: [null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,5}")])],
  }

  constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
    this.cruisedetails_modifications = fb.group(this.formProfile);
    this.cruisedetails_modifications.controls.cruisedetails_id.disable(); //angular foretrekker disablering input herfra og ikke direkte i dom
    this.submitButtonText = "Register";
    this.selected = -1;
    this.alertContent = null; //dersom alertcontet er null, alert vises ikke
  }

  //init funksjon
  ngOnInit() {
    this.fetchCruisedetails();
    this.isFetchingData = true;
  }

  //funksjon som lukker alert
  dissmissAlert() {
    this.alertContent = null;
  }

  //funksjon som legger valgt element inn i formen, hvorfra denne elementet kan endres
  applyForEdit(toEdit: CruiseDetails) {

    this.cruisedetails_modifications.patchValue({ cruisedetails_id: toEdit.id });
    this.cruisedetails_modifications.patchValue({ cruisedetails_max_passengers: toEdit.max_Passengers });
    this.cruisedetails_modifications.patchValue({ cruisedetails_passeger_price: toEdit.passeger_Price });
    this.cruisedetails_modifications.patchValue({ cruisedetails_passenger_underage_price: toEdit.passegner_Underage_Price });
    this.cruisedetails_modifications.patchValue({ cruisedetails_pet_price: toEdit.pet_Price });
    this.cruisedetails_modifications.patchValue({ cruisedetails_vehicle_price: toEdit.vehicle_Price });
 
    this.submitButtonText = "Endre";
    this.selected = toEdit.id;

  }

  //funksjon som enten endrer eller legger til objekt basert p?? input verdier
  submitForm() {

    //hvis id i form er ikke null -> endre objekt med gitt id
    if (this.cruisedetails_modifications.controls.cruisedetails_id.value !== null) {

      this.editCruisedetails();

      //ingen id == register objekt istedenfor
    } else {

      this.addCruisedetails();

    }
  }

  //funksjon som tilbakestiller formen til dens opprinelig tilstand
  resetForm() {

    this.cruisedetails_modifications.reset();
    this.submitButtonText = "Register";
    this.selected = -1;
  }

  //henter alle cruise details fra databasen
  fetchCruisedetails() {
    this.http.get<CruiseDetails[]>("API/CruiseDetails")
      .subscribe(fetchedCruisedetails => {
        this.cruisesdetails = fetchedCruisedetails;
        this.isFetchingData = false;

      }, response => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (response.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }

      });
  }

  //funksjon som henter verdier fra form og s?? legge til et nytt objekt med disse verdier
  addCruisedetails() {
    const newCruisedetails = new CruiseDetails();
    //Id blir null dersom denne krever ikke cruisedetails add metode p?? serveren
    newCruisedetails.max_Passengers = this.cruisedetails_modifications.value.cruisedetails_max_passengers;
    newCruisedetails.passeger_Price = this.cruisedetails_modifications.value.cruisedetails_passeger_price;
    newCruisedetails.passegner_Underage_Price = this.cruisedetails_modifications.value.cruisedetails_passenger_underage_price;
    newCruisedetails.pet_Price = this.cruisedetails_modifications.value.cruisedetails_pet_price;
    newCruisedetails.vehicle_Price = this.cruisedetails_modifications.value.cruisedetails_vehicle_price;

    this.http.post("API/CruiseDetails", newCruisedetails)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchCruisedetails();
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Disse cruise detaljer kunne ikke bli lagt til.";
        }

      });
  }

  //funksjon som henter verdier fra form og s?? endrer bestem objekt med disse verdier
  editCruisedetails() {
    const modifiedCruiseDetails = new CruiseDetails();
    modifiedCruiseDetails.id = this.cruisedetails_modifications.controls.cruisedetails_id.value; //for ?? hente verdi fra et disabled input
    modifiedCruiseDetails.max_Passengers = this.cruisedetails_modifications.value.cruisedetails_max_passengers;
    modifiedCruiseDetails.passeger_Price = this.cruisedetails_modifications.value.cruisedetails_passeger_price;
    modifiedCruiseDetails.passegner_Underage_Price = this.cruisedetails_modifications.value.cruisedetails_passenger_underage_price;
    modifiedCruiseDetails.pet_Price = this.cruisedetails_modifications.value.cruisedetails_pet_price;
    modifiedCruiseDetails.vehicle_Price = this.cruisedetails_modifications.value.cruisedetails_vehicle_price;


    this.http.put("API/CruiseDetails", modifiedCruiseDetails)
      .subscribe(body => { }, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchCruisedetails();
        }

        //bad request
        if (response.status === 400) {
          this.alertContent = "Disse cruise detaljer kunne ikke bli endret.";
        }

      });

  }

  //funksjon som sletter element 
  deleteCruisedetails() {

    const id = this.cruisedetails_modifications.controls.cruisedetails_id.value;

    this.http.delete("API/CruiseDetails/" + id)
      .subscribe(body => {}, response => {

        //success
        if (response.status === 200) {
          this.resetForm();
          this.fetchCruisedetails();
        }
        
        //bad request
        if (response.status === 400) {
          this.alertContent = "Disse cruise detaljer kunne ikke bli fjernet. Er du sikkert p?? at de refereres ikke til andre objekter?";
        }
      });
  }

}

