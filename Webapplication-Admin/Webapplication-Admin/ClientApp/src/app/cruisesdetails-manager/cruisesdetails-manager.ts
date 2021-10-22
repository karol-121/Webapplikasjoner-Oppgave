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
  }

  ngOnInit() {
    this.fetchCruisedetails();
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

  }

  //funksjon som enten endrer eller legger til objekt basert på input verdier
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
  }

  //henter alle ruter fra databasen
  fetchCruisedetails() {
    this.http.get<CruiseDetails[]>("API/CruiseDetails")
      .subscribe(fetchedCruisedetails => {
        this.cruisesdetails = fetchedCruisedetails;

      }, error => {

        //dersom det returneres et http 401 (unauthorized), flyttes brukeren til logg inn side
        if (error.status === 401) {
          this.router.navigate(['/Logg-Inn']);
        }
      });
  }

  //funksjon som henter verdier fra form og så legge til et nytt objekt med disse verdier
  addCruisedetails() {
    const newCruisedetails = new CruiseDetails();
    //Id blir null dersom denne krever ikke cruisedetails add metode på serveren
    newCruisedetails.max_Passengers = this.cruisedetails_modifications.value.cruisedetails_max_passengers;
    newCruisedetails.passeger_Price = this.cruisedetails_modifications.value.cruisedetails_passeger_price;
    newCruisedetails.passegner_Underage_Price = this.cruisedetails_modifications.value.cruisedetails_passenger_underage_price;
    newCruisedetails.pet_Price = this.cruisedetails_modifications.value.cruisedetails_pet_price;
    newCruisedetails.vehicle_Price = this.cruisedetails_modifications.value.cruisedetails_vehicle_price;

    this.http.post("API/CruiseDetails", newCruisedetails)
      .subscribe(response => {

        //her skal man printe et slags alert som sier at det har blit lagt til
        this.resetForm();
        this.fetchCruisedetails();

      }, error => {

        //printe feilmelding
        console.log(error);

      });
  }

  //funksjon som henter verdier fra form og så endrer bestem objekt med disse verdier
  editCruisedetails() {
    const modifiedCruiseDetails = new CruiseDetails();
    modifiedCruiseDetails.id = this.cruisedetails_modifications.controls.cruisedetails_id.value; //for å hente verdi fra et disabled input
    modifiedCruiseDetails.max_Passengers = this.cruisedetails_modifications.value.cruisedetails_max_passengers;
    modifiedCruiseDetails.passeger_Price = this.cruisedetails_modifications.value.cruisedetails_passeger_price;
    modifiedCruiseDetails.passegner_Underage_Price = this.cruisedetails_modifications.value.cruisedetails_passenger_underage_price;
    modifiedCruiseDetails.pet_Price = this.cruisedetails_modifications.value.cruisedetails_pet_price;
    modifiedCruiseDetails.vehicle_Price = this.cruisedetails_modifications.value.cruisedetails_vehicle_price;


    this.http.put("API/CruiseDetails", modifiedCruiseDetails)
      .subscribe(response => {

        //her skal man printe et slags alert som sier at det har blit modifisert
        this.resetForm();
        this.fetchCruisedetails();

      }, error => {

        //printe feilmelding
        console.log(error);

      });

  }

  //funksjon som sletter element 
  deleteCruisedetails() {

    const id = this.cruisedetails_modifications.controls.cruisedetails_id.value;

    this.http.delete("API/CruiseDetails/" + id)
      .subscribe(response => {

        //lage melding
        this.resetForm();
        this.fetchCruisedetails();

      }, error => {

        //feilmelding
        console.log(error);

      });
  }

}

