import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoggInn } from './logg-inn/logg-inn';
import { RoutesManager } from './routes-manager/routes-manager';
import { CruisesdetailsManager } from './cruisesdetails-manager/cruisesdetails-manager';


const appRoots: Routes = [
  { path: 'Logg-Inn', component: LoggInn },
  { path: 'Manage-Routes', component: RoutesManager },
  { path: 'Manage-Cruisedetails', component: CruisesdetailsManager }
]

@NgModule({
  imports: [
    RouterModule.forRoot(appRoots)
  ],
  exports: [
    RouterModule
  ]
})

export class AppRoutingModule {}
