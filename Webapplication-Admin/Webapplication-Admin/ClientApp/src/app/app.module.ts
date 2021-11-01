import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoggInn } from './logg-inn/logg-inn';
import { Dashboard } from './dashboard/dashboard';
import { NavBar } from './nav-bar/nav-bar';
import { RoutesManager } from './routes-manager/routes-manager';
import { CruisesdetailsManager } from './cruisesdetails-manager/cruisesdetails-manager';
import { CruisesManager } from './cruises-manager/cruises-manager';
import { DeparturesManager } from './departures-manager/departures-manager';
import { AppRoutingModule } from './app-routing.module';


@NgModule({
  declarations: [
    AppComponent,
    LoggInn,
    Dashboard,
    NavBar,
    RoutesManager,
    CruisesdetailsManager,
    DeparturesManager,
    CruisesManager
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
