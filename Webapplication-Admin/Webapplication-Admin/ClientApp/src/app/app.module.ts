import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoggInn } from './logg-inn/logg-inn';
import { NavBar } from './nav-bar/nav-bar';
import { RoutesManager } from './routes-manager/routes-manager';
import { CruisesdetailsManager } from './cruisesdetails-manager/cruisesdetails-manager';
import { AppRoutingModule } from './app-routing.module';


@NgModule({
  declarations: [
    AppComponent,
    NavBar,
    LoggInn,
    CruisesdetailsManager,
    RoutesManager
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
